using LesExpo.DataAccess.Data;
using LesExpo.DataAccess.DbInitializer;
using LesExpo.DataAccess.Repository;
using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Utility;
using LesExpo.web.Services;
using LesExpo.web.ViewEngines;
using LesExpo.web.Middleware;
using LesExpo.web.Models.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.IIS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure request size limits for large file uploads
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 200 * 1024 * 1024; // 200MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200MB
    options.ValueLengthLimit = int.MaxValue;
    options.ValueCountLimit = int.MaxValue;
    options.KeyLengthLimit = int.MaxValue;
});




// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add configr

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Admin/Account/Login";
    //options.AccessDeniedPath = $"/Admin/Account/AccessDenied";
});


// Add repository services
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add other services
builder.Services.AddScoped<IFileHelper, FileHelper>();
builder.Services.AddScoped<IHtmlContentService, HtmlContentService>();
builder.Services.AddScoped<IEmailSender, EmailSenderSmtp>(); // Add new SMTP sender

// Configure localized routes from appsettings.json
builder.Services.Configure<LocalizedRoutesConfig>(
    builder.Configuration.GetSection(LocalizedRoutesConfig.SectionName));

// Configure email templates from appsettings.json
builder.Services.Configure<EmailTemplatesConfig>(
    builder.Configuration.GetSection(EmailTemplatesConfig.SectionName));

// Add URL localization service
builder.Services.AddScoped<IUrlLocalizationService, UrlLocalizationService>();

// Add external API service
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();

// Add search content indexing service
builder.Services.AddScoped<IContentIndexService, ContentIndexService>();

// Add memory cache
builder.Services.AddMemoryCache();

// Configure External API settings
builder.Services.Configure<ExternalApiConfig>(
    builder.Configuration.GetSection(ExternalApiConfig.SectionName));

// Register background services
builder.Services.AddHostedService<TempFileCleanupService>();

builder.Services.AddHttpClient("FairApi", client =>
{
    client.BaseAddress = new Uri("https://fair.smartexpo.com.tr/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});





// View Engine Configuration

builder.Services.AddRazorPages()
    .AddRazorOptions(options =>
    {
        // PRIORITY ORDER: Areas first (no language), then language-specific public views

        // 1. AREAS (Admin, etc.) - Use standard ASP.NET Core area placeholders
        // {2} = Area name, {1} = Controller, {0} = Action
        options.ViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");

        // 2. LANGUAGE-SPECIFIC PUBLIC VIEWS - Custom language expansion
        // These will be expanded by CustomViewLocationExpander to use language instead of {2}
        options.ViewLocationFormats.Add("/Views/{2}/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/{2}/Shared/{0}.cshtml");

        // 3. DEFAULT FALLBACK VIEWS - Last resort
        options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

        // Custom expander ONLY for public controllers (replaces {2} with language)
        options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // This is a temporary solution for development to bypass SSL validation for SMTP.
    // WARNING: This should not be used in production as it poses a security risk.
    System.Net.ServicePointManager.ServerCertificateValidationCallback =
        (sender, certificate, chain, sslPolicyErrors) => true;
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add URL canonicalization middleware BEFORE routing
app.UseUrlCanonicalization();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

SeedDatabase();





// 1. AREA ROUTING - For all areas (Admin, etc.) - Language agnostic
app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// 2. PUBLIC LOCALIZED ROUTING - Language-specific public controllers
app.MapControllerRoute(
    name: "localized",
    pattern: "{lang}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { lang = "en|tr" }
);

// 3. DEFAULT FALLBACK ROUTING - Non-localized public controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();



void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        //// Apply pending migrations
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        //// Initialize database with seed data
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}

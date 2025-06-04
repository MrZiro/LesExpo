using LesExpo.DataAccess.Data;
using LesExpo.DataAccess.DbInitializer;
using LesExpo.DataAccess.Repository;
using LesExpo.DataAccess.Repository.IRepository;
using LesExpo.Models;
using LesExpo.Utility;
using LesExpo.web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();




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
// builder.Services.AddScoped<IEmailSender, EmailSenderGrid>(); // Comment out or remove old sender
builder.Services.AddScoped<IEmailSender, EmailSenderSmtp>(); // Add new SMTP sender

// Register background services
builder.Services.AddHostedService<TempFileCleanupService>();

builder.Services.AddHttpClient("FairApi", client =>
{
    client.BaseAddress = new Uri("https://fair.smartexpo.com.tr/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

SeedDatabase();



app.MapControllerRoute(
    name: "AreaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "localized",
    pattern: "{lang}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { lang = "en|tr" }
);

app.MapGet("/", context =>
{
    // Redirect to the localized home page based on the default language
    var defaultLang = "tr"; // Change this to your desired default language
    context.Response.Redirect($"/{defaultLang}/Anasayfa");
    return Task.CompletedTask;
});

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
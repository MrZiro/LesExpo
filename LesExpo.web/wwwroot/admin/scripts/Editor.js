tinymce.init({
  license_key: "gpl",
  selector: ".Editor",
  plugins: [
    "table",
    "image",
    "media",
    "code",
    "codesample",
    "link",
    "lists",
    "advlist",
    "autolink",
    "searchreplace",
    "wordcount",
    "fullscreen",
    "preview",
    "help",
  ],
  toolbar:
    "undo redo | blocks | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | outdent indent | numlist bullist | forecolor backcolor removeformat | pagebreak | fullscreen preview | image media | link codesample | code",
  menu: {
    edit: { title: "Edit", items: "undo redo | selectall | searchreplace" },
    view: {
      title: "View",
      items: "code | preview fullscreen",
    },
    insert: {
      title: "Insert",
      items: "image media | link codesample | hr",
    },
    format: {
      title: "Format",
      items:
        "bold italic underline strikethrough | align | forecolor backcolor | removeformat",
    },
    table: {
      title: "Table",
      items: "inserttable | cell row column | tablecellvalign | deletetable",
    },
    tools: {
      title: "Tools",
      items: "wordcount",
    },
    help: { title: "Help", items: "help" },
  },
  images_upload_url: "/Admin/Common/UploadEditorImage",
  automatic_uploads: true,
  images_reuse_filename: true,
  relative_urls: false,
  remove_script_host: false,
  convert_urls: true,
  height: 500,
  menubar: true,
  statusbar: true,
  branding: false,
  promotion: false,
  codesample_languages: [
    { text: "HTML/XML", value: "markup" },
    { text: "JavaScript", value: "javascript" },
    { text: "CSS", value: "css" },
    { text: "PHP", value: "php" },
    { text: "Ruby", value: "ruby" },
    { text: "Python", value: "python" },
    { text: "Java", value: "java" },
    { text: "C", value: "c" },
    { text: "C#", value: "csharp" },
    { text: "C++", value: "cpp" },
    { text: "SQL", value: "sql" },
    { text: "JSON", value: "json" },
    { text: "XML", value: "xml" },
    { text: "YAML", value: "yaml" },
    { text: "Bash", value: "bash" },
    { text: "Go", value: "go" },
    { text: "TypeScript", value: "typescript" },
  ],
  content_style:
    "body { font-family: -apple-system, BlinkMacSystemFont, San Francisco, Segoe UI, Roboto, Helvetica Neue, sans-serif; font-size: 14px; line-height: 1.6; }",
  block_formats:
    "Paragraph=p; Heading 1=h1; Heading 2=h2; Heading 3=h3; Heading 4=h4; Heading 5=h5; Heading 6=h6; Blockquote=blockquote",
  font_formats:
    "Arial=arial,helvetica,sans-serif; Arial Black=arial black,avant garde; Comic Sans MS=comic sans ms,sans-serif; Courier New=courier new,courier; Georgia=georgia,palatino; Helvetica=helvetica; Impact=impact,chicago; Tahoma=tahoma,arial,helvetica,sans-serif; Times New Roman=times new roman,times; Trebuchet MS=trebuchet ms,geneva; Verdana=verdana,geneva",
  fontsize_formats: "8pt 10pt 12pt 14pt 16pt 18pt 24pt 36pt 48pt",
  setup: function (editor) {
    editor.on("ExecCommand", function (e) {
      if (e.command === "mceMedia") {
        setTimeout(function () {
          var widthInput = document.querySelector('input[name="width"]');
          var heightInput = document.querySelector('input[name="height"]');
          if (widthInput && !widthInput.value) widthInput.value = "500";
          if (heightInput && !heightInput.value) heightInput.value = "500";
        }, 100);
      }
    });
  },
  media_live_embeds: true,
  file_picker_callback: function (callback, value, meta) {
    if (meta.filetype === "media") {
      var input = document.createElement("input");
      input.setAttribute("type", "file");
      input.setAttribute("accept", "video/*");

      input.onchange = function () {
        var file = this.files[0];
        if (file) {
          if (file.size > 10 * 1024 * 1024) {
            alert(
              "Large video file detected. Upload may take a few minutes..."
            );
          }

          var formData = new FormData();
          formData.append("file", file);

          fetch("/Admin/Common/UploadEditorVideo", {
            method: "POST",
            body: formData,
          })
            .then((response) => {
              if (!response.ok) {
                throw new Error(
                  "Network response was not ok: " + response.status
                );
              }
              return response.json();
            })
            .then((data) => {
              console.log("Response data:", data);
              if (data.error) {
                alert("Video upload error: " + data.error);
              } else {
                callback(data.location, {
                  title: file.name,
                  source: data.location,
                });
              }
            })
            .catch((error) => {
              console.error("Video upload error:", error);
              alert("Video upload failed: " + error.message);
            });
        }
      };

      input.click();
    }
  },
});

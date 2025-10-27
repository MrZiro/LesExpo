tinymce.init({
  selector: ".Editor",
  plugins: ["table", "image", "media"],
  toolbar:
    "undo redo | bold italic | table | tablecellverticalalign | image | media",
  menu: {
    edit: { title: "Edit", items: "undo redo | selectall" },
    table: {
      title: "Table",
      items: "inserttable | cell row column | tablecellvalign | deletetable",
    },
    insert: { title: "Insert", items: "image media" },
  },
  images_upload_url: "/Admin/Common/UploadEditorImage",
  automatic_uploads: true,
  images_reuse_filename: true,
  relative_urls: false,
  remove_script_host: false,
  convert_urls: true,
  setup: function (editor) {
    editor.on('ExecCommand', function (e) {
      if (e.command === 'mceMedia') {
        setTimeout(function() {
          var widthInput = document.querySelector('input[name="width"]');
          var heightInput = document.querySelector('input[name="height"]');
          if (widthInput && !widthInput.value) widthInput.value = '500';
          if (heightInput && !heightInput.value) heightInput.value = '500';
        }, 100);
      }
    });
  },
  // Video upload configuration
  media_live_embeds: true,
  // Custom file picker for videos
  file_picker_callback: function (callback, value, meta) {
    if (meta.filetype === "media") {
      // Create file input for video upload
      var input = document.createElement("input");
      input.setAttribute("type", "file");
      input.setAttribute("accept", "video/*");

      input.onchange = function () {
        var file = this.files[0];
        if (file) {
          console.log(
            "Video file selected:",
            file.name,
            "Size:",
            file.size,
            "Type:",
            file.type
          );

          // Show upload progress for large files
          if (file.size > 10 * 1024 * 1024) {
            // 10MB
            alert(
              "Large video file detected. Upload may take a few minutes..."
            );
          }

          // Upload video file
          var formData = new FormData();
          formData.append("file", file);

          console.log("Uploading video to /Admin/Common/UploadEditorVideo");

          fetch("/Admin/Common/UploadEditorVideo", {
            method: "POST",
            body: formData,
          })
            .then((response) => {
              console.log(
                "Response received:",
                response.status,
                response.statusText
              );
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

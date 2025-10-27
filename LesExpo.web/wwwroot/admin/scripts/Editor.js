tinymce.init({
  selector: ".Editor",
  plugins: ["table", "image", "imagetools", "media"],
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
  // Video upload configuration
  media_live_embeds: true,
  media_url_resolver: function (data, resolve) {
    // Handle video uploads
    if (data.url && data.url.includes("/uploads/Temp/")) {
      resolve({
        html:
          '<video controls><source src="' +
          data.url +
          '" type="video/mp4">Your browser does not support the video tag.</video>',
      });
    } else {
      resolve({ html: data.html });
    }
  },
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
          // Upload video file
          var formData = new FormData();
          formData.append("file", file);

          fetch("/Admin/Common/UploadEditorVideo", {
            method: "POST",
            body: formData,
          })
            .then((response) => response.json())
            .then((data) => {
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
              alert("Video upload failed: " + error.message);
            });
        }
      };

      input.click();
    }
  },
});

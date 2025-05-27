tinymce.init({
    selector: '.Editor',
    plugins: [
        'table', 'image', 'imagetools', 'media'
    ],
    toolbar: 'undo redo | bold italic | table | tablecellverticalalign | image',
    menu: {
        edit: { title: 'Edit', items: 'undo redo | selectall' },
        table: { title: 'Table', items: 'inserttable | cell row column | tablecellvalign | deletetable' }
    },
    images_upload_url: '/Admin/Common/UploadEditorImage',
    automatic_uploads: true,
    images_reuse_filename: true,
    relative_urls: false,
    remove_script_host: false,
    convert_urls: true
});
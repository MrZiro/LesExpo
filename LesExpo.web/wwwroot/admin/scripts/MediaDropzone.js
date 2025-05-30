document.addEventListener('DOMContentLoaded', function() {
    // Tab functionality
    const tabButtons = document.querySelectorAll('.tab-button');
    const tabContents = document.querySelectorAll('.tab-content');
    const mediaTypeInput = document.querySelector('input[name="Slider.MediaType"]');
    
    // Initialize media type based on existing data
    if (mediaTypeInput.value === 'video') {
        switchToVideoTab();
    }
    
    tabButtons.forEach(button => {
        button.addEventListener('click', function() {
            const tabId = this.getAttribute('data-tab');
            
            // Update tab buttons
            tabButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');
            
            // Update tab contents
            tabContents.forEach(content => content.classList.remove('active'));
            document.getElementById(tabId + '-tab').classList.add('active');
            
            // Update hidden input
            mediaTypeInput.value = tabId;
            
            // Clear other media inputs when switching tabs
            if (tabId === 'image') {
                clearVideoInputs();
            } else {
                clearImageInputs();
            }
        });
    });

    // Video source radio buttons
    const videoSourceRadios = document.querySelectorAll('input[name="videoSource"]');
    const videoSourceInput = document.querySelector('input[name="Slider.VideoSource"]');
    const videoUploadSection = document.getElementById('video-upload-section');
    const videoYoutubeSection = document.getElementById('video-youtube-section');
    
    // Initialize video source based on existing data
    if (videoSourceInput.value) {
        const targetRadio = document.querySelector(`input[name="videoSource"][value="${videoSourceInput.value}"]`);
        if (targetRadio) {
            targetRadio.checked = true;
            showVideoSection(videoSourceInput.value);
        }
    }
    
    videoSourceRadios.forEach(radio => {
        radio.addEventListener('change', function() {
            const selectedSource = this.value;
            videoSourceInput.value = selectedSource;
            showVideoSection(selectedSource);
            
            // Clear the other video input when switching
            if (selectedSource === 'upload') {
                clearYoutubeInput();
            } else {
                clearVideoFileInput();
            }
        });
    });

    function showVideoSection(source) {
        if (source === 'upload') {
            videoUploadSection.style.display = 'block';
            videoYoutubeSection.style.display = 'none';
        } else if (source === 'youtube') {
            videoUploadSection.style.display = 'none';
            videoYoutubeSection.style.display = 'block';
        }
    }

    function switchToVideoTab() {
        document.querySelector('.tab-button[data-tab="video"]').click();
    }

    // Image dropzone functionality
    const imageDropzone = document.getElementById('image-dropzone');
    const imageInput = document.getElementById('imageInput');
    
    if (imageDropzone && imageInput) {
        setupDropzone(imageDropzone, imageInput, handleImageFile);
    }

    // Video dropzone functionality
    const videoDropzone = document.getElementById('video-dropzone');
    const videoInput = document.getElementById('videoInput');
    
    if (videoDropzone && videoInput) {
        setupDropzone(videoDropzone, videoInput, handleVideoFile);
    }

    function setupDropzone(dropzone, input, fileHandler) {
        dropzone.addEventListener('click', () => input.click());
        
        dropzone.addEventListener('dragover', function(e) {
            e.preventDefault();
            this.classList.add('dragover');
        });
        
        dropzone.addEventListener('dragleave', function(e) {
            e.preventDefault();
            this.classList.remove('dragover');
        });
        
        dropzone.addEventListener('drop', function(e) {
            e.preventDefault();
            this.classList.remove('dragover');
            
            const files = e.dataTransfer.files;
            if (files.length > 0) {
                input.files = files;
                fileHandler(files[0]);
            }
        });
        
        input.addEventListener('change', function() {
            if (this.files && this.files[0]) {
                fileHandler(this.files[0]);
            }
        });
    }

    function handleImageFile(file) {
        if (validateImageFile(file)) {
            const reader = new FileReader();
            reader.onload = function(e) {
                showPreview('image', e.target.result);
            };
            reader.readAsDataURL(file);
        }
    }

    function handleVideoFile(file) {
        if (validateVideoFile(file)) {
            const reader = new FileReader();
            reader.onload = function(e) {
                showPreview('video', e.target.result);
            };
            reader.readAsDataURL(file);
        }
    }

    function validateImageFile(file) {
        const validTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp'];
        const maxSize = 5 * 1024 * 1024; // 5MB
        
        if (!validTypes.includes(file.type)) {
            alert('Geçersiz dosya türü. Lütfen JPG, PNG, GIF veya WEBP dosyası seçin.');
            return false;
        }
        
        if (file.size > maxSize) {
            alert('Dosya boyutu çok büyük. Maksimum 5MB olmalıdır.');
            return false;
        }
        
        return true;
    }

    function validateVideoFile(file) {
        const validTypes = ['video/mp4', 'video/avi', 'video/mov', 'video/wmv', 'video/webm'];
        const maxSize = 50 * 1024 * 1024; // 50MB
        
        if (!validTypes.includes(file.type)) {
            alert('Geçersiz dosya türü. Lütfen MP4, AVI, MOV, WMV veya WEBM dosyası seçin.');
            return false;
        }
        
        if (file.size > maxSize) {
            alert('Dosya boyutu çok büyük. Maksimum 50MB olmalıdır.');
            return false;
        }
        
        return true;
    }

    function showPreview(type, src) {
        const container = document.querySelector('.media-container');
        const caption = document.getElementById('preview-caption');
        const imagePreview = document.getElementById('imagePreview');
        const videoPreview = document.getElementById('videoPreview');
        const youtubePreview = document.getElementById('youtubePreview');
        
        // Hide all previews first
        imagePreview.style.display = 'none';
        videoPreview.style.display = 'none';
        youtubePreview.style.display = 'none';
        
        if (type === 'image') {
            imagePreview.src = src;
            imagePreview.style.display = 'block';
        } else if (type === 'video') {
            videoPreview.src = src;
            videoPreview.style.display = 'block';
        }
        
        container.style.display = 'block';
        caption.style.display = 'block';
        caption.textContent = 'Medya önizleme. Değişiklikleri kaydetmek için formu gönderin.';
    }

    // YouTube preview functionality
    const previewYoutubeBtn = document.getElementById('preview-youtube');
    const youtubeUrlInput = document.querySelector('input[name="Slider.YoutubeUrl"]');
    
    if (previewYoutubeBtn && youtubeUrlInput) {
        previewYoutubeBtn.addEventListener('click', function() {
            const url = youtubeUrlInput.value.trim();
            if (url && isValidYouTubeUrl(url)) {
                const embedUrl = convertToEmbedUrl(url);
                showYouTubePreview(embedUrl);
            } else {
                alert('Geçerli bir YouTube URL\'si girin.');
            }
        });
    }

    function isValidYouTubeUrl(url) {
        const youtubeRegex = /^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/)([a-zA-Z0-9_-]{11})/;
        return youtubeRegex.test(url);
    }

    function convertToEmbedUrl(url) {
        const videoId = extractVideoId(url);
        return `https://www.youtube.com/embed/${videoId}`;
    }

    function extractVideoId(url) {
        const match = url.match(/(?:youtube\.com\/watch\?v=|youtu\.be\/)([a-zA-Z0-9_-]{11})/);
        return match ? match[1] : null;
    }

    function showYouTubePreview(embedUrl) {
        const container = document.querySelector('.media-container');
        const caption = document.getElementById('preview-caption');
        const imagePreview = document.getElementById('imagePreview');
        const videoPreview = document.getElementById('videoPreview');
        const youtubePreview = document.getElementById('youtubePreview');
        
        // Hide other previews
        imagePreview.style.display = 'none';
        videoPreview.style.display = 'none';
        
        youtubePreview.src = embedUrl;
        youtubePreview.style.display = 'block';
        
        container.style.display = 'block';
        caption.style.display = 'block';
        caption.textContent = 'YouTube video önizleme. Değişiklikleri kaydetmek için formu gönderin.';
    }

    // Remove media functionality
    const removeMediaBtn = document.getElementById('removeMedia');
    if (removeMediaBtn) {
        removeMediaBtn.addEventListener('click', function() {
            clearAllInputs();
            hidePreview();
        });
    }

    function clearAllInputs() {
        clearImageInputs();
        clearVideoInputs();
    }

    function clearImageInputs() {
        const imageInput = document.getElementById('imageInput');
        if (imageInput) imageInput.value = '';
    }

    function clearVideoInputs() {
        clearVideoFileInput();
        clearYoutubeInput();
    }

    function clearVideoFileInput() {
        const videoInput = document.getElementById('videoInput');
        if (videoInput) videoInput.value = '';
    }

    function clearYoutubeInput() {
        const youtubeInput = document.querySelector('input[name="Slider.YoutubeUrl"]');
        if (youtubeInput) youtubeInput.value = '';
    }

    function hidePreview() {
        const container = document.querySelector('.media-container');
        const caption = document.getElementById('preview-caption');
        
        if (container) container.style.display = 'none';
        if (caption) caption.style.display = 'none';
        
        // Clear preview sources
        const imagePreview = document.getElementById('imagePreview');
        const videoPreview = document.getElementById('videoPreview');
        const youtubePreview = document.getElementById('youtubePreview');
        
        if (imagePreview) imagePreview.src = '';
        if (videoPreview) videoPreview.src = '';
        if (youtubePreview) youtubePreview.src = '';
    }
});
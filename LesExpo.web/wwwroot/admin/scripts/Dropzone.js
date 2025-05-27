// Setup drag and drop for Featured Image
const featuredDropzone = document.getElementById('image-dropzone');
const featuredImageInput = document.getElementById('featuredImageInput');
const featuredImagePreview = document.getElementById('featuredImagePreview');
const featuredPreviewContainer = document.querySelector('#image-preview-container .img-container');
const featuredPreviewCaption = document.getElementById('preview-caption');
const removeFeaturedImageBtn = document.getElementById('removeImage');

// Setup drag and drop for Card Image
const cardDropzone = document.getElementById('card-image-dropzone');
const cardImageInput = document.getElementById('cardImageInput');
const cardImagePreview = document.getElementById('cardImagePreview');
const cardPreviewContainer = document.querySelector('#card-image-preview-container .img-container');
const cardPreviewCaption = document.getElementById('card-preview-caption');
const removeCardImageBtn = document.getElementById('removeCardImage');



// Helper function to setup drag and drop events
function setupDropzone(dropzone, inputElement, previewElement, previewContainer, previewCaption, removeBtn, isCardImage = false) {
    if (!dropzone) return;

    // Handle drag events
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        dropzone.addEventListener(eventName, preventDefaults, false);
    });

    ['dragenter', 'dragover'].forEach(eventName => {
        dropzone.addEventListener(eventName, function() {
            this.classList.add('dragover');
        }, false);
    });

    ['dragleave', 'drop'].forEach(eventName => {
        dropzone.addEventListener(eventName, function() {
            this.classList.remove('dragover');
        }, false);
    });

    // Handle file drop
    dropzone.addEventListener('drop', function(e) {
        const dt = e.dataTransfer;
        const files = dt.files;
        
        if (files.length) {
            inputElement.files = files;
            handleFiles(files, previewElement, previewContainer, previewCaption, isCardImage);
        }
    }, false);

    // Handle file select via input
    dropzone.addEventListener('click', function() {
        inputElement.click();
    });

    inputElement.addEventListener('change', function() {
        if (this.files && this.files[0]) {
            handleFiles(this.files, previewElement, previewContainer, previewCaption, isCardImage);
        }
    });

    // Remove image button functionality
    if (removeBtn) {
        removeBtn.addEventListener('click', function() {
            previewElement.src = '';
            previewContainer.style.display = 'none';
            if (previewCaption) previewCaption.style.display = 'none';
            inputElement.value = '';
        });
    }
}

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

function handleFiles(files, previewElement, previewContainer, previewCaption, isCardImage) {
    const file = files[0];
    
    // Validate file type
    const fileType = file.type;
    const validImageTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
    
    if (!validImageTypes.includes(fileType)) {
        alert('Please select a valid image file (JPG, PNG, GIF, WEBP)');
        return;
    }
    
    // Validate file size (5MB)
    if (file.size > 5 * 1024 * 1024) {
        alert('Image size cannot exceed 5MB');
        return;
    }
    
    // Display preview
    const reader = new FileReader();
    reader.onload = function(e) {
        // Create an image element to check dimensions
        const img = new Image();
        img.onload = function() {
            // Validate dimensions for card images
            if (isCardImage) {
                if (img.width !== 300 || img.height !== 550) {
                    alert('Card image must be exactly 300×550 pixels. Current dimensions: ' + img.width + '×' + img.height);
                    return;
                }
            }
            
            // If validation passes or not a card image, show preview
            previewElement.src = e.target.result;
            previewContainer.style.display = 'block';
            if (previewCaption) previewCaption.style.display = 'block';
        };
        img.src = e.target.result;
    }
    reader.readAsDataURL(file);
}

// Initialize dropzones
document.addEventListener('DOMContentLoaded', function() {
    
    // Check if dropzones have the 'enabled' attribute set to 'true'
    const cardDropzoneEnabled = cardDropzone && cardDropzone.getAttribute('enabled') === 'true';
    const featuredDropzoneEnabled = featuredDropzone && featuredDropzone.getAttribute('enabled') === 'true';
    
    if (featuredDropzoneEnabled) {
        console.log('Featured dropzone is enabled');
        // Setup Featured Image dropzone
        setupDropzone(
            featuredDropzone, 
            featuredImageInput,
            featuredImagePreview,
            featuredPreviewContainer,
            featuredPreviewCaption,
            removeFeaturedImageBtn,
            false // Not a card image
        );
    }
    
    if (cardDropzoneEnabled) {
        console.log('Card dropzone is enabled');
        // Setup Card Image dropzone
        setupDropzone(
            cardDropzone,
            cardImageInput,
            cardImagePreview,
            cardPreviewContainer,
            cardPreviewCaption,
            removeCardImageBtn,
            true // Is a card image
        );
    }
});

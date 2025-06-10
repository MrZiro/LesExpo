/**
 * Advanced Image Dropzone Handler
 * Provides drag-and-drop file upload functionality with image validation and preview
 */
(function() {
    // Configuration options
    const config = {
        maxFileSize: 5 * 1024 * 1024, // 5MB
        validImageTypes: ['image/jpeg', 'image/png', 'image/gif', 'image/webp'],
        cardImageDimensions: { width: 850, height: 550 }
    };

    // DOM Elements cache
    const elements = {
        featured: {
            dropzone: document.getElementById('image-dropzone'),
            input: document.getElementById('featuredImageInput'),
            preview: document.getElementById('featuredImagePreview'),
            previewContainer: document.querySelector('#image-preview-container .img-container'),
            caption: document.getElementById('preview-caption'),
            removeBtn: document.getElementById('removeImage')
        },
        card: {
            dropzone: document.getElementById('card-image-dropzone'),
            input: document.getElementById('cardImageInput'),
            preview: document.getElementById('cardImagePreview'),
            previewContainer: document.querySelector('#card-image-preview-container .img-container'),
            caption: document.getElementById('card-preview-caption'),
            removeBtn: document.getElementById('removeCardImage')
        }
    };

    /**
     * Event handler utility functions
     */
    const eventHandlers = {
        preventDefaults(e) {
            e.preventDefault();
            e.stopPropagation();
        },
        
        addDragClass(e) {
            this.classList.add('dragover');
        },
        
        removeDragClass(e) {
            this.classList.remove('dragover');
        },
        
        handleDrop(e, inputElement, previewElement, previewContainer, previewCaption, isCardImage) {
            const dt = e.dataTransfer;
            const files = dt.files;
            
            if (files.length) {
                inputElement.files = files;
                fileHandlers.processFiles(files, previewElement, previewContainer, previewCaption, isCardImage);
            }
        },
        
        handleInputChange(e, previewElement, previewContainer, previewCaption, isCardImage) {
            if (this.files && this.files[0]) {
                fileHandlers.processFiles(this.files, previewElement, previewContainer, previewCaption, isCardImage);
            }
        },
        
        removeImage(e, inputElement, previewElement, previewContainer, previewCaption) {
            previewElement.src = '';
            previewContainer.style.display = 'none';
            if (previewCaption) previewCaption.style.display = 'none';
            inputElement.value = '';
            
            // Trigger change event on input for validation
            const event = new Event('change', { bubbles: true });
            inputElement.dispatchEvent(event);
            
            // Show feedback
            ui.showFeedback('Görsel kaldırıldı', 'success');
        }
    };

    /**
     * File processing and validation
     */
    const fileHandlers = {
        processFiles(files, previewElement, previewContainer, previewCaption, isCardImage) {
            const file = files[0];
            
            try {
                // Validate file type
                if (!this.validateFileType(file)) {
                    ui.showFeedback('Lütfen geçerli bir görsel dosyası seçin (JPG, PNG, GIF, WEBP)', 'error');
                    return;
                }
                
                // Validate file size
                if (!this.validateFileSize(file)) {
                    ui.showFeedback('Görsel boyutu 5MB\'ı geçemez', 'error');
                    return;
                }
                
                // Create preview
                this.createPreview(file, previewElement, previewContainer, previewCaption, isCardImage);
            } catch (error) {
                console.error('Dropzone error:', error);
                ui.showFeedback('Görsel işlenirken bir hata oluştu', 'error');
            }
        },
        
        validateFileType(file) {
            return config.validImageTypes.includes(file.type);
        },
        
        validateFileSize(file) {
            return file.size <= config.maxFileSize;
        },
        
        createPreview(file, previewElement, previewContainer, previewCaption, isCardImage) {
            const reader = new FileReader();
            
            reader.onload = function(e) {
                // Create an image element to check dimensions
                const img = new Image();
                
                img.onload = function() {
                    // Validate dimensions for card images
                    if (isCardImage) {
                        if (img.width > config.cardImageDimensions.width || 
                            img.height > config.cardImageDimensions.height) {
                            const message = `Kart görseli ${config.cardImageDimensions.width}×${config.cardImageDimensions.height} piksel veya daha küçük olmalıdır. Mevcut boyut: ${img.width}×${img.height}`;
                            ui.showFeedback(message, 'error');
                            return;
                        }
                    }
                    
                    // If validation passes or not a card image, show preview
                    previewElement.src = e.target.result;
                    previewContainer.style.display = 'block';
                    if (previewCaption) previewCaption.style.display = 'block';
                    
                    // Show feedback
                    ui.showFeedback('Görsel başarıyla yüklendi', 'success');
                };
                
                img.onerror = function() {
                    ui.showFeedback('Görsel yüklenirken bir hata oluştu', 'error');
                };
                
                img.src = e.target.result;
            };
            
            reader.onerror = function() {
                ui.showFeedback('Dosya okunamadı', 'error');
            };
            
            reader.readAsDataURL(file);
        }
    };

    /**
     * UI utilities
     */
    const ui = {
        showFeedback(message, type = 'info') {
            // Create feedback element if it doesn't exist
            let feedbackEl = document.getElementById('dropzone-feedback');
            
            if (!feedbackEl) {
                feedbackEl = document.createElement('div');
                feedbackEl.id = 'dropzone-feedback';
                feedbackEl.style.position = 'fixed';
                feedbackEl.style.bottom = '20px';
                feedbackEl.style.right = '20px';
                feedbackEl.style.padding = '10px 15px';
                feedbackEl.style.borderRadius = '4px';
                feedbackEl.style.boxShadow = '0 2px 10px rgba(0,0,0,0.1)';
                feedbackEl.style.zIndex = '9999';
                feedbackEl.style.transition = 'all 0.3s ease';
                feedbackEl.style.opacity = '0';
                document.body.appendChild(feedbackEl);
            }
            
            // Set styles based on type
            switch (type) {
                case 'error':
                    feedbackEl.style.backgroundColor = '#f8d7da';
                    feedbackEl.style.color = '#721c24';
                    feedbackEl.style.borderLeft = '4px solid #dc3545';
                    break;
                case 'success':
                    feedbackEl.style.backgroundColor = '#d4edda';
                    feedbackEl.style.color = '#155724';
                    feedbackEl.style.borderLeft = '4px solid #28a745';
                    break;
                default:
                    feedbackEl.style.backgroundColor = '#e2e3e5';
                    feedbackEl.style.color = '#383d41';
                    feedbackEl.style.borderLeft = '4px solid #6c757d';
            }
            
            // Set message and show
            feedbackEl.textContent = message;
            feedbackEl.style.opacity = '1';
            
            // Hide after 3 seconds
            setTimeout(() => {
                feedbackEl.style.opacity = '0';
                setTimeout(() => {
                    if (feedbackEl.parentNode) {
                        document.body.removeChild(feedbackEl);
                    }
                }, 300);
            }, 3000);
        }
    };

    /**
     * Setup dropzone with all event listeners
     */
    function setupDropzone(dropzone, inputElement, previewElement, previewContainer, previewCaption, removeBtn, isCardImage = false) {
        if (!dropzone) return;

        // Handle drag events
        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
            dropzone.addEventListener(eventName, eventHandlers.preventDefaults, false);
        });

        ['dragenter', 'dragover'].forEach(eventName => {
            dropzone.addEventListener(eventName, eventHandlers.addDragClass, false);
        });

        ['dragleave', 'drop'].forEach(eventName => {
            dropzone.addEventListener(eventName, eventHandlers.removeDragClass, false);
        });

        // Handle file drop
        dropzone.addEventListener('drop', function(e) {
            eventHandlers.handleDrop.call(this, e, inputElement, previewElement, previewContainer, previewCaption, isCardImage);
        }, false);

        // Handle file select via input
        dropzone.addEventListener('click', function() {
            inputElement.click();
        });

        inputElement.addEventListener('change', function(e) {
            eventHandlers.handleInputChange.call(this, e, previewElement, previewContainer, previewCaption, isCardImage);
        });

        // Remove image button functionality
        if (removeBtn) {
            removeBtn.addEventListener('click', function(e) {
                eventHandlers.removeImage.call(this, e, inputElement, previewElement, previewContainer, previewCaption);
            });
        }
    }

    /**
     * Initialize dropzones on page load
     */
    document.addEventListener('DOMContentLoaded', function() {
        // Check if dropzones have the 'enabled' attribute set to 'true'
        const featuredEnabled = elements.featured.dropzone && elements.featured.dropzone.getAttribute('enabled') === 'true';
        const cardEnabled = elements.card.dropzone && elements.card.dropzone.getAttribute('enabled') === 'true';
        
        if (featuredEnabled) {
            console.log('Featured dropzone initialized');
            setupDropzone(
                elements.featured.dropzone, 
                elements.featured.input,
                elements.featured.preview,
                elements.featured.previewContainer,
                elements.featured.caption,
                elements.featured.removeBtn,
                false // Not a card image
            );
        }
        
        if (cardEnabled) {
            console.log('Card dropzone initialized');
            setupDropzone(
                elements.card.dropzone,
                elements.card.input,
                elements.card.preview,
                elements.card.previewContainer,
                elements.card.caption,
                elements.card.removeBtn,
                true // Is a card image
            );
        }

        // Initialize any existing previews
        if (featuredEnabled && elements.featured.preview && elements.featured.preview.src) {
            elements.featured.previewContainer.style.display = 'block';
            if (elements.featured.caption) elements.featured.caption.style.display = 'block';
        }
        
        if (cardEnabled && elements.card.preview && elements.card.preview.src) {
            elements.card.previewContainer.style.display = 'block';
            if (elements.card.caption) elements.card.caption.style.display = 'block';
        }
    });
})();

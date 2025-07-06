// Car Details Page JavaScript
document.addEventListener('DOMContentLoaded', function () {
    initializeCarDetails();
});

function initializeCarDetails() {
    initializeTooltips();
    initializeThumbnailEffects();
    initializeButtonEffects();
    initializeImageGallery();
    initializeAccessibility();
    initializeColorSwatches();
    initializeImageLazyLoading();
    initializeTabHandling();
    initializeResponsiveFeatures();
}

/**
 * Initialize Bootstrap tooltips
 */
function initializeTooltips() {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

/**
 * Initialize thumbnail image effects and interactions
 */
function initializeThumbnailEffects() {
    const thumbnails = document.querySelectorAll('.thumbnail-img');

    thumbnails.forEach(thumbnail => {
        // Add hover effect for better UX
        thumbnail.addEventListener('mouseenter', function () {
            this.style.transform = 'scale(1.05)';
            this.style.boxShadow = '0 0.25rem 0.5rem rgba(0, 0, 0, 0.15)';
        });

        thumbnail.addEventListener('mouseleave', function () {
            this.style.transform = 'scale(1)';
            this.style.boxShadow = '0 0.125rem 0.25rem rgba(0, 0, 0, 0.1)';
        });

        // Add loading state handling
        thumbnail.addEventListener('load', function () {
            this.style.opacity = '1';
        });

        thumbnail.addEventListener('error', function () {
            this.src = '/images/placeholder.jpg';
            this.alt = 'Image not available';
        });
    });
}

/**
 * Initialize button effects and interactions
 */
function initializeButtonEffects() {
    const buttons = document.querySelectorAll('.btn');

    buttons.forEach(button => {
        button.addEventListener('click', function () {
            // Add click animation
            this.style.transform = 'scale(0.98)';
            setTimeout(() => {
                this.style.transform = 'scale(1)';
            }, 150);
        });
    });
}

/**
 * Initialize image gallery functionality
 */
function initializeImageGallery() {
    const mainImage = document.getElementById('mainImage');
    if (!mainImage) return;

    // Add smooth transitions
    mainImage.style.transition = 'opacity 0.3s ease, transform 0.3s ease';

    // Add zoom effect on hover
    mainImage.addEventListener('mouseenter', function () {
        this.style.transform = 'scale(1.02)';
    });

    mainImage.addEventListener('mouseleave', function () {
        this.style.transform = 'scale(1)';
    });

    // Handle image loading states
    mainImage.addEventListener('load', function () {
        this.style.opacity = '1';
    });

    mainImage.addEventListener('error', function () {
        this.src = '/images/placeholder.jpg';
        this.alt = 'Image not available';
    });
}

/**
 * Swap main image when thumbnail is clicked
 */
function swapImage(thumbnail) {
    const mainImage = document.getElementById('mainImage');
    if (!mainImage || !thumbnail) return;

    // Add loading state
    mainImage.style.opacity = '0.5';

    // Create new image to preload
    const newImage = new Image();
    newImage.onload = function () {
        mainImage.src = thumbnail.src;
        mainImage.alt = thumbnail.alt;
        mainImage.style.opacity = '1';

        // Update active thumbnail indicator
        updateActiveThumbnail(thumbnail);
    };

    newImage.onerror = function () {
        mainImage.style.opacity = '1';
        console.error('Failed to load image:', thumbnail.src);
    };

    newImage.src = thumbnail.src;
}

/**
 * Handle thumbnail keyboard navigation
 */
function handleThumbnailKeydown(event, thumbnail) {
    if (event.key === 'Enter' || event.key === ' ') {
        event.preventDefault();
        swapImage(thumbnail);
    }

    if (event.key === 'ArrowLeft' || event.key === 'ArrowRight') {
        event.preventDefault();
        navigateThumbnails(event.key, thumbnail);
    }
}

/**
 * Navigate through thumbnails with keyboard
 */
function navigateThumbnails(direction, currentThumbnail) {
    const thumbnails = Array.from(document.querySelectorAll('.thumbnail-img'));
    const currentIndex = thumbnails.indexOf(currentThumbnail);

    let nextIndex;
    if (direction === 'ArrowLeft') {
        nextIndex = currentIndex > 0 ? currentIndex - 1 : thumbnails.length - 1;
    } else {
        nextIndex = currentIndex < thumbnails.length - 1 ? currentIndex + 1 : 0;
    }

    thumbnails[nextIndex].focus();
}

/**
 * Update active thumbnail indicator
 */
function updateActiveThumbnail(activeThumbnail) {
    // Remove active class from all thumbnails
    document.querySelectorAll('.thumbnail-img').forEach(thumb => {
        thumb.classList.remove('active');
        thumb.style.outline = 'none';
    });

    // Add active class to current thumbnail
    activeThumbnail.classList.add('active');
    activeThumbnail.style.outline = '2px solid var(--bs-primary)';
}

/**
 * Initialize accessibility features
 */
function initializeAccessibility() {
    // Add ARIA labels to interactive elements
    const thumbnails = document.querySelectorAll('.thumbnail-img');
    thumbnails.forEach((thumbnail, index) => {
        thumbnail.setAttribute('aria-label', `View image ${index + 1}`);
        thumbnail.setAttribute('role', 'button');
    });

    // Add keyboard navigation hints
    const mainImage = document.getElementById('mainImage');
    if (mainImage) {
        mainImage.setAttribute('aria-describedby', 'image-instructions');

        // Create instructions element if it doesn't exist
        if (!document.getElementById('image-instructions')) {
            const instructions = document.createElement('div');
            instructions.id = 'image-instructions';
            instructions.className = 'sr-only';
            instructions.textContent = 'Click on thumbnail images to view them in the main display. Use arrow keys to navigate between thumbnails.';
            mainImage.parentNode.appendChild(instructions);
        }
    }

    // Improve tab navigation for info items
    const infoItems = document.querySelectorAll('.info-item, .technical-detail, .specification-item');
    infoItems.forEach(item => {
        item.setAttribute('tabindex', '0');
        item.setAttribute('role', 'listitem');
    });
}

/**
 * Initialize color swatch interactions
 */
function initializeColorSwatches() {
    const colorSwatches = document.querySelectorAll('.color-swatch');

    colorSwatches.forEach(swatch => {
        // Add hover effect
        swatch.addEventListener('mouseenter', function () {
            this.style.transform = 'scale(1.2)';
            this.style.zIndex = '10';
        });

        swatch.addEventListener('mouseleave', function () {
            this.style.transform = 'scale(1)';
            this.style.zIndex = '1';
        });

        // Add click handler for color selection
        swatch.addEventListener('click', function () {
            const colorValue = this.style.backgroundColor;
            const colorName = this.getAttribute('title');

            // Trigger color selection event
            const colorEvent = new CustomEvent('colorSelected', {
                detail: { color: colorValue, name: colorName }
            });
            document.dispatchEvent(colorEvent);
        });
    });
}

/**
 * Initialize lazy loading for images
 */
function initializeImageLazyLoading() {
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src || img.src;
                    img.classList.remove('lazy');
                    observer.unobserve(img);
                }
            });
        });

        document.querySelectorAll('img[loading="lazy"]').forEach(img => {
            imageObserver.observe(img);
        });
    }
}

/**
 * Initialize tab handling with smooth transitions
 */
function initializeTabHandling() {
    const tabButtons = document.querySelectorAll('[data-bs-toggle="tab"]');

    tabButtons.forEach(button => {
        button.addEventListener('shown.bs.tab', function (event) {
            // Add smooth transition effect
            const targetPane = document.querySelector(event.target.getAttribute('data-bs-target'));
            if (targetPane) {
                targetPane.style.opacity = '0';
                targetPane.style.transform = 'translateY(10px)';

                setTimeout(() => {
                    targetPane.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
                    targetPane.style.opacity = '1';
                    targetPane.style.transform = 'translateY(0)';
                }, 50);
            }
        });
    });
}

/**
 * Initialize responsive features
 */
function initializeResponsiveFeatures() {
    // Handle window resize
    window.addEventListener('resize', debounce(() => {
        adjustLayoutForScreenSize();
    }, 250));

    // Initial layout adjustment
    adjustLayoutForScreenSize();
}

/**
 * Adjust layout based on screen size
 */
function adjustLayoutForScreenSize() {
    const isMobile = window.innerWidth <= 768;
    const sidebarElements = document.querySelectorAll('.car-info-sidebar, .image-gallery-section');

    sidebarElements.forEach(element => {
        if (isMobile) {
            element.style.position = 'static';
            element.style.top = 'auto';
        } else {
            element.style.position = 'sticky';
            element.style.top = '20px';
        }
    });
}

/**
 * Debounce function for performance optimization
 */
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

/**
 * Initialize image gallery with keyboard navigation
 */
function initializeImageGalleryNavigation() {
    document.addEventListener('keydown', function (event) {
        if (event.ctrlKey || event.metaKey) return;

        const activeTab = document.querySelector('.tab-pane.active');
        if (!activeTab) return;

        const thumbnails = Array.from(activeTab.querySelectorAll('.thumbnail-img'));
        const currentFocused = document.activeElement;

        if (thumbnails.includes(currentFocused)) {
            switch (event.key) {
                case 'ArrowLeft':
                case 'ArrowRight':
                    event.preventDefault();
                    navigateThumbnails(event.key, currentFocused);
                    break;
                case 'Home':
                    event.preventDefault();
                    thumbnails[0].focus();
                    break;
                case 'End':
                    event.preventDefault();
                    thumbnails[thumbnails.length - 1].focus();
                    break;
            }
        }
    });
}

/**
 * Handle smooth scrolling for navigation
 */
function initializeSmoothScrolling() {
    const backButton = document.querySelector('.back-button');
    if (backButton) {
        backButton.addEventListener('click', function (event) {
            // Add smooth page transition effect
            document.body.style.transition = 'opacity 0.3s ease';
            document.body.style.opacity = '0.8';

            setTimeout(() => {
                document.body.style.opacity = '1';
            }, 300);
        });
    }
}

/**
 * Initialize advanced features
 */
function initializeAdvancedFeatures() {
    initializeImageGalleryNavigation();
    initializeSmoothScrolling();
    initializeAnimationsOnScroll();
}

/**
 * Initialize animations on scroll
 */
function initializeAnimationsOnScroll() {
    if ('IntersectionObserver' in window) {
        const animationObserver = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.style.animation = 'fadeInUp 0.6s ease forwards';
                }
            });
        }, {
            threshold: 0.1
        });

        document.querySelectorAll('.info-item, .technical-detail, .specification-item').forEach(item => {
            animationObserver.observe(item);
        });
    }
}

/**
 * Add CSS animations
 */
function addCSSAnimations() {
    const style = document.createElement('style');
    style.textContent = `
        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
        
        .lazy {
            opacity: 0;
            transition: opacity 0.3s;
        }
        
        .tab-pane {
            transition: opacity 0.3s ease, transform 0.3s ease;
        }
    `;
    document.head.appendChild(style);
}

// Initialize advanced features and animations
document.addEventListener('DOMContentLoaded', function () {
    addCSSAnimations();
    initializeAdvancedFeatures();
});

// Export functions for potential use in other modules
window.CarDetailsJS = {
    swapImage,
    handleThumbnailKeydown,
    initializeCarDetails
};
document.addEventListener("DOMContentLoaded", function () {
    // Initialize the page with the current culture
    applyCultureSettings();

    // Add event listeners to language selection buttons
    document.querySelectorAll('#form-en button, #form-ar button').forEach(button => {
        button.addEventListener('click', function (e) {
            // The form submission will handle the POST request
            // We just need to do any client-side preparation here if needed
            console.log("Language button clicked: " + this.closest('form').querySelector('input[name="culture"]').value);
        });
    });
});

function applyCultureSettings() {
    // Get current culture from cookies
    const cultureCookie = getCookie('userLanguage') || 'en';
    console.log("Current culture from cookie:", cultureCookie);

    // Apply appropriate direction
    const isRtl = cultureCookie === 'ar';
    document.documentElement.dir = isRtl ? 'rtl' : 'ltr';

    // Apply direction-specific stylesheet
    const rtlStylesheet = document.getElementById('rtl-stylesheet');
    const rtlCoreStylesheet = document.getElementById('rtl-core-stylesheet');
    const ltrStylesheet = document.getElementById('ltr-stylesheet');

    if (isRtl) {
        if (rtlStylesheet) rtlStylesheet.disabled = false;
        if (rtlCoreStylesheet) rtlCoreStylesheet.disabled = false;
        if (ltrStylesheet) ltrStylesheet.disabled = true;
    } else {
        if (rtlStylesheet) rtlStylesheet.disabled = true;
        if (rtlCoreStylesheet) rtlCoreStylesheet.disabled = true;
        if (ltrStylesheet) ltrStylesheet.disabled = false;
    }

    // Apply direction-specific layout adjustments
    applyDirectionLayout(isRtl);
}

function applyDirectionLayout(isRtl) {
    const leftSection = document.getElementById("left-section");
    const rightSection = document.getElementById("right-section");

    if (leftSection && rightSection) {
        if (isRtl) {
            // RTL adjustments
            leftSection.classList.remove("me-auto");
            rightSection.classList.remove("ms-auto");
            rightSection.classList.add("me-auto");

            // Convert margin classes
            document.querySelectorAll('.me-3:not(.converted-rtl)').forEach(el => {
                el.classList.remove('me-3');
                el.classList.add('ms-3', 'converted-rtl');
            });
        } else {
            // LTR adjustments
            leftSection.classList.remove("ms-auto");
            rightSection.classList.add("ms-auto");
            rightSection.classList.remove("me-auto");

            // Convert margin classes back
            document.querySelectorAll('.ms-3.converted-rtl').forEach(el => {
                el.classList.remove('ms-3', 'converted-rtl');
                el.classList.add('me-3');
            });
        }
    }
}

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null;
}

// For compatibility with existing code
function setLanguageCookie(culture) {
    // This function is called from the layout but we don't need to do anything
    // since the form submission will handle setting the cookie via the server
    console.log("Language selection: " + culture);
}

// Full screen toggle function (moved from inline)
function toggleFullScreen() {
    if (!document.fullscreenElement) {
        document.documentElement.requestFullscreen();
    } else {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        }
    }
}
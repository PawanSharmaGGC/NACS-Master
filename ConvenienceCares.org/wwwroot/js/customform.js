
$(document).ready(function () {

    // Helper function to display messages
    function displayMessage(elementId, message, isError = false) {
        const element = document.getElementById(elementId);
        element.style.display = 'inline-block';

        if (isError) {
            element.textContent = message;
        }
    }

    // Generic form submission handler
    function handleFormSubmission(formId, url, hasRecaptcha = false) {
        $(formId).submit(function (e) {
            document.getElementById('thank-you-message').style.display = 'none';
            document.getElementById('error-message').style.display = 'none';

            e.preventDefault();

            const form = $(this);
            const formData = form.serialize();
            let recaptchaResponse = '';
            if (hasRecaptcha) {
                recaptchaResponse = grecaptcha.getResponse();
                if (recaptchaResponse.length === 0) {
                    displayMessage('error-message', 'Please complete the reCAPTCHA.', true);
                    return;
                }
            }

            const data = hasRecaptcha ? formData + '&RecaptchaResponse=' + encodeURIComponent(recaptchaResponse) : formData;

            $.ajax({
                type: 'POST',
                url: url,
                data: data,
                success: function (response) {
                    if (response.success) {
                        document.getElementById('FormPanel').style.display = 'none';
                        displayMessage('thank-you-message', 'Thank you for your submission!');
                    } else {
                        displayMessage('error-message', 'Form submission failed. ' + response.message, true);
                    }
                },
                error: function () {
                    displayMessage('error-message', 'The entered values cannot be saved. Please see the fields below for details.', true);
                }
            });
        });
    }

    // Initialize form handlers
    handleFormSubmission('#contact-form', '/ContactForm/SaveData', true);
    handleFormSubmission('#support-form', '/GetInvolvedForm/SaveData');
    handleFormSubmission('#sponsor-nacs-form', '/SponsorNACSForm/SaveData');
    handleFormSubmission('#corporate-involvement-form', '/CorporateInvolvementForm/SaveData', true);
    handleFormSubmission('#nacs-24-7day-form', '/NACS24by7DayForm/SaveData');
    handleFormSubmission('#awareness-campaign-form', '/AwarenessCampaignForm/SaveData', true);
    handleFormSubmission('#future-form', '/ScholarshipUpdatesForm/UpdateData');
});


$(document).ready(function () {

    // Helper function to display messages
    function displayMessage(elementId, message, isError = false) {
        const element = document.getElementById(elementId);
        element.style.display = 'inline-block';
        if (isError) {
            element.innerHTML = message.replace(/\n/g,'<br>');
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
    handleFormSubmission('#alumni-2023-nacs-show-event-rsvp-form', '/Alumni2023NACSShowEventRSVPForm/SaveData', true);
    handleFormSubmission('#nacs-show-registration-alert-form', '/NACSShowRegistrationAlertForm/SaveData', true);
    handleFormSubmission('#nacs-show-updates-form', '/NACSShowUpdatesForm/SaveData', true);
    handleFormSubmission('#nacs-show-notify-me-form', '/NACSShowNotifyMeForm/SaveData', true);
    handleFormSubmission('#ns-virtual-experience-alert-form', '/NSVirtualExperienceAlertForm/SaveData', true);
    handleFormSubmission('#exhibitor-support-team-questions-form', '/ExhibitorSupportTeamQuestionsForm/SaveData', true);
    handleFormSubmission('#mobile-app-notification-form', '/MobileAppNotificationForm/SaveData', true);

});

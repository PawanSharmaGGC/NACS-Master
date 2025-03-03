$(document).ready(function () {

    var formData = {};

    function loadFormData() {
        // Loop through all input fields and update formData object
        $('input').each(function () {
            var inputId = $(this).attr('id');
            var inputValue = $(this).val();
            formData[inputId] = inputValue;
        });

        // Loop through all labels with class "calc-number" and update formData object
        $('label.calc-number').each(function () {
            var labelId = $(this).attr('id');
            var labelValue = $(this).text();
            formData[labelId] = labelValue;
        });
    }

    // Populate formData object on input change
    $('#goodJobStepsContainer').on('change', 'input', function () {
        // debugger;
        var inputId = $(this).attr('id');
        var inputValue = $(this).val();
        formData[inputId] = inputValue;

        if (!this.checkValidity()) {
            $('#Req' + inputId).show();
        } else {
            $('#Req' + inputId).hide();
        }

    });


    function validateFormFields(containerSelector) {
        var isValid = true;

        $(containerSelector + ' input').each(function () {

            if (!this.checkValidity()) {
                isValid = false;

                var fieldId = $(this).attr('id');
                $('#Req' + fieldId).show();
            } else {

                var fieldId = $(this).attr('id');
                $('#Req' + fieldId).hide();
            }
        });

        return isValid;
    }


    // Handle AJAX button clicks for next and previous steps
    $(document).on('click', '#nextStep, #prevStep', function () {


        var isValid = true;
        if ($(this).attr('id') === 'nextStep') {
            isValid = validateFormFields('.validate-inputs');  // Validate the fields in the steps container
        }


        if (isValid) {

            var stepValue = $(this).data('step');
            var currentStep = $('#hfCurrentStep').val();

            loadFormData();

            // Capture all step fields
            var model = {
                CurrentStep: currentStep,

                // Step 1
                Field0R: formData['Field0R'] || '',
                Field1R: formData['Field1R'] || '',
                Field3R: formData['Field3R'] || '',
                Field4R: formData['Field4R'] || '',
                Field5R: formData['Field5R'] || '',
                Field6R: formData['Field6R'] || '',

                // Step 2
                Field0C: formData['Field0C'] || '',
                Field7C: formData['Field7C'] || '',
                Field2C: formData['Field2C'] || '',
                Field3C: formData['Field3C'] || '',
                Field4C: formData['Field4C'] || '',
                Field5C: formData['Field5C'] || '',
                Field6C: formData['Field6C'] || '',
                Field11C: formData['Field11C'] || '',
                Field8C: formData['Field8C'] || '',
                Field9C: formData['Field9C'] || '',
                Field10C: formData['Field10C'] || '',

                // Step 3

                // Hidden fields
                Text0: formData['Text0'] || '',
                Text1: formData['Text1'] || '',
                Text2: formData['Text2'] || '',
                Text3: formData['Text3'] || '',
                Text4: formData['Text4'] || '',
                Text5: formData['Text5'] || '',
                Text6: formData['Text6'] || '',
                Text7: formData['Text7'] || '',
                Text8: formData['Text8'] || '',
                Text9: formData['Text9'] || '',
                Text10: formData['Text10'] || '',
                Text11: formData['Text11'] || '',
                Text12: formData['Text12'] || '',
                Text13: formData['Text13'] || '',
                Text14: formData['Text14'] || '',
                Text15: formData['Text15'] || '',
                Text16: formData['Text16'] || '',
                Text17: formData['Text17'] || '',
                Text18: formData['Text18'] || '',

                CaField111: formData['CaField111'] || '',
                CaField2: formData['CaField2'] || '',
                Slider1: formData['Slider1'] || '',
                CaField33: formData['CaField33'] || '',
                CaField4: formData['CaField4'] || '',
                Slider2: formData['Slider2'] || '',
                CaField6: formData['CaField6'] || '',
                CaField8: formData['CaField8'] || '',
                Slider4: formData['Slider4'] || '',
                CaField999: formData['CaField999'] || '',
                CaField10: formData['CaField10'] || '',
                Slider5: formData['Slider5'] || '',
                CaField1111: formData['CaField1111'] || '',
                CaField12: formData['CaField12'] || '',
                Slider6: formData['Slider6'] || '',
                CaField14: formData['CaField14'] || '',
                CaField15: formData['CaField15'] || '',
                CaField16: formData['CaField16'] || '',
                CaField17: formData['CaField17'] || '',
                ReField0: formData['ReField0'] || '',
                ReField00: formData['ReField00'] || '',
                ReField1: formData['ReField1'] || '',
                ReField11: formData['ReField11'] || '',
                ReField2: formData['ReField2'] || '',
                ReField22: formData['ReField22'] || '',
                ReField3: formData['ReField3'] || '',
                ReField33: formData['ReField33'] || '',
                ReField4: formData['ReField4'] || '',
                ReField44: formData['ReField44'] || '',
                ReField5: formData['ReField5'] || '',
                ReField55: formData['ReField55'] || '',
                ReField6: formData['ReField6'] || '',
                ReField66: formData['ReField66'] || '',

                CaField1: formData['CaField1'] || '',
                CaField3: formData['CaField3'] || '',
                CaField7: formData['CaField7'] || '',
                CaField9: formData['CaField9'] || '',
                CaField11: formData['CaField11'] || '',
            };


            var container = $('#goodJobStepsContainer');


            var actionUrl = $(this).attr('id') === 'nextStep' ?
                '/GoodJobCalculator/NextStep/' :
                '/GoodJobCalculator/PreviousStep/';

            var $thisButton = $(this);
            $thisButton.prop('disabled', true); // Disable the button to prevent multiple clicks

            $.ajax({
                url: actionUrl,
                method: 'POST',
                data: { step: stepValue, model: model },
                success: function (response) {
                    container.empty();
                    container.html(response); // Update content

                    // hide data on last step
                    if (stepValue > 3) {
                        $('#_step').css("display", "none");
                        $('#_divHowToUse').css("display", "none");
                        $('#_divStepsHeader').css("display", "none");
                    }
                    else {
                        $('#_step').css("display", "block");
                        $('#_divHowToUse').css("display", "block");
                        $('#_divStepsHeader').css("display", "block");
                    }

                },
                error: function (xhr, status, error) {
                    console.error('AJAX request failed:', error);
                },
                complete: function () {
                    $thisButton.prop('disabled', false); // Re-enable the button once the request is complete
                }
            });
        } else {
        }
    });
});
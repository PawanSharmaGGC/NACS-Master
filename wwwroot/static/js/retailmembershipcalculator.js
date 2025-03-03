$(document).ready(function () {

    $('#retailMembershipDuesCalculator').on('change', 'input', function () {
        var inputId = $(this).attr('id');

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

    $('#calculateButton').on('click', function () {
        var customerNumber = $('#customerNumber').val();

        var isValid = validateFormFields('.validate-inputs');  // Validate the fields in the steps container

        if (isValid) {
            $.ajax({
                url: '/retailmembershipcalculator/calculateretailmembershipdues/' + customerNumber,
                type: 'Get',
                //data: { customerNumber: customerNumber },
                success: function (response) {
                    if (response.success) {
                        $('#resultDisplay').html('Your NACS Dues Estimate is ' + response.totalDues + '');
                    }
                    else {
                        $('#resultDisplay').html('<span class="text-danger">Error calculating dues. Please try again.</span>');
                    }
                },
                error: function () {
                    $('#resultDisplay').html('<span class="text-danger">Error calculating dues. Please try again.</span>');
                }
            });
        }
    });

    $('input._txtNumber').keyup(function (event) {
        if (event.which >= 37 && event.which <= 40) return;
        $(this).val(function (index, value) {
            return value
                // Keep only digits, decimal points, and dashes at the start of the string:
                .replace(/[^\d.-]|(?!^)-/g, "")
                // Remove duplicated decimal points, if they exist:
                .replace(/^([^.]*\.)(.*$)/, (_, g1, g2) => g1 + g2.replace(/\./g, ''))
                // Keep only two digits past the decimal point:
                .replace(/\.(\d{2})\d+/, '.$1')
                // Add thousands separators:
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        });
    });

});

function CalculateDues() {
    var CustomerNumber = $('#customerNumber').val();
    var $Result = $('#resultDisplay');

    if ((CustomerNumber != null) || (CustomerNumber.toString() != "")) {
        if ((CustomerNumber >= 0) && (CustomerNumber <= 5999999.99)) {

            var BaseDues250 = 250.00;
            var Escalator0 = 0;

            var BasePlus = (parseFloat(Escalator0) * (parseFloat(CustomerNumber) - parseFloat(0))) / 1000000.00;
            var totalDues = (parseFloat(BaseDues250) + parseFloat(BasePlus));
            $Result.val(parseFloat(totalDues).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }

        if ((parseFloat(CustomerNumber) >= 6000000.00) && (parseFloat(CustomerNumber) <= 24999999.99)) {

            var BaseDues250 = 250.00;
            var Escalator14 = 14;

            var BasePlus = (parseFloat(Escalator14).toFixed(2) * (parseFloat(CustomerNumber).toFixed(2) - parseFloat(6000000).toFixed(2))) / 1000000.00;
            var totalDues = (parseFloat(BaseDues250) + parseFloat(BasePlus));
            $Result.val(parseFloat(totalDues).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }

        if ((parseFloat(CustomerNumber) >= 25000000.00) && (parseFloat(CustomerNumber) <= 99999999.99)) {

            var BaseDues500 = 550.00;
            var Escalator13 = 13;

            var BasePlus = (parseFloat(Escalator13) * (parseFloat(CustomerNumber) - parseFloat(25000000.00))) / 1000000.00;
            var totalDues = (parseFloat(BaseDues500) + parseFloat(BasePlus));
            $Result.val(parseFloat(totalDues).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }

        if ((parseFloat(CustomerNumber) >= 100000000.00) && (parseFloat(CustomerNumber) <= 2469999999.99)) {

            var BaseDues1550 = 1550.00;
            var Escalator12 = 12;

            var BasePlus = (parseFloat(Escalator12) * ((parseFloat(CustomerNumber)) - (parseFloat(100000000.00)))) / 1000000.00;
            var totalDues = ((parseFloat(BaseDues1550)) + (parseFloat(BasePlus)));
            $Result.val(parseFloat(totalDues).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
        }

        if (parseFloat(CustomerNumber) >= 2470000000.00) {

            $Result.val(parseFloat(30000.00).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
        }
    }
}


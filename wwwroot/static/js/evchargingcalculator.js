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
    $('#evChargingStepsContainer').on('change', 'input', function () {

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
    $(document).on('click', '#nextStep, #prevStep, #exportPDF', function () {

        var isValid = validateFormFields('.validate-inputs');  // Validate the fields in the steps container

        if (isValid) {

            var stepValue = $(this).data('step');
            var currentStep = $('#hfCurrentStep').val();

            loadFormData();



            // Capture all step fields
            var model = {

                CurrentStep: currentStep,

                _txtNumberOfCharger: formData['_txtNumberOfCharger'] || '',
                Text0: formData['Text0'] || '',

                _txtNumberChargePerChargerPerDay: formData['_txtNumberChargePerChargerPerDay'] || '',
                Text1: formData['Text1'] || '',

                _txtDaysMonth: formData['_txtDaysMonth'] || '',
                Text2: formData['Text2'] || '',

                _txtkWCharger: formData['_txtkWCharger'] || '',
                Text3: formData['Text3'] || '',

                _txtBaseCStoreEnergyCosts: formData['_txtBaseCStoreEnergyCosts'] || '',
                Text4: formData['Text4'] || '',

                _txtBaseCStoreDemandCharges: formData['_txtBaseCStoreDemandCharges'] || '',
                Text5: formData['Text5'] || '',

                _txtTotalBaseCStoreUtilityCosts: formData['_txtTotalBaseCStoreUtilityCosts'] || '',
                Text6: formData['Text6'] || '',

                _txtEnergyCostkwh: formData['_txtEnergyCostkwh'] || '',
                Text7: formData['Text7'] || '',

                _txtDemandCostkW: formData['_txtDemandCostkW'] || '',
                Text8: formData['Text8'] || '',

                _txtCapitalCostOfWHCharger: formData['_txtCapitalCostOfWHCharger'] || '',
                Text9: formData['Text9'] || '',

                _txtHurdleRateROI: formData['_txtHurdleRateROI'] || '',
                Text10: formData['Text10'] || '',

                Text11: formData['Text11'] || '',
                Text12: formData['Text12'] || '',

                _txtEnergyCharges: formData['_txtEnergyCharges'] || '',
                Text13: formData['Text13'] || '',

                _txtTotalDemandkW: formData['_txtTotalDemandkW'] || '',
                Text14: formData['Text14'] || '',

                _txtkWhCharge: formData['_txtkWhCharge'] || '',
                Text15: formData['Text15'] || '',

                _txtBasekW: formData['_txtBasekW'] || '',
                Text16: formData['Text16'] || '',

                _txtCostPercharges: formData['_txtCostPercharges'] || '',
                Text17: formData['Text17'] || '',
                Text18: formData['Text18'] || '',
                Text19: formData['Text19'] || '',
                Text20: formData['Text20'] || '',
                Text21: formData['Text21'] || '',
                Text22: formData['Text22'] || '',
                Text23: formData['Text23'] || '',
                Text24: formData['Text24'] || '',
                Text25: formData['Text25'] || '',
                Text26: formData['Text26'] || '',
                Text27: formData['Text27'] || '',
                Text28: formData['Text28'] || '',
                Text29: formData['Text29'] || '',
                Text30: formData['Text30'] || '',
                Text31: formData['Text31'] || '',
                Text32: formData['Text32'] || '',
                Text33: formData['Text33'] || '',
                Text34: formData['Text34'] || '',
                Text35: formData['Text35'] || '',
                Text36: formData['Text36'] || '',
                Text37: formData['Text37'] || '',
                Text38: formData['Text38'] || '',
                Text39: formData['Text39'] || '',
                Text40: formData['Text40'] || '',
                Text41: formData['Text41'] || '',
                Text42: formData['Text42'] || '',
                Text43: formData['Text43'] || '',
                Text44: formData['Text44'] || '',

                _txtNumberofChargingStations: formData['_txtNumberofChargingStations'] || '',
                _txtNumberofChargesperStationsperDay: formData['_txtNumberofChargesperStationsperDay'] || '',

                _hdTotalBaseCStoreUtilityCosts: formData['_hdTotalBaseCStoreUtilityCosts'] || '',
                _hdEnergyCostkwh: formData['_hdEnergyCostkwh'] || '',
                _txtNumberOfChargers: formData['_txtNumberOfChargers'] || '',
                _txtDemandkWCharger: formData['_txtDemandkWCharger'] || '',
                _hdDemandkWCharger: formData['_hdDemandkWCharger'] || '',
                _txtChargespermonth: formData['_txtChargespermonth'] || '',
                _hdChargespermonth: formData['_hdChargespermonth'] || '',

                _txtIncrementalMonthlykWh: formData['_txtIncrementalMonthlykWh'] || '',
                _hdIncrementalMonthlykWh: formData['_hdIncrementalMonthlykWh'] || '',

                _txtBaseMonthlykWh: formData['_txtBaseMonthlykWh'] || '',
                _hdBaseMonthlykWh: formData['_hdBaseMonthlykWh'] || '',

                _txtTotalMonthlykWh: formData['_txtTotalMonthlykWh'] || '',
                _hdTotalMonthlykWh: formData['_hdTotalMonthlykWh'] || '',

                _txtTotalEnergyCharge: formData['_txtTotalEnergyCharge'] || '',
                _hdTotalEnergyCharge: formData['_hdTotalEnergyCharge'] || '',

                _txtDemandkW: formData['_txtDemandkW'] || '',
                _hdDemandkW: formData['_hdDemandkW'] || '',

                _hdTotalDemandkW: formData['_hdTotalDemandkW'] || '',

                _txtTotalDemandCharge: formData['_txtTotalDemandCharge'] || '',
                _hdTotalDemandCharge: formData['_hdTotalDemandCharge'] || '',

                _txtMonthlyChargesPerMonth: formData['_txtMonthlyChargesPerMonth'] || '',
                _hdMonthlyChargesPerMonth: formData['_hdMonthlyChargesPerMonth'] || '',

                _hdEnergyCharges: formData['_hdEnergyCharges'] || '',

                _txtDemandCharges: formData['_txtDemandCharges'] || '',
                _hdDemandCharges: formData['_hdDemandCharges'] || '',

                _txtTotalCharges: formData['_txtTotalCharges'] || '',
                _hdTotalCharges: formData['_hdTotalCharges'] || '',

                _txtIncrementOverTotalBase: formData['_txtIncrementOverTotalBase'] || '',
                _hdIncrementOverTotalBase: formData['_hdIncrementOverTotalBase'] || '',

                _hdCostPercharges: formData['_hdCostPercharges'] || '',

                _txtCostPerkWh: formData['_txtCostPerkWh'] || '',
                _hdCostPerkWh: formData['_hdCostPerkWh'] || '',

                _txtIndChargers: formData['_txtIndChargers'] || '',
                _hdIndChargers: formData['_hdIndChargers'] || '',

                _txtIndChargesChargerDay: formData['_txtIndChargesChargerDay'] || '',
                _hdIndChargesChargerDay: formData['_hdIndChargesChargerDay'] || '',

                _txtIndMonthlyChargesPerMonth: formData['_txtIndMonthlyChargesPerMonth'] || '',
                _hdIndMonthlyChargesPerMonth: formData['_hdIndMonthlyChargesPerMonth'] || '',

                _txtIndConvRateStoreTrans: formData['_txtIndConvRateStoreTrans'] || '',
                _hdIndConvRateStoreTrans: formData['_hdIndConvRateStoreTrans'] || '',

                _txtIndBasketSizeInSale: formData['_txtIndBasketSizeInSale'] || '',
                _hdIndBasketSizeInSale: formData['_hdIndBasketSizeInSale'] || '',

                _txtIndGMT: formData['_txtIndGMT'] || '',
                _txtIndGrossMarginTotal: formData['_txtIndGrossMarginTotal'] || '',
                _hdIndGrossMarginTotal: formData['_hdIndGrossMarginTotal'] || '',

                _txtCRGPS1: formData['_txtCRGPS1'] || '',
                _hdCRGPS1: formData['_hdCRGPS1'] || '',

                _txtCRGPS2: formData['_txtCRGPS2'] || '',
                _hdCRGPS2: formData['_hdCRGPS2'] || '',

                _txtCRGPS3: formData['_txtCRGPS3'] || '',
                _hdCRGPS3: formData['_hdCRGPS3'] || '',

                _txtCapChargers: formData['_txtCapChargers'] || '',
                _hdCapChargers: formData['_hdCapChargers'] || '',

                _txtCapChargePerChargerPerDay: formData['_txtCapChargePerChargerPerDay'] || '',
                _hdCapChargePerChargerPerDay: formData['_hdCapChargePerChargerPerDay'] || '',

                _txtCapTotalCapEx: formData['_txtCapTotalCapEx'] || '',
                _hdCapTotalCapEx: formData['_hdCapTotalCapEx'] || '',

                _txtCapHurdleReturn: formData['_txtCapHurdleReturn'] || '',
                _hdCapHurdleReturn: formData['_hdCapHurdleReturn'] || '',

                _txtCapIndirectGPSPerYear: formData['_txtCapIndirectGPSPerYear'] || '',
                _hdCapIndirectGPSPerYear: formData['_hdCapIndirectGPSPerYear'] || '',

                _txtCapChargePerYear: formData['_txtCapChargePerYear'] || '',
                _hdCapChargePerYear: formData['_hdCapChargePerYear'] || '',

                _txtCapRequiredChargingNetS: formData['_txtCapRequiredChargingNetS'] || '',
                _hdCapRequiredChargingNetS: formData['_hdCapRequiredChargingNetS'] || '',

                _txtCapRequiredNetSPerCharge: formData['_txtCapRequiredNetSPerCharge'] || '',
                _hdCapRequiredNetSPerCharge: formData['_hdCapRequiredNetSPerCharge'] || '',

                _txtCapRequiredGrossSPerCharge: formData['_txtCapRequiredGrossSPerCharge'] || '',
                _hdCapRequiredGrossSPerCharge: formData['_hdCapRequiredGrossSPerCharge'] || '',

                _txtCostConsumerPerkWh: formData['_txtCostConsumerPerkWh'] || '',
                _hdCostConsumerPerkWh: formData['_hdCostConsumerPerkWh'] || '',

                _txtConvChargers: formData['_txtConvChargers'] || '',
                _hdConvChargers: formData['_hdConvChargers'] || '',

                _txtConvChargePerChargersPerDay: formData['_txtConvChargePerChargersPerDay'] || '',
                _hdConvChargePerChargersPerDay: formData['_hdConvChargePerChargersPerDay'] || '',

                _txtConvTwentyRequiredGrossSPerCharge: formData['_txtConvTwentyRequiredGrossSPerCharge'] || '',
                _hdConvTwentyRequiredGrossSPerCharge: formData['_hdConvTwentyRequiredGrossSPerCharge'] || '',

                // _txtConvFortyRequiredGrossSPerCharge: formData['_txtConvFortyRequiredGrossSPerCharge'] || '',
                // _hdConvFortyRequiredGrossSPerCharge: formData['_hdConvFortyRequiredGrossSPerCharge'] || '',

                _txtConvSixtyRequiredGrossSPerCharge: formData['_txtConvSixtyRequiredGrossSPerCharge'] || '',
                _hdConvSixtyRequiredGrossSPerCharge: formData['_hdConvSixtyRequiredGrossSPerCharge'] || '',

                _txtConvEightyRequiredGrossSPerCharge: formData['_txtConvEightyRequiredGrossSPerCharge'] || '',
                _hdConvEightyRequiredGrossSPerCharge: formData['_hdConvEightyRequiredGrossSPerCharge'] || '',

                _txtDemandBasekW: formData['_txtDemandBasekW'] || '',
                _hdDemandBasekW: formData['_hdDemandBasekW'] || '',
                _txtChargers: formData['_txtChargers'] || '',
                _hdChargers: formData['_hdChargers'] || '',
                _txtChargesChargerDay: formData['_txtChargesChargerDay'] || '',
                _hdChargesChargerDay: formData['_hdChargesChargerDay'] || '',
                _txtConvFourtyRequiredGrossSPerCharge: formData['_txtConvFourtyRequiredGrossSPerCharge'] || '',
                _hdConvFourtyRequiredGrossSPerCharge: formData['_hdConvFourtyRequiredGrossSPerCharge'] || '',
                _hdNumberofChargingStations: formData['_hdNumberofChargingStations'] || '',
                _hdNumberofChargesperStationsperDay: formData['_hdNumberofChargesperStationsperDay'] || '',
                _hdNumberOfChargers: formData['_hdNumberOfChargers'] || '',
                _txtIndbsis: formData['_txtIndbsis'] || '',

            };



            var container = $('#evChargingStepsContainer');

            if ($(this).attr('id') === 'exportPDF') {

                $.ajax({
                    url: '/EVChargingCalculator/UpdateModelData/',
                    method: 'POST',
                    data: { model: model },
                    success: function (response) {
                        window.location.href = '/EVChargingCalculator/GenerateInvoicePDF/';

                    },
                    error: function (xhr, status, error) {
                        console.error('PDF generate request failed:', error);
                    },
                    //complete: function () {
                    //    $thisButton.prop('disabled', false); // Re-enable the button once the request is complete
                    //}
                });

            }
            else {
                var actionUrl = $(this).attr('id') === 'nextStep' ?
                    '/EVChargingCalculator/NextStep' :
                    '/EVChargingCalculator/PreviousStep/';

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
                            // $('#_divStepsHeader').css("display", "none");
                        }
                        else {
                            $('#_step').css("display", "block");
                            $('#_divHowToUse').css("display", "block");
                            // $('#_divStepsHeader').css("display", "block");
                        }

                    },
                    error: function (xhr, status, error) {
                        console.error('AJAX request failed:', error);
                    },
                    complete: function () {
                        $thisButton.prop('disabled', false); // Re-enable the button once the request is complete
                    }
                });
            }


        } else {
        }
    });
});
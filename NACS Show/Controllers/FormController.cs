using Microsoft.AspNetCore.Mvc;
using NACS.Portal.Core.Services;
using NACSShow.Models;

namespace NACSShow.Controllers;

public class FormController : Controller
{
    private readonly IFormProcessingService _formProcessingService;
    
    public FormController(IFormProcessingService formProcessingService)
    {
        _formProcessingService = formProcessingService;
    }

    #region Form save data

    //Alumni 2023 nacs show event rsvp form submit
    [HttpPost]
    [Route("/Alumni2023NACSShowEventRSVPForm/SaveData")]
    public async Task<IActionResult> Alumni2023NACSShowEventRSVPFormSaveData(AlumniNACSShowEventRSVPFormViewModel formData) =>
        Json(await _formProcessingService.ProcessFormDataAsync(formData, "Alumni2023NACSShowEventRSVP", "EmailSettings:Alumni2023NACSShowEventRSVP", "Alumni 2023 NACS Show Event RSVP Form", true));

    //NACS show registration alert form submit
    [HttpPost]
    [Route("/NACSShowRegistrationAlertForm/SaveData")]
    public async Task<IActionResult> NACSShowRegistrationAlertFormSaveData(NACSRegistrationAlertFormViewModel formData) =>
        Json(await _formProcessingService.ProcessFormDataAsync(formData, "Attend", "", "NACS Show Registration Alert Form", true));

    //NACS show updates form submit
    [HttpPost]
    [Route("/NACSShowUpdatesForm/SaveData")]
    public async Task<IActionResult> NACSShowUpdatesFormSaveData(NACSShowUpdatesFormViewModel formData) =>
        Json(await _formProcessingService.ProcessFormDataAsync(formData, "NACSShowUpdates", "", "NACS Show Updates Form", true));

    //2024 nacs show notify me form submit
    [HttpPost]
    [Route("/NACSShowNotifyMeForm/SaveData")]
    public async Task<IActionResult> NACSShowNotifyMeFormSaveData(NACSShowNotifyMeFormViewModel formData) =>
        Json(await _formProcessingService.ProcessFormDataAsync(formData, "NACSShowNotifyMe", "", "NACS Show Notify Me Form", true));

    //NS virtual experience alert form submit
    [HttpPost]
    [Route("/NSVirtualExperienceAlertForm/SaveData")]
    public async Task<IActionResult> NSVirtualExperienceAlertFormSaveData(NSVirtualExperienceAlertFormViewModel formData) =>
        Json(await _formProcessingService.ProcessFormDataAsync(formData, "NSVirtualExperienceAlert", "", "NS Virtual Experience Alert Form", true));

    //Exhibitor support team questions form submit
    [HttpPost]
    [Route("/ExhibitorSupportTeamQuestionsForm/SaveData")]
    public async Task<IActionResult> ExhibitorSupportTeamQuestionsFormSaveData(ExhibitorSupportTeamQuestionsFormViewModel formData) =>
        Json(await _formProcessingService.ProcessFormDataAsync(formData, "ExhibitorSupportTeamQuestions", "EmailSettings:ExhibitorSupportTeamQuestions", "Exhibitor support team questions Form", true));

    //Mobile app available notification form submit
    [HttpPost]
    [Route("/MobileAppNotificationForm/SaveData")]
    public async Task<IActionResult> MobileAppNotificationFormSaveData(MobileAppNotificationFormViewModel formData) =>
        Json(await _formProcessingService.ProcessFormDataAsync(formData, "MobileAppAvailableNotification", "EmailSettings:MobileAppNotification", "Mobile app available notification Form", true));


    #endregion
}

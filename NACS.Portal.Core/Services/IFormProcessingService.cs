using Microsoft.AspNetCore.Mvc;
using static NACS.Portal.Core.Services.FormProcessingService;

namespace NACS.Portal.Core.Services;

public interface IFormProcessingService
{
    Task<ProcessingResult> ProcessFormDataAsync<T>(
        T formData,
        string formKey,
        string emailSettingsKey,
        string submitFormName,
        bool hasRecaptcha = false) where T : class;
}

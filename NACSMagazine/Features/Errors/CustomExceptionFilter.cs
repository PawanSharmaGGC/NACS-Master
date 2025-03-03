using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSMagazine.Features.Errors
{
    public class CustomExceptionFilter(
        IModelMetadataProvider modelMetadataProvider,
        IMediator mediator) : IAsyncExceptionFilter
    {
        private readonly IModelMetadataProvider modelMetadataProvider = modelMetadataProvider;
        private readonly IMediator mediator = mediator;

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var settings = await mediator.Send(new WebsiteSettingsContentQuery());

            var result = new ViewResult
            {
                ViewName = "~/Features/Errors/Exception.cshtml",
                ViewData = new ViewDataDictionary(modelMetadataProvider, context.ModelState)
                {
                    ModelMetadata = new ErrorPageViewModel
                    {
                        MessageHTML = new(settings.WebsiteSettingsContentExceptionContentHTML)
                    },
                    StatusCodeResult = 500
                }
            };

            context.ExceptionHandled = true;
            context.Result = result;
        }
    }
}

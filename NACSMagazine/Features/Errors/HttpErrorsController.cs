﻿using CMS.Websites;

using MediatR;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSMagazine.Features.Errors
{
    [Route("error")]
    public class HttpErrorsController(IMediator mediator, WebPageMetaService metaService) : Controller
    {
        private readonly IMediator mediator = mediator;
        private readonly WebPageMetaService metaService = metaService;

        [HttpGet("{code:int")]
        public async Task<ActionResult> Error(int code)
        {
            if(code != 404)
            {
                metaService.SetMeta(new("Error", "There was a problem loading the page you requested."));

                return StatusCode(code);
            }

            var settings = await mediator.Send(new WebsiteSettingsContentQuery());

            var model = new ErrorPageViewModel
            {
                MessageHTML = !string.IsNullOrWhiteSpace(settings.WebsiteSettingsContentNotFoundContentHTML)
                    ? new(settings.WebsiteSettingsContentNotFoundContentHTML)
                    : null
            };

            metaService .SetMeta(new("Not Found", "The page you requested could not be found."));

            return View("~/Features/Errors/NotFound.cshtml", model);
        }
    }

    public class ErrorPageViewModel
    {
        public HtmlString? MessageHTML { get; set; }
    }
}

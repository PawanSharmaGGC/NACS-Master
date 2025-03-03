using CMS.Core;
using Convenience.org.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Convenience.org.Controllers;
public class CommonController : Controller
{
    private readonly IEventLogService _eventLogService;
    private readonly IDeepDiveRepository _deepDiveRepository;

    public CommonController(IEventLogService eventLogService, IDeepDiveRepository deepDiveRepository)
    {
        _eventLogService = eventLogService;
        _deepDiveRepository = deepDiveRepository;
    }

    [HttpGet]
    [Route("/deepdivewidget/getcardsbytag/{tagId}")]
    public IActionResult GetCardsByTag(string tagId, int topN)
    {

        try
        {
            var selectedTagId = Guid.Parse(tagId);
            var tagIdentifiers = new List<Guid> { selectedTagId };
            var deepDiveCardItems = _deepDiveRepository.GetDeepDiveContentRepositoryAsync(tagIdentifiers, topN).Result;

            return new JsonResult(new { success = true, cards = deepDiveCardItems });
        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(CommonController), nameof(GetCardsByTag), ex, $"Selected Tag - {tagId}");
            return new JsonResult(new { success = false });
        }

    }

    [HttpGet]
    [Route("/retailmembershipcalculator/calculateretailmembershipdues/{customerNumber}")]
    public IActionResult CalculateRetailMembershipDues(double customerNumber)
    {

        try
        {
            double totalDues = 0;

            if (customerNumber >= 0.00 && customerNumber <= 5999999.00)
            {
                double BaseDues250 = 250;
                double Escalator0 = 0;

                var BasePlus = (Escalator0 * (customerNumber - 0.00)) / 1000000.00;
                totalDues = (BaseDues250 + BasePlus);
                //totalDues = 250;
            }
            else if (customerNumber >= 6000000 && customerNumber <= 24999999.99)
            {
                double BaseDues250 = 250;
                double Escalator14 = 14;

                var BasePlus = (Escalator14 * (customerNumber - 6000000.00)) / 1000000.00;
                totalDues = (BaseDues250 + BasePlus);
                //totalDues = 250 + (14 * (customerNumber - 6000000) / 1000000);
            }
            if ((customerNumber >= 25000000.00) && (customerNumber <= 99999999.99))
            {
                double BaseDues500 = 550;
                double Escalator13 = 13;

                var BasePlus = (Escalator13 * (customerNumber - 25000000.00)) / 1000000.00;
                totalDues = (BaseDues500 + BasePlus);
                //totalDues = 550 + (13 * (customerNumber - 25000000) / 1000000);
            }

            if ((customerNumber >= 100000000.00) && (customerNumber <= 2469999999.99))
            {
                double BaseDues1550 = 1550;
                double Escalator12 = 12;

                var BasePlus = (Escalator12 * (customerNumber - 100000000.00)) / 1000000.00;
                totalDues = (BaseDues1550 + BasePlus);
                //totalDues = 1550 + (12 * (customerNumber - 100000000) / 1000000);
            }

            if (customerNumber >= 2470000000.00)
            {
                totalDues = 30000.00;
            }

            return new JsonResult(new { success = true, totalDues = "$ " + totalDues.ToString("N2") });
        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(CommonController), nameof(CalculateRetailMembershipDues), ex, $"Input number - {customerNumber}");
            return new JsonResult(new { success = false });
        }

    }
}

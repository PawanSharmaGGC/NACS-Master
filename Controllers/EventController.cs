using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CMS.Websites;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Controllers
{
    public class EventController : Controller
    {
        protected readonly IEventPageRepository _eventPageRepository;
        private readonly IWebPageUrlRetriever _webPageUrlRetriever;

        public EventController(IEventPageRepository eventPageRepository, IWebPageUrlRetriever webPageUrlRetriever)
        {
            _eventPageRepository = eventPageRepository;
            _webPageUrlRetriever = webPageUrlRetriever;
        }

        [Route("/event/downloadeventcalendar/{pageGuid}")]
        public IActionResult Index(Guid pageGuid)
        {
            var guids = new List<Guid>();
            guids.Add(pageGuid);

            var selectedEvent = _eventPageRepository.GetEventsRepository(guids)?.FirstOrDefault();
            if (selectedEvent != null)
            {
                string fileContent = GetContent(selectedEvent);

                var result = Content(fileContent, "text/calendar");
                HttpContext.Response.Headers.Add("content-disposition", string.Format("attachment; filename={0}.ics", "ConvenienceEventReminder"));
                return result;
            }
            return null;
        }

        private string GetContent(EventPage eventPage)
        {
            if (eventPage == null)
                return string.Empty;

            EventICSViewModel eventICSView = new EventICSViewModel();

            eventICSView.EventStartDate = eventPage.StartDate;
            eventICSView.EventEndDate = eventPage.EndDate;
            eventICSView.CurrentDate = DateTime.Now;

            eventICSView.EventLocation = eventPage.Location;
            eventICSView.EventDescription = eventPage.Description;
            eventICSView.EventSummary = eventICSView.EventTitle;
            eventICSView.EventTitle = eventPage.Title;
            var req = HttpContext.Request;
            eventICSView.EventUrl = req.Scheme + "://" + req.Host + "/" + eventPage.SystemFields.WebPageUrlPath;

            return eventICSView.GenerateICSFile();
        }

        [Route("/event/grid")]
        public IActionResult Grid(int sortby, bool isAsc = true, int? page = 1)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            int totalAttendies;
            var customerData = (from at in _eventPageRepository.GetFilteredData(sortColumn, out totalAttendies, page ?? 1, pageSize).ToList() select at);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                sortColumn = sortColumn.Replace(" ", "").Trim();
                
                if (sortColumnDirection == "desc")
                {
                    customerData = customerData.AsQueryable().CustomOrderBy(sortColumn, false).ToList();
                }
                else
                {
                    customerData = customerData.AsQueryable().CustomOrderBy(sortColumn, true).ToList();
                }
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = customerData.Where(m => m.BadgeName.Contains(searchValue)
                                            || m.LastName.Contains(searchValue)
                                            || m.CompanyName.Contains(searchValue)
                                            || m.Email.Contains(searchValue));
            }
            recordsTotal = customerData.Count();
            var data = customerData.Skip(skip).Take(pageSize).ToList();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);

        }

    }

    public static class QueryableExtensions
    {
        public static IQueryable<T> CustomOrderBy<T>(this IQueryable<T> source, string columnName, bool isAscending = true)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

            MemberExpression property = Expression.Property(parameter, columnName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                  new Type[] { source.ElementType, property.Type },
                                  source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}

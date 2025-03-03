using System;
using System.Collections.Generic;
using System.Linq;
using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using NACS.Protech.Entities;

namespace Convenience.org.Repositories
{
    public class EventPageRepository : IEventPageRepository
    {
        private readonly IContentQueryExecutor _executor;
        private readonly IWebsiteChannelContext _channelContext;

        public EventPageRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext)
        {
            _executor = executor;
            _channelContext = channelContext;
        }

        public List<NACSAttendeeViewModel> GetFilteredData(string sortby, out int totalItems, int pageNumber = 1, int pageSize = 20)
        {

            List<NACSAttendeeViewModel> attendees = GetData();
            totalItems = attendees.Count;

            //Get only current page attendies list
            var takeAwayCount = pageSize * pageNumber;
            if (attendees.Count > 0 && attendees.Count > takeAwayCount)
            {
                attendees.OrderBy(x => x.CompanyName).Take(takeAwayCount).ToList();
            }

            return attendees;
        }



        public List<NACSAttendeeViewModel> GetData()
        {
            List<NACSAttendeeViewModel> attendees = new List<NACSAttendeeViewModel>();

            try
            {
                var evt = Event.GetByCode("23DOH");
                if (evt != null)
                {
                    evt = Event.GetByCode("24DOH");
                }
                var coEventRegs = Event.GetRegistrantsByEvent(evt?.Id); //All registrants for current Event

                if (coEventRegs != null)
                {
                    foreach (var eventReg in coEventRegs)
                    {
                        if (eventReg.IsCancelled == false)
                        {
                            try
                            {
                                attendees.Add(new NACSAttendeeViewModel()
                                {
                                    LastName = Convert.ToString(eventReg.LastName).Trim(),
                                    BadgeName = Convert.ToString(eventReg.BadgeName).Trim(),
                                    CompanyName = Convert.ToString(eventReg.AccountName).Trim(),
                                    Title = "", //Convert.ToString(eventReg.Title).Trim(),
                                    Email = "", //Convert.ToString(eventReg.EmailAddress).Trim(),
                                    CityState = "", //Convert.ToString(eventReg.AddressCity).Trim() + ", " + Convert.ToString(eventReg.AddressStateProvince).Trim(),
                                    Country = "" //Convert.ToString(eventReg.AddressCountry).Trim()
                                });
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                else
                {
                    attendees.Add(new NACSAttendeeViewModel()
                    {
                        StatusMessage = "ERROR: No Attendees Found"
                    });
                }

            }
            catch (Exception ex)
            {
                //attendees.Add(new NACSAttendeeViewModel()
                //{
                //    LastName = "Error",
                //    BadgeName = "Error",
                //    CompanyName = ex.Message.ToString(),
                //    StatusMessage = "ERROR: " + ex.Message.ToString() + " | STACK TRACE: " + ex.StackTrace.ToString()
                //});
                attendees = null;
            }

            return attendees;
        }


        public List<EventPage> GetEventsRepository(List<Guid> WebPageGuids)
        {
            // Prepares a query that retrieves article pages matching the selected GUIDs
            var pageQuery = new ContentItemQueryBuilder()
                    .ForContentTypes(parameters =>
                        parameters.ForWebsite(WebPageGuids).WithLinkedItems(1).OfContentType(EventPage.CONTENT_TYPE_NAME).WithContentTypeFields());

            IEnumerable<EventPage> events =
                     _executor.GetMappedWebPageResult<EventPage>(pageQuery)?.Result;

            return events.ToList() ?? new List<EventPage>();

        }

    }
}

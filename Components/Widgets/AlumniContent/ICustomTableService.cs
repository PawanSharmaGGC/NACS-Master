using Customtable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Components.Widgets.AlumniContent
{
    public interface ICustomTableService
    {
        List<AlumniItem> GetData(List<string> filters, int limit, int offset);
       
    }
    public class CustomTableService : ICustomTableService
    {
        public List<AlumniItem> GetData(List<string> filters, int limit, int offset = 0)
        {
            var query = AlumniDirectoryUpdatesTableInfoProvider.ProviderObject
                        .Get()
                        .Columns("Title", "Description", "URL", "Topic", "Type", "StartDate")
                        .WhereGreaterOrEquals("StartDate", DateTime.Today.AddDays(-100))
                        .OrderByDescending("StartDate");

            if (filters != null && filters.Any())
            {
                query.WhereIn("Topic", filters);
            }

            var paginatedQuery = query.Skip(offset).Take(limit);

            return paginatedQuery.ToList()
                .Select(item => new AlumniItem
                {
                    Title = item.GetStringValue("Title", ""),
                    Description = item.GetStringValue("Description", ""),
                    URL = item.GetStringValue("URL", ""),
                    Topic = item.GetStringValue("Topic", ""),
                    Type = item.GetStringValue("Type", ""),
                    StartDate = item.GetDateTimeValue("StartDate", DateTime.MinValue)
                })
                .ToList();
        }




    }
}

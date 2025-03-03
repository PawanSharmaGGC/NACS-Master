using System;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.AlumniContent
{
    public class AlumniContentViewModel
    {
        public List<string> Filters { get; set; }
        public int Limit { get; set; } = 5;
        public bool CanShowMore { get; set; } 
        public List<AlumniItem> Items { get; set; }

    }
    public class AlumniItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Topic { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
    }
}

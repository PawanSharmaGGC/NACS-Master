using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.StateExecDirectory
{
    public class StateExecDirectoryViewModel
    {
        public List<Executive> Executives { get; set; } = new List<Executive>();
        public string SearchTerm { get; set; }
        public int CurrentPage { get; set; }
        public bool HasMorePages { get; set; }
        public int TotalCount { get; set; }
        public List<string> AvailableCountries { get; set; } = new List<string>();
        public List<string> AvailableStates { get; set; } = new List<string>();
        public string[] SelectedCountries { get; set; }
        public string[] SelectedStates { get; set; }
    }

    public class Executive
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string ProfileImage { get; set; }
        public List<string> CountriesServed { get; set; }
        public List<string> StatesServed { get; set; }

    }
}

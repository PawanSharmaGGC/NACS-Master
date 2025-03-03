using System;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.AlumniDirectory
{
    public class Program
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
        public int Year { get; set; }
    }

    public class AlumniMember
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string LinkedInURL { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string Location { get; set; }
        public string ProfileImage { get; set; }
        public List<Program> ProgramsAttended { get; set; }
        public bool MasterOfConvenience { get; set; }
    }
}

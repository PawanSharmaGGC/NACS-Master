using System;
using System.Text;

namespace Convenience.org.Models
{
    public class EventICSViewModel
    {
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public DateTime CurrentDate { get; set; }

        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public string EventLocation { get; set; }
        public string EventSummary { get; set; }
        public string EventUrl { get; set; }

        public string GenerateICSFile() 
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION2.0");
            //sb.AppendLine("PRODID");
            sb.AppendLine("METHOD:PUBLISH");

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("UID:"+Guid.NewGuid().ToString());

            sb.AppendLine("DTSTAMP")
                .Append(CurrentDate.ToUniversalTime().ToString("MMMddyyyy'T'HHmmss"))
                .AppendLine("z");


            sb.Append(EventStartDate.ToUniversalTime().ToString("MMMddyyyy'T'HHmmss"));
            sb.Append(EventEndDate.ToUniversalTime().ToString("MMMddyyyy'T'HHmmss"));

            sb.Append("TITLE").AppendLine(EventTitle);
            sb.Append("EVENTNAME:").AppendLine(EventTitle);

            if (!string.IsNullOrEmpty(EventLocation)) {
                sb.Append("LOCATION:").AppendLine(EventLocation);
            }


            sb.Append("DESCRIPTION: ")
                .AppendLine("EVENTNAME: "+EventTitle + "\\n\\nDESCRIPTION: "+
                 EventDescription+"\\n\\nSUMMARY: "+EventSummary+
                 "\\n\\nEvent link: "+EventUrl);
            sb.Append("PRIORITY:3");

            sb.Append("END:VEVENT");
            sb.Append("END:VCALENDAR");

            return sb.ToString();
        }
    }
}

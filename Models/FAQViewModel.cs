using System.Collections.Generic;

namespace Convenience.org.Models;

public class FAQViewModel
{
    public string Heading { get; set; }
    public List<FAQItem> FAQItems { get; set; }

    public static FAQViewModel GetViewModel(string heading)
    {
        return new FAQViewModel
        {
            Heading = heading,
            FAQItems = new List<FAQItem>()
        };
    }
}

public class FAQItem
{
    public string Question { get; set; }
    public string Answer { get; set; }
}

using System.Collections.Generic;
using CMS.EmailEngine;
using Convenience.org.Models;

namespace Convenience.org.Components.Widgets.Cards.Tier1ContentCard
{
    public class Tier1ContentCardWidgetViewModel
    {
        public string? ImageUrl { get; set; }
        public string? ImageAltText { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? DateTime { get; set; }
        public string? LocationOrReadTime { get; set; }
        public string? EyebrowTitle { get; set; }
        public string? EyebrowStatus { get; set; }
        public string CTA1Text { get; set; } = string.Empty;
        public string CTA1Link { get; set; } = string.Empty;
        public string CTA2Text { get; set; } = string.Empty;
        public string CTA2Link { get; set; } = string.Empty;

        public static Tier1ContentCardWidgetViewModel GetViewModel(Tier1ContentCardWidgetProperties properties)
        {
            if (properties != null)
            {
                return new Tier1ContentCardWidgetViewModel()
                {
                    DateTime = properties.DateTime.ToString(),
                    Description = properties.Description,
                    EyebrowTitle = properties.EyebrowTitle,
                    LocationOrReadTime = !string.IsNullOrEmpty(properties.LocationOrReadTime) ? properties.LocationOrReadTime : properties.DateTime?.ToString("HH:mm \"GMT\"zzz"),
                    Title = properties.Title,
                    CTA1Text = properties.CTA1Text,
                    CTA1Link = properties.CTA1Link,
                    CTA2Text = properties.CTA2Text,
                    CTA2Link = properties.CTA2Link,
                    EyebrowStatus = properties.EyebrowStatus,
                };
            }
            else
            {
                return null;
            }
        }
    }
}

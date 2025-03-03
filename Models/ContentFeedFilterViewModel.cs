using System.Collections.Generic;
using System;
using Tag = CMS.ContentEngine.Tag;

namespace Convenience.org.Models
{
    public class ContentFeedFilterViewModel
    {
        public List<Tag> WebinarType { get; set; }
        public List<VideoExtended> Videos { get; set; }
    }
    public class VideoExtended : Video
    {
        public string HeaderImagePath { get; set; }
        public string RollupImagePath { get; set; }
        public string RollupImageURLPath { get; set; }
        public string ImagePath { get; set; }
    }

}

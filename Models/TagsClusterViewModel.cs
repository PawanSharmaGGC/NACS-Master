using CMS.ContentEngine;
using CMS.ContentEngine;
using CMS.MediaLibrary;
using System.Collections.Generic;
using System;
using Tag = CMS.ContentEngine.Tag;
namespace Convenience.org.Models
{
    public class TagsClusterViewModel
    {
        public List<Tag> TagCategory { get; set; }
        public List<ArticleExtended> Articles { get; set; }
    }
    public class ArticleExtended : Article
    {
        public string RollupImageURLPath { get; set; }
        public string HeaderImagePath { get; set; }
        public string RollupImagePath { get; set; }
        public string ImagePath { get; set; }
        public string AuthorImagePath { get; set; }
    }
}

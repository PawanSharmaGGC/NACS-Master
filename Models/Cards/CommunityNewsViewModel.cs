using CMS.MediaLibrary;
using System.Collections.Generic;
using System;

namespace Convenience.org.Models.Cards
{
    public class CommunityNewsViewModel
    {
        /// <summary>
        /// Title of Community News
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// SubTitle of Community News
        /// </summary>
        public string SubTitle { get; set; }


        /// <summary>
        /// Author of Community News
        /// </summary>
        public string AuthorName { get; set; }


        /// <summary>
        /// Description of Community News
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Author Image
        /// </summary>
        public IEnumerable<AssetRelatedItem> AuthorImage { get; set; }


        /// <summary>
        /// Publish Date
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Author Image Path
        /// </summary>
        public string AuthorImagePath { get; set; }

        /// <summary>
        /// Image Alt Text
        /// </summary>
        public string ImageAltText { get; set; }

        /// <summary>
        /// Page URL
        /// </summary>
        public string PageUrl { get; set; }
    }
}

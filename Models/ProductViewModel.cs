using CMS.MediaLibrary;
using System;
using System.Collections.Generic;

namespace Convenience.org.Models
{
    public class ProductViewModel
    {
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public decimal ListPrice { get; set; }
        public string Department { get; set; }
        public string TaxClass { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string Supplier { get; set; }
        public string Collection { get; set; }
        public IEnumerable<AssetRelatedItem> Image { get; set; }
        public List<ImageAssetsViewModel> Images { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Representing { get; set; }
        public string ProductKey { get; set; }
        public string ProtechProductId { get; set; }
        public DateTime ShowNewUntilDate { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsBestseller { get; set; }
        public string ProductYear { get; set; }
        public string ProductCategory { get; set; }
        public string NACSProductID { get; set; }
        public string CrossPromotionSKUs { get; set; }
        public string NACSSKUNumber { get; set; }
        public DateTime PublishFrom { get; set; }
        public DateTime PublishTo { get; set; }
        public DateTime InStoreFrom { get; set; }
        public string PublicStatus { get; set; }
        public string InternalStatus { get; set; }
        public bool AllowForSale { get; set; }
    }

    public class ImageAssetsViewModel
    {
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }
    }
}

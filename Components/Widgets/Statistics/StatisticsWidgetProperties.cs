using System.ComponentModel.DataAnnotations;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.Statistics
{
    public class StatisticsWidgetProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Number - Retail Gas Price (AAA)", Order = 0)]
        public string NumberRetailGasPrice { get; set; }

        [TextInputComponent(Label = "Number - Retail Gas Price Variation (AAA)", Order = 1)]
        public string NumberRetailGasPriceVariation { get; set; }

        [Required]
        [DropDownComponent(Label = "Number - Retail Gas Price Status", Options = "up;UP\ndown;Down", Order = 2)]
        public string NumberRetailGasStatus { get; set; }

        [TextInputComponent(Label = "Number - Retail Diesel Price (AAA)", Order = 3)]
        public string NumberRetailDieselPrice { get; set; }

        [TextInputComponent(Label = "Number - Retail Diesel Variation (AAA)", Order = 4)]
        public string NumberRetailDieselPriceVariation { get; set; }

        [Required]
        [DropDownComponent(Label = "Number - Retail Diesel Status", Options = "up;UP\ndown;Down", Order = 5)]
        public string NumberRetailDieselStatus { get; set; }

        [TextInputComponent(Label = "Number - Cruid Oil (NYMEX)", Order = 6)]
        public string NumberCruidOilPrice { get; set; }

        [TextInputComponent(Label = "Number - Cruid Oil Variation (NYMEX)", Order = 7)]
        public string NumberCruidOilPriceVariation { get; set; }

        [Required]
        [DropDownComponent(Label = "Number - Cruid Oil Status", Options = "up;UP\ndown;Down", Order = 8)]
        public string NumberCruidOilStatus { get; set; }

        [TextInputComponent(Label = "Number - Retail Fuel Margin (OPIS)", Order = 9)]
        public string NumberRetailFuelMarginPrice { get; set; }

        [TextInputComponent(Label = "Number - Retail Fuel Margin Variation (OPIS)", Order = 10)]
        public string NumberRetailFuelMarginPriceVariation { get; set; }

        [Required]
        [DropDownComponent(Label = "Number - Retail Fuel Margin Status", Options = "up;UP\ndown;Down", Order = 11)]
        public string NumberRetailFuelMarginStatus { get; set; }

        [RichTextEditorComponent(Label = "Factoid - Price", Order = 12)]
        public string FactoidPrice { get; set; }

        [RichTextEditorComponent(Label = "Factoid - Summary text", Order = 13)]
        public string FactoidSummary { get; set; }
    }
}

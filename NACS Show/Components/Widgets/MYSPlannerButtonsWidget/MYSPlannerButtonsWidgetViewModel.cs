using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSShow.Components.Widgets.MYSPlannerButtonsWidget
{
	public class MYSPlannerButtonsWidgetViewModel
	{
		public string ErrorMessage { get; set; } = "";
		public string PlanButtonLink { get; set; } = "";
		public string PlanButtonText { get; set; } = "Start Planning";
		public string SavedItemsToSync { get; set; } = "";

		public MYSPlannerButtonsWidgetViewModel(MYSPlannerButtonsWidgetProperties props)
		{
			PlanButtonText = props.ButtonText;
			PlanButtonLink = props.ButtonLink;
		}

		public MYSPlannerButtonsWidgetViewModel() { }
	}
}

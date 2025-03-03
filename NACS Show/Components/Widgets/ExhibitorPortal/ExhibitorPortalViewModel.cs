using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSShow.Components.Widgets.ExhibitorPortal
{
    public class ExhibitorPortalViewModel
    {
        //Url's
        public string? MapYourShowDashboardURL { get; set; }
        public string? NACSShowRegURL { get; set; }
        public string? NACSHousingURL { get; set; }
        public string? NACSShowLeadRetrievalURL { get; set; }
        public string? ContractorsLink { get; set; }
        public string? AmbassadorsLink { get; set; }
        public string? AmbassadorsSelfEmailLink { get; set; }

        //Panel's Visibility
        public bool PNLMYSDashboardVisible { get; set; }
        public bool PNLRegistrationVisible { get; set; }
        public bool PNLHousingVisible { get; set; }
        public bool PNLLeadRetrievalVisible { get; set; }
        public bool PNLServiceKitVisible { get; set; }
        public bool PNLMarketingVisible { get; set; }
        public bool PNLAmbassadorsVisible { get; set; }
        public bool PNLContractorsVisible { get; set; }
        public bool PNLAttendeeListsVisible { get; set; }
        public bool PNLExhibitorSelectionVisible { get; set; }

        //Panel's Holder Visibility
        public bool PNLMYSDashboardHolderVisible { get; set; }
        public bool PNLRegistrationHolderVisible { get; set; }
        public bool PNLHousingHolderVisible { get; set; }
        public bool PNLLeadRetrievalHolderVisible { get; set; }
        public bool PNLServiceKitHolderVisible { get; set; }
        public bool PNLMarketingHolderVisible { get; set; }
        public bool PNLAmbassadorHolderVisible { get; set; }
        public bool PNLContractorHolderVisible { get; set; }

        //Label Status Messages
        public string? LBLStatusMsg_Registration { get; set; }
        public string? LBLStatusMsg_Housing { get; set; }
        public string? LBLStatusMsg_ServiceKit { get; set; }
        public string? LBLStatusMsg_Marketing { get; set; }
        public string? LBLStatusMsg_Ambassadors { get; set; }
        public string? LBLStatusMsg_Contractors { get; set; }
        public string? LBLStatusMsg_AttendeeLists { get; set; }



        //Description Messages
        public string? LBLDescription_MYSDashboard { get; set; }
        public string? LBLDescription_Registration { get; set; }
        public string? LBLDescription_Housing { get; set; }
        public string? LBLDescription_LeadRetrieval { get; set; }
        public string? LBLDescription_ServiceKit { get; set; }
        public string? LBLDescription_Marketing { get; set; }
        public string? LBLDescription_Ambassadors { get; set; }
        public string? LBLDescription_Contractors { get; set; }
        public string? LBLDescription_AttendeeLists { get; set; }

        //Other information
        public string? LBLExhibitorName { get; set; }
        
    }
}

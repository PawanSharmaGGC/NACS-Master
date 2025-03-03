using CMS.Membership;
using Convenience.org.Components.Widgets.AttestationCheck;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using CMS.DataEngine;
using CMS.OnlineForms;
using System.Linq;

[assembly: RegisterWidget(identifier: AttestationCheckViewComponent.IDENTIFIER, name: "AttestationCheck",
    propertiesType: typeof(AttestationCheckProperties), viewComponentType: typeof(AttestationCheckViewComponent), Description = "AttestationCheck",
    IconClass = "icon-box", AllowCache = true)]

namespace Convenience.org.Components.Widgets.AttestationCheck
{
    public class AttestationCheckViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string IDENTIFIER = "AttestationCheck";
        private readonly IInfoProvider<BizFormInfo> _bizFormInfoProvider;
        public string CustomerID = (MembershipContext.AuthenticatedUser != null && MembershipContext.AuthenticatedUser.UserName != "public") ? MembershipContext.AuthenticatedUser.UserName : string.Empty;

        public AttestationCheckViewComponent(IHttpContextAccessor httpContextAccessor, IInfoProvider<BizFormInfo> bizFormInfoProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _bizFormInfoProvider = bizFormInfoProvider; 
        }
        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<AttestationCheckProperties> widgetProperties)
        {
            AttestationCheckViewModel vm = new AttestationCheckViewModel();
            string attestationQS = _httpContextAccessor.HttpContext.Request.Query["attestation"];
            try
            {
                if (!string.IsNullOrEmpty(attestationQS))
                {
                    string agreement = attestationQS;
                    bool isNew;

                    BizFormItem item = GetAttestation(out isNew);
                    if (item != null)
                    {
                        item.SetValue("Agreement", Convert.ToBoolean(agreement));
                        if (isNew)
                        {
                            item.Insert();
                        }
                        else
                        {
                            item.Update();
                        }
                    }
                    string redirectUrl = "/";
                    try
                    {
                        redirectUrl = widgetProperties.Properties.RedirectURL;
                    }
                    finally
                    {
                        if (!Convert.ToBoolean(agreement))
                            vm.RedirectURL = redirectUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                vm.ErrorText = ex.ToString();
            }

            return View("~/Components/Widgets/AttestationCheck/_AttestationCheck.cshtml", vm);
        }

        /// <summary>
        /// Get attestation value
        /// </summary>
        /// <param name="isNew"></param>
        /// <returns></returns>
        private BizFormItem GetAttestation(out bool isNew)
        {
            UserInfo currentUser = MembershipContext.AuthenticatedUser;

            // Gets the form object representing the 'CommitteePortalAttestations' form
            BizFormInfo formObject = _bizFormInfoProvider.Get("CommitteePortalAttestations");
            BizFormItem item = null;
            isNew = false;
            if (formObject != null)
            {
                // Gets the class name of the 'CommitteePortalAttestations' form
                DataClassInfo formClass = DataClassInfoProvider.GetDataClassInfo(formObject.FormClassID);
                string formClassName = formClass.ClassName;

                // Loads all data records from the form whose 'Username' is equal 
                item = BizFormItemProvider.GetItems(formClassName)
                                               .WhereEquals("Username", this.CustomerID)
                                               .FirstOrDefault();
                if (item == null)
                {
                    isNew = true;
                    item = BizFormItem.New(formClassName);
                    item.SetValue("Username", CustomerID);
                    item.SetValue("UserID", currentUser.UserID.ToString());
                    item.SetValue("Agreement", false);
                }
            }
            return item;
        }
    }
}

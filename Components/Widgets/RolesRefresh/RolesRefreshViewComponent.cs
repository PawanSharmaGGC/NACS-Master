using CMS.Membership;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.ContactManagement;
using CMS.Core;
using CMS.Globalization;
using System.Collections.Generic;
using CMS.DataEngine;
using NACS.Helper.AuthService;
using System.Text.RegularExpressions;
using NACS_Classes;



namespace Convenience.org.Components.Widgets
{
    public class RolesRefreshViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserInfoProvider userInfoProvider;
        private readonly IRoleInfoProvider roleInfoProvider;
        private readonly IUserRoleInfoProvider userRoleProvider;
        private readonly IContactInfoProvider contactInfoProvider;
        private readonly IStateInfoProvider stateInfoProvider;
        private readonly ICountryInfoProvider countryInfoProvider;
        private readonly IUserRoleInfoProvider userRoleInfoProvider;
        private readonly IInfoProvider<ApplicationPermissionInfo> applicationPermissionInfoProvider;

        public RolesRefreshViewComponent(IHttpContextAccessor httpContextAccessor, IUserInfoProvider userInfoProvider, 
        IRoleInfoProvider roleInfoProvider, IUserRoleInfoProvider userRoleProvider,IInfoProvider<ApplicationPermissionInfo> applicationPermissionInfoProvider, 
        IContactInfoProvider contactInfoProvider, IStateInfoProvider stateInfoProvider, 
        ICountryInfoProvider countryInfoProvider)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userInfoProvider = userInfoProvider;
            this.roleInfoProvider = roleInfoProvider;
            this.userRoleProvider = userRoleProvider;
            this.applicationPermissionInfoProvider = applicationPermissionInfoProvider;
            this.contactInfoProvider = contactInfoProvider;
            this.stateInfoProvider = stateInfoProvider;
            this.countryInfoProvider = countryInfoProvider;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            RolesRefreshViewModel vm = new RolesRefreshViewModel();
            UserInfo currentUser = MembershipContext.AuthenticatedUser;
            if (currentUser is not null)
            {
                if (HttpContext.Session.GetInt32("RoleRefreshes") == null)
                {
                    string status = EnsureUser(currentUser.UserName, true);
                    HttpContext.Session.SetInt32("RoleRefreshes", 1);
                    vm.Role = "Roles have been updated for this user.";

                    //string returnUrl = NACSUtilities.GetQueryStringValue("returnurl");
                    //if (!string.IsNullOrEmpty(returnUrl))
                    //{
                    //    return new RedirectResult(returnUrl);
                    //}
                    //string returnUrl = NACSUtilities.GetQueryStringValue("returnurl");
                    if (HttpContext.Request.Query.ContainsKey("returnurl"))
                    {
                        string returnUrl = NACSUtilities.GetQueryStringValue("returnurl");
                        vm.RedirectUrl = returnUrl;
                    }
                }
                else
                {
                    vm.Role = "Roles have been refreshed, but access is still denied for some reason.";
                }

                vm.Role += "\nRoles: " + GetRoleNames(currentUser);
            }

            return View("~/Components/Widgets/RolesRefresh/RolesRefresh.cshtml", vm);
        }


        private string GetRoleNames(UserInfo user)
        {
            if (user == null)
            {
                return string.Empty;
            }

            UserInfo selectedUserInfo = userInfoProvider.Get(user.UserID);
            var roleidname = UserInfoProvider.GetRolesForUser(user.UserName);


            if (roleidname == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            foreach (string rolename in roleidname)
            {
                if (rolename != null && !rolename.Contains("Authenticated", StringComparison.OrdinalIgnoreCase) && !rolename.Contains("Everyone", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine(rolename.ToLower());
                }
            }
            return sb.ToString().Trim();
        }
        public static string GetNACSAPIKey()
        {
            return System.Configuration.ConfigurationManager.AppSettings["NACSAPIKey"];
        }
        public static string GetEnvironment()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Environment"];
        }

        private string CleanRoleName(string roleName)
        {
            roleName = Regex.Replace(roleName, "\\s", "-");
            roleName = Regex.Replace(roleName, "[^A-Za-z0-9]", "");
            return roleName;
        }
        //public static bool RoleExists(string roleName, string siteName)
        //{
        //    return AbstractInfoProvider<RoleInfo, RoleInfoProvider, ObjectQuery<RoleInfo>>.ProviderObject.RoleExistsInternal(roleName, siteName);
        //}
        protected string EnsureUser(string protechNum, bool isCurrentUser)
        {
            string output = "";
            string NACSAPIKey = GetNACSAPIKey();
            try
            {


                NACSAPIAuthenticationSoapClient authService = new NACSAPIAuthenticationSoapClient();
                NACS.Helper.AuthService.NACSIndividual nacsUser = authService.Individual_GetById(protechNum, "", NACSAPIKey);

                if (nacsUser != null && !string.IsNullOrEmpty(nacsUser.ProtechNumber))
                {
                    var userObj = userInfoProvider.Get(protechNum.Trim());

                    if (userObj == null)
                    {

                        userObj = new UserInfo();
                        userObj.UserName = protechNum.Trim();
                        userObj.UserEnabled = true;
                        //userObj.PreferredCultureCode = "en-us";
                        userObj.SetValue("PreferredCultureCode", "en-us");
                        userObj.UserIsExternal = true;
                        userInfoProvider.Set(userObj);
                        //UserInfoProvider.SetUserInfo(userObj);

                        userObj = userInfoProvider.Get(protechNum.Trim());
                    }

                    //decide on email for test environments
                    string email = "";

                    if (GetEnvironment() == "PROD")
                    {
                        email = nacsUser.Email;
                    }
                    else
                    {
                        if (nacsUser.Email.Contains("@convenience.org") || nacsUser.Email.Contains("@nacsonline.com"))
                        {
                            email = nacsUser.Email;
                        }
                        else
                        {
                            email = nacsUser.Email + "zzz";
                        }
                    }

                    userObj.FirstName = nacsUser.FirstName;

                    userObj.LastName = nacsUser.LastName;
                    userObj.Email = email;
                    //userObj.FullName = userObj.FullName;
                    userObj.SetValue("FullName", userObj.FullName);

                    //Functionality for the UserMessagingNotificationEmail was removed in Kentico 12. 
                    //userObj.UserMessagingNotificationEmail = email;

                    if (!string.IsNullOrEmpty(nacsUser.MiddleName))
                        userObj.SetValue("MiddleName", nacsUser.MiddleName);

                    if (!string.IsNullOrEmpty(nacsUser.NickName))
                        userObj.SetValue("UserNickName", nacsUser.NickName);

                    if (!string.IsNullOrEmpty(nacsUser.Username))
                        userObj.SetValue("netForumUsername", nacsUser.Username);

                    if (!string.IsNullOrEmpty(nacsUser.IndividualId))
                        userObj.SetValue("RecNo", nacsUser.IndividualId);

                    if (!string.IsNullOrEmpty(nacsUser.IndividualKey))
                        userObj.SetValue("CstKey", nacsUser.IndividualKey);

                    if (!string.IsNullOrEmpty(nacsUser.ProtechNumber))
                        userObj.SetValue("ProtechNumber", nacsUser.ProtechNumber);

                    if (!string.IsNullOrEmpty(nacsUser.ProtechId))
                        userObj.SetValue("ProtechId", nacsUser.ProtechId);

                    if (!string.IsNullOrEmpty(nacsUser.Title))
                        userObj.SetValue("Title", nacsUser.Title);

                    if (!string.IsNullOrEmpty(nacsUser.MobilePhone))
                        userObj.SetValue("MobilePhone", nacsUser.MobilePhone);

                    if (!string.IsNullOrEmpty(nacsUser.PostalCode))
                        userObj.SetValue("Zip", nacsUser.PostalCode);

                    if (!string.IsNullOrEmpty(nacsUser.CompanyName))
                        userObj.SetValue("CompanyName", nacsUser.CompanyName);

                    if (!string.IsNullOrEmpty(nacsUser.AddressLine1))
                    {
                        userObj.SetValue("AddressLine1", nacsUser.AddressLine1);
                    }
                    else
                    {
                        userObj.SetValue("AddressLine1", "");
                    }

                    if (!string.IsNullOrEmpty(nacsUser.AddressLine2))
                        userObj.SetValue("AddressLine2", nacsUser.AddressLine2);

                    if (!string.IsNullOrEmpty(nacsUser.City))
                        userObj.SetValue("City", nacsUser.City);

                    if (!string.IsNullOrEmpty(nacsUser.State))
                        userObj.SetValue("State", nacsUser.State);

                    if (!string.IsNullOrEmpty(nacsUser.Country))
                        userObj.SetValue("Country", nacsUser.Country);

                    if (!string.IsNullOrEmpty(nacsUser.Phone))
                        userObj.SetValue("Phone", nacsUser.Phone);

                    if (!string.IsNullOrEmpty(nacsUser.PhoneExtension))
                        userObj.SetValue("PhoneExtension", nacsUser.PhoneExtension);

                    //CountryInfo country = CountryInfoProvider.GetCountries().WhereEquals("CountryDisplayName", nacsUser.Country).FirstOrDefault();
                    var country = countryInfoProvider.Get().WhereEquals("CountryDisplayName", nacsUser.Country).FirstOrDefault();
                    if (country != null)
                    {
                        userObj.SetValue("CountryID", country.CountryID);
                    }

                    var state = stateInfoProvider.Get(nacsUser.State);

                    if (state == null)
                    {
                        state = StateInfoProvider.GetStateInfoByCode(nacsUser.State);
                    }

                    if (state != null)
                    {
                        userObj.SetValue("StateID", state.StateID);
                    }

                    // Save the user to the database
                    //UserInfoProvider.SetUserInfo(userObj);
                    userInfoProvider.Set(userObj);
                    //userObj = UserInfoProvider.GetUserInfo(protechNum);
                    userObj = userInfoProvider.Get(protechNum);

                    #region Roles



                    //ADD/REMOVE ROLES, ADD USER TO ROLES------------------------------------------------------------
                    //Add role to Kentico if it does not exist yet.
                    //Add user to role(s) in Kentico.


                    List<string> nfRoles = new List<string>();

                    try
                    {
                        NACS.Helper.AuthService.NACSRole[] roles = authService.AuthProvider_GetAllRolesForUser(protechNum, NACSAPIKey);

                        //1. Add roles
                        foreach (NACS.Helper.AuthService.NACSRole role in roles)
                        {
                            try
                            {
                                nfRoles.Add(role.Role.ToLower());
                                string cleanRole = CleanRoleName(role.Role);

                                if (!roleInfoProvider.Equals(cleanRole))
                                {
                                    // Creates a new role object
                                    RoleInfo newRole = new RoleInfo();

                                    // Sets the role properties
                                    newRole.RoleDisplayName = role.Role;
                                    newRole.RoleName = cleanRole;
                                    newRole.RoleDescription = "netFORUM role";

                                    // Saves the role to the database
                                    //RoleInfoProvider.SetRoleInfo(newRole);
                                    roleInfoProvider.Set(newRole);
                                }

                                if (!roleInfoProvider.Equals(userObj))
                                {
                                    // Add the user to the role
                                    //UserInfoProvider.AddUserToRole(userObj.UserName, cleanRole, "");
                                    userInfoProvider.Set(userObj);

                                }
                            }
                            catch
                            { //skips over bad data
                            }
                        }

                        //2. REMOVE USER FROM ROLES--------------------------------------------------------------
                        //Grab all roles the user is in, in Kentico. If role is not in GetAllRoleForUser from AMS, remove

                        //var userRoleIDs = UserRoleInfoProvider.GetUserRoles().Column("RoleID").WhereEquals("UserID", userObj.UserID);
                        var userRoleIDs = userRoleInfoProvider.Get().Column("RoleID").WhereEquals("UserID", userObj.UserID);
                        //var kenticoRoles = RoleInfoProvider.GetRoles().WhereIn("RoleID", userRoleIDs);
                        var kenticoRoles = roleInfoProvider.Get().WhereIn("RoleID", userRoleIDs);

                        foreach (RoleInfo kenticoRole in kenticoRoles)
                        {
                            if (kenticoRole.RoleDescription.ToLower().Contains("netforum role") && !nfRoles.Contains(kenticoRole.RoleDisplayName.ToLower()))
                            {
                                //UserInfoProvider.RemoveUserFromRole(userObj.UserName, kenticoRole.RoleName, "");
                                userRoleInfoProvider.Remove(userObj.UserID, kenticoRole.RoleID);

                            }
                        }

                    }
                    catch
                    { //skips over bad call to API, or empty roles set
                    }




                    //NOTE: It never removes old roles, just in case

                    #endregion

                    //Got Issues of namespace CMS.Ecommerce so need to rewrite in future as per discussion with Tara and Mike

                    //#region Customer
                    ////var customer = contactInfoProvider.Get(userObj.UserID);//.GetCustomerInfoByUserID(userObj.UserID);
                    //var customer = CustomerInfoProvider.GetCustomerInfoByUserID(userObj.UserID);

                    //if (customer == null)
                    //{
                    //    customer = new customerinfo
                    //    {
                    //        customerfirstname = nacsuser.firstname,
                    //        customerlastname = nacsuser.lastname,
                    //        customeremail = email,
                    //        customeruserid = userobj.userid,
                    //        customerphone = nacsuser.phone,
                    //        customercompany = nacsuser.companyname
                    //    };

                    //    customerinfoprovider.setcustomerinfo(customer);
                    //    customer = customerinfoprovider.getcustomerinfobyuserid(userobj.userid);
                    //}
                    //else
                    //{
                    //    customer.customerfirstname = nacsuser.firstname;
                    //    customer.customerlastname = nacsuser.lastname;
                    //    customer.customeremail = email;
                    //    customer.customeruserid = userobj.userid;
                    //    customer.customerphone = nacsuser.phone;
                    //    customer.customercompany = nacsuser.companyname;

                    //    customerinfoprovider.setcustomerinfo(customer);
                    //    customer = customerinfoprovider.getcustomerinfobyuserid(userobj.userid);
                    //}

                    ////BSM changed on 3/24/21 to handle no-state country addresses
                    ////if (state != null && country != null)
                    //if (country != null)
                    //{
                    //    int stID = (state == null) ? 0 : state.StateID;

                    //    addressinfo address = addressinfoprovider.getaddresses()
                    //    .whereequals("addresscustomerid", customer.customerid)
                    //    .and()
                    //    .whereequals("addressline1", nacsuser.addressline1)
                    //    .and()
                    //    .whereequals("addressline2", nacsuser.addressline2)
                    //    .and()
                    //    .whereequals("addresscity", nacsuser.city)
                    //    .and()
                    //    .whereequals("addresszip", nacsuser.postalcode)
                    //    .and()
                    //    .whereequals("addressstateid", stid)
                    //    .and()
                    //    .whereequals("addresscountryid", country.countryid).firstordefault();

                    //    if (address == null && !nacsuser.addressline1.isnullorempty())
                    //    {
                    //        // creates a new address object and sets its properties
                    //        address = new addressinfo
                    //        {
                    //            addressname = nacswebpart.buildaddressname(nacsuser.addressline1, nacsuser.addressline2, nacsuser.city, nacsuser.state, nacsuser.postalcode),
                    //            addressline1 = nacsuser.addressline1,
                    //            addressline2 = nacsuser.addressline2,
                    //            addresscity = nacsuser.city,
                    //            addresszip = nacsuser.postalcode,
                    //            addresspersonalname = customer.customerinfoname,
                    //            addresscustomerid = customer.customerid,
                    //            addresscountryid = country.countryid,
                    //            addressstateid = stid
                    //        };

                    //        // saves the address to the dataabase
                    //        addressinfoprovider.setaddressinfo(address);
                    //    }
                    //}

                    //#endregion

                    #region Contact

                    output += "Checkpoint 1: " + DateTime.Now.ToString() + "<br/>";

                    //ContactInfo contact = ContactInfoProvider.GetContactInfo(email);  //26 seconds! BSM 11/19/21
                    //ContactInfo contact = ContactInfoProvider.GetContacts().WhereEquals("ContactEmail", email).FirstObject; //Same result = 26 seconds! BSM 11/19/21
                    ContactInfo contact = ContactInfoProvider.GetContactInfo(email); //Same result = 26 seconds! BSM 11/19/21
                    output += "Checkpoint 2: " + DateTime.Now.ToString() + "<br/>";

                    if (contact == null)
                    {
                        if (isCurrentUser && httpContextAccessor.HttpContext.Request.Cookies["CurrentContact"] != null)
                        {
                            // convert anonymous contact
                            //string contactId = HttpContext.Current.Request.Cookies["CurrentContact"].Value;
                            string contactId = HttpContext.Request.Cookies["CurrentContact"];
                            contact = ContactInfoProvider.GetContactInfo(new Guid(contactId).ToString());
                            //if (contact.Users != null && contact.Users.Count > 0)
                            if (contact != null)
                            {
                                // contact belongs to another user
                                contact = null;
                            }
                        }

                        if (contact == null)
                        {
                            contact = new ContactInfo()
                            {
                                ContactLastName = nacsUser.LastName,
                                ContactFirstName = nacsUser.FirstName,
                                ContactCompanyName = nacsUser.CompanyName,
                                ContactEmail = email,
                                ContactJobTitle = (nacsUser.Title.Length > 50) ? nacsUser.Title.Substring(0, 50) : nacsUser.Title, //System default is 50 characters
                                ContactMonitored = true
                            };

                            if (nacsUser.AddressLine1 != null)
                            {
                                contact.ContactAddress1 = nacsUser.AddressLine1;
                            }
                            else
                            {
                                contact.ContactAddress1 = "";
                            }
                            if (nacsUser.City != null)
                                contact.ContactCity = nacsUser.City;
                            if (nacsUser.PostalCode != null)
                                contact.ContactZIP = nacsUser.PostalCode;
                            if (nacsUser.MiddleName != null)
                                contact.ContactMiddleName = nacsUser.MiddleName;
                            if (nacsUser.MobilePhone != null)
                                contact.ContactMobilePhone = nacsUser.MobilePhone;
                            if (state != null)
                                contact.ContactStateID = state.StateID;
                            if (country != null)
                                contact.ContactCountryID = country.CountryID;

                            // Saves the contact to the database
                            //ContactInfoProvider.SetContactInfo(contact);
                            contactInfoProvider.Set(contact);

                            contact = ContactInfoProvider.GetContactInfo(email);
                        }

                        //Got Issues of namespace CMS.Ecommerce so need to rewrite in future as per discussion with Tara and Mike
                        //if (userObj != null)
                        //{
                        //    Service.Resolve<IContactRelationAssigner>().Assign(
                        //        userObj.UserID,
                        //        MemberTypeEnum.CmsUser,
                        //        contact.ContactID);
                        //}
                        //if (userObj != null)
                        //{
                        //    contactInfoProvider.Set(contact);
                        //}

                        //if (customer != null)
                        //{
                        //    Service.Resolve<IContactRelationAssigner>().Assign(
                        //        customer.CustomerID,
                        //        MemberTypeEnum.EcommerceCustomer,
                        //        contact.ContactID);
                        //}
                        //if (userObj != null)
                        //{
                        //    contactInfoProvider.Set(contact);
                        //}

                        ////For customers
                        //if (customer != null)
                        //{
                        //    customer.CustomerID = contact.ContactID;

                        //    CustomerContactInfoProvider.AddContactToCustomer(contact.ContactID, customer.CustomerID);
                        //}

                    }
                    else
                    {
                        contact.ContactLastName = nacsUser.LastName;
                        contact.ContactFirstName = nacsUser.FirstName;

                        if (nacsUser.AddressLine1 != null)
                        {
                            contact.ContactAddress1 = nacsUser.AddressLine1;
                        }
                        else
                        {
                            contact.ContactAddress1 = "";
                        }
                        if (nacsUser.City != null)
                            contact.ContactCity = nacsUser.City;
                        if (nacsUser.PostalCode != null)
                            contact.ContactZIP = nacsUser.PostalCode;
                        if (nacsUser.MiddleName != null)
                            contact.ContactMiddleName = nacsUser.MiddleName;
                        if (nacsUser.MobilePhone != null)
                            contact.ContactMobilePhone = nacsUser.MobilePhone;
                        if (state != null)
                            contact.ContactStateID = state.StateID;
                        if (country != null)
                            contact.ContactCountryID = country.CountryID;

                        contact.ContactCompanyName = nacsUser.CompanyName;
                        contact.ContactEmail = email;
                        contact.ContactJobTitle = (nacsUser.Title.Length > 50) ? nacsUser.Title.Substring(0, 50) : nacsUser.Title; //System default is 50 characters

                        //ContactInfoProvider.SetContactInfo(contact);
                        contactInfoProvider.Set(contact);
                    }

                    output += "Checkpoint 3: " + DateTime.Now.ToString() + "<br/>";

                    var httpContext = httpContextAccessor.HttpContext;
                    if (contact != null && httpContext != null)
                    {
                        //var cookie = new HttpCookie("CurrentContact", contact.ContactGUID.ToString());
                        //Response.Cookies.Add(cookie);
                        httpContext.Response.Cookies.Append("CurrentContact", contact.ContactGUID.ToString());
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                output = ex.Message.ToString() + ", " + output;
            }

            return output;
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

using NACS.Helper.MaritzExportService;
using NACS.Protech.Framework;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NACSShow.Components.Widgets.LoginRetrievalWidget
{
    public class LoginRetrievalFormPostController : Controller
    {
        protected string NACSAPIKey = GetNACSAPIKey();

        private static readonly string encryptionKey = "6789RESET@@PASSWORD12345"; //needs to be 16 or 24 characters long, and match Change Password Control
        private string origin = "";

        //[HttpGet]
        //public IActionResult Index(LoginRetrievalWidgetViewModel model)
        //{
        //    model.IsPostback = false;
        //    return View();
        //}

        [HttpPost]
        public IActionResult Index([FromForm]LoginRetrievalWidgetViewModel model)
        {
            model.IsPostback = true;
            
            var errorMsg = model.ErrorMsg;
            var username = model.Username;
            var passwordResetUrl = model.PasswordResetUrl;

            CurrentAttendeeUser? dt = GetCurrentAttendee(model.ConfirmationNumber.Trim(), model.LastName.Trim(), errorMsg);
            //StringBuilder sb = new StringBuilder();
            //Image img = new Image();
            //img.CssClass = "checkmark";

            //_pnlLogin.Controls.Clear();

            if (dt != null)
            {

                if (!string.IsNullOrEmpty(dt.ContactNumber))
                {

                    ContactRepository conRepo = new ContactRepository();
                    var contact = conRepo.GetByNumber(dt.ContactNumber);

                    if (contact != null)
                    {
                        string resetkey = "nacsshow2015" + dt.ContactNumber;
                        username = contact.WebLoginName;
                        passwordResetUrl = "/reset-password?nacsid=" + dt.ContactNumber + "&src=" + origin + "&resetkey=" + Encrypt(resetkey, false);

                        //_pnlLoginFound.Visible = true;
                        errorMsg = "";
                    }
                    else
                    {
                        errorMsg = "<i class='fa fa-info-circle fa-pull-left'></i>&nbsp;Attendee found, but there may be a sync issue with your record. Please try again or contact NACS for help.";
                        //_pnlLoginFound.Visible = false;
                    }
                }
                else
                {
                    if (dt.RegType.StartsWith("X"))
                    {
                        errorMsg = "<i class='fa fa-info-circle'></i>&nbsp;As an exhibitor, you do not have a login assigned automatically. Please contact <a href='mailto:loginhelp@convenience.org'>loginhelp@convenience.org</a> for assistance.";
                    }
                    else
                    {
                        errorMsg = "<i class='fa fa-info-circle'></i>&nbsp;Attendee record found, but you most likely do not have a login yet. Please contact <a href='mailto:loginhelp@convenience.org'>loginhelp@convenience.org</a> for assistance.";
                    }
                    //_pnlLoginFound.Visible = false;
                }
            }
            else
            {
                errorMsg = "<i class='fa fa-info-circle'></i>&nbsp;Attendee not found. Please try again.";
                //_pnlLoginFound.Visible = false;
            }

            var updatedModel = new LoginRetrievalWidgetViewModel
            {
                IsPostback = model.IsPostback,
                Username = username,
                PasswordResetUrl = passwordResetUrl,
                ErrorMsg = errorMsg
            };

            return PartialView("~/Components/Widgets/LoginRetrievalWidget/_IndexPartial.cshtml", updatedModel);
        }

        //private NACSIndividual GetPerson(string VendorAccountID, string Email, string LastName)
        //{
        //    NACSAPIAuthenticationSoapClient service = new NACSAPIAuthenticationSoapClient();
        //    NACSUser user = service.User_GetByVendorAccountID(VendorAccountID, LastName, this.NACSAPIKey);

        //    if (user != null)
        //        return service.Individual_GetById(user.IndividualId, user.IndividualKey, this.NACSAPIKey);

        //    return null;
        //}

        private CurrentAttendeeUser? GetCurrentAttendee(string VendorAccountID, string LastName, string errorMsg)
        {
            CurrentAttendeeUser user = new();

            //Get Maritz Data
            #region Maritz

            try
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress("http://ews.experientevent.com/RealTimeServices/Export.asmx");
                ExportSoapClient ws = new ExportSoapClient(binding, endpoint);

                DataExportHeader AuthHeader = new DataExportHeader();

                AuthHeader.HeaderShowCode = "NAC231";
                AuthHeader.HeaderUsername = "brendanmoyer";
                AuthHeader.HeaderPassword = "nacs2021!";
                AuthHeader.HeaderSQLEnvironment = SQLEnvironment.PROD;
                AuthHeader.PagedResultsPageSize = 5000;
                AuthHeader.UserAccountDomain = "";

                string results = "";
                string error = "";
                string regid = "";

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                results = ws.PullRegistrantByID(AuthHeader, Convert.ToInt32(VendorAccountID));

                doc.LoadXml(results);

                error = this.RetrieveErrors(doc);

                if (error.Length > 0)
                {
                    errorMsg = error;
                    return null;
                }
                else if (doc.DocumentElement != null && doc.DocumentElement.ChildNodes.Count > 0)
                {
                    var firstChild = doc.DocumentElement.ChildNodes[0];
                    if (firstChild != null)
                    {
                        foreach (XmlNode node in firstChild.ChildNodes)
                        {
                            if (node.Name == "RegistrantID")
                            {
                                user.RegistrationId = node.InnerText;
                                regid = node.InnerText;
                            }
                            else if (node.Name == "MemberNumber")
                            {
                                user.ContactNumber = node.InnerText;
                            }
                            else if (node.Name == "LastName")
                            {
                                user.LastName = node.InnerText;
                            }
                            else if (node.Name == "NickName")
                            {
                                user.BadgeName = node.InnerText;
                            }
                            else if (node.Name == "RegState")
                            {
                                user.RegStatus = node.InnerText;
                            }
                            else if (node.Name == "IsPended")
                            {
                                user.RegPending = Convert.ToBoolean(node.InnerText);
                            }
                            else if (node.Name == "RegTypeCode")
                            {
                                user.RegType = node.InnerText;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message.ToString();
                return null;
            }

            #endregion

            if (user.LastName.ToLower() == LastName.ToLower())
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        private string RetrieveErrors(XmlDocument doc)
        {
            StringBuilder ErrorList = new StringBuilder();

            if (doc.DocumentElement == null || !doc.DocumentElement.HasChildNodes)
                return "[Warning] No records exist for the given criteria.";

            var firstChild = doc.DocumentElement.FirstChild;
            if (firstChild == null || firstChild.Name != "Errors")
                return "";

            foreach (XmlNode ErrorNode in firstChild.ChildNodes)
            {
                ErrorList.Append(ErrorNode.InnerText);
                ErrorList.Append(System.Environment.NewLine);
            }

            return ErrorList.ToString();
        }

        // Inside the Encrypt method
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt); // Use Encoding.UTF8

            // If hashing use get hashcode regards to your key
            if (useHashing)
            {
                using (MD5 hashmd5 = MD5.Create())
                {
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey)); // Use Encoding.UTF8
                }
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(encryptionKey); // Use Encoding.UTF8
            }

            // Set the secret key for the tripleDES algorithm
            using (TripleDES tdes = TripleDES.Create())
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                // Transform the specified region of bytes array to resultArray
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                // Return the encrypted data into unreadable string format
                //NOTE: The old code used HttpServerUtility.UrlTokenEncode, but it is not available in .NET Core so we are switching these calls to use WebEncoders.Base64UrlEncode
                return WebEncoders.Base64UrlEncode(resultArray);
            }
        }

        // Inside the Decrypt method
        //public static string Decrypt(string cipherString, bool useHashing)
        //{
        //    byte[] keyArray;
        //    //NOTE: The old code used HttpServerUtility.UrlTokenDecode, but it is not available in .NET Core so we are switching these calls to use WebEncoders.Base64UrlDecode
        //    byte[] toEncryptArray = WebEncoders.Base64UrlDecode(cipherString);

        //    if (useHashing)
        //    {
        //        // If hashing was used get the hash code with regards to your key
        //        using (MD5 hashmd5 = MD5.Create())
        //        {
        //            keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey)); // Use Encoding.UTF8
        //        }
        //    }
        //    else
        //    {
        //        // If hashing was not implemented get the byte code of the key
        //        keyArray = Encoding.UTF8.GetBytes(encryptionKey); // Use Encoding.UTF8
        //    }

        //    // Set the secret key for the tripleDES algorithm
        //    using (TripleDES tdes = TripleDES.Create())
        //    {
        //        tdes.Key = keyArray;
        //        tdes.Mode = CipherMode.ECB;
        //        tdes.Padding = PaddingMode.PKCS7;

        //        ICryptoTransform cTransform = tdes.CreateDecryptor();
        //        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        //        // Return the Clear decrypted TEXT
        //        return Encoding.UTF8.GetString(resultArray); // Use Encoding.UTF8
        //    }
        //}

        private static string GetNACSAPIKey()
        {
            return ConfigurationManager.AppSettings["NACSAPIKey"] ?? string.Empty;
        }
    }

    class CurrentAttendeeUser
    {
        public string ContactNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string BadgeName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public bool IsRegistered { get; set; }
        public string RegType { get; set; } = string.Empty;
        public string RegistrationId { get; set; } = string.Empty;
        public string RegStatus { get; set; } = string.Empty;
        public bool RegPending { get; set; }
    }
}


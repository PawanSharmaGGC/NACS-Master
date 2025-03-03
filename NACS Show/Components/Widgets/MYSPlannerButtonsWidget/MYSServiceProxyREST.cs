using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;



namespace MYSServiceProxyREST
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class MYSExhibitor
    {
        public string exhid { get; set; } = string.Empty;
        public string package { get; set; } = string.Empty;
        public string exhname { get; set; } = string.Empty;
        public string sortkey { get; set; } = string.Empty;
        public string alpha { get; set; } = string.Empty;
        public string address1 { get; set; } = string.Empty;
        public string address2 { get; set; } = string.Empty;
        public string address3 { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
        public string zip { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string phone2 { get; set; } = string.Empty;
        public string phone3 { get; set; } = string.Empty;
        public string fax { get; set; } = string.Empty;
        public string fax2 { get; set; } = string.Empty;
        public string website { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string isactive { get; set; } = string.Empty;
        public string dateadded { get; set; } = string.Empty;
        public string newexhibitor { get; set; } = string.Empty;
        public string twitter { get; set; } = string.Empty;
        public string linkedin { get; set; } = string.Empty;
        public string facebook { get; set; } = string.Empty;
        public string name1 { get; set; } = string.Empty;
        public string name2 { get; set; } = string.Empty;
        public string orgid { get; set; } = string.Empty;
        public string approvestatus { get; set; } = string.Empty;
        public string logo { get; set; } = string.Empty;
        public List<MYSBooth> booths { get; set; } = [];
        public List<MYSContact> contacts { get; set; } = [];
        public List<MYSProductCategories> productcategories { get; set; } = [];  
    }

    public class MYSSavedExhibitor
    {
        public string dateadded { get; set; } = string.Empty;
        public string exhid { get; set; } = string.Empty;
    }

    public class MYSExhibitorStub
    {
        public string exhid { get; set; } = string.Empty;
        public string exhstatus { get; set; } = string.Empty;
    }

    public class MYSBoothExhibitor
    {
        public string exhid { get; set; } = string.Empty;
        public string boothtype { get; set; } = string.Empty;
    }

    public class MYSContact
    {
        public string Zip { get; set; } = string.Empty;
        public string Lname { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Fname { get; set; } = string.Empty;
        public string Altid { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Phone2 { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Address3 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
    }

    public class MYSBooth
    {
        public string hallid { get; set; } = string.Empty;
        public string dimensions { get; set; } = string.Empty;
        public string boothnumber { get; set; } = string.Empty;
        public string boothtype { get; set; } = string.Empty;
        public string hall { get; set; } = string.Empty;
        public string squareft { get; set; } = string.Empty;
        public string pavilion { get; set; } = string.Empty;
        public string boothtypedisplay { get; set; } = string.Empty;
    }

    public class MYSProductCategories
    {
        public string parentcategoryid { get; set; } = string.Empty;
        public string toplevelparent { get; set; } = string.Empty;
        public string categoryid { get; set; } = string.Empty;
        public string categoryname { get; set; } = string.Empty;
        public string categorydisplay { get; set; } = string.Empty;
    }

    public class MYSSession
    {
        public string location { get; set; } = string.Empty;
        public string sessionidshow { get; set; } = string.Empty;
        public string date { get; set; } = string.Empty;
        public string sessionid { get; set; } = string.Empty;
        public string starttime { get; set; } = string.Empty;
        public string scheduleid { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string endtime { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string level { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
    }

    public class MYSSavedSession
    {
        public string sessionidshow { get; set; } = string.Empty;
        public string dateinserted { get; set; } = string.Empty;
        public string sessionid { get; set; } = string.Empty;
        public string scheduleid { get; set; } = string.Empty;
    }

    public class MYSPlannerUser
    {
        public string lname { get; set; } = string.Empty;
        public string altregid { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
        public string regid { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string fname { get; set; } = string.Empty;
        public string company { get; set; } = string.Empty;
        public string allowexhcontact { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string validated { get; set; } = string.Empty;
        public string fullname { get; set; } = string.Empty;
        public string prodid { get; set; } = string.Empty;
        public string error { get; set; } = string.Empty;
    }

    public class MYSServiceProxyREST
    {

        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }

        public MYSServiceProxyREST(bool useprod)
        {
            endPoint = (useprod) ? "https://api.mapyourshow.com/mysRest/v2/" : "https://api.mysstaging.com/mysRest/v2/"; // ConfigurationManager.AppSettings["MYSRESTAPIURL"].ToString();
            httpMethod = httpVerb.GET;
        }

        public string ValidateUser(string username, string password, string showcode)
        {
            string strResponse = string.Empty;
            string authtoken = string.Empty;

            string endPointURL = endPoint + "Authorize?showCode=" + showcode;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["username"] = username;
            request.Headers["password"] = password;
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(username + ":" + password));

            strResponse = MakeRequest(request);

            DataTable? dt = JsonConvert.DeserializeObject<DataTable>(strResponse);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    authtoken = dr["mysGUID"]?.ToString() ?? string.Empty;
                }
            }

            return authtoken;
        }

        public List<MYSExhibitorStub> GetModifiedExhibitors(string authtoken, DateTime start, DateTime end)
        //public string GetModifiedExhibitors(string authtoken, DateTime start, DateTime end)
        {
            //Had to add this manually, since 11/7/17, to connect to MYS service after they switched to TLS 1.2
            //Also had to change to .NET 4.6 in order to get the SecurityProtocolType.Tls12 value
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<MYSExhibitorStub> ds = new List<MYSExhibitorStub>();

            string strResponse = string.Empty;

            string endPointURL = endPoint + "Exhibitors/Modified?fromDate=" + start.ToString("yyyy-MM-dd HH:mm") + "&toDate=" + end.ToString("yyyy-MM-dd HH:mm");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            if (strResponse == "[{\"exhibitors\":[]}]")
            {
                ds = [] ;
            }
            else
            {
                //remove MYS-added text that messes with JSON response format
                strResponse = strResponse.Replace("[{\"exhibitors\":[", "[");
                strResponse = strResponse.Replace("}]}]", "}]");

                ds = JsonConvert.DeserializeObject<List<MYSExhibitorStub>>(strResponse) ?? [];
            }

            //return strResponse;
            return ds;
        }

        public List<MYSSession> GetAllSessions(string authtoken)
        {
            //Had to add this manually, since 11/7/17, to connect to MYS service after they switched to TLS 1.2
            //Also had to change to .NET 4.6 in order to get the SecurityProtocolType.Tls12 value
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<MYSSession> ds = new List<MYSSession>();

            string strResponse = string.Empty;

            string endPointURL = endPoint + "Sessions/List";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            if (strResponse == "[{\"sessions\":[]}]")
            {
                ds = [];
            }
            else
            {
                //remove MYS-added text that messes with JSON response format
                strResponse = strResponse.Replace("[{\"sessions\":[", "[");
                strResponse = strResponse.Replace("}]}]", "}]");

                ds = JsonConvert.DeserializeObject<List<MYSSession>>(strResponse) ?? new List<MYSSession>();
            }

            //return strResponse;
            return ds;
        }

        public List<MYSPlannerUser> GetPlannerUser(string authtoken, string email)
        {
            //Had to add this manually, since 11/7/17, to connect to MYS service after they switched to TLS 1.2
            //Also had to change to .NET 4.6 in order to get the SecurityProtocolType.Tls12 value
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<MYSPlannerUser> ds = new List<MYSPlannerUser>();

            string strResponse = string.Empty;

            string endPointURL = endPoint + "Users?emailAddress=" + email;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            if (strResponse == "[{\"users\":[]}]")
            {
                ds = [];
            }
            else
            {
                //remove MYS-added text that messes with JSON response format
                strResponse = strResponse.Replace("[{\"users\":[", "[");
                strResponse = strResponse.Replace("}]}]", "}]");

                ds = JsonConvert.DeserializeObject<List<MYSPlannerUser>>(strResponse) ?? [];
            }

            //return strResponse;
            return ds;
        }

        public List<MYSSavedSession> GetPlannerSessionsByUser(string authtoken, string email)
        {
            //Had to add this manually, since 11/7/17, to connect to MYS service after they switched to TLS 1.2
            //Also had to change to .NET 4.6 in order to get the SecurityProtocolType.Tls12 value
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<MYSSavedSession> ds = new List<MYSSavedSession>();

            string strResponse = string.Empty;

            string endPointURL = endPoint + "Agendas/Sessions?emailAddress=" + email;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            if (strResponse == "[{\"sessions\":[]}]")
            {
                ds = [];
            }
            else
            {
                //remove MYS-added text that messes with JSON response format
                strResponse = strResponse.Replace("[{\"sessions\":[", "[");
                strResponse = strResponse.Replace("}]}]", "}]");

                ds = JsonConvert.DeserializeObject<List<MYSSavedSession>>(strResponse) ?? [];
            }

            //return strResponse;
            return ds;
        }

        public List<MYSSavedExhibitor> GetPlannerExhibitorsByUser(string authtoken, string email)
        {
            //Had to add this manually, since 11/7/17, to connect to MYS service after they switched to TLS 1.2
            //Also had to change to .NET 4.6 in order to get the SecurityProtocolType.Tls12 value
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<MYSSavedExhibitor> ds = new List<MYSSavedExhibitor>();

            string strResponse = string.Empty;

            string endPointURL = endPoint + "Agendas/Exhibitors?emailAddress=" + email;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            if (strResponse == "[{\"exhibitors\":[]}]")
            {
                ds = [];
            }
            else
            {
                //remove MYS-added text that messes with JSON response format
                strResponse = strResponse.Replace("[{\"exhibitors\":[", "[");
                strResponse = strResponse.Replace("}]}]", "}]");

                ds = JsonConvert.DeserializeObject<List<MYSSavedExhibitor>>(strResponse) ?? [];
            }

            //return strResponse;
            return ds;
        }

        public string GetModifiedExhibitorsRAW(string authtoken, DateTime start, DateTime end)
        {
            //Had to add this manually, since 11/7/17, to connect to MYS service after they switched to TLS 1.2
            //Also had to change to .NET 4.6 in order to get the SecurityProtocolType.Tls12 value
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string strResponse = string.Empty;

            string endPointURL = endPoint + "Exhibitors/Modified?fromDate=" + start.ToString("yyyy-MM-dd HH:mm") + "&toDate=" + end.ToString("yyyy-MM-dd HH:mm");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            return strResponse;
        }

        public List<MYSBoothExhibitor> GetExhibitorsInBooth(string authtoken, string boothnumber)
        {
            string strResponse = string.Empty;

            string endPointURL = endPoint + "Exhibitors?booth=" + boothnumber;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            //remove MYS-added text that messes with JSON response format
            //strResponse = strResponse.Replace("[{\"exhibitor\":", "[");
            //strResponse = strResponse.Replace("}}]", "}]");

            strResponse = strResponse.Replace("[{\"exhibitor\":", "");
            strResponse = strResponse.Replace("}]}]", "}]");

            List<MYSBoothExhibitor> ds = JsonConvert.DeserializeObject<List<MYSBoothExhibitor>>(strResponse) ?? [];

            return ds;
        }

        public string GetExhibitorsInBoothRAW(string authtoken, string boothnumber)
        {
            string strResponse = string.Empty;

            string endPointURL = endPoint + "Exhibitors?booth=" + boothnumber;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            return strResponse;
        }

        public MYSExhibitor GetExhibitorInfo(string authtoken, string exhibid)
        {
            string strResponse = string.Empty;

            string endPointURL = endPoint + "Exhibitors?exhID=" + exhibid;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            //remove MYS-added text that messes with JSON response format
            strResponse = strResponse.Replace("[{\"exhibitor\":", "");
            strResponse = strResponse.Replace("}}]", "}");

            //Cast data into object
            MYSExhibitor? ex = JsonConvert.DeserializeObject<MYSExhibitor>(strResponse);

            return ex ?? new MYSExhibitor();
        }

        public string GetExhibitorInfoRAW(string authtoken, string exhibid)
        {
            string strResponse = string.Empty;

            string endPointURL = endPoint + "Exhibitors?exhID=" + exhibid;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            return strResponse;
        }

        public string Template(string authtoken)
        {
            string strResponse = string.Empty;

            string endPointURL = endPoint + "Authorize";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
            request.Method = httpMethod.ToString();
            request.Headers["Authorization"] = "Bearer " + authtoken;

            strResponse = MakeRequest(request);

            return strResponse;
        }

        public string MakeRequest(HttpWebRequest request)
        {
            //Had to add this manually, since 11/7/17, to connect to MYS service after they switched to TLS 1.2
            //Also had to change to .NET 4.6 in order to get the SecurityProtocolType.Tls12 value
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string strResponseValue = string.Empty;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                using (StreamReader reader = new StreamReader(responseStream))
                                {
                                    strResponseValue = reader.ReadToEnd();

                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ApplicationException("error!");
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = ex.Message.ToString();
            }

            return strResponseValue;
        }

    }
}

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

using NACS.Helper.EventsService;
using NACS.Helper.MaritzExportService;

using System.ServiceModel;
using System.Xml;

using Document = Lucene.Net.Documents.Document;

namespace NACSShow.Components.Widgets.ExhibitorPortal
{
    public class GetAttendeeListForSale
    {

        public static List<NACSShowAttendee> GetAttendees()
        {
            List<NACSShowAttendee> attendees = RetrieveAttendeesList();

            var luceneVersion = LuceneVersion.LUCENE_48;
            var analyzer = new StandardAnalyzer(luceneVersion);
            //TODO: The directory needs customized
            var indexDirectory = FSDirectory.Open(new DirectoryInfo("C:\\temp\\LuceneIndex"));

            CreateIndex(luceneVersion, analyzer, indexDirectory, attendees);

            var queryText = "*:*"; //This value comes from the old GetAttendees() method of the ExhibitorPortal widget

            var searchedAttendees = SearchAttendees(luceneVersion, analyzer, indexDirectory, queryText);

            return searchedAttendees;
        }

        public static List<NACSShowAttendee> RetrieveAttendeesList()
        {
            List<NACSShowAttendee> attendees = [];

            //Maritz API Code
            //#region Maritz API Code
            try
            {
                int pagingErrors = 0;
                int pageErrors = 0;
                int resultsErrors = 0;
                //int attErrors = 0;

                DateTime start = DateTime.Now;

                var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                binding.MaxReceivedMessageSize = 2147483647;
                binding.ReaderQuotas = new XmlDictionaryReaderQuotas()
                {
                    MaxStringContentLength = 2147483647
                };
                var endpoint = new EndpointAddress("https://ews.experientevent.com/RealTimeServices/Export.asmx");

                ExportSoapClient ws = new(binding, endpoint);
                
                DataExportHeader AuthHeader = new DataExportHeader();

                AuthHeader.HeaderShowCode = "NAC231";
                AuthHeader.HeaderUsername = "brendanmoyer";
                AuthHeader.HeaderPassword = "nacs2021!";
                AuthHeader.HeaderSQLEnvironment = SQLEnvironment.PROD; //SQLEnvironment.QA;
                AuthHeader.PagedResultsPageSize = 7900;
                AuthHeader.UserAccountDomain = "";

                string results = "";
                string paging_results = "";
                string error = "";
                string paging_error = "";
                string token = "";

                XmlDocument results_doc = new();

                DateTime startPull = new DateTime(2023, 4, 1);
                DateTime endPull = DateTime.Now;
                XmlDocument paging_doc = new();

                paging_results = ws.InitializePagedPull(AuthHeader, ExportDataType.Registrant, startPull, endPull, "");
                paging_doc.LoadXml(paging_results);
                paging_error = RetrieveErrors(paging_doc);

                //Get Pages
                if (paging_error.Length > 0)
                {
                    pagingErrors++;
                }

                int totalPages = 0;
                if (paging_doc != null)
                {
                    token = paging_doc.DocumentElement?.FirstChild?.Attributes["PageToken"]?.Value ?? string.Empty;
                    //int totalRecords = Convert.ToInt32(paging_doc.DocumentElement.FirstChild.Attributes["TotalRecords"].Value);
                    totalPages = Convert.ToInt32(paging_doc.DocumentElement?.FirstChild?.Attributes["TotalPages"]?.Value ?? "0");
                }

                //loop through each page of results, adding them to list
                for (int p = 1; p <= totalPages; p++)
                {
                    try
                    {
                        results = ws.PullRegistrantList(AuthHeader, startPull, DateTime.Now, token, p); //<registrantlist><registrant><firstname/></registrant></registrantlist>
                        results_doc.LoadXml(results);

                        //Combine and keep duplicates
                        //var combinedWithDups = xml1.Descendants("AllNodes")
                        //.Concat(xml2.Descendants("AllNodes"));

                        error = RetrieveErrors(results_doc);

                        if (error.Length > 0)
                        {
                            resultsErrors++;
                        }
                        else
                        {
                            foreach (XmlNode node in results_doc.DocumentElement.ChildNodes)
                            {
                                NACSShowAttendee new_attendee = new NACSShowAttendee();
                                string regTypeCode = "";
                                string regState = "";
                                string regid = "";
                                bool IsPended = false;
                                int IsCancelled = 0;
                                string FirstName = "";
                                int IsBalanceDue = 0;

                                foreach (XmlNode subnode in node.ChildNodes)
                                {
                                    if (subnode.Name == "RegistrantID")
                                    {
                                        regid = subnode.InnerText;
                                        new_attendee.RegistrantID = subnode.InnerText;
                                    }
                                    else if (subnode.Name == "MemberNumber") { new_attendee.ProtechNumber = subnode.InnerText; }
                                    else if (subnode.Name == "InsertDate") { new_attendee.RegistrationDate = subnode.InnerText; }
                                    else if (subnode.Name == "LastName") { new_attendee.RegistrantLastName = subnode.InnerText; }
                                    else if (subnode.Name == "NickName") { new_attendee.RegistrantBadgeName = subnode.InnerText; }
                                    else if (subnode.Name == "FirstName")
                                    {
                                        FirstName = subnode.InnerText;
                                        new_attendee.RegistrantFirstName = subnode.InnerText;
                                    }
                                    else if (subnode.Name == "Suffix") { new_attendee.Suffix = subnode.InnerText; }
                                    else if (subnode.Name == "Prefix") { new_attendee.Prefix = subnode.InnerText; }
                                    else if (subnode.Name == "Title") { new_attendee.Title = subnode.InnerText; }
                                    else if (subnode.Name == "Company") { new_attendee.CompanyName1 = subnode.InnerText; }
                                    else if (subnode.Name == "Company2") { new_attendee.CompanyName2 = subnode.InnerText; }
                                    else if (subnode.Name == "Address") { new_attendee.AddressLine1 = subnode.InnerText; }
                                    else if (subnode.Name == "Address2") { new_attendee.AddressLine2 = subnode.InnerText; }
                                    else if (subnode.Name == "City") { new_attendee.City = subnode.InnerText; }
                                    else if (subnode.Name == "StateCode") { new_attendee.State = subnode.InnerText; }
                                    else if (subnode.Name == "ZipCode") { new_attendee.PostalCode = subnode.InnerText; }
                                    else if (subnode.Name == "CountryCode") { new_attendee.CountryName = subnode.InnerText; }
                                    else if (subnode.Name == "PhoneCountryPrefix") { new_attendee.PhoneCountryPrefix = subnode.InnerText; }
                                    else if (subnode.Name == "Phone") { new_attendee.PhoneNumber = subnode.InnerText; }
                                    else if (subnode.Name == "PhoneExtension") { new_attendee.PhoneExtension = subnode.InnerText; }
                                    else if (subnode.Name == "RegTypeCode")
                                    {
                                        regTypeCode = subnode.InnerText;
                                        new_attendee.RegistrationTypeCode = subnode.InnerText;
                                    }
                                    else if (subnode.Name == "RegState") // values: COM|INP
                                    {
                                        regState = subnode.InnerText;
                                        new_attendee.RegState = subnode.InnerText;
                                    }
                                    else if (subnode.Name == "IsCancelled") //values: 0|1
                                    {
                                        IsCancelled = Convert.ToInt32(subnode.InnerText);
                                        new_attendee.IsCancelled = Convert.ToInt32(subnode.InnerText);
                                    }
                                    else if (subnode.Name == "IsPended") //values: true|false
                                    {
                                        IsPended = Convert.ToBoolean(subnode.InnerText);
                                        new_attendee.IsPended = Convert.ToBoolean(subnode.InnerText);
                                    }
                                    else if (subnode.Name == "IsBalanceDue") //values: 0|1
                                    {
                                        IsBalanceDue = Convert.ToInt32(subnode.InnerText);
                                        new_attendee.IsBalanceDue = Convert.ToInt32(subnode.InnerText);
                                    }
                                    else if (subnode.Name == "Demographics")
                                    {
                                        foreach (XmlNode subsubnode in subnode.ChildNodes)
                                        {
                                            if (subsubnode.Name == "STORECLASS")
                                            {
                                                if (subsubnode.InnerText == "A")
                                                { new_attendee.StoreClass = "A - 1-10"; }
                                                else if (subsubnode.InnerText == "B")
                                                { new_attendee.StoreClass = "B - 11-50"; }
                                                else if (subsubnode.InnerText == "C")
                                                { new_attendee.StoreClass = "C - 51-200"; }
                                                else if (subsubnode.InnerText == "D")
                                                { new_attendee.StoreClass = "D - 201-500"; }
                                                else if (subsubnode.InnerText == "E")
                                                { new_attendee.StoreClass = "E - Over 500"; }
                                                else
                                                { new_attendee.StoreClass = ""; }
                                            }
                                            else if (subsubnode.Name == "BUSINESSTYPE")
                                            {
                                                if (subsubnode.InnerText == "A")
                                                { new_attendee.BusinessType = "Retailer/Fuel Marketer"; }
                                                else if (subsubnode.InnerText == "B")
                                                { new_attendee.BusinessType = "Retailer (Non-Convenience)"; }
                                                else if (subsubnode.InnerText == "C")
                                                { new_attendee.BusinessType = "Manufacturer for Resale"; }
                                                else if (subsubnode.InnerText == "D")
                                                { new_attendee.BusinessType = "Product/Service/Equipment Provider"; }
                                                else if (subsubnode.InnerText == "E")
                                                { new_attendee.BusinessType = "Broker"; }
                                                else if (subsubnode.InnerText == "F")
                                                { new_attendee.BusinessType = "Fuel Marketer/Jobber"; }
                                                else if (subsubnode.InnerText == "G")
                                                { new_attendee.BusinessType = "Convenience Distributor"; }
                                                else if (subsubnode.InnerText == "H")
                                                { new_attendee.BusinessType = "National Association"; }
                                                else if (subsubnode.InnerText == "I")
                                                { new_attendee.BusinessType = "State Association"; }
                                                else if (subsubnode.InnerText == "J")
                                                { new_attendee.BusinessType = "Media"; }
                                                else if (subsubnode.InnerText == "K")
                                                { new_attendee.BusinessType = "Advertising Agency"; }
                                            }
                                            else if (subsubnode.Name == "ParentProtechNumber") { new_attendee.ParentProtechNumber = subsubnode.InnerText; }
                                        }
                                    }
                                }

                                //Final buyers only
                                if (IsAttendee(regTypeCode) == true && IsPended == false && IsCancelled == 0 && regState == "COM" && IsBalanceDue == 0 && FirstName != "OPTOUT")
                                {
                                    attendees.Add(new_attendee);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        pageErrors++;
                    }
                }

            }
            catch (Exception ex)
            {
                new Exception(ex.ToString());
            }
            finally
            {
                //
            }

            return attendees;
        }

        public static bool IsAttendee(string regTypeCode)
        {
            // Implement the logic to determine if the regTypeCode represents an attendee
            return true; // Placeholder implementation
        }

        public static string RetrieveErrors(XmlDocument doc)
        {
            // Implement the logic to retrieve errors from the XmlDocument
            return ""; // Placeholder implementation
        }

        public static void CreateIndex(LuceneVersion luceneVersion, StandardAnalyzer analyzer, FSDirectory indexDirectory, List<NACSShowAttendee> attendees)
        {


            using var writer = new IndexWriter(indexDirectory, new IndexWriterConfig(luceneVersion, analyzer));

            foreach (var attendee in attendees)
            {
                try
                {
                    #region Retrieve and format attendee

                    string country = "";

                    if (attendee.CountryName == "")
                    { country = "United States"; }
                    else
                    { country = attendee.CountryName; }

                    var content = attendee.RegistrantFirstName + " " + attendee.RegistrantLastName;
                    content += attendee.CompanyName1 + " " + attendee.CompanyName2;
                    content += attendee.RegistrationTypeCode + " ";
                    content += attendee.City + " ";
                    content += attendee.State + " ";
                    content += country + " ";
                    content += attendee.PostalCode;

                    var description = attendee.RegistrationTypeCode + " | ";
                    description += attendee.City + " | ";
                    description += attendee.State + " | ";
                    description += country + " | ";
                    description += attendee.PostalCode + " | ";
                    description += "RegType: " + attendee.BusinessType + " | ";
                    description += "StoreClass: " + attendee.StoreClass;

                    var location = attendee.City + " ";
                    description += attendee.State + " ";
                    description += country;


                    # endregion

                    DateTime updatedDate = DateTime.Now;

                    var doc = new Document
                    {
                        new TextField("Content", content, Field.Store.YES),
                        new StringField("Title", attendee.RegistrantFirstName + " " + attendee.RegistrantLastName, Field.Store.YES),
                        new TextField("Description", description, Field.Store.YES),
                        new TextField("SearchType", "Attendee", Field.Store.YES),
                        new TextField("Location", location, Field.Store.YES),
                        new TextField("AddressLine1", attendee.AddressLine1, Field.Store.YES),
                        new TextField("AddressLine2", attendee.AddressLine2, Field.Store.YES),
                        new TextField("City", attendee.City, Field.Store.YES),
                        new TextField("CompanyName", attendee.CompanyName1 + " " + attendee.CompanyName2, Field.Store.YES),
                        new TextField("CompanyName1", attendee.CompanyName1, Field.Store.YES),
                        new TextField("CompanyName2", attendee.CompanyName2, Field.Store.YES),
                        new TextField("CountryName", country, Field.Store.YES),
                        new TextField("PhoneExtension", attendee.PhoneExtension, Field.Store.YES),
                        new TextField("PhoneNumber", attendee.PhoneNumber, Field.Store.YES),
                        new TextField("PostalCode", attendee.PostalCode, Field.Store.YES),
                        new TextField("Prefix", attendee.Prefix, Field.Store.YES),
                        new TextField("RegistrantBadgeName", attendee.RegistrantBadgeName, Field.Store.YES),
                        new TextField("RegistrantFirstName", attendee.RegistrantFirstName, Field.Store.YES),
                        new TextField("RegistrantID", attendee.RegistrantID, Field.Store.YES),
                        new TextField("RegistrantLastName", attendee.RegistrantLastName, Field.Store.YES),
                        new TextField("RegistrationDate", attendee.RegistrationDate, Field.Store.YES),
                        new TextField("RegistrationType", attendee.RegistrationTypeCode, Field.Store.YES),
                        new TextField("ProtechNumber", attendee.ProtechNumber, Field.Store.YES),
                        new TextField("ParentProtechNumber", attendee.ParentProtechNumber, Field.Store.YES),
                        new TextField("BusinessType", attendee.BusinessType, Field.Store.YES),
                        new TextField("RegState", attendee.RegState, Field.Store.YES),
                        new TextField("State", attendee.State, Field.Store.YES),
                        new TextField("StoreClass", attendee.StoreClass, Field.Store.YES),
                        new TextField("Suffix", attendee.Suffix, Field.Store.YES),
                        new TextField("Title", attendee.Title, Field.Store.YES)
                    };

                    writer.AddDocument(doc);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

        public static List<NACSShowAttendee> SearchAttendees(LuceneVersion luceneVersion, StandardAnalyzer analyzer, FSDirectory indexDirectory, string queryText)
        {
            var results = new List<NACSShowAttendee>();

            using var reader = DirectoryReader.Open(indexDirectory);
            var searcher = new IndexSearcher(reader);

            var parser = new QueryParser(luceneVersion, "Content", analyzer);
            var query = parser.Parse(queryText);

            var hits = searcher.Search(query, 10).ScoreDocs;

            foreach (var hit in hits)
            {
                var foundDoc = searcher.Doc(hit.Doc);
                var attendee = new NACSShowAttendee
                {
                    RegistrantID = foundDoc.Get("RegistrantID"),
                    RegistrantFirstName = foundDoc.Get("RegistrantFirstName"),
                    RegistrantLastName = foundDoc.Get("RegistrantLastName"),
                    CompanyName1 = foundDoc.Get("CompanyName"),
                    City = foundDoc.Get("City"),
                    State = foundDoc.Get("State"),
                    CountryName = foundDoc.Get("CountryName"),
                    PostalCode = foundDoc.Get("PostalCode"),
                    // Add other fields as needed
                };
                results.Add(attendee);
            }

            return results;
        }

        public class NACSShowAttendee
        {
            public string RegistrantID { get; set; } = "";
            public string ProtechNumber { get; set; } = "";
            public string ParentProtechNumber { get; set; } = "";
            public string RegistrationDate { get; set; } = "";
            public string RegistrantLastName { get; set; } = "";
            public string RegistrantBadgeName { get; set; } = "";
            public string RegistrantFirstName { get; set; } = "";
            public string Suffix { get; set; } = "";
            public string Prefix { get; set; } = "";
            public string Title { get; set; } = "";
            public string CompanyName1 { get; set; } = "";
            public string CompanyName2 { get; set; } = "";
            public string AddressLine1 { get; set; } = "";
            public string AddressLine2 { get; set; } = "";
            public string City { get; set; } = "";
            public string State { get; set; } = "";
            public string PostalCode { get; set; } = "";
            public string CountryName { get; set; } = "";
            public string PhoneCountryPrefix { get; set; } = "";
            public string PhoneNumber { get; set; } = "";
            public string PhoneExtension { get; set; } = "";
            public string RegistrationTypeCode { get; set; } = "";
            public string BusinessType { get; set; } = "";
            public string StoreClass { get; set; } = "";
            public string RegState { get; set; } = "";
            public bool IsPended { get; set; } = false;
            public int IsCancelled { get; set; } = 0;
            public int IsBalanceDue { get; set; } = 0;


        }
    } 
}

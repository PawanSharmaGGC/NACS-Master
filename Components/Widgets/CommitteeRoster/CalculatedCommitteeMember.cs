using NACS.Helper.CustomerService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Components.Widgets.CommitteeRoster
{
    public class CalculatedCommitteeMember
    {
        #region class definition
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string IndividualId { get; set; }
        public string ProtechNumber { get; set; }
        public string LastName { get; set; }
        public string CommitteeTitle { get; set; }
        public int Index { get; set; }
        public string TotalStores { get; set; }
        public string Website { get; set; }
        public string AddrLine1 { get; set; }
        public string AddrLine2 { get; set; }
        public string CityStateProvinceZip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public List<string> Committees { get; set; }
        public string Bio { get; set; }
        public string Msg { get; set; }
        public string Stats { get; set; }
        public string BioLinkMargin { get; set; }
        #endregion

        public CalculatedCommitteeMember(NACSCommitteeMember member, int index, List<CommitteeMember> allmembers)
        {

            var startName = DateTime.Now;

            this.Name = member.FirstName + " ";
            if (!string.IsNullOrEmpty(member.MiddleName))
                this.Name += member.MiddleName + " ";
            this.Name += member.LastName;

            this.Email = member.Email;
            this.CommitteeTitle = member.CmteTitle;
            this.Company = member.Company;

            var endName = DateTime.Now;
            TimeSpan tsName = endName - startName;


            var startPage = DateTime.Now;

            //TBD need to update content type mapping for Convenience.CommitteeMember after content migration
            //DocumentQuery pages = tree.SelectNodes("Convenience.CommitteeMember")
            //                .Path("/CommitteePortals/Bios/", PathTypeEnum.Children)
            //                .WhereLike("NACS_ID", member.ProtechNumber.ToString())
            //                .Or()
            //                .WhereLike("NACS_ID", member.IndividualId.ToString())
            //                .TopN(1);



            var endPage = DateTime.Now;
            TimeSpan tsPage = endPage - startPage;

            var startImg = DateTime.Now;

            //try
            //{
            //    if (pages != null)
            //    {
            //        foreach (CMS.DocumentEngine.TreeNode drBio in pages)
            //        {
            //            string bioGUID = drBio.GetValue("RollupImage").ToString();
            //            string imageURL = "/CMSPages/GetFile.aspx?guid=" + bioGUID + "";
            //            this.PhotoUrl = (imageURL != "") ? imageURL : "";
            //            this.BioLinkMargin = (imageURL != "") ? "" : "margin-top:40px;";
            //            this.Bio = drBio.GetStringValue("PageContent", "");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{ //do nought - move on
            //}

            var endImg = DateTime.Now;
            TimeSpan tsImg = endImg - startImg;

            var startData = DateTime.Now;
            string fieldErr = "";

            try
            {
                fieldErr = "1";
                this.FirstName = member.FirstName;
                fieldErr = "2";
                this.LastName = member.LastName;
                fieldErr = "3";
                this.IndividualId = member.IndividualId;
                fieldErr = "4";
                this.Index = index;
                fieldErr = "5";
                this.TotalStores = (member.TotalStores == null) ? "" : member.TotalStores;
                fieldErr = "6";
                this.Website = (member.Website == null) ? "" : member.Website;
                fieldErr = "7";
                this.Title = (member.Title == null) ? "" : member.Title;
                fieldErr = "8";
                this.AddrLine1 = (member.AddrLine1 == null) ? "" : member.AddrLine1;
                fieldErr = "9";
                this.AddrLine2 = (member.AddrLine2 == null) ? "" : member.AddrLine2;
                fieldErr = "10";
                this.CityStateProvinceZip = member.CityStateProvinceZip;
                fieldErr = "11";
                this.Phone = (member.Phone == null) ? "" : member.Phone;
                fieldErr = "12";
                this.Fax = (member.Fax == null) ? "" : member.Fax;
                fieldErr = "";
                this.Committees = new List<string>();
                fieldErr = "13";
                this.ProtechNumber = member.ProtechNumber;
                fieldErr = "14";

                var comms = allmembers.Where(m => m.ContactNumber == member.ProtechNumber);

                foreach (CommitteeMember cm in comms)
                {
                    if (cm.CommitteeName.ToString() != "Past Presidents/Chairman" && cm.CommitteeName.ToString() != "CSX" && !cm.CmteTitle.Contains("Proxy"))
                    {
                        string committee = cm.CommitteeName;

                        if (cm.CmteTitle != "")
                            committee += ", " + cm.CmteTitle;

                        this.Committees.Add(committee);
                    }
                }
            }
            catch (Exception ex)
            { //do nought - move on
                this.Msg = "<br/><span style='font-size:12px;color:#ccc'>Error getting " + member.ProtechNumber + ": " + ex.Message.ToString() + ": field " + fieldErr + "</span>";
            }

            var endData = DateTime.Now;
            TimeSpan tsData = endData - startData;

            try
            {
                this.Stats = "<br/><span style='font-size:12px;color:#ccc'>";
                this.Stats += "N: " + tsName.TotalSeconds.ToString();//.Substring(0, 4);
                this.Stats += " | P: " + tsPage.TotalSeconds.ToString();//.Substring(0, 4);
                this.Stats += " | I: " + tsImg.TotalSeconds.ToString();//.Substring(0, 4);
                this.Stats += " | D: " + tsData.TotalSeconds.ToString();//.Substring(0, 4);
                this.Stats += "</span>";

                this.Stats = "";
            }
            catch (Exception ex)
            {
                this.Msg = "<br/><span style='font-size:12px;color:#ccc'>Problem with stats: " + ex.Message.ToString() + "</span>";
            }
        }
    }
}

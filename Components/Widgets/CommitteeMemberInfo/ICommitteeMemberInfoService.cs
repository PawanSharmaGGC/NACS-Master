using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BalluunApi;
using Convenience.org.Components.Widgets.CommitteeMemberInfo;
using Microsoft.Extensions.Configuration;
using NACS.Helper.CustomerService;

namespace Convenience.org.Components.Widgets.CommitteeListing
{
    public interface ICommitteeMemberInfoService
    {
        Task<CommitteeMemberInfoModel> GetCommitteeMembershipsAsync(string committeeId);
    }

    public class CommitteeMemberInfoService : ICommitteeMemberInfoService
    {
        private readonly NACSAPICustomerSoapClient _apiClient;
        private readonly string _apiKey;

        public CommitteeMemberInfoService(NACSAPICustomerSoapClient apiClient, IConfiguration configuration)
        {
            _apiClient = apiClient;
            _apiKey = configuration["NACSAPIKey"];
        }

        public async Task<CommitteeMemberInfoModel> GetCommitteeMembershipsAsync(string committeeId)
        {
            try
            {
                var memberships = await _apiClient.Committee_MemberDetails_GetByIdAsync(committeeId, _apiKey);
                var cmteInfoModel = new CommitteeMemberInfoModel();
                List<string> committees = new List<string>();
                if (memberships != null && memberships.Length > 0)
                {
                    bool first = true;
                    foreach (var membership in memberships)
                    {
                        if (membership.CommitteeName != "Past Presidents/Chairman" && membership.CommitteeName != "CSX" && !membership.CmteTitle.Contains("Proxy"))
                        {
                            string committee = membership.CommitteeName;

                            if (membership.CmteTitle != "")
                                committee += ", " + membership.CmteTitle;

                            committees.Add(committee);
                        }

                        if (first)
                        {
                            cmteInfoModel.NACSID = membership.IndividualId;
                            cmteInfoModel.FirstName = membership.FirstName;
                            cmteInfoModel.LastName = membership.LastName;
                            cmteInfoModel.Title = membership.Title;
                            cmteInfoModel.Company = membership.Company;
                            cmteInfoModel.Stores = membership.TotalStores;
                            cmteInfoModel.Website = membership.Website;
                            cmteInfoModel.Address1 = membership.AddrLine1;
                            cmteInfoModel.Address2 = membership.AddrLine2;
                            cmteInfoModel.CityStateZip = membership.CityStateProvinceZip;
                            cmteInfoModel.Phone = membership.Phone;
                            cmteInfoModel.Fax = membership.Fax;
                            cmteInfoModel.Email = membership.Email;
                        }

                        first = false;
                    }
                }
                return cmteInfoModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NACS.Helper.CustomerService;
using Org.BouncyCastle.Tls.Crypto;

namespace Convenience.org.Components.Widgets.CommitteeRoster
{
    public interface ICommitteeRosterService
    {
        Task<CommitteeRosterViewModel> GetCommitteeRosterMembersAsync(string committeeId);
    }

    public class CommitteeRosterService : ICommitteeRosterService
    {
        private readonly NACSAPICustomerSoapClient _apiClient;
        private readonly string _apiKey;

        public CommitteeRosterService(NACSAPICustomerSoapClient apiClient, IConfiguration configuration)
        {
            _apiClient = apiClient;
            _apiKey = configuration["NACSAPIKey"];
        }

        public async Task<CommitteeRosterViewModel> GetCommitteeRosterMembersAsync(string _committeeId)
        {
            var committerRosterListing = new CommitteeRosterViewModel();

            //Pull list of all committee members for helping to populate the bio section of each person
            List<CommitteeMember> allmembers = GetAllCommitteeMembers();

            try
            {
             
                NACSCommitteeMember[] cmteMembers = _apiClient.Committee_GetById(_committeeId, _apiKey).OrderBy(c => c.CmtePositionRank).ThenBy(c => c.LastName).ThenBy(c => c.FirstName).ToArray();

                Dictionary<string, List<CalculatedCommitteeMember>> groupings = new Dictionary<string, List<CalculatedCommitteeMember>>();
                int index = 1;

                foreach (NACSCommitteeMember member in cmteMembers)
                {
                    if (!member.CmtePositionDesc.Contains("Proxy"))
                    {
                        //if (!String.IsNullOrEmpty(member.Email))
                        //    emailAddresses += member.Email.ToString() + ";";

                        if (member.CmteTitle != null)
                        {
                            if (!groupings.ContainsKey(member.CmteTitle))
                            {
                                groupings.Add(member.CmteTitle, new List<CalculatedCommitteeMember>());
                            }

                            groupings[member.CmteTitle].Add(new CalculatedCommitteeMember(member, index, allmembers));
                            index++;
                        }
                    }
                }

                committerRosterListing.Groupings = groupings;
            }
            catch (Exception ex)
            {
                return null;
            }
            return committerRosterListing;
        }

        private List<CommitteeMember> GetAllCommitteeMembers()
        {
            List<CommitteeMember> list = new List<CommitteeMember>();
            NACSCommitteeMember[] AllCommMembers = _apiClient.Committee_Members_GetAll(_apiKey).OrderBy(c => c.ProtechNumber).ToArray();

            foreach (var member in AllCommMembers)
            {
                list.Add(new CommitteeMember
                {
                    ContactNumber = member.ProtechNumber,
                    CommitteeName = member.CommitteeName,
                    CmteTitle = member.CmteTitle
                });
            }
            return list;

        }

    }

}

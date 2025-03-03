using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NACS.Helper.CustomerService;

namespace Convenience.org.Components.Widgets.CommitteeListing
{
    public interface ICommitteeService
    {
        Task<List<CommitteeMemberViewModel>> GetCommitteeMembersAsync(string committeeId);
    }

    public class CommitteeService : ICommitteeService
    {
        private readonly NACSAPICustomerSoapClient _apiClient;
        private readonly string _apiKey;

        public CommitteeService(NACSAPICustomerSoapClient apiClient, IConfiguration configuration)
        {
            _apiClient = apiClient;
            _apiKey = configuration["NACSAPIKey"]; 
        }

        public async Task<List<CommitteeMemberViewModel>> GetCommitteeMembersAsync(string committeeId)
        {
            try
            {
                var members = await _apiClient.Committee_GetByIdAsync(committeeId, _apiKey);

                return members
                    .Where(m => !m.CmtePositionDesc.Contains("Proxy"))
                    .OrderBy(m => m.CmtePositionRank)
                    .ThenBy(m => m.CmtePositionDesc)
                    .ThenBy(m => m.LastName)
                    .ThenBy(m => m.FirstName)
                    .Select(m => new CommitteeMemberViewModel
                    {
                        Position = m.CmtePositionDesc,
                        Name = $"{m.FirstName} {(!string.IsNullOrEmpty(m.MiddleName) ? m.MiddleName + " " : "")}{m.LastName}",
                        Title = m.Title,
                        CommitteeName = m.CommitteeName
                    })
                    .ToList();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }

}

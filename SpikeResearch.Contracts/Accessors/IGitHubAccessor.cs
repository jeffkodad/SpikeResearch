using System.Collections.Generic;
using SpikeResearch.DataContracts;

namespace SpikeResearch.Contracts.Accessors
{
    public interface IGitHubAccessor
    {
        void Init();

        GitHubUser GetUser(string userName);

        GitHubRepo GetRepo(string userName, string repoName);

        List<GitHubRepo> GetReposByUserName(string userName);

        List<GitHubRepo> GetReposByOrganization(string organizationName);

        GitHubOrganization GetOrganizationByName(string organizationName);
    }
}

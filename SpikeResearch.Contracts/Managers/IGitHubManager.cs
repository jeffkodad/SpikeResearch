using System.Collections.Generic;
using SpikeResearch.DataContracts;

namespace SpikeResearch.Contracts.Managers
{
    public interface IGitHubManager
    {
        void Init();

        GitHubUser GetUser(string userName);

        GitHubRepo GetRepo(string userName, string repoName);

        List<GitHubRepo> GetReposByUserName(string userName);

        List<GitHubRepo> GetReposByOrganization(string organizationName);

        GitHubOrganization GetOrganizationByName(string organizationName);

        List<Dictionary<string, object>> OneTimeCall(string path, Dictionary<string, string> paramaters);
    }
}

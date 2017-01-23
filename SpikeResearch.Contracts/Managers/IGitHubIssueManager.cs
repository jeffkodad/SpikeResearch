using System.Collections.Generic;
using SpikeResearch.DataContracts;

namespace SpikeResearch.Contracts.Managers
{
    public interface IGitHubIssueManager
    {
        GitHubIssue GetIssue(string userName, string repoName, string issueId);

        List<GitHubIssue> ListRepoIssues(string userName, string repoName);
    }
}

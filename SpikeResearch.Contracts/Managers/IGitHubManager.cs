using SpikeResearch.DataContracts;

namespace SpikeResearch.Contracts.Managers
{
    public interface IGitHubManager
    {
        void Init();

        GitHubUser GetUser(string userName);

        GitHubRepo GetRepo(string userName, string repoName);
    }
}

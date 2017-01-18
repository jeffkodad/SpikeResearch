using SpikeResearch.DataContracts;

namespace SpikeResearch.Contracts.Accessors
{
    public interface IGitHubAccessor
    {
        void Init();

        GitHubUser GetUser(string userName);

        GitHubRepo GetRepo(string userName, string repoName);
    }
}

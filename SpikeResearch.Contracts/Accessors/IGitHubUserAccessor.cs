using SpikeResearch.DataContracts;

namespace SpikeResearch.Contracts.Accessors
{
    public interface IGitHubUserAccessor
    {
        GitHubUser AuthenticateUser(string userName, string password);
    }
}

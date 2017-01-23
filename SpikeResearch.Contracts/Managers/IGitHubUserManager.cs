using SpikeResearch.DataContracts;

namespace SpikeResearch.Contracts.Managers
{
    public interface IGitHubUserManager
    {
        GitHubUser AuthenticateUser(string userName, string password);
    }
}

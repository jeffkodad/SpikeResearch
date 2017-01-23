namespace SpikeResearch.Contracts.Managers
{
    public interface IGitHubUserManager
    {
        bool AuthenticateUser(string userName, string password);
    }
}

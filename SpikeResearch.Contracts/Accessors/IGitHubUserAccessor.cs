namespace SpikeResearch.Contracts.Accessors
{
    public interface IGitHubUserAccessor
    {
        bool AuthenticateUser(string userName, string password);
    }
}

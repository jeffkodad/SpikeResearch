using System;
using SpikeResearch.Contracts.Accessors;
using SpikeResearch.Contracts.Managers;
using SpikeResearch.Utilities;
using SpikeResearch.DataContracts;
using System.Collections.Generic;

namespace SpikeResearch.Managers
{
    public class GitHubManager : IGitHubManager
    {
        #region Fields

        private IGitHubAccessor _gitHubAccessor;

        #endregion

        #region Properties

        public IGitHubAccessor GitHubAccessor
        {
            get { return _gitHubAccessor ?? (_gitHubAccessor = ClassFactory.CreateClass<IGitHubAccessor>()); }
            set { _gitHubAccessor = value; }
        }

        #endregion

        #region Methods
        public void Init()
        {
            GitHubAccessor.Init();
        }

        public GitHubUser GetUser(string userName)
        {
            return GitHubAccessor.GetUser(userName);
        }

        public GitHubRepo GetRepo(string userName, string repoName)
        {
            return GitHubAccessor.GetRepo(userName, repoName);
        }

        public List<GitHubRepo> GetReposByUserName(string userName)
        {
            return GitHubAccessor.GetReposByUserName(userName);
        }

        public List<GitHubRepo> GetReposByOrganization(string organizationName)
        {
            return GitHubAccessor.GetReposByOrganization(organizationName);
        }

        public GitHubOrganization GetOrganizationByName(string organizationName)
        {
            return GitHubAccessor.GetOrganizationByName(organizationName);
        }

        #endregion
    }
}

using System;
using SpikeResearch.Contracts.Accessors;
using SpikeResearch.Contracts.Managers;
using SpikeResearch.Utilities;
using SpikeResearch.DataContracts;
using System.Collections.Generic;

namespace SpikeResearch.Managers
{
    public class GitHubManager : 
        IGitHubManager,
        IGitHubUserManager,
        IGitHubIssueManager
    {
        #region Fields

        private IGitHubAccessor _gitHubAccessor;

        private IGitHubUserAccessor _gitHubUserAccessor;

        private IGitHubIssueAccessor _gitHubIssueAccessor;

        #endregion

        #region Properties

        public IGitHubAccessor GitHubAccessor
        {
            get { return _gitHubAccessor ?? (_gitHubAccessor = ClassFactory.CreateClass<IGitHubAccessor>()); }
            set { _gitHubAccessor = value; }
        }

        public IGitHubUserAccessor GitHubUserAccessor
        {
            get { return _gitHubUserAccessor ?? (_gitHubUserAccessor = ClassFactory.CreateClass<IGitHubUserAccessor>()); }
            set { _gitHubUserAccessor = value; }
        }

        public IGitHubIssueAccessor GitHubIssueAccessor
        {
            get { return _gitHubIssueAccessor ?? (_gitHubIssueAccessor = ClassFactory.CreateClass<IGitHubIssueAccessor>()); }
            set { _gitHubIssueAccessor = value; }
        }

        #endregion

        #region Methods

        #region UserMethods

        public bool AuthenticateUser(string userName, string password)
        {
            return GitHubUserAccessor.AuthenticateUser(userName, password);
        }

        #endregion

        #region IssueMethods
        public GitHubIssue GetIssue(string userName, string repoName, string issueId)
        {
            return GitHubIssueAccessor.GetIssue(userName, repoName, issueId);
        }

        public List<GitHubIssue> ListRepoIssues(string userName, string repoName)
        {
            return GitHubIssueAccessor.ListRepoIssues(userName, repoName);
        }

        #endregion
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

        public List<Dictionary<string, object>> OneTimeCall(string path, Dictionary<string, string> paramaters)
        {
            return GitHubAccessor.OneTimeCall(path, paramaters);
        }



        #endregion
    }
}

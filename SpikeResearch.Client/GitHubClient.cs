using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SpikeResearch.Contracts.Managers;
using SpikeResearch.DataContracts;
using SpikeResearch.Utilities;
using System.Runtime.Remoting.Contexts;

namespace SpikeResearch.Client
{
    public class GitHubClient : ServiceBase
    {
        #region Fields

        private IGitHubManager _gitHubManager;

        private GitHubUser _currentUser;

        private GitHubRepo _currtRepo;

        private GitHubOrganization _currentOrganization;

        private List<GitHubUser> UserList;

        private List<GitHubRepo> RepoList;

        private List<OptionItem> _gitHubOptions;

        private List<OptionItem> _userOptions;

        private List<OptionItem> _repoOptions;

        private List<OptionItem> _organizationOptions;

        #endregion

        #region Properties

        public IGitHubManager GitHubManager
        {
            get { return _gitHubManager ?? (_gitHubManager = ClassFactory.CreateClass<IGitHubManager>()); }
            set { _gitHubManager = value; }
        }

        public GitHubUser CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public GitHubRepo CurrentRepo
        {
            get { return _currtRepo; }
            set { _currtRepo = value; }
        }

        public GitHubOrganization CurrentOrganization
        {
            get { return _currentOrganization; }
            set { _currentOrganization = value; }
        }

        public List<OptionItem> GitHubOptions
        {
            get { return _gitHubOptions ?? (_gitHubOptions = PopulateGitHubOptions()); }
            set { _gitHubOptions = value; }
        }

        public List<OptionItem> UserOptions
        {
            get { return _userOptions ?? (_userOptions = PopulateUserOptions()); }
            set { _userOptions = value; }
        }

        public List<OptionItem> RepoOptions
        {
            get { return _repoOptions ?? (_repoOptions = PopulateRepoOptions()); }
            set { _repoOptions = value; }
        }

        public List<OptionItem> OrganizationOptions
        {
            get { return _organizationOptions ?? (_organizationOptions = PopulateOrganizationOptions()); }
            set { _organizationOptions = value; }
        }

        #endregion

        #region Methods

        public GitHubClient()
        {
            DisplayGitHubOptions();
        }

        #region DisplayOptions

        public void DisplayGitHubOptions()
        {
            DisplayOptionItemList<string>("GitHub Client", GitHubOptions, null);
            var input = GetNumberInput(GitHubOptions.Count);
            GitHubOptions.First(x => x.Index == Convert.ToInt32(input)).Action.Invoke();
        }

        public void DisplayUserOptions()
        {
            DisplayOptionItemList<GitHubUser>("User Options", UserOptions, CurrentUser);
            var input = GetNumberInput(GitHubOptions.Count);
            UserOptions.First(x => x.Index == Convert.ToInt32(input)).Action.Invoke();
        }

        public void DisplayRepoOptions()
        {
            DisplayOptionItemList<GitHubRepo>("Repo Options", RepoOptions, CurrentRepo);
            var input = GetNumberInput(GitHubOptions.Count);
            RepoOptions.First(x => x.Index == Convert.ToInt32(input)).Action.Invoke();
        }

        public void DisplayOrganizationOptions()
        {
            DisplayOptionItemList<GitHubOrganization>("Organization Options", OrganizationOptions, CurrentOrganization);
            var input = GetNumberInput(GitHubOptions.Count);
            OrganizationOptions.First(x => x.Index == Convert.ToInt32(input)).Action.Invoke();
        }

        #endregion

        #region GitHubOptions

        public void FindUser()
        {
            Console.WriteLine("Enter a username to search");
            var input = Console.ReadLine();

            var gitHubUser = GitHubManager.GetUser(input);

            if (!string.IsNullOrEmpty(gitHubUser.Id))
            {
                CurrentUser = gitHubUser;
                DisplayMessageAndWait("User found, press any key to continue", new Action(DisplayUserOptions));
            }
            else
            {
                DisplayMessageAndWait("No user found, press any key to reset", new Action(DisplayGitHubOptions));
            }
        }

        public void FindOrganization()
        {
            Console.WriteLine("Enter an organization name to search");
            var input = Console.ReadLine();

            var org = GitHubManager.GetOrganizationByName(input);

            if (!string.IsNullOrEmpty(org.Id))
            {
                CurrentOrganization = org;
                DisplayMessageAndWait("Organization found, press any key to continue", new Action(DisplayOrganizationOptions));
            }
        }

        #endregion

        #region UserOptions

        public void FindRepo()
        {
            Console.WriteLine("Enter the name of the Repo");
            var repoName = Console.ReadLine();

            var repo = GitHubManager.GetRepo(CurrentUser.UserName, repoName);

            if (!string.IsNullOrEmpty(repo.Id))
            {
                CurrentRepo = repo;
                DisplayRepoOptions();
            }
            else
            {
                DisplayMessageAndWait("No Repo Found, press any key to continue", new Action(DisplayUserOptions));
            }
        }

        public void ListReposForUser()
        {
            RepoList = GitHubManager.GetReposByUserName(CurrentUser.UserName);
            Console.Clear();
            PrintHeading($"Repo list for {CurrentUser.UserName}");

            if (RepoList.Count != 0)
            {
                foreach (var repo in RepoList)
                {
                    Console.WriteLine($"ID: {repo.Id}  Name: {repo.Name}");
                }
                Console.WriteLine("Enter a repo ID for options");
                var selectedId = Console.ReadLine();
                if (RepoList.Any(x => x.Id == selectedId))
                {
                    CurrentRepo = RepoList.FirstOrDefault(x => x.Id == selectedId);
                    DisplayRepoOptions();
                }
            }
            else
            {
                DisplayMessageAndWait("No repos found, press any key to continue", new Action(DisplayUserOptions));
            }
        }

        #endregion

        #region RepoOptions

        public void ListRepoUsers()
        {

        }

        public void ListRepoBranches()
        {

        }

        #endregion

        #region OrganizationOptions

        public void ListOrganizationRepos()
        {
            RepoList = GitHubManager.GetReposByOrganization(CurrentOrganization.Login);
            Console.Clear();
            PrintHeading($"Repo list for {CurrentOrganization.Name}");

            if (RepoList.Count != 0)
            {
                foreach (var repo in RepoList)
                {
                    Console.WriteLine($"ID: {repo.Id}  Name: {repo.Name}");
                }
                Console.WriteLine("Enter a repo ID for options");
                var selectedId = Console.ReadLine();
                if (RepoList.Any(x => x.Id == selectedId))
                {
                    CurrentRepo = RepoList.FirstOrDefault(x => x.Id == selectedId);
                    DisplayRepoOptions();
                }
            }
            else
            {
                DisplayMessageAndWait("No repos found, press any key to continue", new Action(DisplayOrganizationOptions));
            }
        }

        #endregion

        #endregion

        #region HelperMethods

        public List<OptionItem> PopulateGitHubOptions()
        {
            var list = new List<OptionItem>();
            list.Add(new OptionItem
            {
                Index = 1,
                Description = "Find User",
                Action = new Action(FindUser)
            });
            list.Add(new OptionItem
            {
                Index = 2,
                Description = "Find Organization",
                Action = new Action(FindOrganization)
            });
            list.Add(GetResetItem(3));
            list.Add(GetExitOption(4));
            return list;
        }

        public List<OptionItem> PopulateUserOptions()
        {
            var list = new List<OptionItem>();
            list.Add(new OptionItem
            {
                Index = 1,
                Description = "List Repos",
                Action = new Action(ListReposForUser)
            });
            list.Add(new OptionItem
            {
                Index = 2,
                Description = "Search for Repo",
                Action = new Action(FindRepo)
            });
            list.Add(GetResetItem(3));
            list.Add(GetExitOption(4));
            return list;
        }

        public List<OptionItem> PopulateRepoOptions()
        {
            var list = new List<OptionItem>();
            list.Add(new OptionItem
            {
                Index = 1,
                Description = "List Repo Users",
                Action = new Action(ListRepoUsers)
            });
            list.Add(new OptionItem
            {
                Index = 2,
                Description = "List Branches",
                Action = new Action(ListRepoBranches)
            });
            list.Add(GetResetItem(3));
            list.Add(GetExitOption(4));
            return list;
        }

        public List<OptionItem> PopulateOrganizationOptions()
        {
            var list = new List<OptionItem>();
            list.Add(new OptionItem(1, "List Repos", new Action(ListOrganizationRepos)));
            list.Add(GetResetItem(3));
            list.Add(GetExitOption(4));
            return list;
        }

        #endregion
    }
}

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
using Newtonsoft.Json;

namespace SpikeResearch.Client
{
    public class GitHubClient : ServiceBase
    {
        #region Fields

        private IGitHubManager _gitHubManager;

        private IGitHubUserManager _gitHubUserManager;

        private IGitHubIssueManager _gitHubIssueManager;

        private GitHubUser _currentUser;

        private GitHubRepo _currtRepo;

        private GitHubIssue _currentIssue;

        private GitHubOrganization _currentOrganization;

        private List<GitHubUser> UserList;

        private List<GitHubRepo> RepoList;

        private List<OptionItem> _gitHubOptions;

        private List<OptionItem> _userOptions;

        private List<OptionItem> _repoOptions;

        private List<OptionItem> _organizationOptions;

        private List<OptionItem> _issueOptions;

        private Dictionary<string, string> EmptyParams = new Dictionary<string, string>();

        private bool _authenticated = false;

        #endregion

        #region Properties

        public IGitHubManager GitHubManager
        {
            get { return _gitHubManager ?? (_gitHubManager = ClassFactory.CreateClass<IGitHubManager>()); }
            set { _gitHubManager = value; }
        }

        public IGitHubUserManager GitHubUserManager
        {
            get { return _gitHubUserManager ?? (_gitHubUserManager = ClassFactory.CreateClass<IGitHubUserManager>()); }
            set { _gitHubUserManager = value; }
        }

        public IGitHubIssueManager GitHubIssueManager
        {
            get { return _gitHubIssueManager ?? (_gitHubIssueManager = ClassFactory.CreateClass<IGitHubIssueManager>()); }
            set { _gitHubIssueManager = value; }
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

        public GitHubIssue CurrentIssue
        {
            get { return _currentIssue; }
            set { _currentIssue = value; }
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

        public List<OptionItem> IssueOptions
        {
            get { return _issueOptions ?? (_issueOptions = PopulateIssueOptions()); }
            set { _issueOptions = value; }
        }

        public bool Authenticated
        {
            get { return _authenticated; }
            set { _authenticated = value; }
        }

        #endregion

        #region Methods

        public GitHubClient()
        {
            try
            {
                DisplayGitHubOptions();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                DisplayMessageAndWait("Press Any Key to Restart", new Action(DisplayGitHubOptions));
            }
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

        public void DisplayIssueOptions()
        {
            DisplayOptionItemList<GitHubIssue>("Issue Options", IssueOptions, CurrentIssue);
            var input = GetNumberInput(IssueOptions.Count);
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

        public void CreateCustomCall()
        {
            PrintHeading("Custom GitHub Call");

            Console.WriteLine("Enter a path for the gitHub call");
            var path = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Enter your paramaters {\"Name\":\"Value\",\"Name\":\"Value\"}, press escape when finished");
            var paramInput = Console.ReadLine();
            var paramaters = JsonConvert.DeserializeObject<Dictionary<string, string>>(paramInput);

            Console.Clear();
            PrintHeading("Custom Call Results");
            Console.WriteLine($"Path: {path}");
            if (!string.IsNullOrEmpty(paramInput) && paramaters.Count > 0)
            {
                Console.WriteLine("Paramaters:");
                foreach (var item in paramaters)
                {
                    Console.WriteLine($"{item.Key}=>{item.Value}");
                }
            }
            PrintHeading("Results");

            var text = GitHubManager.OneTimeCall(path, EmptyParams);

            foreach (var item in text)
            {
                PrintSectionBreak();
                foreach (var obj in item)
                {
                    Console.WriteLine($"{obj.Key}: {obj.Value}");
                }
            }

            DisplayMessageAndWait("Press any key to go back", DisplayGitHubOptions);
        }

        public void AuthenticateUser()
        {
            PrintHeading("User Authentication");
            Console.WriteLine("Enter username");
            var userName = Console.ReadLine();
            Console.WriteLine("\nEnter Password");
            var pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    pass += key.KeyChar;
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);

            var auth = GitHubUserManager.AuthenticateUser(userName, pass);

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

        public void ListRepoIssues()
        {
            var issueList = GitHubIssueManager.ListRepoIssues(CurrentUser.UserName, CurrentRepo.Name);
            Console.Clear();
            PrintHeading($"Issue list for {CurrentRepo.Name}");

            if (issueList.Count != 0)
            {
                foreach (var issue in issueList)
                {
                    Console.WriteLine($"ID: {issue.Id}  Name: {issue.Title}");
                }
                Console.WriteLine("Enter an issue ID for options");
                var selectedId = Console.ReadLine();
                if (issueList.Any(x => x.Id == selectedId))
                {
                    CurrentIssue = issueList.FirstOrDefault(x => x.Id == selectedId);
                    DisplayIssueOptions();
                }
            }
            else
            {
                DisplayMessageAndWait("No issues found, press any key to continue", new Action(DisplayRepoOptions));
            }
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

        #region IssueOptions

        public void EditIssue()
        {
            
        }

        public void AddIssueComment()
        {
            
        }

        #endregion

        #endregion

        #region HelperMethods

        public List<OptionItem> PopulateGitHubOptions()
        {
            return new List<OptionItem>
            {
                new OptionItem(1, "Find User", new Action(FindUser)),
                new OptionItem(2, "Find Organization", new Action(FindOrganization)),
                new OptionItem(3, "Custom Call", new Action(CreateCustomCall)),
                new OptionItem(4, "Authenticate", new Action(AuthenticateUser)),
                GetResetItem(4),
                GetExitOption(5)
            };
        }

        public List<OptionItem> PopulateUserOptions()
        {
            return new List<OptionItem>
            {
                new OptionItem(1, "List Repos", new Action(ListReposForUser)),
                new OptionItem(2, "Search for Repo", new Action(FindRepo)),
                GetResetItem(3),
                GetExitOption(4)
            };
        }

        public List<OptionItem> PopulateRepoOptions()
        {
            return new List<OptionItem>
            {
                new OptionItem(1, "List Repo Users", new Action(ListRepoUsers)),
                new OptionItem(2, "List Branches", new Action(ListRepoBranches)),
                new OptionItem(3, "List Issues", new Action(ListRepoIssues)),
                GetResetItem(4),
                GetExitOption(5)
            };
        }

        public List<OptionItem> PopulateOrganizationOptions()
        {
            return new List<OptionItem>
            {
                new OptionItem(1, "List Repos", new Action(ListOrganizationRepos)),
                GetResetItem(3),
                GetExitOption(4)
            };
        }

        public List<OptionItem> PopulateIssueOptions()
        {
            return new List<OptionItem>
            {
                new OptionItem(1, "Edit Issue", new Action(ListOrganizationRepos)),
                new OptionItem(2, "Add Comment", new Action(AddIssueComment)),
                GetResetItem(3),
                GetExitOption(4)
            };
        }

        #endregion
    }
}

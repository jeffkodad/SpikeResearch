using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SpikeResearch.Contracts.Managers;
using SpikeResearch.Utilities;

namespace SpikeResearch.Client
{
    public class GitHubClient : ServiceBase
    {
        #region Fields

        private IGitHubManager _gitHubManager;

        #endregion

        #region Properties

        public IGitHubManager GitHubManager
        {
            get { return _gitHubManager ?? (_gitHubManager = ClassFactory.CreateClass<IGitHubManager>()); }
            set { _gitHubManager = value; }
        }

        #endregion

        #region Methods

        public GitHubClient()
        {
            
            GitHubInit();
        }

        public void GitHubInit()
        {
            Console.Clear();
            PrintHeading("GitHub Client");
            Console.WriteLine("Select an option");
            Console.WriteLine("1. Find User");
            Console.WriteLine("2. Find a Repo");
            Console.WriteLine("3. Restart App");
            Console.WriteLine("4. Exit");

            switch (GetNumberInput(4))
            {
                case "1":
                    FindMember();
                    break;
                case "2":
                    FindRepo();
                    break;
                case "3":
                    ResetApp();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
            }

            GitHubManager.Init();

        }

        public void FindMember()
        {
            Console.WriteLine("Enter a username to search");
            var input = Console.ReadLine();

            var gitHubUser = GitHubManager.GetUser(input);

            if (!string.IsNullOrEmpty(gitHubUser.Id))
            {
                DisplayAllObjectProperties(gitHubUser);
            }
            else
            {
                Console.WriteLine("No User Found");
            }

            Console.WriteLine("Press Any Key To Start Over");
            WaitForAnyKey();
            GitHubInit();
        }

        public void FindRepo()
        {
            Console.WriteLine("Enter the username of the Repo OWNER");
            var userName = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the name of the Repo");
            var repoName = Console.ReadLine();

            var repo = GitHubManager.GetRepo(userName, repoName);

            if (!string.IsNullOrEmpty(repo.Id))
            {
                DisplayAllObjectProperties(repo);
            }
            else
            {
                Console.WriteLine("No Repo Found");
            }

            Console.WriteLine("Press Any Key To Start Over");
            WaitForAnyKey();
            GitHubInit();
        }



        #endregion
    }
}

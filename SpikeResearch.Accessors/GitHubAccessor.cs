using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using DontPanic.Helpers;
using Newtonsoft.Json;
using SpikeResearch.Contracts.Accessors;
using SpikeResearch.DataContracts;
using System.Collections;
using System.Text;

namespace SpikeResearch.Accessors
{
    public class GitHubAccessor :  
        IGitHubAccessor,
        IGitHubUserAccessor,
        IGitHubIssueAccessor 
    {
        #region Fields

        private HttpClient _httpClient;

        private bool _authenticated;

        #endregion

        #region Properties

        public HttpClient GitHubClient
        {
            get { return _httpClient ?? (_httpClient = CreateNewClient()); }
            set { _httpClient = value; }
        }

        public bool Authenticated
        {
            get { return _authenticated; }
            set { _authenticated = value; }
        }

        #endregion

        #region Methods

        #region UserAccessorMethods

        public GitHubUser AuthenticateUser(string userName, string password)
        {
            var request = CreateNewRequest(HttpMethod.Get, "user", new Dictionary<string, string>());
            Console.WriteLine("Test");
            var testread = Console.ReadLine();
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(String.Format("{0}:{1}", userName, password))));
            var gitUser = ProcessRequest<GitHubUser>(request);

            if (gitUser.Id != null)
            {
                Authenticated = true;
            }

            return gitUser;
        }

        #endregion

        #region IssueAccessorMethods

        public GitHubIssue GetIssue(string userName, string repoName, string issueId)
        {
            var request = CreateNewRequest(HttpMethod.Get, $"repos/{userName}/{repoName}/issues/{issueId}", new Dictionary<string, string>());
            return ProcessRequest<GitHubIssue>(request);
        }

        public List<GitHubIssue> ListRepoIssues(string userName, string repoName)
        {
            var request = CreateNewRequest(HttpMethod.Get, $"repos/{userName}/{repoName}/issues", new Dictionary<string, string>());
            return ProcessRequest<List<GitHubIssue>>(request);
        }

        #endregion

        public void Init()
        {
            var paramaters = new Dictionary<string, string>();

            var request = CreateNewRequest(HttpMethod.Get, "users/jeffkodad", paramaters);

            //var resp = client.SendAsync(request).Result;
            //var content = resp.Content.ReadAsStringAsync().Result;



            var result = ProcessRequest<GitHubUser>(request);

            Console.WriteLine(result.UserName);




            //var user = JsonConvert.DeserializeObject<GitHubUser>(content);
            //var u = JsonConvert.DeserializeObject(content);
        }

        public GitHubUser GetUser(string userName)
        {
            var request = CreateNewRequest(HttpMethod.Get, $"users/{userName}", new Dictionary<string, string>());
            return ProcessRequest<GitHubUser>(request);
        }

        public GitHubRepo GetRepo(string userName, string repoName)
        {
            var request = CreateNewRequest(HttpMethod.Get, $"repos/{userName}/{repoName}",
                new Dictionary<string, string>());
            return ProcessRequest<GitHubRepo>(request);
        }

        public List<GitHubRepo> GetReposByUserName(string userName)
        {
            var paramaters = new Dictionary<string, string>();
            paramaters["type"] = "all";
            var request = CreateNewRequest(HttpMethod.Get, $"users/{userName}/repos", paramaters);
            return ProcessRequest<List<GitHubRepo>>(request);
        }

        public List<GitHubRepo> GetReposByOrganization(string organizationName)
        {
            var request = CreateNewRequest(HttpMethod.Get, $"orgs/{organizationName}/repos", new Dictionary<string, string>());
            return ProcessRequest<List<GitHubRepo>>(request);
        }

        public GitHubOrganization GetOrganizationByName(string organizationName)
        {
            var request = CreateNewRequest(HttpMethod.Get, $"orgs/{organizationName}", new Dictionary<string, string>());
            return ProcessRequest<GitHubOrganization>(request);
        }

        public List<Dictionary<string, object>> OneTimeCall(HttpMethod method, bool auth, string path, Dictionary<string, string> paramaters)
        {
            var request = CreateNewRequest(method, path, paramaters);

            if (auth)
            {
                request.Headers.Authorization = AddAuthentication();
            }

            var resp = GitHubClient.SendAsync(request).Result;
            var content = resp.Content.ReadAsStringAsync().Result;

            var list = new List<Dictionary<string, object>>();

            if (!content.StartsWith("["))
            {
                list.Add(JsonConvert.DeserializeObject<Dictionary<string, object>>(content));
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(content);
            }

            return list;
        }

        #region HelperMethods

        private HttpClient CreateNewClient()
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.github.com/")
            };

            return client;
        }

        private HttpRequestMessage CreateNewRequest(HttpMethod httpMethod, string requestUri,
            Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(GitHubClient.BaseAddress, requestUri)
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("User-Agent", "SpikeResearch");

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return request;
        }

        private T ProcessRequest<T>(HttpRequestMessage request)
        {
            var resp = GitHubClient.SendAsync(request).Result;
            var content = resp.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<T>(content);
        }

        private AuthenticationHeaderValue AddAuthentication()
        {
            Console.WriteLine("Authentication Required");
            Console.WriteLine("Enter username");
            var userName = Console.ReadLine();
            Console.WriteLine();
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

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{pass}")));
        }

        #endregion

        #endregion
    }
}

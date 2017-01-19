using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using DontPanic.Helpers;
using Newtonsoft.Json;
using SpikeResearch.Contracts.Accessors;
using SpikeResearch.DataContracts;

namespace SpikeResearch.Accessors
{
    public class GitHubAccessor :  IGitHubAccessor
    {
        #region Fields

        private HttpClient _httpClient;

        #endregion

        #region Properties

        public HttpClient GitHubClient
        {
            get { return _httpClient ?? (_httpClient = CreateNewClient()); }
            set { _httpClient = value; }
        }

        #endregion

        #region Methods

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

        #endregion

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpikeResearch.DataContracts
{
    public class GitHubRepo
    {
        [JsonProperty("Owner")]
        public GitHubUser Owner { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }
       
        [JsonProperty("html_url")]
        public string Url { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}

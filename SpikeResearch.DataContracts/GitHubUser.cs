using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpikeResearch.DataContracts
{
    public class GitHubUser
    {
        [JsonProperty("login")]
        public string UserName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }
    }
}

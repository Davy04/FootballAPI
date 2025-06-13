using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpAPI.Models
{
    public class Root
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("matches")]
        public List<Match> Matches { get; set; }
    }
}
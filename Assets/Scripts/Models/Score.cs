using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpAPI.Models
{
    public class Score
    {
        [JsonProperty("ft")]
        public List<int> Ft { get; set; }
    }
}
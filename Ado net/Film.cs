using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ado_net
{
    public class Film
    {
        public string Title { get; set; }
        [JsonProperty(PropertyName = "episode_id")]
        public string Episode_id { get; set; }
        [JsonProperty(PropertyName = "opening_crawl")]
        public string Opening_crawl { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        [JsonProperty(PropertyName = "release_date")]
        public string Release_date { get; set; }
        public List<string> Characters { get; set; }
        public List<string> Planets { get; set; }
        public List<string> Starships { get; set; }
        public List<string> Vehicles { get; set; }
        public List<string> Species { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }
    }
}

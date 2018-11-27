using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace Ado_net
{
    public class Planet:IGetFromAPI<Planet>
    {
        public string Name { get; set; }
        [JsonProperty(PropertyName = "rotation_period")]
        public string RotationPeriod { get; set; }
        [JsonProperty(PropertyName = "orbital_period")]
        public string OrbitalPeriod { get; set; }
        public string Diameter { get; set; }
        public string Climate { get; set; }
        public string Gravity { get; set; }
        public string Terrain { get; set; }
        [JsonProperty(PropertyName = "surface_water")]
        public string SurfaceWater { get; set; }
        public string Population { get; set; }
        public List<string> Residents { get; set; }
        public List<string> Films { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }

        public List<Planet> GetListObjects(string url, out string urlNext)
        {
            List<Planet> planets = new List<Planet>();
            using (WebClient client = new WebClient())
            {
                JObject json = JObject.Parse(client.DownloadString(url));
                planets.AddRange(JsonConvert.DeserializeObject<List<Planet>>(json.GetValue("results").ToString()));
                urlNext = json.GetValue("next").ToString();
                Console.WriteLine("Download people...");
                Console.WriteLine("Complete...");
            }
            return planets;
        }

        public Planet GetObject(int id)
        {
            string URI = $"https://swapi.co/api/planets/{id}";
            Planet obj;
            using (WebClient webClient = new WebClient())
            {
                Stream stream = webClient.OpenRead(URI);
                StreamReader reader = new StreamReader(stream);
                string request = reader.ReadToEnd();
                obj = JsonConvert.DeserializeObject<Planet>(request);
            }
            return obj;
        }
    }
}

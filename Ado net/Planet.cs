using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ado_net
{
    public class Planet
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Ado_net
{
    public class Person
    {
        public string Name { get; set; }
        public string Height { get; set; }
        public string Mass { get; set; }
        [JsonProperty(PropertyName = "hair_color")]
        public string HairColor { get; set; }
        [JsonProperty(PropertyName = "skin_color")]
        public string SkinColor { get; set; }
        [JsonProperty(PropertyName = "eye_color")]
        public string EyeColor { get; set; }
        [JsonProperty(PropertyName = "birth_year")]
        public string BirthYear { get; set; }
        public string Gender { get; set; }
        public string Homeworld { get; set; }
        public List<string> Films { get; set; }
        public List<string> Species { get; set; }
        public List<object> Vehicles { get; set; }
        public List<object> Starships { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }
    }
}

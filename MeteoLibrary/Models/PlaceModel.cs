using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoLibrary.Models
{
    public class PlaceModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("administrativeDivision")]
        public string AdministrativeDivision { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }
}

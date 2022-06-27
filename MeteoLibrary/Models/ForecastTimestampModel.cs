using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoLibrary.Models
{
    public class ForecastTimestampModel
    {
        [JsonProperty("forecastTimeUtc")]
        public string ForecastTimeUtc { get; set; }

        [JsonProperty("airTemperature")]
        public double AirTemperature { get; set; }

        [JsonProperty("windSpeed")]
        public int WindSpeed { get; set; }

        [JsonProperty("windGust")]
        public int WindGust { get; set; }

        [JsonProperty("windDirection")]
        public int WindDirection { get; set; }

        [JsonProperty("cloudCover")]
        public int CloudCover { get; set; }

        [JsonProperty("seaLevelPressure")]
        public int SeaLevelPressure { get; set; }

        [JsonProperty("relativeHumidity")]
        public int RelativeHumidity { get; set; }

        [JsonProperty("totalPrecipitation")]
        public int TotalPrecipitation { get; set; }

        [JsonProperty("conditionCode")]
        public string ConditionCode { get; set; }
    }
}

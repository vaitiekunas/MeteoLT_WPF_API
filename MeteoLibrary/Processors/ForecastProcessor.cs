using MeteoLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeteoLibrary.Processors
{
    public class ForecastProcessor
    {
        public static async Task<IList<ForecastTimestampModel>> LoadForecast(string placeCode)
        {
            string url = "https://api.meteo.lt/v1/places";

            HttpClient httpClient = new HttpClient();

            try
            {
                var httpResponseMessage = await httpClient.GetAsync($"{url}/{placeCode}/forecasts/long-term");

                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                JObject keyValuePairs = JObject.Parse(jsonResponse);

                IList<JToken> forecasts = keyValuePairs["forecastTimestamps"].Children().ToList();

                IList<ForecastTimestampModel> forecastTimestamps = new List<ForecastTimestampModel>();

                foreach (JToken forecast in forecasts)
                {
                    ForecastTimestampModel forecastTimestampModel = forecast.ToObject<ForecastTimestampModel>();
                    forecastTimestamps.Add(forecastTimestampModel);

                }

                return forecastTimestamps;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}

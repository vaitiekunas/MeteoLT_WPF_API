using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MeteoLibrary.Models;
using Newtonsoft.Json;

namespace MeteoLibrary.Processors
{
    public class PlaceProcessor
    {
        public static string PlaceCode { get; set; }

        public static async Task<List<String>> LoadPlace()
        {
            string url = "https://api.meteo.lt/v1/places";

            HttpClient httpClient = new HttpClient();

            try
            {
                var httpResponseMessage = await httpClient.GetAsync(url);

                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                var places = JsonConvert.DeserializeObject<PlaceModel[]>(jsonResponse);

                List<String> placeNames = new List<string>();

                foreach (var place in places)
                {
                    placeNames.Add($"{place.Name}, {place.AdministrativeDivision}");
                }
                return placeNames;
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

        public static async Task<String> GetPlaceCode(string placeName)
        {
            string url = "https://api.meteo.lt/v1/places";

            HttpClient httpClient = new HttpClient();

            try
            {
                var httpResponseMessage = await httpClient.GetAsync(url);

                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                var places = JsonConvert.DeserializeObject<PlaceModel[]>(jsonResponse);

                foreach (var place in places)
                {
                    if ($"{place.Name}, {place.AdministrativeDivision}" == placeName)
                    {
                        PlaceCode = place.Code;
                        break;
                    }
                }

                return PlaceCode;

            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}

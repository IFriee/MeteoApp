using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoApp
{
    public class RestService
    {
        HttpClient client;
       //constructeur de la classe RestService 
        public RestService()
        {
            client = new HttpClient();
        }
        // envoit requete HTTP à un serv (https://openweathermap.org/) Si true, deserialise en objet WheatherData avec Nugget Newton (Json)
        //Sinon, return Null
        public async Task<WeatherData> GetWeatherData(string query){
            WeatherData weatherData = null;

            try
            {
                var response = await client.GetAsync(query);
                if (response.IsSuccessStatusCode) {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return weatherData;

        }
    }
}
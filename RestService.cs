using Newtonsoft.Json; // Utilisation de la bibliothèque Newtonsoft.Json pour la désérialisation JSON.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoApp
{
    // Classe RestService qui permet de communiquer avec un serveur distant via des requêtes HTTP.
    public class RestService
    {
        HttpClient client; // HttpClient pour effectuer des requêtes HTTP.

        // Constructeur de la classe RestService qui initialise le client HTTP.
        public RestService()
        {
            client = new HttpClient();
        }

        // Méthode asynchrone qui envoie une requête HTTP à un serveur distant (https://openweathermap.org/).
        // Si la requête réussit (code de statut 2xx), désérialise la réponse JSON en un objet WeatherData avec la bibliothèque Newtonsoft.Json.
        // Sinon, retourne null.
        public async Task<WeatherData> GetWeatherData(string query)
        {
            WeatherData weatherData = null;

            try
            {
                var response = await client.GetAsync(query);

                if (response.IsSuccessStatusCode)
                {
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

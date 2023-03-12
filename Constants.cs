using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoApp
{
    // Cette classe statique contient les constantes utilisées dans l'application.
    public static class Constants
    {
        // L'URL de l'API OpenWeatherMap pour obtenir les données de météo.
        public static string OpenWeatherMapEndpoint = "https://api.openweathermap.org/data/2.5/weather";

        // La clé API OpenWeatherMap utilisée pour authentifier les requêtes.
        public static string OpenWeatherMapAPIKey = "7644931008d77c7f5c8fd01185d87427";
    }
}

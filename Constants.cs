using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MeteoApp
{
    // Cette classe statique contient les constantes utilisées dans l'application.
    public static class Constants
    {
        public const string DatabaseFilename = "weather.db";
        
        public const SQLite.SQLiteOpenFlags Flags =
            // ouvrir la base de données en mode lecture/écriture
            SQLite.SQLiteOpenFlags.ReadWrite |
            // créer la base de données si elle n'existe pas
            SQLite.SQLiteOpenFlags.Create |
            // activer l'accès multi-thread à la base de données
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        // L'URL de l'API OpenWeatherMap pour obtenir les données de météo.
        public static string OpenWeatherMapEndpoint = "https://api.openweathermap.org/data/2.5/weather";

        // La clé API OpenWeatherMap utilisée pour authentifier les requêtes.
        public static string OpenWeatherMapAPIKey = "7644931008d77c7f5c8fd01185d87427";
    }
}

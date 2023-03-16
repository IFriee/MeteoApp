using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MeteoApp
{
    // Classe UnsplashService qui permet de récupérer des images aléatoires à partir de l'API Unsplash.
    public class UnsplashService
    {
        private readonly HttpClient _client; // HttpClient pour effectuer des requêtes HTTP.

        // Constructeur qui initialise le client HTTP.
        public UnsplashService()
        {
            _client = new HttpClient();
        }

        // Méthode asynchrone qui récupère une URL d'image aléatoire en fonction de la requête.
        // La requête est effectuée à l'aide de l'API Unsplash et nécessite une clé d'API valide (Constants.UnsplashAPIKey).
        public async Task<string> GetImageUrlAsync(string query)
        {
            // Construction de l'URL de l'API Unsplash avec la requête et la clé d'API.
            string url = $"https://api.unsplash.com/photos/random?query={query}&client_id={Constants.UnsplashAPIKey}";

            // Envoi de la requête HTTP avec le client HTTP.
            HttpResponseMessage response = await _client.GetAsync(url);

            // Vérification si la requête s'est bien déroulée (code de statut 2xx).
            if (response.IsSuccessStatusCode)
            {
                // Récupération du contenu de la réponse HTTP (au format JSON).
                string json = await response.Content.ReadAsStringAsync();

                // Désérialisation du JSON en un objet dynamique.
                dynamic result = JsonConvert.DeserializeObject(json);

                // Récupération de l'URL de l'image régulière dans l'objet dynamique.
                return result.urls.regular;
            }

            // Si la requête a échoué, retourne null.
            return null;
        }
    }
}

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MeteoApp
{
    public class UnsplashService
    {
        private readonly HttpClient _client;

        public UnsplashService()
        {
            _client = new HttpClient();
        }

        public async Task<string> GetImageUrlAsync(string query)
        {
            string url = $"https://api.unsplash.com/photos/random?query={query}&client_id={Constants.UnsplashAPIKey}";
            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(json);
                return result.urls.regular;
            }

            return null;
        }
    }
}

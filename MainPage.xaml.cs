using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using XamarinEssentials = Xamarin.Essentials;

namespace MeteoApp
{
    public partial class MainPage : ContentPage
    {
        RestService _restService;

        // Constructeur de la classe MainPage
        public MainPage()
        {
            InitializeComponent();
            _restService = new RestService(); // Création d'une instance de RestService
            LoadWeatherData();
        }

        public void SetWeatherData(WeatherData weatherData)
        {
            BindingContext = weatherData;
        }


        // Génère l'URL de la requête pour l'API OpenWeatherMap
        string GenerateRequestURL(string endPoint, double latitude, double longitude)
        {
            string requestUri = endPoint;
            requestUri += $"?lat={latitude}&lon={longitude}"; // Ajoute la latitude et la longitude à la requête
            requestUri += "&units=metric"; // Définit l'unité de mesure en Celsius
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}"; // Ajoute la clé API

            return requestUri;
        }

        public async Task<XamarinEssentials.Location> GetCurrentLocation()
        {
            try
            {
                var request = new XamarinEssentials.GeolocationRequest(XamarinEssentials.GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await XamarinEssentials.Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                    return location;
                }
            }
            catch (XamarinEssentials.FeatureNotSupportedException fnsEx)
            {
                // La fonction de géolocalisation n'est pas supportée sur l'appareil
                Console.WriteLine(fnsEx);
            }
            catch (XamarinEssentials.FeatureNotEnabledException fneEx)
            {
                // La fonction de géolocalisation n'est pas activée sur l'appareil
                Console.WriteLine(fneEx);
            }
            catch (XamarinEssentials.PermissionException pEx)
            {
                // L'autorisation de géolocalisation a été refusée
                Console.WriteLine(pEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        private async void LoadWeatherData()
        {
            XamarinEssentials.Location location = await GetCurrentLocation();

            if (location != null)
            {
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint, location.Latitude, location.Longitude));
                BindingContext = weatherData;
            }
            // Après avoir mis à jour les données météorologiques
            UpdateBackgroundImage();
        }

        // Méthode appelée lorsque l'utilisateur clique sur le bouton "Afficher"
        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            // Vérifie si la ville a été saisie
                XamarinEssentials.Location location = await GetCurrentLocation();

                if (location != null)
                {
                    // Appel de l'API pour obtenir les données de météo
                    WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint, location.Latitude, location.Longitude));

                    if (weatherData == null)
                    {
                        await DisplayAlert("Erreur", "Impossible de récupérer les données météorologiques pour la ville saisie. Vérifiez si le nom de la ville est correct.", "OK");
                    }
                    else
                    {
                        // Met à jour le contexte de liaison de données
                        BindingContext = weatherData;
                    }
                }
            }
        

        private void OnCitySelected(WeatherData weatherData)
        {
            SetWeatherData(weatherData);
            UpdateBackgroundImage();
        }

        // Bouton vers favoritecitypage
        private async void OnFavoriteCitiesButtonClicked(object sender, EventArgs e)
        {
            var favoriteCitiesPage = new FavoriteCitiesPage();
            favoriteCitiesPage.CitySelected += OnCitySelected;
            await Navigation.PushAsync(favoriteCitiesPage);
        }


        private async void UpdateBackgroundImage()
        {
            var unsplashService = new UnsplashService();
            string query = GetWeatherQuery(); // Cette fonction doit retourner un terme de recherche en fonction des conditions météorologiques actuelles
            string imageUrl = await unsplashService.GetImageUrlAsync(query);

            if (!string.IsNullOrEmpty(imageUrl))
            {
                BackgroundImage.Source = imageUrl;
            }
        }



        private string GetWeatherQuery()
        {
            WeatherData weatherData = (WeatherData)BindingContext;
            if (weatherData != null)
            {
                return weatherData.Title; // Retourne le nom de la ville
            }

            return "city";
        }






    }
}
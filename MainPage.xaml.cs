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
        }
        public void SetWeatherData(WeatherData weatherData)
        {
            BindingContext = weatherData;
        }


        // Méthode appelée lorsque l'utilisateur clique sur le bouton "Afficher"
        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            // Vérifie si la ville a été saisie
            if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
            {
                // Appel de l'API pour obtenir les données de météo
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint));

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

        // Génère l'URL de la requête pour l'API OpenWeatherMap
        string GenerateRequestURL(string endPoint)
        {
            string requestUri = endPoint;
            requestUri += $"?q={_cityEntry.Text}"; // Ajoute la ville à la requête
            requestUri += "&units=metric"; // Définit l'unité de mesure en Celsius
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}"; // Ajoute la clé API

            return requestUri;
        }

        private void OnCitySelected(WeatherData weatherData)
        {
            SetWeatherData(weatherData);
        }

        // Bouton vers favoritecitypage
        private async void OnFavoriteCitiesButtonClicked(object sender, EventArgs e)
        {
            var favoriteCitiesPage = new FavoriteCitiesPage();
            favoriteCitiesPage.CitySelected += OnCitySelected;
            await Navigation.PushAsync(favoriteCitiesPage);
        }


    }
}
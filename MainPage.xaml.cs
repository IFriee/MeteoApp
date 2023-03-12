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

        // Méthode appelée lorsque l'utilisateur clique sur le bouton "Afficher"
        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            // Vérifie si la ville a été saisie
            if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
            {
                // Appel de l'API pour obtenir les données de météo
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint));

                // Met à jour le contexte de liaison de données
                BindingContext = weatherData;
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
    }
}


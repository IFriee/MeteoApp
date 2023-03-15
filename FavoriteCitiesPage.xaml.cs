using System.Collections.ObjectModel;

namespace MeteoApp
{
    public partial class FavoriteCitiesPage : ContentPage
    {
        private FavoriteCityDatabase _database;
        RestService _restService;

        public ObservableCollection<FavoriteCity> FavoriteCities { get; set; }
        public FavoriteCity CityToUpdate { get; set; }

        public FavoriteCitiesPage()
        {
            // Initialise les composants de la page
            InitializeComponent();

            // Construit le chemin d'accès à la base de données SQLite en utilisant le dossier d'application local
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "weather.db3");
            // Crée une nouvelle instance de la base de données FavoriteCityDatabase en utilisant le chemin d'accès spécifié
            _database = new FavoriteCityDatabase(dbPath);

            // Appelle la méthode pour charger les villes favorites de manière asynchrone
            LoadFavoriteCitiesAsync();

            _restService = new RestService(); // Ajoutez cette ligne pour initialiser _restService
        }

        private async void LoadFavoriteCitiesAsync()
        {
            // Récupère la liste des villes favorites de manière asynchrone depuis la base de données
            var favoriteCitiesList = await _database.GetFavoriteCitiesAsync();
            // Crée une nouvelle ObservableCollection à partir de la liste des villes favorites récupérées
            FavoriteCities = new ObservableCollection<FavoriteCity>(favoriteCitiesList);
            // Définit le contexte de liaison (BindingContext) pour la page en cours
            BindingContext = this;
        }

        // Méthode pour ajouter ou mettre à jour une ville favorite
        async void OnAddButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_favoritecityEntry.Text))
            {
                if (CityToUpdate != null)
                {
                    // Met à jour le nom de la ville favorite existante
                    int index = FavoriteCities.IndexOf(CityToUpdate);
                    CityToUpdate.Name = _favoritecityEntry.Text;
                    await _database.SaveFavoriteCityAsync(CityToUpdate);
                    FavoriteCities[index] = CityToUpdate;
                    CityToUpdate = null;
                }
                else
                {
                    // Ajoute une nouvelle ville favorite
                    var favoriteCity = new FavoriteCity { Name = _favoritecityEntry.Text };
                    await _database.SaveFavoriteCityAsync(favoriteCity);
                    FavoriteCities.Add(favoriteCity);
                }

                _favoritecityEntry.Text = string.Empty;
            }
        }



        // Méthode pour modifier une ville favorite
        async void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var favoriteCity = button?.BindingContext as FavoriteCity;

            if (favoriteCity != null)
            {
                // Définit la valeur du champ _favoritecityEntry sur le nom de la ville favorite actuelle
                _favoritecityEntry.Text = favoriteCity.Name;

                // Définit la propriété CityToUpdate
                CityToUpdate = favoriteCity;
            }
        }



        // Méthode pour supprimer une ville favorite
        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var favoriteCity = button?.BindingContext as FavoriteCity;

            if (favoriteCity != null)
            {
                await _database.DeleteFavoriteCityAsync(favoriteCity);
                FavoriteCities.Remove(favoriteCity);
            }
        }

        string GenerateRequestURL(string endPoint, string cityName)
        {
            string requestUri = endPoint;
            requestUri += $"?q={cityName}"; // Ajoute la ville à la requête
            requestUri += "&units=metric"; // Définit l'unité de mesure en Celsius
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}"; // Ajoute la clé API

            return requestUri;
        }

        async void OnFavoriteCityTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedCity = e.Item as FavoriteCity;
            if (selectedCity != null)
            {
                var weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint, selectedCity.Name));

                if (weatherData == null)
                {
                    await DisplayAlert("Erreur", "Impossible de récupérer les données météorologiques pour la ville sélectionnée. Vérifiez si le nom de la ville est correct.", "OK");
                }
                else
                {
                    var mainPage = new MainPage();
                    mainPage.SetWeatherData(weatherData);
                    await Navigation.PushAsync(mainPage);
                }
            }
        }




    }
}




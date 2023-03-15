using System.Collections.ObjectModel;

namespace MeteoApp
{
    public partial class FavoriteCitiesPage : ContentPage
    {
        private FavoriteCityDatabase _database;
        RestService _restService;

        public ObservableCollection<FavoriteCity> FavoriteCities { get; set; }

        public FavoriteCitiesPage()
        {
            // Initialise les composants de la page
            InitializeComponent();

            // Construit le chemin d'acc�s � la base de donn�es SQLite en utilisant le dossier d'application local
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "weather.db3");
            // Cr�e une nouvelle instance de la base de donn�es FavoriteCityDatabase en utilisant le chemin d'acc�s sp�cifi�
            _database = new FavoriteCityDatabase(dbPath);

            // Appelle la m�thode pour charger les villes favorites de mani�re asynchrone
            LoadFavoriteCitiesAsync();

            _restService = new RestService(); // Ajoutez cette ligne pour initialiser _restService
        }

        private async void LoadFavoriteCitiesAsync()
        {
            // R�cup�re la liste des villes favorites de mani�re asynchrone depuis la base de donn�es
            var favoriteCitiesList = await _database.GetFavoriteCitiesAsync();
            // Cr�e une nouvelle ObservableCollection � partir de la liste des villes favorites r�cup�r�es
            FavoriteCities = new ObservableCollection<FavoriteCity>(favoriteCitiesList);
            // D�finit le contexte de liaison (BindingContext) pour la page en cours
            BindingContext = this;
        }

        // M�thode pour ajouter une ville favorite
        async void OnAddButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_favoritecityEntry.Text))
            {
                var favoriteCity = new FavoriteCity { Name = _favoritecityEntry.Text };
                await _database.SaveFavoriteCityAsync(favoriteCity);
                FavoriteCities.Add(favoriteCity);
                _favoritecityEntry.Text = string.Empty;
            }
        }

        // M�thode pour modifier une ville favorite
        async void OnEditButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var favoriteCity = button?.BindingContext as FavoriteCity;

            if (favoriteCity != null)
            {
                int index = FavoriteCities.IndexOf(favoriteCity);
                string newCity = _favoritecityEntry.Text;

                if (index >= 0 && !string.IsNullOrWhiteSpace(newCity))
                {
                    favoriteCity.Name = newCity;
                    await _database.SaveFavoriteCityAsync(favoriteCity);
                    FavoriteCities[index] = favoriteCity;
                    _favoritecityEntry.Text = string.Empty;
                }
            }
        }

        // M�thode pour supprimer une ville favorite
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
            requestUri += $"?q={cityName}"; // Ajoute la ville � la requ�te
            requestUri += "&units=metric"; // D�finit l'unit� de mesure en Celsius
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}"; // Ajoute la cl� API

            return requestUri;
        }

        async void OnFavoriteCityTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedCity = e.Item as FavoriteCity;
            if (selectedCity != null)
            {
                var weatherData = await _restService.GetWeatherData(GenerateRequestURL(Constants.OpenWeatherMapEndpoint, selectedCity.Name));
                var mainPage = new MainPage();
                mainPage.SetWeatherData(weatherData);
                await Navigation.PushAsync(mainPage);
            }
        }



    }
}




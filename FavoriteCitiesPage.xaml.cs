using System.Collections.ObjectModel;

namespace MeteoApp;

public partial class FavoriteCitiesPage : ContentPage
{
    public ObservableCollection<string> FavoriteCities { get; set; }

    public FavoriteCitiesPage()
    {
        InitializeComponent();
        FavoriteCities = new ObservableCollection<string>(App.FavoriteCities);
        BindingContext = this;
    }

    void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(_favoritecityEntry.Text))
        {
            FavoriteCities.Add(_favoritecityEntry.Text);
            App.FavoriteCities = new List<string>(FavoriteCities);
            _favoritecityEntry.Text = string.Empty;
        }
    }

    void OnEditButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var city = button?.BindingContext as string;

        if (city != null)
        {
            int index = FavoriteCities.IndexOf(city);
            string newCity = _favoritecityEntry.Text;

            if (index >= 0 && !string.IsNullOrWhiteSpace(newCity))
            {
                FavoriteCities[index] = newCity;
                App.FavoriteCities = new List<string>(FavoriteCities);
                _favoritecityEntry.Text = string.Empty;
            }
        }
    }

    void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var city = button?.BindingContext as string;

        if (city != null)
        {
            FavoriteCities.Remove(city);
            App.FavoriteCities = new List<string>(FavoriteCities);
        }
    }
}

   

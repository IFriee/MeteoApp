namespace MeteoApp;

public partial class App : Application
{
    public static List<string> FavoriteCities { get; set; } = new List<string>();

    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

}

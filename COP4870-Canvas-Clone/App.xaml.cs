namespace COP4870_Canvas_Clone;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new AppShell();  // Ensures that Shell is used as the main page
    }
}


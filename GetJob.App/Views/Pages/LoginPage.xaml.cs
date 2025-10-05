using System.Threading.Tasks;

namespace GetJob.App.Views.Pages;

public partial class LoginPage : ContentPage
{
    private readonly HttpClient _httpClient;

	public LoginPage()
	{
		InitializeComponent();
        var factory = App.Current.Handler.MauiContext.Services
         .GetService<IHttpClientFactory>();
        _httpClient = factory.CreateClient("CareerLinker");
	}

    private void Button_Clicked(object sender, EventArgs e)
    {

    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

    }
}
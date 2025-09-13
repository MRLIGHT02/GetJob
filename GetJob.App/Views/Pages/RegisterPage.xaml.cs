namespace GetJob.App.Views.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

   

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private void RegisterButton_Clicked(object sender, EventArgs e)
    {

    }



    //private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    //{
    //    Shell.Current.GoToAsync(nameof(LoginPage));
    //}
}
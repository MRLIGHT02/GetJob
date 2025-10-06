using GetJob.Dtos;
using GetJob.Entities;
using System.Net.Http.Json;

namespace GetJob.App.Views.Pages;

public partial class RegisterPage : ContentPage
{

    private readonly HttpClient _httpClient;
    public RegisterPage(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("CareerLinker");
        InitializeComponent();
  

    }




    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));

    }

    private async void OnRegister_Clicked(object sender, EventArgs e)
    {
        try
        {

            var data = new UserRegistrationDto()
            {
                Name = NameEntry.Text
             ,
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text,
               
            };

            if (data.Password != ConfirmPasswordEntry.Text)
            {
                checkPassword.Text = "Password does not matched";
            }
            else
            {
                checkPassword.Text = "";
                bool result = await RegisterAsync(data);
                if (result)
                {
                    await DisplayAlert("Success", "Registration successful!", "OK");
                    await Shell.Current.GoToAsync(nameof(LoginPage));
                }
                else
                {

                    await DisplayAlert("Error", "Registration failed!", "OK");
                }
            }
        }
        catch (InvalidNavigationException ex)
        {
            await DisplayAlert("Route Failed", ex.Message, "Ok");
        }

    }

    private void OnSignIn_Clicked(object sender, EventArgs e)
    {

    }


    public async Task<bool> RegisterAsync(UserRegistrationDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync
                ("api/Users/register", dto);

            if (response.IsSuccessStatusCode)
                return true; // success
            else
                return false; // failure
        }
        catch (Exception ex)
        {
            // TODO: handle/log exception
            return false;
        }
    }


    //private async void RegisterButton_Clicked(object sender, EventArgs e)
    //{
    

    //}



    //private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    //{
    //   
    //}
}
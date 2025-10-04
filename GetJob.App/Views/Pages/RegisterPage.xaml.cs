using GetJob.Dtos;
using GetJob.Entities;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GetJob.App.Views.Pages;

public partial class RegisterPage : ContentPage
{

    private readonly HttpClient _httpClient;
	public RegisterPage()
	{
		InitializeComponent();
        RolePicker.ItemsSource = Enum.GetNames(typeof(UserRole));
        _httpClient = new HttpClient { BaseAddress = new Uri("https://192.168.1.33:7143/") };
    }




    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
        
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
    

    private async void RegisterButton_Clicked(object sender, EventArgs e)
    {
       var data= new UserRegistrationDto() { Name=NameEntry.Text
        ,Email=EmailEntry.Text,Password=PasswordEntry.Text,Role=RolePicker.SelectedItem?.ToString()};

        if (data.Password != ConfirmPasswordEntry.Text)
        {
            checkPassword.Text = "Password does not matched";
        }
        else
        {
            checkPassword.Text = "";
            bool result =await RegisterAsync(data);
            if (result)
            {
               await Shell.Current.GoToAsync(nameof(LoginPage));
               await DisplayAlert("Success", "Registration successful!", "OK");
            }
            else
            {

                await DisplayAlert("Error", "Registration failed!", "OK");
            }
        }

        
    }



    //private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    //{
    //   
    //}
}
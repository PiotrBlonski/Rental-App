using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Pages;
using ICarus_Rental.Models;

namespace ICarus_Rental.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        string name;

        [ObservableProperty]
        string lastName;

        [ObservableProperty]
        string login;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string message;

        [RelayCommand]
        public async Task Register()
        {
            User User = new();

            HttpResponseMessage Response = await User.SignUp(Name, LastName, Login, Password);
            if (!Response.IsSuccessStatusCode)
            {
                Message = await Response.Content.ReadAsStringAsync();
                return;
            }

            Response = await User.LogIn(Login, Password);
            if (Response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"../{nameof(MainPage)}",
                            new Dictionary<string, object> { { "User", User } });
            }
            else Message = await Response.Content.ReadAsStringAsync();
        }

        public void RestartData()
        {
            Login = ""; Password = ""; Name = ""; LastName = ""; Message = "";
        }
    }
}

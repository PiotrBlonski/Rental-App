using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Pages;
using ICarus_Rental.Models;

namespace ICarus_Rental.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        string login;

        [ObservableProperty]
        string password;

        [ObservableProperty]
        string message;

        [RelayCommand]
        public async Task LogIn(string Token)
        {
            User User = new();

            HttpResponseMessage Response = Token == null ? await User.LogIn(Login, Password) : await User.LogIn(Token);

            if (Response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"{nameof(MainPage)}",
                            new Dictionary<string, object> { { "User", User } });
            }
            else Message = await Response.Content.ReadAsStringAsync();
        }

        public void RestartData()
        {
            Login = ""; Password = ""; Message = "";
        }
    }
}

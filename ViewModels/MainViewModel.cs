using CommunityToolkit.Mvvm.ComponentModel;
using ICarus_Rental.Models;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Pages.User;
using Microsoft.Maui.Controls.Maps;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("User", "User")]
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public User user;

        [RelayCommand]
        async Task OpenProfile() => await Shell.Current.GoToAsync($"{nameof(ProfilePage)}", new Dictionary<string, object> { { "User", User } });
    }
}

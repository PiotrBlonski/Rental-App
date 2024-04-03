using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;
using ICarus_Rental.Pages.User;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("User", "User")]
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        User user;

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task Logout() => await User.LogOut();

        [RelayCommand]
        void Refresh() => UpdateData();

        [RelayCommand]
        async Task GoToSortPage(List<Car> Cars) => await Shell.Current.GoToAsync($"{nameof(SortPage)}",
                                                               new Dictionary<string, object> { { "User", User }, { "History", Cars == User.RentHistory } });

        [RelayCommand]
        async Task GoToSettingsPage(User User) => await Shell.Current.GoToAsync($"{nameof(SettingsPage)}",
                                                              new Dictionary<string, object> { { "User", User } });

        [RelayCommand]
        async Task GoToCardsPage(User User) => await Shell.Current.GoToAsync($"{nameof(CardsPage)}",
                                                           new Dictionary<string, object> { { "User", User } });
        [RelayCommand]
        public async Task GoToRentInfo(Car Car)
        {
            if (Car != null && User != null)
                await Shell.Current.GoToAsync($"{nameof(RentInfoPage)}",
                    new Dictionary<string, object> { { "User", User }, { "Car", Car } });
        }

        partial void OnUserChanged(User value) => UpdateData();

        async void UpdateData()
        {
            IsRefreshing = true;
            await User.FetchProfile();
            await User.UpdateHistory();
            await User.UpdateRentedCars();
            IsRefreshing = false;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;
using ICarus_Rental.Pages;
using Microsoft.Maui.Controls.Maps;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("Pin", "Pin")]
    [QueryProperty("User", "User")]
    public partial class LocationViewModel : ObservableObject
    {
        [ObservableProperty]
        Pin pin;

        [ObservableProperty]
        List<Car> cars;

        [ObservableProperty]
        User user;

        async partial void OnPinChanged(Pin value) => Cars = await ApiClient.GetLocationCars(Pin);

        [RelayCommand]
        async Task GoToCarPage(Car Car)
        {
            if (Cars != null && User != null && Pin != null)
                await Shell.Current.GoToAsync($"{nameof(CarRentPage)}",
                  new Dictionary<string, object> { { "User", User }, { "Car", Car }, { "Pin", Pin } });
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;
using ICarus_Rental.Pages;
using Microsoft.Maui.Controls.Maps;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("Car", "Car")]
    [QueryProperty("User", "User")]
    [QueryProperty("Pin", "Pin")]
    public partial class CarRentViewModel : ObservableObject
    {
        [ObservableProperty]
        Car car;

        [ObservableProperty]
        User user;

        [ObservableProperty]
        Pin pin;

        [RelayCommand]
        public async Task GoToHistory()
        {
            if(Car != null) await Shell.Current.GoToAsync($"{nameof(CarHistoryPage)}",
                                  new Dictionary<string, object> { { "Car", Car } });
        }

        [RelayCommand]        
        public async Task Rent()
        {
            if (Car != null && User != null && Pin != null)
            {
                GeolocationRequest Request = new GeolocationRequest(GeolocationAccuracy.Best);
                Location Location = await Geolocation.GetLocationAsync(Request);
                double Distance = Location.CalculateDistance(Pin.Location, Location, DistanceUnits.Kilometers);
                if (Distance > 0.5) await Shell.Current.DisplayAlert("Error", "You are too far from this location", "OK");
                else await Shell.Current.GoToAsync($"{nameof(RentSummaryPage)}",
                           new Dictionary<string, object> { { "Car", Car }, { "User", User }, { "Pin", Pin } });
            }
        }
    }
}

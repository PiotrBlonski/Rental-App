using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;
using ICarus_Rental.Pages;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("Car", "Car")]
    [QueryProperty("User", "User")]
    public partial class RentSummaryViewModel : ObservableObject
    {
        [ObservableProperty]
        Car car;

        [ObservableProperty]
        User user;

        [RelayCommand]
        public async Task Rent()
        {
            if (Car == null || User == null)
                return;

            HttpResponseMessage Response = await User.RentCar(Car);

            if(!Response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Error", await Response.Content.ReadAsStringAsync(), "OK");
                return;
            }

            await Shell.Current.DisplayAlert("Success", "Car has been rented", "OK");
            await Shell.Current.GoToAsync("../../..");
        }
    }
}

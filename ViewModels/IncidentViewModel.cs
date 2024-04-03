using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("Car", "Car")]
    [QueryProperty("User", "User")]
    public partial class IncidentViewModel : ObservableObject
    {
        [ObservableProperty]
        Car car;

        [ObservableProperty]
        User user;

        [ObservableProperty]
        string details;

        [RelayCommand]
        public async Task Submit()
        {
            HttpResponseMessage Response = await User.ReportIncident(Car, Details);
            if(Response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Success", "Incident reported our support will contact you soon", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else await Shell.Current.DisplayAlert("Error", "Could not report incident", "OK");
        }
    }
}

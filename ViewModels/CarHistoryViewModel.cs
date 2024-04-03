using CommunityToolkit.Mvvm.ComponentModel;
using ICarus_Rental.Models;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("Car", "Car")]
    public partial class CarHistoryViewModel : ObservableObject
    {
        [ObservableProperty]
        Car car;

        [ObservableProperty]
        List<Incident> history;

        async partial void OnCarChanged(Car value)
        {
            if (Car != null)
                History = await ApiClient.GetCarHistory(Car);
        }
    }
}

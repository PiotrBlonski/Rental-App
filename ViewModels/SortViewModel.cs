using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;
using ICarus_Rental.Pages.User;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("User", "User")]
    [QueryProperty("History", "History")]
    public partial class SortViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SortedCars))]
        User user;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SortedCars))]
        bool history;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SortedCars))]
        DateTime fromDate = DateTime.Today.AddYears(-1);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SortedCars))]
        DateTime toDate = DateTime.Now.Date;

        public IEnumerable<Car> SortedCars => (History ? User?.RentHistory : User?.RentedCars)?.Where(car => car.StartDate >= FromDate && car.EndDate <= ToDate);

        [RelayCommand]
        public async Task GoToRentInfo(Car Car)
        {
            if (Car != null && User != null)
                await Shell.Current.GoToAsync($"{nameof(RentInfoPage)}",
                    new Dictionary<string, object> { { "User", User },  { "Car", Car }  });
        }

        public void UpdateList() => OnPropertyChanged(nameof(SortedCars));
    }
}

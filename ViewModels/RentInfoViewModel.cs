using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;
using ICarus_Rental.Pages.User;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("Car", "Car")]
    [QueryProperty("User", "User")]
    public partial class RentInfoViewModel : ObservableObject
    {
        [ObservableProperty]
        Car car;

        [ObservableProperty]
        User user;

        [ObservableProperty]
        public bool isPaid = true;

        [ObservableProperty]
        public bool showButton;

        [ObservableProperty]
        public string status;

        [RelayCommand]
        public void SelectCard(Card Card)
        {
            foreach (Card UserCard in User.Cards)
                if (UserCard != Card)
                    UserCard.Selected = false;

            Card.Selected = !Card.Selected;
        }

        [RelayCommand]
        public async Task GoToIncidentPage()
        {
            await Shell.Current.GoToAsync($"{nameof(IncidentPage)}",
                new Dictionary<string, object> { { "User", User }, { "Car", Car } });
        }
    }
}

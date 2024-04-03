using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;
using ICarus_Rental.Pages.User;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("User", "User")]
    public partial class CardsViewModel : ObservableObject
    {
        [ObservableProperty]
        User user;

        [RelayCommand]
        public async Task GoToAddPage()
        {
            if (User != null)
                await Shell.Current.GoToAsync($"{nameof(CardAddPage)}",
                  new Dictionary<string, object> { { "User", User } });
        }

        [RelayCommand]
        public void RemoveCard(Card Card) => User.RemoveCard(Card);
    }
}

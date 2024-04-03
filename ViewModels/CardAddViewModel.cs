using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;

namespace ICarus_Rental.ViewModels
{
    [QueryProperty("User", "User")]
    public partial class CardAddViewModel : ObservableObject
    {
        [ObservableProperty]
        User user;

        [ObservableProperty]
        Card card = new();

        [RelayCommand]
        public async Task AddCard()
        {
            Card.Number = string.Concat(Card.Number.Where(c => !char.IsWhiteSpace(c)));
            if (Card.Number.Length != 16 || !long.TryParse(Card.Number, out long num))
            {
                await Shell.Current.DisplayAlert("Error", "Invalid card number", "OK");
                return;
            }
            if (Card.CVC.Length != 3 || !int.TryParse(Card.CVC, out int cvc))
            {
                await Shell.Current.DisplayAlert("Error", "Invalid CVC", "OK");
                return;
            }
            if (Card.ExpirationDate.Length != 7 || !int.TryParse(Card.ExpirationDate.Split('/')[0], out int mon) || !int.TryParse(Card.ExpirationDate.Split('/')[1], out int year))
            {
                await Shell.Current.DisplayAlert("Error", "Invalid expiration date", "OK");
                return;
            }
            if (Card.Name.Length < 1)
            {
                await Shell.Current.DisplayAlert("Error", "Name must contain characters", "OK");
                return;
            }
            await Shell.Current.GoToAsync("..");
            User.AddCard(Card);
        }
    }
}

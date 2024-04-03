using ICarus_Rental.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using ICarus_Rental.Models;
using Map = Microsoft.Maui.Controls.Maps.Map;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace ICarus_Rental.Pages.User;

public partial class RentInfoPage : ContentPage
{
    RentInfoViewModel viewModel;
	public RentInfoPage(RentInfoViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        viewModel = vm;
	}

    Map Map;
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        GeolocationRequest Request = new GeolocationRequest(GeolocationAccuracy.Best);
        Location Location = await Geolocation.GetLocationAsync(Request);
        var Map = new Map(MapSpan.FromCenterAndRadius(Location, Distance.FromKilometers(2)));
        Map.HeightRequest = 250;
        Map.IsShowingUser = true;
        var p = 0;
        foreach (Pin Pin in (await ApiClient.GetLocations())?.Select(loc => loc.Pin))
        {
            Map.Pins.Add(Pin);
            p++;
        }
        this.Map = Map;
        MapBorder.Content = Map;
        Update();
    }

    private async void ReturnCar(object sender, EventArgs e)
    {
        GeolocationRequest Request = new GeolocationRequest(GeolocationAccuracy.Best);
        Location Location = await Geolocation.GetLocationAsync(Request);
        foreach (Pin Pin in Map.Pins)
        {
            if (Location.CalculateDistance(Location, Pin.Location, DistanceUnits.Kilometers) < 0.1)
            {
                Card Card = viewModel.User.Cards.FirstOrDefault(card => card.Selected);
                HttpResponseMessage Response = await viewModel.User.ReturnCar(viewModel.Car, Pin.Label);

                if (Response.IsSuccessStatusCode)
                {
                    string PaymentMessage = "";
                    if(Card != null)
                    {
                        HttpResponseMessage PayResponse = await viewModel.User.Pay(viewModel.Car, Card);
                        PaymentMessage = PayResponse.IsSuccessStatusCode ? " with successful payment" : " with failed payment (try again later)";
                        Update();
                    }
                    await Shell.Current.DisplayAlert("Success", "Car has been returned" + PaymentMessage, "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else await Shell.Current.DisplayAlert("Error", await Response.Content.ReadAsStringAsync(), "OK");
                return;
            }
        }
        await Shell.Current.DisplayAlert("Error", "You are too far from any location to return this car", "OK");
    }

    private async void Pay(object sender, EventArgs e)
    {
        Card Card = viewModel.User.Cards.FirstOrDefault(card => card.Selected);

        if (Card != null)
        {
            HttpResponseMessage PayResponse = await viewModel.User.Pay(viewModel.Car, Card);
            if (PayResponse.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Success", "Payment accepted", "OK");
                Update();
            }
            else await Shell.Current.DisplayAlert("Error", "Payment failed", "OK");
        }
        else await Shell.Current.DisplayAlert("Error", "No Card Selected", "OK");
    }

    async void Update()
    {
        bool Paid = await viewModel.User.GetPaymentStatus(viewModel.Car);
        viewModel.IsPaid = Paid;
        viewModel.Status = viewModel.Car != null ? (Paid ? "Paid" : "Not Paid") : "";
        viewModel.ShowButton = viewModel.Car.HasEnded && !Paid;
    }
}
using ICarus_Rental.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace ICarus_Rental.Pages
{
    public partial class MainPage : ContentPage
    {
        readonly MainViewModel viewModel;
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            viewModel = vm;
        }

        protected override bool OnBackButtonPressed() => true;

        Pin Pin;
        private void Pin_MarkerClicked(object sender, PinClickedEventArgs e) => Pin = sender as Pin;

        private async void GoToLocation(object sender, EventArgs e)
        {
            if (Pin == null) await DisplayAlert("Error", "Pick location first!", "OK");
            else await Shell.Current.GoToAsync($"{nameof(LocationPage)}", new Dictionary<string, object> { { "Pin", Pin }, { "User", viewModel.User } });
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            GeolocationRequest Request = new GeolocationRequest(GeolocationAccuracy.Best);
            Location Location = await Geolocation.GetLocationAsync(Request);
            var Map = new Map(MapSpan.FromCenterAndRadius(Location, Distance.FromKilometers(2)));
            Map.HeightRequest = 500;
            Map.IsShowingUser = true;
            var p = 0;
            foreach (Pin Pin in (await ApiClient.GetLocations())?.Select(loc => loc.Pin))
            {
                Map.Pins.Add(Pin);
                Map.Pins[p].MarkerClicked += Pin_MarkerClicked;
                p++;
            }
            MapBorder.Content = Map;
        }
    }
}

using ICarus_Rental.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace ICarus_Rental.Pages;
[QueryProperty("Pin", "Pin")]
public partial class RentSummaryPage : ContentPage
{
    Pin pin;
    public Pin Pin
    {
        get => pin;
        set { pin = value; OnPropertyChanged(); }
    }
    public RentSummaryPage(RentSummaryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Pin != null)
        {
            var Map = new Map(MapSpan.FromCenterAndRadius(Pin.Location, Distance.FromMeters(500)));
            Map.IsScrollEnabled = false;
            Map.IsZoomEnabled = false;
            Map.Pins.Add(Pin);
            MapBorder.Content = Map;
        }
    }
}
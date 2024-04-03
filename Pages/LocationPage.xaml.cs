using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages;

public partial class LocationPage : ContentPage
{
	public LocationPage(LocationViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
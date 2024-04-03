using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages;

public partial class CarRentPage : ContentPage
{
	public CarRentPage(CarRentViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
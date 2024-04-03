using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages;

public partial class CarHistoryPage : ContentPage
{
	public CarHistoryPage(CarHistoryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages.User;

public partial class CardsPage : ContentPage
{
	public CardsPage(CardsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
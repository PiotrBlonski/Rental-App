using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages.User;

public partial class CardAddPage : ContentPage
{
	public CardAddPage(CardAddViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
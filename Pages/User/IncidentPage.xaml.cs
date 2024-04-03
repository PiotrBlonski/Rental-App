using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages.User;

public partial class IncidentPage : ContentPage
{
	public IncidentPage(IncidentViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
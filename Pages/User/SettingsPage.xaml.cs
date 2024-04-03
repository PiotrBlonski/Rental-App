using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages.User;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		vm.SettingsPage = this;
	}
}
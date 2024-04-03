using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages.User;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
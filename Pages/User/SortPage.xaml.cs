using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages.User;

public partial class SortPage : ContentPage
{
	SortViewModel viewModel;
	public SortPage(SortViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		viewModel = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.UpdateList();
    }
}
using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages.Authorization;

public partial class RegisterPage : ContentPage
{
    readonly RegisterViewModel viewModel;
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.RestartData();
    }
}
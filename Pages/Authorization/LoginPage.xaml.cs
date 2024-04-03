using ICarus_Rental.ViewModels;

namespace ICarus_Rental.Pages.Authorization;

public partial class LoginPage : ContentPage
{
    readonly LoginViewModel viewModel;
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        viewModel.RestartData();

        string FilePath = Path.Combine(FileSystem.AppDataDirectory, "Token.txt");

        if (File.Exists(FilePath))
            await viewModel.LogIn(await File.ReadAllTextAsync(FilePath));
    }

    private async void GoToSignUpPage(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(RegisterPage));

    protected override bool OnBackButtonPressed() => true;
}
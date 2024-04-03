using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICarus_Rental.Models;
using ICarus_Rental.Pages.User;
namespace ICarus_Rental.ViewModels
{
    [QueryProperty("User", "User")]
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        User user;

        [ObservableProperty]
        SettingsPage settingsPage;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string lastName;

        [ObservableProperty]
        string oldPassword = "";

        [ObservableProperty]
        string newPassword = "";

        partial void OnUserChanged(User value)
        {
            Name = User?.FullName.Split(' ')[0];
            LastName = User?.FullName.Split(' ')[1];
        }

        [RelayCommand]
        public async Task UploadAvatar()
        {
            FileResult Result = await FilePicker.Default.PickAsync();

            if (Result != null && (Result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || Result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase)))
            {
                HttpResponseMessage Response = await User.UploadAvatar(Result.FullPath);
                if(Response.IsSuccessStatusCode)
                    await SettingsPage.DisplayAlert("Success", "Avatar uploaded you may need to restart app for changed to show up", "OK");
                else
                    await SettingsPage.DisplayAlert("Error", "Incorrect file format or upload failed", "OK");
            }
            else if (Result != null) await SettingsPage.DisplayAlert("Error", "Incorrect file format only .jpg or .png files are compatible", "OK");
        }

        [RelayCommand]
        public async Task UpdatePassword()
        {
            string UserResponse = await SettingsPage.DisplayActionSheet("Change password? (You will be logged out", null, null, "Yes", "No");

            if (UserResponse != "Yes")
                return;

            HttpResponseMessage Response = await User.UpdatePassword(NewPassword);
            if (Response.IsSuccessStatusCode)
            {
                RestartData();
                await User.LogOut();
            }
            else await SettingsPage.DisplayAlert("Error", await Response.Content.ReadAsStringAsync(), "OK");
        }
        [RelayCommand]
        public async Task UpdateDetails()
        {
            HttpResponseMessage Response = await User.UpdateDetails(Name, LastName);
            if (Response.IsSuccessStatusCode)
            {
                RestartData();
                await SettingsPage.DisplayAlert("Success", "Details have been changed", "OK");
            }
            else await SettingsPage.DisplayAlert("Error", await Response.Content.ReadAsStringAsync(), "OK");
        }

        void RestartData()
        {
            OldPassword = "";
            NewPassword = "";
            Name = User?.FullName.Split(' ')[0];
            LastName = User?.FullName.Split(' ')[1];
        }
    }
}

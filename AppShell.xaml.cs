using ICarus_Rental.Pages;
using ICarus_Rental.Pages.Authorization;
using ICarus_Rental.Pages.User;

namespace ICarus_Rental
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(SortPage), typeof(SortPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(LocationPage), typeof(LocationPage));
            Routing.RegisterRoute(nameof(CarRentPage), typeof(CarRentPage));
            Routing.RegisterRoute(nameof(CarHistoryPage), typeof(CarHistoryPage));
            Routing.RegisterRoute(nameof(RentSummaryPage), typeof(RentSummaryPage));
            Routing.RegisterRoute(nameof(RentInfoPage), typeof(RentInfoPage));
            Routing.RegisterRoute(nameof(CardsPage), typeof(CardsPage));
            Routing.RegisterRoute(nameof(CardAddPage), typeof(CardAddPage));
            Routing.RegisterRoute(nameof(IncidentPage), typeof(IncidentPage));
        }
    }
}

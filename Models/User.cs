using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using ICarus_Rental.Pages.Authorization;
using ICarus_Rental.Web;
using Microsoft.Maui.Controls.Maps;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Linq;

namespace ICarus_Rental.Models
{
    public partial class User : ObservableObject, IDisposable
    {
        readonly Client Client;

        public User()
        {
            Client = new(Statics.Address);
            Cards = new();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName), nameof(ProfilePicture))]
        UserProfile profile;

        [ObservableProperty]
        List<Car> rentedCars;

        [ObservableProperty]
        List<Car> rentHistory;

        [ObservableProperty]
        ObservableCollection<Card> cards;

        public string FullName => Profile.Name + " " + Profile.LastName;

        public string ProfilePicture => Profile.ImageUrl;

        public async Task<HttpResponseMessage> LogIn(string Login, string Password)
        {
            HttpResponseMessage Response = await Client.GetToken(Login, Password);
            if (Response.IsSuccessStatusCode)
            {
                await FetchProfile();
                Client.SaveToken();
            }
            return Response;
        }

        public async Task<HttpResponseMessage> LogIn(string Token)
        {
            HttpResponseMessage Response = await Client.GetToken(Token);

            if (Response.IsSuccessStatusCode)
            {
                await FetchProfile();
                Cards = Card.Load().ToObservableCollection();
            }

            return Response;
        }

        public async Task<HttpResponseMessage> LogOut()
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Post, "/user/logout", true);

            if (Response.IsSuccessStatusCode)
            {
                Client.LoggedOut = true;
                File.Delete(Path.Combine(FileSystem.AppDataDirectory, "Token.txt"));
                File.Delete(Path.Combine(FileSystem.AppDataDirectory, "Cards.txt"));
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                Dispose();
            }

            return Response;
        }

        public async Task<HttpResponseMessage> SignUp(string Name, string LastName, string Login, string Password)
        {
            var UserData = new Dictionary<string, string>
            {
                { "name", Name },
                { "lastname", LastName },
                { "login", Login },
                { "password", Password },
            };
            return await Client.SendRequest(HttpMethod.Post, "/user/register", false, UserData);
        }

        public async Task<HttpResponseMessage> UpdatePassword(string NewPassword)
        {
            var PasswordData = new Dictionary<string, string> { { "newPassword", NewPassword } };
            return await Client.SendRequest(HttpMethod.Post, "/user/changepassword", true, PasswordData);
        }

        public async Task<HttpResponseMessage> UploadAvatar(string FilePath)
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Post, "/user/avatar", true, null, FilePath);
            await FetchProfile();
            return Response;
        }

        public async Task<HttpResponseMessage> UpdateDetails(string Name, string LastName)
        {
            var UserData = new Dictionary<string, string>
            {
                { "name", Name },
                { "lastname", LastName }
            };
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Post, "/user/changedetails", true, UserData);
            await FetchProfile();
            return Response;
        }

        public async Task<HttpResponseMessage> UpdateRentedCars()
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Get, "/user/cars", true);

            if (Response.IsSuccessStatusCode)
            {
                RentedCars = await Client.GetData<List<Car>>(Response);
                RentedCars = RentedCars.OrderByDescending(car => car.StartDate).ToList();
            }

            return Response;
        }

        public async Task<HttpResponseMessage> UpdateHistory()
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Get, "/user/history", true);

            if (Response.IsSuccessStatusCode)
            {
                RentHistory = await Client.GetData<List<Car>>(Response);
                RentHistory = RentHistory.OrderByDescending(car => car.EndDate).ToList();
            }

            return Response;
        }

        public async Task<HttpResponseMessage> RentCar(Car Car)
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Post, "/user/rent/" + Car.Id, true);

            if (Response.IsSuccessStatusCode)
                await UpdateRentedCars();

            return Response;
        }
        public async Task<HttpResponseMessage> ReturnCar(Car Car, string Location)
        {

            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Post, "/user/return/" + Car.Rentid + "/" + Location, true);

            if (Response.IsSuccessStatusCode)
            {
                await UpdateRentedCars();
                await UpdateHistory();
            }

            return Response;
        }

        public async Task<HttpResponseMessage> Pay(Car Car, Card Card)
        {
            var CardData = new Dictionary<string, string>
            {
                { "Number", Card.Number },
                { "CVC", Card.CVC },
                { "ExpireDate", Card.ExpirationDate }
            };

            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Post, "/user/pay/" + Car.Rentid, true, CardData);

            return Response;
        }

        public async Task<bool> GetPaymentStatus(Car Car)
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Get, "/user/status/" + Car.Rentid, true);
            bool Paid = await Response.Content.ReadAsStringAsync() == "Paid";
            return Paid;
        }

        public async Task<HttpResponseMessage> FetchProfile()
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Get, "/user/", true);

            if (Response.IsSuccessStatusCode)
                Profile = await Client.GetData<UserProfile>(Response);

            return Response;
        }

        public async Task<HttpResponseMessage> ReportIncident(Car Car, string Details)
        {
            var Data = new Dictionary<string, string>
            {
                { "Details", Details }
            };
            return await Client.SendRequest(HttpMethod.Post, "/user/report/" + Car.Rentid, true, Data); ;
        }

        public void AddCard(Card Card)
        {
            Cards.Add(Card);
            Card.Save(Cards.ToList());
        }

        public void RemoveCard(Card Card)
        {
            Cards.Remove(Card);
            Card.Save(Cards.ToList());
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}

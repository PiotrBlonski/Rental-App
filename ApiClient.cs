using ICarus_Rental.Models;
using ICarus_Rental.Web;
using Microsoft.Maui.Controls.Maps;

namespace ICarus_Rental
{
    public static class ApiClient
    {
        readonly static Client Client = new(Statics.Address);
        public static async Task<List<Models.Location>> GetLocations()
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Get, "/api/locations", false);
            return Response.IsSuccessStatusCode ? await Client.GetData<List<Models.Location>>(Response) : null;
        }

        public static async Task<List<Car>> GetLocationCars(Pin Pin)
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Get, "/api/locations/" + Pin.Label, false);
            return Response.IsSuccessStatusCode ? await Client.GetData<List<Car>>(Response) : null;
        }

        public static async Task<List<Incident>> GetCarHistory(Car Car)
        {
            HttpResponseMessage Response = await Client.SendRequest(HttpMethod.Get, "/api/cars/history/" + Car.Id, false);
            return Response.IsSuccessStatusCode ? await Client.GetData<List<Incident>>(Response) : null;
        }
    }
}

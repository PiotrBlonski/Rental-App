using CommunityToolkit.Mvvm.ComponentModel;

namespace ICarus_Rental.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int Rentid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
        public string State { get; set; }
        public string ImageUrl => Statics.Address + "/api/image/cars/" + Brand + "/" + Model;
        public string FullName => Brand + " " + Model;
        public string Details => Brand + " " + Model + " " + Year;
        public string StartDateString => StartDate.ToShortDateString();
        public string EndDateString => EndDate.ToShortDateString();
        public string FullPrice => Price + " PLN/day";
        public bool HasEnded => EndDate >= StartDate;
        public string Bill => (HasEnded ? 50 + (EndDate.Date - StartDate.Date).TotalDays * Price  : (50 + (DateTime.Today - StartDate.Date).Days * Price)) + " PLN";
    }
}

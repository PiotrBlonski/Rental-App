namespace ICarus_Rental.Models
{
    public class UserCar
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string ImageUrl => Statics.Address + "/api/image/cars/" + Brand + "/" + Model;
        public string FullName => Brand + " " + Model;
        public string StartDateString => StartDate.ToShortDateString();
        public string EndDateString => EndDate.ToShortDateString();
    }
}

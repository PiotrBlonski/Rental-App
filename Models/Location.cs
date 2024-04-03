using Microsoft.Maui.Controls.Maps;

namespace ICarus_Rental.Models
{
    public class Location
    {
        public string Name { get; set; }
        public string Cordinates { get; set; }
        public Pin Pin => new() { Label = Name, Location = new(double.Parse(Cordinates.Split(',')[0]), double.Parse(Cordinates.Split(',')[1])) };
    }
}

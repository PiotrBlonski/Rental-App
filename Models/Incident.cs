using Microsoft.Maui.Controls.Maps;

namespace ICarus_Rental.Models
{
    public class Incident
    {
        public string Details { get; set; }
        public DateTime Date { get; set; }
        public string DateOnly => Date.ToShortDateString();
    }
}

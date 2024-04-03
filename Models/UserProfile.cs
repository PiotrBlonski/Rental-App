namespace ICarus_Rental.Models
{
    public class UserProfile
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public string ImageUrl => Statics.Address + "/api/image/user/" + Id;
    }
}

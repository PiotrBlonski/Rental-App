using CommunityToolkit.Mvvm.ComponentModel;

namespace ICarus_Rental.Models
{
    public partial class Card : ObservableObject
    {
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string number;
        [ObservableProperty]
        public string cVC;
        [ObservableProperty]
        public string expirationDate;
        [ObservableProperty]
        public bool selected;

        public Card()
        {
            Number = "";
            CVC = "";
            ExpirationDate = "";
            Name = "My Card";
        }
        public override string ToString() => Number + "," + CVC + "," + ExpirationDate + "," + Name;
        public static void Save(List<Card> Cards)
        {
            string CardData = "";
            for (var c = 0; c < Cards.Count; c++)
                CardData += Cards[c].ToString() + (c == Cards.Count - 1 ? "" : ":");
            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "Cards.txt"), CardData);
        }
        public static List<Card> Load()
        {
            List<Card> Cards = new();

            if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, "Cards.txt")))
                return new();

            string CardsData = File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, "Cards.txt"));
            string[] CardsString = CardsData.Split(':');

            for (var c = 0; c < CardsString.Length; c++)
            {
                string[] CardData = CardsString[c].Split(',');
                Cards.Add(new Card()
                {
                    Number = CardData[0],
                    CVC = CardData[1],
                    ExpirationDate = CardData[2],
                    Name = CardData[3]
                });
            }

            return Cards;
        }
    }
}

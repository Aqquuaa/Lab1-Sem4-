namespace SmallDB
{
    using System.Text.Json.Serialization;
    public class Tour
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Price")]
        public int Price { get; set; }
        [JsonPropertyName("Space")]
        public int TourSpace { get; set; }
        [JsonPropertyName("SignedUsers")]
        public List<String> SignedUsers { get; set; } = new List<String>();
        public Tour()
        {
            SignedUsers = new List<string>();
        }
        public Tour(string tourName, int price = 0, int size = 0) 
        {
            Name = tourName;
            Price = price;
            TourSpace = size;
            SignedUsers = new List<String>();
        }
        public void EditPrice(int value)
        {
            Price = value;
            Console.WriteLine($"Succesfully changed {Name} price to {value}");
        }
        public bool SignUser(User user)
        {
            if (TourSpace > 0)
            {
                if(user.BuyTour(this))
                {
                    SignedUsers.Add(user.Username);
                    TourSpace--;
                    return true;
                }
            }
            else
            Console.WriteLine("Tour is full!");
            return false;
        }
        public void EditSize(int value)
        {
            TourSpace = value + 1;
            Console.WriteLine($"Succesfully changed {Name} size to {value}");
        }
    }
}

namespace SmallDB
{
    using System.Text.Json.Serialization;
    public class User
    {
        [JsonPropertyName("username")]
        public string Username { get; }

        [JsonPropertyName("password")]
        public string Password { get; }

        [JsonPropertyName("money")]
        public double Money { get; set; }

        [JsonPropertyName("discount")]
        public short Discount { get; set; }

        [JsonPropertyName("spent")]
        public double Spent { get; set; }

        [JsonPropertyName("currentTour")]
        public string CurrentTour { get; set; }

        public User(string username, string password = "0000")
        {
            Username = username;
            Password = password;
            Money = 0;
            Discount = 0;
            Spent = 0;
            CurrentTour = string.Empty;
        }

        [JsonConstructor]
        public User(
            string username,
            string password,
            double money,
            short discount,
            double spent,
            string currentTour)
        {
            Username = username;
            Password = password;
            Money = money;
            Discount = discount;
            Spent = spent;
            CurrentTour = currentTour;
        }
        public void AddMoney(int value)
        {
            Money += value;
            Console.WriteLine($"Succesfully added {value} to {Username}");
        }
        public void ChangeDiscount(int value)
        {
            Discount = (short)value;
            Console.WriteLine($"Succesfully changed discount to {value} for {Username}");
        }
        public bool BuyTour(Tour tour)
        {
             if (tour.Price <= Money)
             {
                Money -= tour.Price - tour.Price * (Discount/100);
                Spent += tour.Price - tour.Price * (Discount/100);
                Console.WriteLine($"Succesfully bought {tour.Name} for {tour.Price * (Discount/100)}");
                return true;
             }
            Console.WriteLine($"Not enough money to buy {tour.Name} for {tour.Price * (Discount / 100)}!");
            return false;
        }

    }

}

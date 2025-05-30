namespace SmallDB.Data
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents an application user with credentials, balance, discount, spending, and current tour.
    /// </summary>
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
        /// <summary>
        /// Initializes a new user with the specified username and optional password.
        /// </summary>
        /// <param name="username">The user's unique username.</param>
        /// <param name="password">The user's password. Defaults to "0000".</param>
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
        /// <summary>
        /// Adds the specified amount to the user's balance.
        /// </summary>
        /// <param name="value">The amount of money to add.</param>
        public void AddMoney(int value)
        {
            Money += value;
            Console.WriteLine($"Succesfully added {value} to {Username}");
        }

        /// <summary>
        /// Changes the user's discount to the specified percentage.
        /// </summary>
        /// <param name="value">The new discount value (0–100).</param>
        public void ChangeDiscount(int value)
        {
            Discount = (short)value;
            Console.WriteLine($"Succesfully changed discount to {value} for {Username}");
        }

        /// <summary>
        /// Attempts to purchase the given tour. If successful, deducts price (with discount)
        /// from the user's balance and increments their spent total.
        /// </summary>
        /// <param name="tour">The tour to purchase.</param>
        /// <returns>True if purchase succeeded; false if insufficient funds.</returns>
        public bool BuyTour(Tour tour)
        {
            if (tour.Price <= Money)
            {
                Money -= tour.Price - tour.Price * (Discount / 100);
                Spent += tour.Price - tour.Price * (Discount / 100);
                Console.WriteLine($"Succesfully bought {tour.Name} for {tour.Price * (Discount / 100)}");
                return true;
            }
            Console.WriteLine($"Not enough money to buy {tour.Name} for {tour.Price * (Discount / 100)}!");
            return false;
        }

    }

}

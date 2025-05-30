namespace SmallDB.Data
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a tour with a name, price, available space, and signed‐up users.
    /// </summary>
    public class Tour
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Price")]
        public int Price { get; set; }
        [JsonPropertyName("Space")]
        public int TourSpace { get; set; }
        [JsonPropertyName("SignedUsers")]
        public List<string> SignedUsers { get; set; } = new List<string>();
        public Tour()
        {
            SignedUsers = new List<string>();
        }
        /// <summary>
        /// Initializes a new tour with the given name, price, and space.
        /// </summary>
        /// <param name="tourName">The name of the tour.</param>
        /// <param name="price">The price of the tour.</param>
        /// <param name="size">The initial available spaces.</param>
        public Tour(string tourName, int price = 0, int size = 0)
        {
            Name = tourName;
            Price = price;
            TourSpace = size;
            SignedUsers = new List<string>();
        }

        /// <summary>
        /// Updates the price of this tour.
        /// </summary>
        /// <param name="value">The new price value.</param>
        public void EditPrice(int value)
        {
            Price = value;
            Console.WriteLine($"Succesfully changed {Name} price to {value}");
        }
        /// <summary>
        /// Attempts to sign up the specified user for this tour.
        /// If successful, deducts one from TourSpace and adds the user's username.
        /// </summary>
        /// <param name="user">The user attempting to sign up.</param>
        /// <returns>True if sign up succeeded; false if no space available or user funds insufficient.</returns>
        public bool SignUser(User user)
        {
            if (TourSpace > 0)
            {
                if (user.BuyTour(this))
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

        /// <summary>
        /// Updates the number of available spaces for this tour.
        /// </summary>
        /// <param name="value">The new space count.</param>
        public void EditSize(int value)
        {
            TourSpace = value + 1;
            Console.WriteLine($"Succesfully changed {Name} size to {value}");
        }
    }
}

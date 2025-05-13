using System.Text.Json;

namespace SmallDB
{
    internal static class JSONFileHelper
    {
        const string UserFilePath = "users.json";
        const string TourFilePath = "tours.json";
        const string AdminFilePath = "admin.json";

        private static readonly Dictionary<Type, string> FilePath = new()
    {
        { typeof(User), UserFilePath },
        { typeof(Tour), TourFilePath }
    };
        public static List<Admin> LoadAdmins()
        {
            if (!File.Exists(AdminFilePath))
                return new List<Admin>();

            var json = File.ReadAllText(AdminFilePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<List<Admin>>(json, options);
        }
        public static List<User> LoadUsers()
        {
            if (!File.Exists(UserFilePath))
                return new List<User>();

            var json = File.ReadAllText(UserFilePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<List<User>>(json, options);
        }
        public static List<Tour> LoadTours()
        {
            if (!File.Exists(TourFilePath))
                return new List<Tour>();

            var json = File.ReadAllText(TourFilePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<List<Tour>>(json, options);
        }
        public static void SaveUsers(List<User> u) => SaveList(u);
        public static void SaveTours(List<Tour> t) => SaveList(t);

        public static void SaveList<T>(List<T> list)
        {
            var type = typeof(T);
            if (!FilePath.TryGetValue(type, out var path))
                throw new InvalidOperationException($"No path registered for type {type.Name}");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(list, options);
            File.WriteAllText(path, json);
        }

        public static void AddUser(User newUser)
        {
            var users = LoadUsers();
            users.Add(newUser);
            SaveUsers(users);
            Console.WriteLine($"Added {newUser.Username}");
        }
        public static void AddTour(Tour newTour)
        {
            var tours = LoadTours();
            tours.Add(newTour);
            SaveTours(tours);
            Console.WriteLine($"Added {newTour.Name}");
        }
        public static void SaveUserTour(User _user, string tourName)
        {
            var users = LoadUsers();
            foreach (var user in users)
            {
                if (_user.Username == user.Username)
                {
                    user.CurrentTour = _user.CurrentTour;
                    user.Money = _user.Money;
                    user.Spent = _user.Spent;
                    AddUserToTour(tourName, _user.Username);
                    break;
                }
            }
            SaveUsers(users);
        }
        private static void AddUserToTour(string tourName, string usernameToAdd)
        {
            var tours = LoadTours();
            foreach (var tour in tours)
            {
                if (tour.Name == tourName)
                {
                    tour.TourSpace--;
                    tour.SignedUsers.Add(usernameToAdd);
                }
            }
            SaveTours(tours);
        }
        public static void EditTourPrice(string tourToFind, int price)
        {
            var tours = LoadTours();
            foreach (var tour in tours)
            {
                if (tour.Name == tourToFind)
                {
                    tour.EditPrice(price); 
                }
            }
            SaveTours(tours);
        }
        public static void EditTourSpace(string tourToFind, int space)
        {
            var tours = LoadTours();
            foreach (var tour in tours)
            {
                if (tour.Name == tourToFind)
                {
                    tour.EditSize(space);
                }
            }
            SaveTours(tours);
        }
        public static void AddMoneyToUser(string usernameToFind, int money)
        {
            var users = LoadUsers();
            foreach (var user in users)
            {
                if (user.Username == usernameToFind)
                {
                    user.AddMoney(money);
                    Console.WriteLine($"Added {money} to {user.Username}");
                    break;
                }
            }
            SaveUsers(users);
        }
        public static void DeleteUser(string usernameToDelete)
        {
            var users = LoadUsers();
            int removedCount = users.RemoveAll(u =>
                string.Equals(u.Username, usernameToDelete, StringComparison.OrdinalIgnoreCase));

            if (removedCount > 0)
            {
                SaveUsers(users);
                Console.WriteLine($"Deleted {removedCount} user(s) with username '{usernameToDelete}'.");
            }
            else
            {
                Console.WriteLine($"No user found with username '{usernameToDelete}'.");
            }
        }
        public static void DeleteTour(string tourToDelete)
        {
            var tours = LoadTours();
            int removedCount = tours.RemoveAll(t =>
                string.Equals(t.Name, tourToDelete, StringComparison.OrdinalIgnoreCase));

            if (removedCount > 0)
            {
                SaveTours(tours);
                Console.WriteLine($"Deleted {removedCount} tour(s) with name '{tourToDelete}'.");
            }
            else
            {
                Console.WriteLine($"No tour found with name '{tourToDelete}'.");
            }
        }
    }
}

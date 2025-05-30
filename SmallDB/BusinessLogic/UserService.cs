using SmallDB.Data;
using SmallDB.Services;

namespace SmallDB.BusinessLogic
{
    /// <summary>
    /// Encapsulates all business operations on <see cref="User"/> entities,
    /// backed by an <see cref="IStorageService{User}"/>.
    /// </summary>
    public class UserService
    {
        private List<User> _users = new List<User>();
        private readonly IStorageService<User> _userStorage;
        /// <summary>
        /// Initializes a new UserService that loads/saves users via <paramref name="userStorage"/>.
        /// </summary>
        /// <param name="userStorage">Storage implementation for users. Cannot be null.</param>
        public UserService(IStorageService<User> userStorage)
        {
            _userStorage = userStorage;
            _users = _userStorage.Load();
        }
        /// <summary>
        /// Prints all users to the console.
        /// </summary>
        public void ShowAllUsers()
        {
            Console.WriteLine("List of users: ");
            Generics.ShowList(_users, user => user.Username);
        }

        /// <summary>
        /// Adds money to the specified user's balance and persists the change.
        /// </summary>
        public void AddMoney(int value, User currentUser)
        {
            foreach (var user in _users)
            {
                if (user == currentUser)
                { 
                    user.Money += value;
                    break;
                }
            }
            _userStorage.Save(_users);
        }
        /// <summary>
        /// Adds a new user and persists it.
        /// </summary>
        public void AddUser(User user)
        {
            if(user != null)
            {
            _users.Add(user);
            _userStorage.Save(_users);
            }
        }
        /// <summary>
        /// Removes the given user if found, and persists the change.
        /// </summary>
        /// <returns>True if removed; false if not found.</returns>
        public bool RemoveUser(User user)
        {
            if(Generics.RemoveItemFromList
                (_users,
                u => string.Equals(u.Username, user.Username, StringComparison.OrdinalIgnoreCase)
                ))
            {
                _userStorage.Save(_users);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Charges the user for <paramref name="currentTour"/> (if they have funds)
        /// and persists the user list.
        /// </summary>
        public bool SignUser(User currentUser, Tour currentTour)
        {
            if (currentTour.TourSpace > 0)
            {
                currentUser.BuyTour(currentTour);
                _userStorage.Save(_users);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Finds a user by username.
        /// </summary>
        /// <param name="nameToFind">Username to search for.</param>
        /// <returns>The matching <see cref="User"/>, or null if none.</returns>
        public User? ReturnUserByName(string nameToFind)
        {
            var CurrentUser = Generics.SelectItemByName(_users, u => u.Username, nameToFind);
            if (CurrentUser != null)
            {
                Console.WriteLine($"Current user set to {CurrentUser.Username}");
                return CurrentUser;
            }
            else
            {
                Console.WriteLine("No such user found.");
                return null;
            }
        }
    }
}

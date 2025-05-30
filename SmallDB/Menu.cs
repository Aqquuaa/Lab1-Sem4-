using SmallDB.BusinessLogic;
using SmallDB.Data;
using SmallDB.Services;

namespace SmallDB
{
    /// <summary>
    /// Console menu that drives the SmallDB application.
    /// Handles admin login and dispatches to user/tour submenus.
    /// </summary>
    internal class Menu
    {
        private JsonStorageService<Admin> adminStorage = new JsonStorageService<Admin>("Data/admins.json");
        private JsonStorageService<Tour> tourStorage = new JsonStorageService<Tour>("Data/tours.json");
        private readonly UserService _userService;
        private readonly TourService _tourService;
        private string UserName;
        private string Password;
        private User CurrentUser = new User("None");
        private Tour CurrentTour = new Tour("None");
        private List<Admin> AdminList;
        /// <summary>
        /// Starts the main menu (login + user/tour options).
        /// </summary>
        public Menu()
        {
            var _userStorage = new JsonStorageService<User>("Data/users.json");
            var _tourStorage = new JsonStorageService<Tour>("Data/tours.json");

            _userService = new UserService(_userStorage);
            _tourService = new TourService(_tourStorage);
        }
        /// <summary>
        /// Prompts and validates admin credentials against admins.json.
        /// </summary>
        private bool Initialization()
        {
            AdminList = adminStorage.Load();
            Console.WriteLine("Enter Username:");
            UserName = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            Password = Console.ReadLine();
            foreach (var admin in AdminList)
            {
                if (admin.getName() == UserName
                    && admin.getPassword() == Password)
                {
                    return true;
                }
            }

            Console.WriteLine("Invalid username or password.");
            return false;
        }
        /// <summary>
        /// Generic helper that selects an item via a lookup function and assigns it.
        /// </summary>
        private void SelectItemByName<T>(Func<string, T?> selector, Action<T> assign)
            where T : class
        {
            Console.Write("Enter name: ");
            var name = Console.ReadLine()?.Trim() ?? "";

            var item = selector(name);
            if (item != null)
            {
                assign(item);
                Console.WriteLine($"Selected {typeof(T).Name}: {name}");
            }
            else
            {
                Console.WriteLine($"No {typeof(T).Name.ToLower()} found with name '{name}'.");
            }
        }
        /// <summary>
        /// Helper that selects current user via a lookup function and assigns it.
        /// </summary>
        private void SelectCurrentUser()
        {
            SelectItemByName(_userService.ReturnUserByName, u => CurrentUser = u);
        }
        /// <summary>
        /// Helper that selects current tour via a lookup function and assigns it.
        /// </summary>
        private void SelectCurrentTour()
        {
            SelectItemByName(_tourService.ReturnTourByName, t => CurrentTour = t);
        }
        /// <summary>
        /// Adds a new user via console input and saves to storage.
        /// </summary>
        private void AddUser()
        {
            Console.WriteLine("Enter Username for new user");
            string username = Console.ReadLine();
            Console.WriteLine("Enter Password for new user");
            string password = Console.ReadLine();
            _userService.AddUser(new User(username.Trim(), password.Trim()));
        }
        /// <summary>
        /// Attempts to parse an integer from a trimmed string input.
        /// Prints error and returns -1 if parsing fails.
        /// </summary>
        /// <param name="value">User input string.</param>
        /// <returns>Parsed integer or -1.</returns>
        private static int StringToInt(string value)
        {
            value = value.Trim();
            if (value.Length <= 5 && Char.IsDigit(value[0]))
            {
                return int.Parse(value);
            }

            Console.WriteLine("Wrong input value");
            return -1;
        }
        /// <summary>
        /// Removes item of required type from a list, also it can perform required action on deletion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">List from which element should be deleted</param>
        /// <param name="predicate">A function that tests each element for a specific condition</param>
        /// <param name="OnRemoved">Optional callback that is invoked after item deletion</param>
        /// <example>
        /// <code>
        /// RemoveItemFromList
        /// (UserList,
        /// u => string.Equals(u.Username, CurrentUser.Username, StringComparison.OrdinalIgnoreCase),
        /// () => CurrentUser = new User("None"));
        /// </code>
        /// </example>
        private void RemoveItemFromList<T>(List<T> list,Func<T,bool> predicate, Action OnRemoved = null)
        {
            int elementID = list.FindIndex(x => predicate(x));
            if (elementID == -1)
                return;
            list.RemoveAt(elementID);
            OnRemoved?.Invoke();
        }
        /// <summary>
        /// Removes the current user and resets selection if successful.
        /// </summary>
        private void RemoveUser()
        {
            if (_userService.RemoveUser(CurrentUser))
                CurrentUser = new User("None");
        }

        /// <summary>
        /// Removes the current tour and resets selection if successful.
        /// </summary>
        private void RemoveTour()
        {
            if (_tourService.RemoveTour(CurrentTour))
                CurrentTour = new Tour("None");
        }
        /// <summary>
        /// Adds funds to the current user's balance via user input.
        /// </summary>
        private void AddMoney()
        {
            Console.WriteLine("Enter amount of money received");
            int value = StringToInt(Console.ReadLine());
            if (value > 0)
            {
                _userService.AddMoney(value, CurrentUser);
            }
        }
        /// <summary>
        /// Updates the price of the current tour based on user input.
        /// </summary>
        private void ChangeTourPrice()
        {
            Console.WriteLine("Set tour price");
            int value = StringToInt(Console.ReadLine());
            if (value > 0)
            {
                _tourService.ChangePrice(value,CurrentTour);
            }
        }

        /// <summary>
        /// Updates the space (seats) of the current tour based on user input.
        /// </summary>
        private void ChangeTourSpace()
        {
            Console.WriteLine("Set tour space");
            int value = StringToInt(Console.ReadLine());
            if (value > 0)
            {
                _tourService.ChangeSpace(value,CurrentTour);
            }
        }

        /// <summary>
        /// Adds a new tour from console input and saves to storage.
        /// </summary>
        private void AddTour()
        {
            Console.WriteLine("Enter tour name");
            string tName = Console.ReadLine();
            Console.WriteLine("Enter tour price");
            string tPrice = Console.ReadLine();
            Console.WriteLine("Enter tour space amount");
            string tSize = Console.ReadLine();
            _tourService.AddTour(new Tour(tName.Trim(), StringToInt(tPrice), StringToInt(tSize)));

        }

        /// <summary>
        /// Signs the current user up for the selected tour.
        /// </summary>
        private void BuyTourForUser()
        {
            if(_userService.SignUser(CurrentUser, CurrentTour))
            {
                _tourService.SignUser(CurrentTour, CurrentUser);
            }
        }
        /// <summary>
        /// Top-level menu that allows switching between user/tour management or exiting.
        /// </summary>
        /// <param name="input">User choice as a string.</param>
        /// <returns>-1 if exit selected; otherwise 0.</returns>
        private int SecondMenu(string input)
        {
            int intInput = StringToInt(input);
            switch (intInput)
            {
                case 1:
                    UserOptions();
                    break;
                case 2:
                    TourOptions();
                    break;
                case 3:
                    return -1;
                default: break;
            }
            return 0;
        }

        /// <summary>
        /// Displays and processes the user management menu.
        /// </summary>
        private void UserOptions()
        {
            do
            {
                Console.WriteLine("----------");
                Console.WriteLine(">Current User<");
                Console.WriteLine(CurrentUser.Username + " Price: " + CurrentUser.Money + " Discount: " +
                CurrentUser.Discount + " Current tour: " + CurrentUser.CurrentTour);
                Console.WriteLine(">Current Tour<");
                Console.WriteLine(CurrentTour.Name + " Price: " + CurrentTour.Price + " Free space: " + CurrentTour.TourSpace);
                Console.WriteLine("1.Show users");
                Console.WriteLine("2.Select user");
                Console.WriteLine("3.Add user");
                Console.WriteLine("4.Delete user");
                Console.WriteLine("5.Add money to user");
                Console.WriteLine("6.Buy tour for user");
                Console.WriteLine("7.Exit");
            }
            while (UserMenu(Console.ReadLine()) != -1);

        }
        /// <summary>
        /// Displays and processes the tour management menu.
        /// </summary>
        private void TourOptions()
        {
            do
            {
                Console.WriteLine("----------");
                Console.WriteLine(">Current Tour<");
                Console.WriteLine(CurrentTour.Name + " Price: " + CurrentTour.Price + " Space: " + CurrentTour.TourSpace);
                Console.WriteLine("1.Show tours");
                Console.WriteLine("2.Select tour");
                Console.WriteLine("3.Add tour");
                Console.WriteLine("4.Delete tour");
                Console.WriteLine("5.Change tour price");
                Console.WriteLine("6.Change amount of space");
                Console.WriteLine("7.Exit");
            }
            while (TourMenu(Console.ReadLine()) != -1);
        }
        /// <summary>
        /// Processes a single tour menu command.
        /// </summary>
        /// <param name="input">User input string.</param>
        /// <returns>-1 to exit; otherwise 0.</returns>
        private int TourMenu(string input)
        {
            int intInput = StringToInt(input);
            switch (intInput)
            {
                case 1:
                    _tourService.ShowAllTours();
                    break;
                case 2:
                    SelectCurrentTour();
                    break;
                case 3:
                    AddTour();
                    break;
                case 4:
                    RemoveTour();
                    break;
                case 5:
                    ChangeTourPrice();
                    break;
                case 6:
                    ChangeTourSpace();
                    break;
                case 7:
                    return -1;
                default:
                    break;
            }
            return 0;
        }
        /// <summary>
        /// Processes a single user menu command.
        /// </summary>
        /// <param name="input">User input string.</param>
        /// <returns>-1 to exit; otherwise 0.</returns>
        private int UserMenu(string input)
        {
            int intInput = StringToInt(input);
            switch (intInput)
            {
                case 1:
                    _userService.ShowAllUsers();
                    break;
                case 2:
                    SelectCurrentUser();
                    break;
                case 3:
                    AddUser();
                    break;
                case 4:
                    RemoveUser();
                    break;
                case 5:
                    AddMoney();
                    break;
                case 6:
                    BuyTourForUser();
                    break;
                case 7:
                    return -1;
                default: break;
            }
            return 0;
        }
        /// <summary>
        /// Starts the main application menu after admin authentication.
        /// </summary>
        public void MainMenu()
        {
            if (Initialization())   
            {
                do
                {
                    Console.WriteLine("----------");
                    Console.WriteLine("1.User menu");
                    Console.WriteLine("2.Tour menu");
                    Console.WriteLine("3.Exit");
                }
                while (SecondMenu(Console.ReadLine()) != -1);
            }
        }

    }
}

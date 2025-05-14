namespace SmallDB
{
    internal class Menu
    {
        private string UserName;
        private string Password;
        private User CurrentUser = new User("None");
        private Tour CurrentTour = new Tour("None");
        private List<User> UserList;
        private List<Tour> TourList;
        private List<Admin> AdminList;
        private bool Initialization()
        {
            Console.WriteLine("Enter Username:");
            UserName = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            Password = Console.ReadLine();
            AdminList = JSONFileHelper.LoadAdmins();
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
        private void SelectCurrentType<T>(List<T> list)
        {
            string value = Console.ReadLine();
            switch(list)
            {
                case List<User>:
                    foreach (var users in UserList)
                    {
                        if (users.Username == value)
                            CurrentUser = users;
                    }
                    break;
                case List<Tour>:
                    foreach (var tours in TourList)
                    {
                        if (tours.Name == value)
                            CurrentTour = tours;
                    }
                    break;
            }

        }
        private void AddUser()
        {
            Console.WriteLine("Enter Username and Password for new user");
            string username = Console.ReadLine();
            string password = Console.ReadLine();
            User user = new User(username.Trim(), password.Trim());
            JSONFileHelper.AddUser(user);
            UserList.Add(user);
        }
        private static void ShowList<T>(IEnumerable<T> list, Func<T, string> getName)
        {
            int i = 1;
            foreach (var item in list)
            Console.WriteLine($"{i++}. {getName(item)}");
        }
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
        private void RemoveItemFromList<T>(List<T> list)
        {
            switch (list)
            {
                case List<User> users:
                    users.RemoveAll(u =>
                        string.Equals(u.Username, CurrentUser.Username, StringComparison.OrdinalIgnoreCase));
                    break;

                case List<Tour> tours:
                    tours.RemoveAll(t =>
                        string.Equals(t.Name, CurrentTour.Name, StringComparison.OrdinalIgnoreCase));
                    break;

                default:
                    throw new ArgumentException(
                        "RemoveFromLocalList only supports List<User> or List<Tour>.", nameof(list));
            }
        }

        private void RemoveUser()
        {
            JSONFileHelper.DeleteUser(CurrentUser.Username);
            RemoveItemFromList(UserList);
            CurrentUser = new User("None");
        }
        private void RemoveTour()
        {
            JSONFileHelper.DeleteTour(CurrentTour.Name);
            RemoveItemFromList(TourList);
            CurrentTour = new Tour("None");
        }
        private void AddMoney()
        {
            Console.WriteLine("Enter amount of money received");
            int value = StringToInt(Console.ReadLine());
            if (value > 0)
                JSONFileHelper.AddMoneyToUser(CurrentUser.Username, value);
        }
        private void ChangeTourPrice()
        {
            Console.WriteLine("Set tour price");
            int value = StringToInt(Console.ReadLine());
            if (value > 0)
                JSONFileHelper.EditTourPrice(CurrentTour.Name, value);
        }
        private void ChangeTourSpace()
        {
            Console.WriteLine("Set tour space");
            int value = StringToInt(Console.ReadLine());
            if (value > 0)
                JSONFileHelper.EditTourSpace(CurrentTour.Name, value);
        }
        private void AddTour()
        {
            Console.WriteLine("Enter tour name");
            string tName = Console.ReadLine();
            Console.WriteLine("Enter tour price");
            string tPrice = Console.ReadLine();
            Console.WriteLine("Enter tour space amount");
            string tSize = Console.ReadLine();
            Tour tour = new Tour(tName.Trim(), StringToInt(tPrice), StringToInt(tSize));
            JSONFileHelper.AddTour(tour);
            TourList.Add(tour);

        }
        private void BuyTourForUser()
        {
            if(CurrentTour.SignUser(CurrentUser))
            JSONFileHelper.SaveUserTour(CurrentUser,CurrentTour.Name);
        }
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
        private int TourMenu(string input)
        {
            int intInput = StringToInt(input);
            switch (intInput)
            {
                case 1:
                    ShowList(TourList, Tour => Tour.Name);
                    break;
                case 2:
                    SelectCurrentType(TourList);
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
        private int UserMenu(string input)
        {
            int intInput = StringToInt(input);
            switch (intInput)
            {
                case 1:
                    ShowList(UserList, User => User.Username);
                    break;
                case 2:
                    SelectCurrentType(UserList);
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
        public void MainMenu()
        {
            if (Initialization())
            {
                UserList = JSONFileHelper.LoadUsers();
                TourList = JSONFileHelper.LoadTours();
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

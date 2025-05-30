
using SmallDB.Data;
using SmallDB.Services;

namespace SmallDB.BusinessLogic
{
    /// <summary>
    /// Encapsulates all business operations on <see cref="Tour"/> entities,
    /// backed by an <see cref="IStorageService{Tour}"/>.
    /// </summary>
    public class TourService
    {
        private List<Tour> _tours = new List<Tour>();
        private readonly IStorageService<Tour> _tourStorage;
        /// <summary>
        /// Initializes a new TourService that loads/saves tours via <paramref name="tourStorage"/>.
        /// </summary>
        /// <param name="tourStorage">Storage implementation for tours. Cannot be null.</param>
        public TourService(IStorageService<Tour> tourStorage)
        {
            _tourStorage = tourStorage;
            _tours = _tourStorage.Load();
        }
        /// <summary>
        /// Prints all tours to the console.
        /// </summary>
        public void ShowAllTours()
        {
            Console.WriteLine("List of tours: ");
            Generics.ShowList(_tours, tour => tour.Name);
        }
        /// <summary>
        /// Updates the price of <paramref name="currentTour"/> and saves.
        /// </summary>
        public void ChangePrice(int value, Tour currentTour)
        {
            foreach (var tour in _tours)
            {
                if (tour == currentTour)
                {
                    tour.Price += value;
                    break;
                }
            }
            _tourStorage.Save(_tours);
        }
        /// <summary>
        /// Adds a new tour and persists it.
        /// </summary>
        public void AddTour(Tour tour)
        {
            if (tour != null)
            {
                _tours.Add(tour);
                _tourStorage.Save(_tours);
            }
        }
        /// <summary>
        /// Adds the <paramref name="currentUser"/> to the tour if space allows, then saves.
        /// </summary>
        public void SignUser(Tour currentTour, User currentUser)
        {
            foreach (var tour in _tours)
            {
                if (tour == currentTour && tour.SignUser(currentUser))
                {
                    //tour.SignUser(currentUser);
                    _tourStorage.Save(_tours);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes the specified tour if found, and persists the change.
        /// </summary>
        public bool RemoveTour(Tour tour)
        {
            if (Generics.RemoveItemFromList
                (_tours,
                t => string.Equals(t.Name, tour.Name, StringComparison.OrdinalIgnoreCase)
                ))
            {
                _tourStorage.Save(_tours);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Updates the available space of <paramref name="currentTour"/> and saves.
        /// </summary>
        public void ChangeSpace(int value, Tour currentTour)
        {
            foreach (var tour in _tours)
            {
                if (tour == currentTour)
                {
                    tour.TourSpace = value;
                    break;
                }
            }
            _tourStorage.Save(_tours);
        }
        /// <summary>
        /// Finds a tour by name.
        /// </summary>
        /// <param name="tourToFind">Name of the tour to search for.</param>
        /// <returns>The matching <see cref="Tour"/>, or null if none.</returns>
        public Tour? ReturnTourByName(string tourToFind)
        {
            var CurrentTour = Generics.SelectItemByName(_tours, t => t.Name, tourToFind);
            if (CurrentTour != null)
            {
                Console.WriteLine($"Current tour set to {CurrentTour.Name}");
                return CurrentTour;
            }
            else
            {
                Console.WriteLine("No such tour found.");
                return null;
            }
        }
    }
}

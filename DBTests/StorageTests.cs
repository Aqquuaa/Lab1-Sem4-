using SmallDB.BusinessLogic;
using SmallDB.Data;

namespace DBTests
{
    /// <summary>
    /// In‐memory stub for IStorageService<T> so we can test without touching files.
    /// </summary>
    class InMemoryStorage<T> : IStorageService<T>
    {
        public List<T> Data = new List<T>();

        public List<T> Load() => new List<T>(Data);

        public void Save(List<T> items)
        {
            // replace contents
            Data.Clear();
            Data.AddRange(items);
        }
    }

    [TestClass]
    public class UserServiceTests
    {
        private InMemoryStorage<User> _storage;
        private UserService _service;

        [TestInitialize]
        public void Init()
        {
            _storage = new InMemoryStorage<User>();
            // seed with one user
            _storage.Data.Add(new User("alice", "pw") { Money = 100, Discount = 10, Spent = 0, CurrentTour = "" });
            _service = new UserService(_storage);
        }

        [TestMethod]
        public void AddUser_ShouldAppendToStorage()
        {
            var bob = new User("bob", "pw2");
            _service.AddUser(bob);

            // underlying storage must contain alice + bob
            CollectionAssert.AreEquivalent(
                new[] { "alice", "bob" },
                _storage.Data.ConvertAll(u => u.Username)
            );
        }

        [TestMethod]
        public void RemoveUser_ShouldReturnTrue_WhenUserExists()
        {
            var alice = _service.ReturnUserByName("alice");
            bool removed = _service.RemoveUser(alice);

            Assert.IsTrue(removed);
            Assert.AreEqual(0, _storage.Data.Count);
        }
    }

    [TestClass]
    public class TourServiceTests
    {
        private InMemoryStorage<Tour> _storage;
        private TourService _service;

        [TestInitialize]
        public void Init()
        {
            _storage = new InMemoryStorage<Tour>();
            // seed one tour
            _storage.Data.Add(new Tour("Beach", price: 200, size: 3));
            _service = new TourService(_storage);
        }

        [TestMethod]
        public void AddTour_ShouldAppendTour()
        {
            var t2 = new Tour("Mountain", 150, 2);
            _service.AddTour(t2);

            CollectionAssert.AreEquivalent(
                new[] { "Beach", "Mountain" },
                _storage.Data.ConvertAll(t => t.Name)
            );
        }

        [TestMethod]
        public void RemoveTour_ShouldReturnTrue_WhenTourExists()
        {
            var beach = _service.ReturnTourByName("Beach");
            bool removed = _service.RemoveTour(beach);

            Assert.IsTrue(removed);
            Assert.AreEqual(0, _storage.Data.Count);
        }

        [TestMethod]
        public void ChangePrice_ShouldUpdateTourPrice()
        {
            var beach = _service.ReturnTourByName("Beach");
            _service.ChangePrice(50, beach);

            Assert.AreEqual(250, _storage.Data[0].Price);
        }

        [TestMethod]
        public void SignUser_ShouldAddUsername_AndDecreaseSpace()
        {
            // arrange a user with enough money
            var user = new User("chloe", "pw") { Money = 500 };
            var beach = _service.ReturnTourByName("Beach");

            _service.SignUser(beach, user);

            Assert.AreEqual(2, beach.TourSpace);
            CollectionAssert.Contains(beach.SignedUsers, "chloe");
        }
    }
}

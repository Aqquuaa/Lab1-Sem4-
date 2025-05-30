using SmallDB.Data;

namespace SmallDB.BusinessLogic
{
    /// <summary>
/// Common generic utilities for list operations and item selection.
/// </summary>
    public static class Generics
    {
        /// <summary>
        /// Removes the first element matching <paramref name="predicate"/> from <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="list">List to remove from. Must not be null.</param>
        /// <param name="predicate">Condition to match. Must not be null.</param>
        /// <returns>True if an element was removed; false otherwise.</returns>
        public static bool RemoveItemFromList<T>(List<T> list, Func<T, bool> predicate)
        {
            int elementID = list.FindIndex(x => predicate(x));
            if (elementID == -1)
                return false;
            list.RemoveAt(elementID);
            return true;
        }
        /// <summary>
        /// Writes each element’s name (via <paramref name="getName"/>) to the console, prefixed by an index.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="list">Sequence of elements. Must not be null.</param>
        /// <param name="getName">Function mapping an element to its display string.</param>
        public static void ShowList<T>(IEnumerable<T> list, Func<T, string> getName)
        {
            int i = 1;
            foreach (var item in list)
                Console.WriteLine($"{i++}. {getName(item)}");
        }
        /// <summary>
        /// Returns the first element in <paramref name="list"/> whose <paramref name="nameSelector"/>
        /// matches <paramref name="nameToFind"/> exactly (case-insensitive).
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="list">List to search. Must not be null.</param>
        /// <param name="nameSelector">Function extracting the comparison key.</param>
        /// <param name="nameToFind">Name to match; if null or empty, returns default.</param>
        /// <returns>The matching element, or <c>default(T)</c> if none.</returns>
        public static T SelectItemByName<T>(List<T> list, Func<T, string> nameSelector, string nameToFind)
        {
            nameToFind = nameToFind?.Trim() ?? "";
            foreach (var item in list)
            {
                if(nameSelector(item) == nameToFind)
                    return item;
            }
            return default;
        }
    }
}

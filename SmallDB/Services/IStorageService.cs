/// <summary>
/// Defines generic persistence operations for a collection of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object to persist.</typeparam>
public interface IStorageService<T>
{
    /// <summary>
    /// Loads all items of type <typeparamref name="T"/> from the backing store.
    /// </summary>
    /// <returns>A list of <typeparamref name="T"/>. Never null.</returns>
    List<T> Load();

    /// <summary>
    /// Saves the given list of items to the backing store, overwriting any existing data.
    /// </summary>
    /// <param name="items">The list of <typeparamref name="T"/> to save. Must not be null.</param>
    void Save(List<T> items);
}

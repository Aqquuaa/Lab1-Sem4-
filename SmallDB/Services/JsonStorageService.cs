using System.Text.Json;

namespace SmallDB.Services
{
    /// <summary>
    /// A JSON‐file implementation of <see cref="IStorageService{T}"/>.
    /// Reads and writes <typeparamref name="T"/> collections from/to a specified file path.
    /// </summary>
    /// <typeparam name="T">The element type to serialize/deserialize.</typeparam>
    public class JsonStorageService<T> : IStorageService<T>
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;
        /// <summary>
        /// Initializes a new instance targeting the given JSON file.
        /// </summary>
        /// <param name="filePath">Path to the JSON file. Cannot be null.</param>
        public JsonStorageService(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        /// <inheritdoc/>
        public List<T> Load()
        {
            if (!File.Exists(_filePath))
                return new List<T>();
            using var stream = File.OpenRead(_filePath);
            var list = JsonSerializer.Deserialize<List<T>>(stream, _options);
            return list ?? new List<T>();
        }
        /// <inheritdoc/>
        public void Save(List<T> items)
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using var stream = File.Create(_filePath);
            JsonSerializer.SerializeAsync(stream, items, _options);
        }
    }
}

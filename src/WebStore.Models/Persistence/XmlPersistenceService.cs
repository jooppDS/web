using System.Xml.Serialization;

namespace WebStore.Models.Persistence
{
    public static class XmlPersistenceService
    {
        private static readonly string DefaultDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        public static void SaveToXml<T>(IEnumerable<T> items, string fileName, string? directory = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            directory ??= DefaultDirectory;

            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var filePath = Path.Combine(directory, $"{fileName}.xml");

            var serializer = new XmlSerializer(typeof(List<T>));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, items.ToList());
            }
        }

        public static List<T> LoadFromXml<T>(string fileName, string? directory = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            directory ??= DefaultDirectory;
            var filePath = Path.Combine(directory, $"{fileName}.xml");

            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            var serializer = new XmlSerializer(typeof(List<T>));
            using (var reader = new StreamReader(filePath))
            {
                var result = serializer.Deserialize(reader);
                return result != null ? (List<T>)result : new List<T>();
            }
        }


        public static bool FileExists(string fileName, string? directory = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            directory ??= DefaultDirectory;
            var filePath = Path.Combine(directory, $"{fileName}.xml");
            return File.Exists(filePath);
        }
    }
}


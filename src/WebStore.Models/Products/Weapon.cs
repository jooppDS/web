using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Weapon : Product
    {
        private static List<Weapon> _extent = new List<Weapon>();

        private string _caliber = string.Empty;
        private int _numberOfRounds;
        private decimal _range;

        [Required(ErrorMessage = "Caliber is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Caliber must be between 1 and 50 characters")]
        public string Caliber
        {
            get => _caliber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Caliber cannot be null or empty", nameof(Caliber));
                if (value.Length > 50)
                    throw new ArgumentException("Caliber cannot exceed 50 characters", nameof(Caliber));
                _caliber = value;
            }
        }

        [Required(ErrorMessage = "Number of rounds is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of rounds must be positive")]
        public int NumberOfRounds
        {
            get => _numberOfRounds;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(NumberOfRounds), 
                        "Number of rounds must be positive");
                _numberOfRounds = value;
            }
        }

        [Required(ErrorMessage = "Range is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Range must be non-negative")]
        public decimal Range
        {
            get => _range;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Range), 
                        "Range cannot be negative");
                _range = value;
            }
        }

        public static List<Weapon> GetAll()
        {
            return new List<Weapon>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Weapons", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Weapons", directory))
                return;

            var loadedWeapons = XmlPersistenceService.LoadFromXml<Weapon>("Weapons", directory);

            _extent.Clear();
            foreach (var weapon in loadedWeapons)
            {
                _extent.Add(weapon);
            }
        }

        public Weapon()
        {
        }

        public Weapon(string name, string description, decimal price, bool isAdultProduct, decimal weight, int stockQuantity, 
            string caliber, int numberOfRounds, decimal range, decimal storeFeePercentage = 5)
            : base(name, description, price, isAdultProduct, weight, stockQuantity, storeFeePercentage)
        {
            Caliber = caliber;
            NumberOfRounds = numberOfRounds;
            Range = range;
            _extent.Add(this);
        }
    }
}

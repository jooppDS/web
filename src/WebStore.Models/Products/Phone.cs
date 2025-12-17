using System.ComponentModel.DataAnnotations;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Phone : Product
    {
        private static List<Phone> _extent = new List<Phone>();

        private bool _isWaterproof;
        private int _storageCapacity;
        private int _batteryCapacity;
        private string _cpu = string.Empty;

        public bool IsWaterproof
        {
            get => _isWaterproof;
            set => _isWaterproof = value;
        }

        [Required(ErrorMessage = "Storage capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Storage capacity must be positive")]
        public int StorageCapacity
        {
            get => _storageCapacity;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(StorageCapacity), 
                        "Storage capacity must be positive");
                _storageCapacity = value;
            }
        }

        [Required(ErrorMessage = "Battery capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Battery capacity must be positive")]
        public int BatteryCapacity
        {
            get => _batteryCapacity;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(BatteryCapacity), 
                        "Battery capacity must be positive");
                _batteryCapacity = value;
            }
        }

        [Required(ErrorMessage = "CPU is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "CPU must be between 2 and 100 characters")]
        public string CPU
        {
            get => _cpu;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CPU cannot be null or empty", nameof(CPU));
                if (value.Length < 2 || value.Length > 100)
                    throw new ArgumentException("CPU must be between 2 and 100 characters", nameof(CPU));
                _cpu = value;
            }
        }

        public static List<Phone> GetAll()
        {
            return new List<Phone>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Phones", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Phones", directory))
                return;

            var loadedPhones = XmlPersistenceService.LoadFromXml<Phone>("Phones", directory);

            _extent.Clear();
            foreach (var phone in loadedPhones)
            {
                _extent.Add(phone);
            }
        }

        public Phone()
        {
        }

        public Phone(string name, string description, decimal price, bool isAdultProduct, decimal weight, int stockQuantity, 
            TimeSpan warrantyPeriod, bool isWaterproof, int storageCapacity, int batteryCapacity, string cpu, Seller seller)
            : base(name, description, price, isAdultProduct, weight, stockQuantity, seller, warrantyPeriod)
        {
            IsWaterproof = isWaterproof;
            StorageCapacity = storageCapacity;
            BatteryCapacity = batteryCapacity;
            CPU = cpu;
            _extent.Add(this);
        }
        
        public Phone(string name, string description, decimal price, bool isAdultProduct, decimal weight, int stockQuantity, 
            ProductCondition productCondition, string defectsDescription, bool isWaterproof, int storageCapacity, int batteryCapacity, string cpu, Seller seller)
            : base(name, description, price, isAdultProduct, weight, stockQuantity, seller, productCondition, defectsDescription)
        {
            IsWaterproof = isWaterproof;
            StorageCapacity = storageCapacity;
            BatteryCapacity = batteryCapacity;
            CPU = cpu;
            _extent.Add(this);
        }
    }
}

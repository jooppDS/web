using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Discount
    {
        private static List<Discount> _extent = new List<Discount>();

        private decimal _totalPercentage;
        private string _description = string.Empty;
        private DateTime _startDate;
        private DateTime _endDate;

        [Required(ErrorMessage = "Total percentage is required")]
        [Range(0, 100, ErrorMessage = "Total percentage must be between 0 and 100")]
        public decimal TotalPercentage
        {
            get => _totalPercentage;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(TotalPercentage), 
                        "Total percentage must be between 0 and 100");
                _totalPercentage = value;
            }
        }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 500 characters")]
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be null or empty", nameof(Description));
                if (value.Length < 5 || value.Length > 500)
                    throw new ArgumentException("Description must be between 5 and 500 characters", nameof(Description));
                _description = value;
            }
        }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value < _startDate)
                    throw new ArgumentOutOfRangeException(nameof(EndDate), 
                        "End date cannot be earlier than start date");
                _endDate = value;
            }
        }

        public bool IsActive
        {
            get
            {
                var now = DateTime.Now;
                return now >= StartDate && now <= EndDate;
            }
        }

        public static List<Discount> GetAll()
        {
            return new List<Discount>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Discounts", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Discounts", directory))
                return;

            var loadedDiscounts = XmlPersistenceService.LoadFromXml<Discount>("Discounts", directory);

            _extent.Clear();
            foreach (var discount in loadedDiscounts)
            {
                _extent.Add(discount);
            }
        }

        public Discount()
        {
        }

        public Discount(decimal totalPercentage, string description, DateTime startDate, DateTime endDate)
        {
            TotalPercentage = totalPercentage;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            _extent.Add(this);
        }
    }
}

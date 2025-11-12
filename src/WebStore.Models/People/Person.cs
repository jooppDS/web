using System.ComponentModel.DataAnnotations;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public abstract class Person
    {
        private static List<Person> _extent = new List<Person>();

        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _phoneNumber = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name cannot be null or empty", nameof(FirstName));
                if (value.Length < 2 || value.Length > 50)
                    throw new ArgumentException("First name must be between 2 and 50 characters", nameof(FirstName));
                _firstName = value;
            }
        }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name cannot be null or empty", nameof(LastName));
                if (value.Length < 2 || value.Length > 50)
                    throw new ArgumentException("Last name must be between 2 and 50 characters", nameof(LastName));
                _lastName = value;
            }
        }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number must be in valid format")]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Phone number cannot be null or empty", nameof(PhoneNumber));
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\+?[1-9]\d{1,14}$"))
                    throw new ArgumentException("Phone number must be in valid format", nameof(PhoneNumber));
                _phoneNumber = value;
            }
        }

        [Range(1, 150, ErrorMessage = "Legal adult age must be between 1 and 150")]
        public static int LegalAdultAge { get; set; } = 18;

        public static List<Person> GetAll()
        {
            return new List<Person>(_extent);
        }

     
        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Persons", directory);
        }

    
        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Persons", directory))
                return;

            var loadedPersons = XmlPersistenceService.LoadFromXml<Person>("Persons", directory);
            
            
            _extent.Clear();
            foreach (var person in loadedPersons)
            {
                _extent.Add(person);
            }
        }

     
        protected Person()
        {
          
        }

        protected Person(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            _extent.Add(this);
        }
    }
}


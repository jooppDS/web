using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;
using WebStore.Models.ValueObjects;

namespace WebStore.Models
{

    public class Person
    {
        private static List<Person> _extent = new List<Person>();
        
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _phoneNumber = string.Empty;
        private static int _legalAdultAge = 18;
        
        public PersonRole PersonRole { get; set; }
        

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
        public static int LegalAdultAge
        {
            get => _legalAdultAge;
            set
            {
                if (value < 1 || value > 150)
                    throw new ArgumentOutOfRangeException(nameof(LegalAdultAge),
                        "Legal adult age must be between 1 and 150");
                _legalAdultAge = value;
            }
        }
        
        
        // customer
        private DateTime? _dateOfBirth;
        public DateTime? DateOfBirth
        {
            get
            {
                if (PersonRole != PersonRole.EmployeeCustomer && PersonRole != PersonRole.Customer)
                    throw new InvalidOperationException("Cannot get date of birth for not customer or employee customer");
                return _dateOfBirth;
            }
            set
            {
                if (PersonRole != PersonRole.EmployeeCustomer && PersonRole != PersonRole.Customer)
                    throw new InvalidOperationException("Cannot set date of birth for not customer or employee customer");
                if (value is null)
                    throw new ArgumentNullException(nameof(DateOfBirth));
                if (value > DateTime.Today)
                    throw new ArgumentOutOfRangeException(nameof(DateOfBirth), 
                        "Date of birth cannot be in the future");
                if (value < DateTime.Today.AddYears(-150))
                    throw new ArgumentOutOfRangeException(nameof(DateOfBirth), 
                        "Date of birth cannot be more than 150 years ago");
                _dateOfBirth = value;
            }
        }

        private List<Address>? _shippingAddress;
        public List<Address>? ShippingAddress
        {
            get
            {
                if (PersonRole != PersonRole.EmployeeCustomer && PersonRole != PersonRole.Customer)
                    throw new InvalidOperationException("Cannot get shipping address for not customer or employee customer");
                return _shippingAddress;
            }
            set
            {
                if (PersonRole != PersonRole.EmployeeCustomer && PersonRole != PersonRole.Customer)
                    throw new InvalidOperationException("Cannot set shipping address for not customer or employee customer");
                if (value is null)
                    throw new ArgumentNullException(nameof(ShippingAddress));
                _shippingAddress = value;
            }
        }
        public int? Age
        {
            get
            {
                if (PersonRole != PersonRole.EmployeeCustomer && PersonRole != PersonRole.Customer)
                    throw new InvalidOperationException("Cannot get age for not customer or employee customer");
                
                if (DateOfBirth is null) return null;
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth?.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
        
        // employee
        private EmployeeRole? _employeeRole;
        private decimal? _salary;
        
        public EmployeeRole? EmployeeRole
        {
            get
            {
                if (PersonRole != PersonRole.Employee && PersonRole != PersonRole.EmployeeCustomer) 
                    throw new InvalidOperationException("Cannot get employee role for not employee or employee customer");
                return _employeeRole;
            }
            set
            {
                if (PersonRole != PersonRole.Employee && PersonRole != PersonRole.EmployeeCustomer) 
                    throw new InvalidOperationException("Cannot set employee role for not employee or employee customer");
                if (value is null)
                    throw new ArgumentNullException(nameof(EmployeeRole));
                if (!Enum.IsDefined(typeof(EmployeeRole), value))
                    throw new ArgumentOutOfRangeException(nameof(EmployeeRole), 
                        "Role must be a valid EmployeeRole value");
                _employeeRole = value;
            }
        }
        
        public decimal? Salary
        {
            get
            {
                if (PersonRole != PersonRole.Employee && PersonRole != PersonRole.EmployeeCustomer) 
                    throw new InvalidOperationException("Cannot get salary for not employee or employee customer");
                return _salary;
            }
            set
            {
                if (PersonRole != PersonRole.Employee && PersonRole != PersonRole.EmployeeCustomer) 
                    throw new InvalidOperationException("Cannot set salary for not employee or employee customer");
                if (value is null)
                    throw new ArgumentNullException(nameof(Salary));
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Salary), 
                        "Salary cannot be negative");
                _salary = value;
            }
        }

        public void CheckSalary()
        {
            if (PersonRole != PersonRole.Employee && PersonRole != PersonRole.EmployeeCustomer)
                throw new InvalidOperationException("Cannot check salary for not employee or employee customer");
            throw new NotImplementedException(); // nobody needs that method LOL
        }
        
        

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



        //customer
        public Person(string firstName, string lastName, string phoneNumber, DateTime dateOfBirth)
        {
            PersonRole = PersonRole.Customer;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            
            _extent.Add(this);
        }

        //employee
        public Person(string firstName, string lastName, string phoneNumber, EmployeeRole employeeRole, decimal salary)
        {
            PersonRole = PersonRole.Employee;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            EmployeeRole = employeeRole;
            Salary = salary;
            
            _extent.Add(this);
        }

        //employeecustomer
        public Person(string firstName, string lastName, string phoneNumber, DateTime dateOfBirth,
            EmployeeRole employeeRole, decimal salary)
        {
            PersonRole = PersonRole.EmployeeCustomer;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            EmployeeRole = employeeRole;
            Salary = salary;
            
            _extent.Add(this);
        }
    }
}


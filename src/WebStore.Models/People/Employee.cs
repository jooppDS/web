using System.ComponentModel.DataAnnotations;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Employee : Person
    {
        private static List<Employee> _extent = new List<Employee>();

        private EmployeeRole _role;
        private decimal _salary;

        [Required(ErrorMessage = "Role is required")]
        public EmployeeRole Role
        {
            get => _role;
            set
            {
                if (!Enum.IsDefined(typeof(EmployeeRole), value))
                    throw new ArgumentOutOfRangeException(nameof(Role), 
                        "Role must be a valid EmployeeRole value");
                _role = value;
            }
        }

        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be non-negative")]
        public decimal Salary
        {
            get => _salary;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Salary), 
                        "Salary cannot be negative");
                _salary = value;
            }
        }

        public static List<Employee> GetAll()
        {
            return new List<Employee>(_extent);
        }

        public new static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Employees", directory);
        }

        public new static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Employees", directory))
                return;

            var loadedEmployees = XmlPersistenceService.LoadFromXml<Employee>("Employees", directory);

            _extent.Clear();
            foreach (var employee in loadedEmployees)
            {
                _extent.Add(employee);
            }
        }

        public Employee()
        {
        }

        public Employee(string firstName, string lastName, string phoneNumber, EmployeeRole role, decimal salary)
            : base(firstName, lastName, phoneNumber)
        {
            Role = role;
            Salary = salary;
            _extent.Add(this);
        }
    }
}

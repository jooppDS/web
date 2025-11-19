using System.Reflection;
using WebStore.Models;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Tests;

[TestFixture]
public class PersonTests
{
    private class TestPerson : Person
    {
        public TestPerson(string firstName, string lastName, string phoneNumber)
            : base(firstName, lastName, phoneNumber)
        {
        }
    }
    
    private string _testDir = "../../../Data";
    
    [SetUp]
    public void SetUp()
    {
        ClearExtent<Person>();
    }
    
    private static void ClearExtent<T>()
    {
        var type = typeof(T);
        var extentField = type.GetField("_extent", BindingFlags.NonPublic | BindingFlags.Static);
        if (extentField != null)
        {
            var extent = extentField.GetValue(null);
            if (extent is System.Collections.IList list)
            {
                list.Clear();
            }
        }
    }
    
    [Test]
    public void PersonCreatedProperly()
    {
        var initialCount = Person.GetAll().Count;

        var person = new TestPerson("Vasia", "Pupkin", "+123456789");
        Assert.That(Person.GetAll().Count, Is.EqualTo(initialCount + 1));
        Assert.That(person.FirstName, Is.EqualTo("Vasia"));
        Assert.That(person.LastName, Is.EqualTo("Pupkin"));
        Assert.That(person.PhoneNumber, Is.EqualTo("+123456789"));
    }
    
    [Test]
    public void InvalidFirstNameThrowsArgumentException()
    {
        var person = new TestPerson("Vasia", "Pupkin", "+123456789");
        Assert.Throws<ArgumentException>(() => person.FirstName = "");
        Assert.Throws<ArgumentException>(() => person.FirstName = null);
        Assert.Throws<ArgumentException>(() => person.FirstName = "A");
        Assert.Throws<ArgumentException>(() => person.FirstName = new string('a', 51));
    }
    
    [Test]
    public void InvalidLastNameThrowsArgumentException()
    {
        var person = new TestPerson("Vasia", "Pupkin", "+123456789");
        Assert.Throws<ArgumentException>(() => person.LastName = "");
        Assert.Throws<ArgumentException>(() => person.LastName = null);
        Assert.Throws<ArgumentException>(() => person.LastName = "A");
        Assert.Throws<ArgumentException>(() => person.LastName = new string('a', 51));
    }
    
    [Test]
    public void InvalidPhoneNumberThrowsArgumentException()
    {
        var person = new TestPerson("Vasia", "Pupkin", "+123456789");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = null);
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "abc123");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "+012345");
        Assert.Throws<ArgumentException>(() => person.PhoneNumber = "+12345678901234567");
    }

    [Test]
    public void InvalidLegalAdultAgeThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Person.LegalAdultAge = 0);
        Assert.Throws<ArgumentOutOfRangeException>(() => Person.LegalAdultAge = 151);
    }
    
    [Test]
    public void ExtentCreationSavingAndLoadingWorksCorrectlyWithDerivedClasses()
    {
        ClearExtent<Person>();
        ClearExtent<Employee>();
        ClearExtent<Customer>();
        
        Assert.That(Person.GetAll().Count, Is.EqualTo(0));
        Assert.That(Employee.GetAll().Count, Is.EqualTo(0));
        Assert.That(Customer.GetAll().Count, Is.EqualTo(0));
        
        var personInitValue = Person.GetAll().Count;
        var employeeInitValue = Employee.GetAll().Count;
        var customerInitValue = Customer.GetAll().Count;
        
        var employee = new Employee("Vasilii", "Pupkinidze", "+123456789", EmployeeRole.Manager, 123);
        var customer = new Customer("Vasia", "Pupkin", "+12345678", new DateTime(2000, 11, 10));
        
        Assert.That(Person.GetAll().Count, Is.EqualTo(2));
        Assert.That(Employee.GetAll().Count, Is.EqualTo(1));
        Assert.That(Customer.GetAll().Count, Is.EqualTo(1));
        
        Employee.SaveToXml(_testDir);
        Customer.SaveToXml(_testDir);
        Person.SaveToXml(_testDir);
        
        Assert.That(XmlPersistenceService.FileExists("Persons", _testDir), Is.True);
        Assert.That(XmlPersistenceService.FileExists("Employees", _testDir), Is.True);
        Assert.That(XmlPersistenceService.FileExists("Customers", _testDir), Is.True);
        
        ClearExtent<Person>();
        ClearExtent<Employee>();
        ClearExtent<Customer>();
        
        Assert.That(Person.GetAll().Count, Is.EqualTo(0));
        Assert.That(Employee.GetAll().Count, Is.EqualTo(0));
        Assert.That(Customer.GetAll().Count, Is.EqualTo(0));
        
        Employee.LoadFromXml(_testDir);
        Customer.LoadFromXml(_testDir);
        Person.LoadFromXml(_testDir);
        
        Assert.That(Person.GetAll().Count, Is.EqualTo(2));
        Assert.That(Employee.GetAll().Count, Is.EqualTo(1));
        Assert.That(Customer.GetAll().Count, Is.EqualTo(1));
        
        Assert.That(Employee.GetAll()[0].FirstName, Is.EqualTo(employee.FirstName));
        Assert.That(Employee.GetAll()[0].Salary, Is.EqualTo(employee.Salary));
        Assert.That(Customer.GetAll()[0].FirstName, Is.EqualTo(customer.FirstName));
        Assert.That(Customer.GetAll()[0].DateOfBirth, Is.EqualTo(customer.DateOfBirth));
    }
}
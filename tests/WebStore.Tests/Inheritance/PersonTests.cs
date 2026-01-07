using WebStore.Models;
using WebStore.Models.Enums;

namespace WebStore.Tests.Inheritance;


[TestFixture]
public class PersonTests
{
    //employee
    [Test]
    public void Employee_PersonRole_Is_Employee()
    {
        var employee = new Person("Code", "10", "8800553535", EmployeeRole.Manager, 90000);
        
        Assert.That(employee.PersonRole, Is.EqualTo(PersonRole.Employee));
    }
    
    [Test]
    public void Employee_CanAccess_EmployeeRole_And_Salary()
    {
        var employee = new Person("Code", "10", "8800553535", EmployeeRole.Manager, 90000);

        Assert.That(employee.EmployeeRole, Is.EqualTo(EmployeeRole.Manager));
        Assert.That(employee.Salary, Is.EqualTo(90000));
    }
    
    [Test]
    public void Employee_CannotGetOrSet_DateOfBirth()
    {
        var employee = new Person("Code", "10", "8800553535", EmployeeRole.Manager, 90000);

        Assert.Throws<InvalidOperationException>(() =>
        {
            var dob = employee.DateOfBirth;
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            employee.DateOfBirth = new DateTime(2000, 10, 1);
        });
    }
    
    [Test]
    public void Employee_CannotGet_Age()
    {
        var employee = new Person("Code", "10", "8800553535", EmployeeRole.Manager, 90000);

        Assert.Throws<InvalidOperationException>(() =>
        {
            var age = employee.Age;
        });
    }
    
    //customer tests
    [Test]
    public void Customer_PersonRole_Is_Customer()
    {
        var customer = new Person("Fa", "Shneine", "8800553535", new DateTime(2000, 1, 1));
        Assert.That(customer.PersonRole, Is.EqualTo(PersonRole.Customer));
    }
    
    [Test]
    public void Customer_CanGetOrSet_DateOfBirth_And_Age()
    {
        var dob = new DateTime(2000, 1, 1);
        var customer = new Person("Fa", "Shneine", "8800553535", dob);

        Assert.That(customer.DateOfBirth, Is.EqualTo(dob));
        customer.DateOfBirth = dob;
        Assert.That(customer.DateOfBirth, Is.EqualTo(dob));
        Assert.That(customer.Age, Is.GreaterThan(0));
    }
    
    [Test]
    public void Customer_CannotGetOrSet_EmployeeRole()
    {
        var customer = new Person("Fa", "Shneine", "8800553535", new DateTime(2000, 1, 1));

        Assert.Throws<InvalidOperationException>(() =>
        {
            var role = customer.EmployeeRole;
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            customer.EmployeeRole = EmployeeRole.Manager;
        });
    }
    
    [Test]
    public void Customer_CannotGetOrSet_Salary()
    {
        var customer = new Person("Fa", "Shneine", "8800553535", new DateTime(2000, 1, 1));

        Assert.Throws<InvalidOperationException>(() =>
        {
            var salary = customer.Salary;
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            customer.Salary = 123323;
        });
    }
    
    //employeecustomer tests
    [Test]
    public void EmployeeCustomer_PersonRole_Is_EmployeeCustomer()
    {
        var empCust = new Person("Cowboy", "Click", "8800553535", new DateTime(2000, 1, 1), EmployeeRole.Moderator, 90000);
        Assert.That(empCust.PersonRole, Is.EqualTo(PersonRole.EmployeeCustomer));
    }
    
    [Test]
    public void EmployeeCustomer_CanGetOrSet_Both_Customer_And_Employee_Data()
    {
        var dob = new DateTime(2000, 1, 1);
        var empCust = new Person("Cowboy", "Click", "8800553535", new DateTime(2000, 1, 1), EmployeeRole.Moderator, 90000);

        Assert.That(empCust.DateOfBirth, Is.EqualTo(dob));
        empCust.DateOfBirth = dob;
        Assert.That(empCust.DateOfBirth, Is.EqualTo(dob));
        Assert.That(empCust.EmployeeRole, Is.EqualTo(EmployeeRole.Moderator));
        empCust.EmployeeRole = EmployeeRole.Manager;
        Assert.That(empCust.EmployeeRole, Is.EqualTo(EmployeeRole.Manager));
        Assert.That(empCust.Salary, Is.EqualTo(90000));
        empCust.Salary = 123323;
        Assert.That(empCust.Salary, Is.EqualTo(123323));
        Assert.That(empCust.Age, Is.GreaterThan(0));
    }
}
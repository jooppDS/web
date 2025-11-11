using WebStore.Models;

namespace WebStore.Tests;

public class CustomerTests
{
    [Test]
    public void DateOfBirthThrowsIfIsInFuture()
    {
        var futureDate = DateTime.Today.AddDays(1);
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Customer(futureDate));
        Assert.That(ex!.Message, Does.Contain("cannot be in the future"));
    }

    [Test]
    public void DateOfBirthThrowsWhenMoreThan150YearsAgo()
    {
        var oldDate = DateTime.Today.AddYears(-151);
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Customer(oldDate));
        Assert.That(ex!.Message, Does.Contain("more than 150 years ago"));
    }

    [Test]
    public void AgeCalculatedCorrectly()
    {
        var dob = new DateTime(2000, 11, 10);
        var customer = new Customer(dob);
        var expectedAge = DateTime.Today.Year - 2000;
        if (DateTime.Today < new DateTime(DateTime.Today.Year, 11, 10))
            expectedAge--;
        Assert.That(customer.Age, Is.EqualTo(expectedAge));
    }
    
}
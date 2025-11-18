using System.Reflection;
using WebStore.Models;
using WebStore.Models.ValueObjects;

namespace WebStore.Tests;

[TestFixture]
public class ManufacturerTests
{
    [SetUp]
    public void SetUp()
    {
        var type = typeof(Manufacturer);
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
    public void ManufacturerCreatedProperly()
    {
        var initialCount = Manufacturer.GetAll().Count;
        
        var address = new Address("Solidarnosci", "Warsaw", "CA", "12351", "USA");
        var manufacturer = new Manufacturer("Apple", address);
        Assert.That(Manufacturer.GetAll().Count, Is.EqualTo(initialCount + 1));
        Assert.That(manufacturer.Name, Is.EqualTo("Apple"));
        Assert.That(manufacturer.Address, Is.EqualTo(address));
    }

    [Test]
    public void NullAddressThrowsArgumentNullException()
    {
        var validAddress = new Address("Solidarnosci", "Warsaw", "CA", "12351", "USA");
        var manufacturer = new Manufacturer("Apple", validAddress);
        Assert.Throws<ArgumentNullException>(() => manufacturer.Address = null!);
    }

    [Test]
    public void InvalidNameThrowsArgumentException()
    {
        var address = new Address("Solidarnosci", "Warsaw", "CA", "12351", "USA");
        Assert.Throws<ArgumentException>(() => new Manufacturer("", address));
        Assert.Throws<ArgumentException>(() => new Manufacturer("A", address));
        Assert.Throws<ArgumentException>(() => new Manufacturer(new string('a', 101), address));
    }

    [Test]
    public void ParameterlessConstructorDoesNotAddToExtent()
    {
        var initialCount = Manufacturer.GetAll().Count;

        var manufacturer = new Manufacturer();

        Assert.That(Manufacturer.GetAll().Count, Is.EqualTo(initialCount));
    }
}

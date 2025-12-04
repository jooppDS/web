using System.Reflection;
using WebStore.Models;
using WebStore.Models.Enums;

namespace WebStore.Tests;


[TestFixture]
public class BasicAssociationTests
{
    [SetUp]
    public void Setup()
    {
        ClearExtent<Order>();
        ClearExtent<Customer>();
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

    public void CreatingOder_AssociatesWithCustomer()
    {
        var customer = new Customer("Viktor", "Korneplod", "88005553535", new DateTime(1945, 1, 1));
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, customer);
        
        Assert.Contains(order, (System.Collections.ICollection)customer.Orders);
        Assert.That(order.Customer, Is.EqualTo(customer));
        
    }

    [Test]

    public void AddOrder_UpdatesBothSides()
    {
        var c1 = new Customer("Viktor", "Korneplod", "88005553535", new DateTime(1945, 1, 1));
        var c2 = new Customer("Kai", "Angle", "88005553535", new DateTime(1999, 1, 1));
        
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, c1);
        
        c2.AddOrder(order);
        
        Assert.False(c1.Orders.Contains(order));
        Assert.True(c2.Orders.Contains(order));
        Assert.That(order.Customer, Is.EqualTo(c2));
        
    }

    [Test]
    
    public void AddExistingAssociation_Throws()
    {
        var customer = new Customer("Viktor", "Korneplod", "88005553535", new DateTime(1945, 1, 1));
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, customer);

        Assert.Throws<InvalidOperationException>(() => customer.AddOrder(order));
        
    }

    [Test]
    public void RemoveOrderFromCustomer_Throws()
    {
        var customer = new Customer("Viktor", "Korneplod", "88005553535", new DateTime(1945, 1, 1));
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, customer);
        
        Assert.Throws<InvalidOperationException>(() => customer.RemoveOrder(order));
    }

    [Test]
    public void ChangeCustomer_RemovesFromOld_AddsToNewOrder()
    {
        var c1 = new Customer("Viktor", "Korneplod", "88005553535", new DateTime(1945, 1, 1));
        var c2 = new Customer("Kai", "Angle", "88005553535", new DateTime(1999, 1, 1));
        
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, c1);
        
        order.ChangeCustomer(c2);
        
        Assert.True(c2.Orders.Contains(order));
        Assert.That(order.Customer, Is.EqualTo(c2));
    }

    [Test]
    public void OrderDelete_RemovesOrderFromCustomer()
    {
        var customer = new Customer("Viktor", "Korneplod", "88005553535", new DateTime(1945, 1, 1));
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, customer);
        
        order.Delete();
        
        Assert.False(customer.Orders.Contains(order));
    }

}
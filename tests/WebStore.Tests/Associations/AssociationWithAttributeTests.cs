using System.Reflection;
using WebStore.Models;
using WebStore.Models.Enums;

namespace WebStore.Tests;

[TestFixture]
public class AssociationWithAttributeTests
{
    [SetUp]
    public void SetUp()
    {
        ClearExtent<ProductInOrder>();
        ClearExtent<Order>();
        ClearExtent<Product>();
        ClearExtent<Seller>();
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
    public void CreateProductInOrder_NullProductOrOrder_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>((() => new ProductInOrder(null!, new Order(), 1)));
        Assert.Throws<ArgumentNullException>((() => new ProductInOrder(new New(), null!, 1)));
    }

    [Test]
    public void Order_AddProduct_ShouldCreateProductInOrder()
    {
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, new Customer());
        var product = new New("product", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        
        var productInOrder = order.AddProduct(product, 1);
        Assert.That(productInOrder, Is.Not.Null);
        Assert.That(order.ProductsInOrder.Count, Is.EqualTo(1));
        Assert.That(order.ProductsInOrder.Contains(productInOrder), Is.True);
        Assert.That(product.ProductsInOrder.Count, Is.EqualTo(1));
        Assert.That(product.ProductsInOrder.Contains(productInOrder), Is.True);
    }
    
    [Test]
    public void Product_AddProductToOrder_ShouldCreateProductInOrder()
    {
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, new Customer());
        var product = new New("product", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        
        var productInOrder = product.AddProductToOrder(order, 1);
        Assert.That(productInOrder, Is.Not.Null);
        Assert.That(order.ProductsInOrder.Count, Is.EqualTo(1));
        Assert.That(order.ProductsInOrder.Contains(productInOrder), Is.True);
        Assert.That(product.ProductsInOrder.Count, Is.EqualTo(1));
        Assert.That(product.ProductsInOrder.Contains(productInOrder), Is.True);
    }

    [Test]
    public void DeleteProductInOrder_LastProductInOrder_ShouldThrowInvalidOperationException()
    {
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, new Customer());
        var product = new New("product", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        
        var productInOrder = order.AddProduct(product, 1);
        Assert.Throws<InvalidOperationException>((() => productInOrder.Delete()));
    }

    [Test]
    public void DeleteProductInOrder_ShouldRemoveProductInOrderFromOrderAndProduct()
    {
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, new Customer());
        var product1 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        var product2 = new New("product2", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        
        var productInOrder = order.AddProduct(product1, 1);
        var productInOrder2 = order.AddProduct(product2, 1);
        productInOrder.Delete();

        Assert.That(ProductInOrder.GetAll().Contains(productInOrder), Is.False);
        Assert.That(ProductInOrder.GetAll().Count, Is.EqualTo(1));
        Assert.That(order.ProductsInOrder.Count, Is.EqualTo(1));
        Assert.That(product1.ProductsInOrder.Count, Is.EqualTo(0));
    }

    [Test]
    public void DeleteProduct_ShouldRemoveProductInOrderFromOrder()
    {
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, new Customer());
        var product1 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        var product2 = new New("product2", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        
        var productInOrder = order.AddProduct(product1, 1);
        var productInOrder2 = order.AddProduct(product2, 1);
        Assert.That(ProductInOrder.GetAll().Contains(productInOrder), Is.True);
        Assert.That(order.ProductsInOrder.Contains(productInOrder), Is.True);
        
        product1.Delete();
        Assert.That(ProductInOrder.GetAll().Contains(productInOrder), Is.False);
        Assert.That(ProductInOrder.GetAll().Count, Is.EqualTo(1));
        Assert.That(order.ProductsInOrder.Contains(productInOrder), Is.False);
        Assert.That(order.ProductsInOrder.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void DeleteOrder_ShouldRemoveProductInOrderFromProduct()
    {
        var order = new Order(new DateTime(1, 1, 1), OrderStatus.Pending, DeliveryType.Delivery, new Customer());
        var product = new New("product", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
        
        var productInOrder = order.AddProduct(product, 1);
        Assert.That(ProductInOrder.GetAll().Contains(productInOrder), Is.True);
        Assert.That(order.ProductsInOrder.Contains(productInOrder), Is.True);
        
        order.Delete();
        Assert.That(ProductInOrder.GetAll().Contains(productInOrder), Is.False);
        Assert.That(ProductInOrder.GetAll().Count, Is.EqualTo(0));
        Assert.That(product.ProductsInOrder.Contains(productInOrder), Is.False);
        Assert.That(product.ProductsInOrder.Count, Is.EqualTo(0));
    }
}
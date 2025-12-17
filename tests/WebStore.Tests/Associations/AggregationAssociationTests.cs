/*using System.Reflection;
using WebStore.Models;

namespace WebStore.Tests;


[TestFixture]
public class AggregationAssociationTests
{
    [SetUp]
    public void Setup()
    {
        ClearExtent<Discount>();
        ClearExtent<Product>();
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
    public void AddProduct_ShouldAddProductToDiscount_AndDiscountToProduct()
    {
        var product = new New("Koks", "moshnii sochnii", 228, true, 100, 2, TimeSpan.FromDays(1), new Seller());
        var discount = new Discount(10, "ZAEBIS", DateTime.Now, DateTime.Now.AddMonths(1), product);
        Assert.That(discount.Products.Contains(product), Is.True);
        Assert.That(product.Discounts.Contains(discount), Is.True);
    }

    [Test]
    public void Calling_AddProductTwice_DoesNotDuplicate()
    {
        var product = new New("Koks", "moshnii sochnii", 228, true, 100, 2, TimeSpan.FromDays(1), new Seller());
        var discount = new Discount(10, "ZAEBIS", DateTime.Now, DateTime.Now.AddMonths(1), product);
        
        discount.AddProduct(product);
        
        Assert.That(discount.Products.Count, Is.EqualTo(1));
        Assert.That(product.Discounts.Count, Is.EqualTo(1));
    }

    [Test]
    public void RemoveProduct_RemovesDiscountFromProduct()
    {
        var product1 = new New("Koks", "moshnii sochnii", 228, true, 100, 2, TimeSpan.FromDays(1), new Seller());
        var product2 = new New("Anasha", "moshnii sochnii", 1337, true, 100, 3, TimeSpan.FromDays(1), new Seller());
        var discount = new Discount(10, "ZAEBIS", DateTime.Now, DateTime.Now.AddMonths(1), product1);
        discount.AddProduct(product2);
        discount.RemoveProduct(product2);
        Assert.That(discount.Products.Contains(product2), Is.False);
        Assert.That(product2.Discounts.Contains(discount), Is.False);
    }

    [Test]
    public void RemovingProduct_WhenOnlyOneLeft_ThrowsInvalidOperationException()
    {
        var product = new New("Koks", "moshnii sochnii", 228, true, 100, 2, TimeSpan.FromDays(1), new Seller());
        var discount = new Discount(10, "ZAEBIS", DateTime.Now, DateTime.Now.AddMonths(1), product);
        
        Assert.Throws<InvalidOperationException>(() => discount.RemoveProduct(product));
        Assert.That(discount.Products.Count, Is.EqualTo(1));
        Assert.That(product.Discounts.Count, Is.EqualTo(1));
        Assert.That(discount.Products.Contains(product), Is.True);
        Assert.That(product.Discounts.Contains(discount), Is.True);
    }

    [Test]
    public void DeleteDiscount_RemovesDiscountFromAllProducts()
    {
        var product1 = new New("Koks", "moshnii sochnii", 228, true, 100, 2, TimeSpan.FromDays(1), new Seller());
        var product2 = new New("Anasha", "moshnii sochnii", 1337, true, 100, 3, TimeSpan.FromDays(1), new Seller());
        var discount = new Discount(10, "ZAEBIS", DateTime.Now, DateTime.Now.AddMonths(1), product1);
        discount.AddProduct(product2);
        
        discount.Delete();
        
        Assert.That(product1.Discounts.Contains(discount), Is.False);
        Assert.That(product2.Discounts.Contains(discount), Is.False);
        Assert.That(Discount.GetAll().Contains(discount), Is.False);
    }

    [Test]
    public void RemoveProduct_RemovesProductFromDiscount()
    {
        var product1 = new New("Koks", "moshnii sochnii", 228, true, 100, 2, TimeSpan.FromDays(1), new Seller());
        var product2 = new New("Anasha", "moshnii sochnii", 1337, true, 100, 3, TimeSpan.FromDays(1), new Seller());
        var discount = new Discount(10, "ZAEBIS", DateTime.Now, DateTime.Now.AddMonths(1), product1);
        discount.AddProduct(product2);
        
        product1.Delete();
        
        Assert.That(Product.GetAll().Contains(product1), Is.False);
        Assert.That(discount.Products.Contains(product1), Is.False);
        Assert.That(discount.Products.Count, Is.EqualTo(1));
    }
}*/
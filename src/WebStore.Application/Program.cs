using WebStore.Models.ValueObjects;
using System.Reflection;
using WebStore.Models;

var product1 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());
var product2 = new New("product1", "description", 10, false, 10, 10, new TimeSpan(1), new Seller());

var path = "../../../Data";

Console.WriteLine("1. Creating objects.");
Console.WriteLine();

var address1 = new Address("Main Street", "Moscow", "Moscow Oblast", "101000", "Russia");
var address2 = new Address("Lenin Avenue", "Saint Petersburg", "Leningrad Oblast", "190000", "Russia");
var address3 = new Address("Broadway", "New York", "New York", "10001", "USA");

Console.WriteLine($"Created addresses: {Address.GetAll().Count}");
foreach (var addr in Address.GetAll())
{
    Console.WriteLine($"- {addr.Street}, {addr.City}, {addr.Country}");
}

Console.WriteLine();
Console.WriteLine("2. Saving data to XML files");
Console.WriteLine();

Address.SaveToXml(path);

Console.WriteLine("Files saved:");
Console.WriteLine($"- src/WebStore.Application/Data/Addresses.xml - {File.Exists(Path.Combine(path, "Addresses.xml"))}");

Console.WriteLine();
Console.WriteLine("3. Clearing collections in memory");
Console.WriteLine();

ClearExtent<Address>();

Console.WriteLine($"Addresses in memory: {Address.GetAll().Count}");

Console.WriteLine();
Console.WriteLine("4. Loading data from XML files");
Console.WriteLine();

Address.LoadFromXml(path);

Console.WriteLine($"Loaded addresses: {Address.GetAll().Count}");

Console.WriteLine();
Console.WriteLine("5. Verifying loaded data");
Console.WriteLine();

Console.WriteLine("Addresses:");
foreach (var addr in Address.GetAll())
{
    Console.WriteLine($"- {addr.Street}, {addr.City}, {addr.State}, {addr.PostalCode}, {addr.Country}");
}

void ClearExtent<T>()
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
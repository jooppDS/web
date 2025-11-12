using WebStore.Models.ValueObjects;
using System.Reflection;

var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");

Console.WriteLine($"Data directory: {dataDirectory}");
Console.WriteLine();

Console.WriteLine("1. Creating objects...");
Console.WriteLine();

var address1 = new Address("Main Street", "Moscow", "Moscow Oblast", "101000", "Russia");
var address2 = new Address("Lenin Avenue", "Saint Petersburg", "Leningrad Oblast", "190000", "Russia");
var address3 = new Address("Broadway", "New York", "New York", "10001", "USA");

Console.WriteLine($"Created addresses: {Address.GetAll().Count}");
foreach (var addr in Address.GetAll())
{
    Console.WriteLine($"  - {addr.Street}, {addr.City}, {addr.Country}");
}

Console.WriteLine();
Console.WriteLine("2. Saving data to XML files...");
Console.WriteLine();

Address.SaveToXml(dataDirectory);

Console.WriteLine("Files saved:");
Console.WriteLine($"  - {Path.Combine(dataDirectory, "Addresses.xml")} - {File.Exists(Path.Combine(dataDirectory, "Addresses.xml"))}");

Console.WriteLine();
Console.WriteLine("3. Clearing collections in memory...");
Console.WriteLine();

ClearExtent<Address>();

Console.WriteLine($"Addresses in memory: {Address.GetAll().Count}");

Console.WriteLine();
Console.WriteLine("4. Loading data from XML files...");
Console.WriteLine();

Address.LoadFromXml(dataDirectory);

Console.WriteLine($"Loaded addresses: {Address.GetAll().Count}");

Console.WriteLine();
Console.WriteLine("5. Verifying loaded data...");
Console.WriteLine();

Console.WriteLine("Addresses:");
foreach (var addr in Address.GetAll())
{
    Console.WriteLine($"  - {addr.Street}, {addr.City}, {addr.State}, {addr.PostalCode}, {addr.Country}");
}

void ClearExtent<T>() where T : class
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
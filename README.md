# Web Store Project

## Implemented Features

### Classes
- **People**: Person (abstract), Employee, Customer
- **Products**: Product, Phone, Weapon, Clothing, New, Used
- **Business**: Seller, Manufacturer
- **Orders**: Order, Review, Discount

### Enums
- `EmployeeRole` - Manager, Moderator
- `ClothingSize` - XS, S, M, L, XL, XXL
- `Gender` - Male, Female, Unisex
- `ReviewRating` - OneStar through FiveStars
- `OrderStatus` - Pending, Completed, Cancelled, Accepted
- `DeliveryType` - Delivery, SelfPickup
- `ProductCondition` - MinimalWear, FieldTested, WellWorn, BattleScarred

### Value Objects
- `Address` - Street, City, State, PostalCode, Country

### Implemented Fields
- **Person**: FirstName, LastName, PhoneNumber, LegalAdultAge
- **Customer**: DateOfBirth, Age (derived), ShippingAddress
- **Manufacturer**: Name, Address
- **Review**: Rating, Comment

### Class Extent (Class Extent Persistence)
All implemented classes have class extent for storing all created objects:
- `Manufacturer` - `Manufacturer.GetAll()`
- `Person` - `Person.GetAll()`
- `Customer` - `Customer.GetAll()`
- `Review` - `Review.GetAll()`
- `Address` - `Address.GetAll()`

All objects are automatically added to the collection upon creation and accessible through the static method `GetAll()`.
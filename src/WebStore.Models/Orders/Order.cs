using System.ComponentModel.DataAnnotations;
using WebStore.Models.Enums;
using WebStore.Models.Persistence;

namespace WebStore.Models
{
    public class Order
    {
        private static List<Order> _extent = new List<Order>();

        private DateTime _date;
        private OrderStatus _status;
        private DeliveryType _deliveryType;
        private bool _isHidden;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        [Required(ErrorMessage = "Status is required")]
        public OrderStatus Status
        {
            get => _status;
            set
            {
                if (!Enum.IsDefined(typeof(OrderStatus), value))
                    throw new ArgumentOutOfRangeException(nameof(Status), 
                        "Status must be a valid OrderStatus value");
                _status = value;
            }
        }

        [Required(ErrorMessage = "Delivery type is required")]
        public DeliveryType DeliveryType
        {
            get => _deliveryType;
            set
            {
                if (!Enum.IsDefined(typeof(DeliveryType), value))
                    throw new ArgumentOutOfRangeException(nameof(DeliveryType), 
                        "DeliveryType must be a valid DeliveryType value");
                _deliveryType = value;
            }
        }

        public bool IsHidden
        {
            get => _isHidden;
            set => _isHidden = value;
        }

        public static List<Order> GetAll()
        {
            return new List<Order>(_extent);
        }

        public static void SaveToXml(string? directory = null)
        {
            XmlPersistenceService.SaveToXml(_extent, "Orders", directory);
        }

        public static void LoadFromXml(string? directory = null)
        {
            if (!XmlPersistenceService.FileExists("Orders", directory))
                return;

            var loadedOrders = XmlPersistenceService.LoadFromXml<Order>("Orders", directory);

            _extent.Clear();
            foreach (var order in loadedOrders)
            {
                _extent.Add(order);
            }
        }

        public Order()
        {
        }

        public Order(DateTime date, OrderStatus status, DeliveryType deliveryType, bool isHidden = false)
        {
            Date = date;
            Status = status;
            DeliveryType = deliveryType;
            IsHidden = isHidden;
            _extent.Add(this);
        }
    }
}

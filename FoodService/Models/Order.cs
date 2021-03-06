using System;
using System.Collections.Generic;

namespace FoodService.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int UserId { get; set; }
        public int CourierId { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }

        public virtual CourierProfile Courier { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

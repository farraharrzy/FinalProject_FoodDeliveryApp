﻿using System;
using System.Collections.Generic;

namespace FoodService.Models
{
    public partial class CourierProfile
    {
        public CourierProfile()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string CourierName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool? Availability { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}

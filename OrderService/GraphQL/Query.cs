using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.GraphQL
{
    public class Query
    {
        [Authorize (Roles = new[] { "BUYER" })]
        public IQueryable<Order> GetOrdersByToken([Service] FoodDeliveryAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == username).FirstOrDefault();
            if (user != null)
            {
                var orders = context.Orders.Where(o => o.UserId == user.Id).Include(o => o.OrderDetails);
                return orders.AsQueryable();
            }
            return new List<Order>().AsQueryable();
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public IQueryable<Order> GetOrders([Service] FoodDeliveryAppContext context) =>
            context.Orders.Include(o => o.OrderDetails);

    }
}

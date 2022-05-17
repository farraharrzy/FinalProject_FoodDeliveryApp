using HotChocolate.AspNetCore.Authorization;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.GraphQL
{
    public class Mutation
    {
        [Authorize (Roles = new[] { "BUYER" })]
        public async Task<Order> AddOrderAsync(
            OrderInput input,
            [Service] FoodDeliveryAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == username).FirstOrDefault();
            if (user != null)
            {
                var order = new Order
                {
                    Code = input.Code,
                    UserId = input.UserId
                };

                for (int i = 0; i < input.OrderDetails.Count; i++)
                {
                    var orderdetails = new OrderDetail
                    {
                        OrderId = order.Id,
                        FoodId = input.OrderDetails[i].FoodId,
                        Quantity = input.OrderDetails[i].Quantity

                    };
                    
                    order.OrderDetails.Add(orderdetails);
                }
                var ret = context.Orders.Add(order);
                await context.SaveChangesAsync();
                return ret.Entity;
            }
            else
            {
                return new Order();
            }

        }
    }
}

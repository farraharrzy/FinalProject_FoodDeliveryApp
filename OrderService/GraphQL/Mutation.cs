using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "BUYER" })]
        public async Task<OrderData> AddOrderAsync(
        OrderData input,
        ClaimsPrincipal claimsPrincipal,
        [Service] FoodDeliveryAppContext context)
        {
            using var transaction = context.Database.BeginTransaction();
            var userName = claimsPrincipal.Identity.Name;
            try
            {
                var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
                var kurir = context.CourierProfiles.Where(o => o.Id == input.CourierId).FirstOrDefault();
                if (user != null)
                {
                    // EF
                    if(kurir.Availability == true)
                    {
                        var order = new Order
                        {
                            Code = Guid.NewGuid().ToString(), // generate random chars using GUID
                            UserId = user.Id,
                            CourierId = input.CourierId
                        };

                        foreach (var item in input.OrderDetails)
                        {
                            var detail = new OrderDetail
                            {
                                OrderId = order.Id,
                                FoodId = item.FoodId,
                                Quantity = item.Quantity
                            };
                            order.OrderDetails.Add(detail);
                        }
                        context.Orders.Add(order);

                        kurir.Availability = false;
                        context.CourierProfiles.Update(kurir);

                        context.SaveChanges();
                        await transaction.CommitAsync();
                    }

                    //input.Code = order.Code;
                }
                else
                    throw new Exception("user was not found");
            }
            catch (Exception err)
            {
                transaction.Rollback();
            }

            return input;
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<OrderData> UpdateOrderAsync(
            OrderData input,
            [Service] FoodDeliveryAppContext context)
        {
            var order = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();
            if (order != null)
            {
                // EF
                order.Code = Guid.NewGuid().ToString();
                order.UserId = input.UserId;
                order.CourierId = input.CourierId;

                context.Orders.Update(order);
                context.SaveChanges();
            }
            return input;
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<Order> DeleteOrderByIdAsync(
            int id,
            [Service] FoodDeliveryAppContext context)
        {
            var order = context.Orders.Where(o => o.Id == id).Include(o=>o.OrderDetails).FirstOrDefault();
            if (order != null)
            {
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
            }

            return await Task.FromResult(order);
        }

        //Add Tracking By Courier
        [Authorize(Roles = new[] { "COURIER" })]
        public async Task<TrackingOrder> AddTrackingOrderAsync(
            TrackingOrder input,
            [Service] FoodDeliveryAppContext context)
        {
            var order = context.Orders.Where(o => o.Id == input.Id).FirstOrDefault();
            if (order != null)
            {
                // EF
                order.Id = input.Id;
                order.Longitude = input.Longitude;
                order.Latitude = input.Latitude;

                context.Orders.Update(order);
                context.SaveChanges();
            }
            return input;
        }

        //Complete Order By Courier
        public async Task<Order> CompleteOrderAsync(
            int id,
            [Service] FoodDeliveryAppContext context)
        {
            var order = context.Orders.Where(o => o.Id == id).FirstOrDefault();
            var kurir = context.CourierProfiles.Where(o => o.Id == order.CourierId).FirstOrDefault();
            if (order != null)
            {
                // EF
                order.Id = id;
                kurir.Availability = true;
                context.CourierProfiles.Update(kurir);
                context.SaveChanges();
            }
            return await Task.FromResult(order);
        }



    }





}
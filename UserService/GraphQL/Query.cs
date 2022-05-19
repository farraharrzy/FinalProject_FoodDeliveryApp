﻿using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using UserService.Models;

namespace UserService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "ADMIN" })] // dapat diakses kalau sudah login
        public IQueryable<UserData> GetUsers([Service] FoodDeliveryAppContext context) =>
            context.Users.Select(p => new UserData()
            {
                Id = p.Id,
                FullName = p.FullName,
                Email = p.Email,
                Username = p.Username
            });

        [Authorize]
        public IQueryable<Profile> GetProfilesbyToken([Service] FoodDeliveryAppContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            if (user != null)
            {
                var profiles = context.Profiles.Where(o => o.UserId == user.Id);
                return profiles.AsQueryable();
            }
            return new List<Profile>().AsQueryable();
        }

        //Courier
        [Authorize(Roles = new[] { "MANAGER" })]
        public IQueryable<Courier> GetCouriers([Service] FoodDeliveryAppContext context) =>
            context.Couriers.Select(p => new Courier()
            {
                Id = p.Id,
                CourierName = p.CourierName,
                PhoneNumber = p.PhoneNumber
            });

    }
}

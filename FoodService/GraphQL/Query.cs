using FoodService.Models;
using HotChocolate.AspNetCore.Authorization;

namespace FoodService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "BUYER" })]
        public IQueryable<Food> GetFoods([Service] FoodDeliveryAppContext context) =>
            context.Foods;
    }
}

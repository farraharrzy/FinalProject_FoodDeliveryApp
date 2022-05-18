using OrderService.Models;

namespace OrderService.GraphQL
{
    public record OrderData
    (
       int? Id,
       string? Code,
       int? UserId,
       int CourierId,
       List<OrderDetailData> OrderDetails
    );
}

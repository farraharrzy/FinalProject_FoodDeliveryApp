using OrderService.Models;

namespace OrderService.GraphQL
{
    public record OrderInput
    (
       int? Id,
       string Code,
       int UserId,
       List<OrderDetail> OrderDetails
    );
}

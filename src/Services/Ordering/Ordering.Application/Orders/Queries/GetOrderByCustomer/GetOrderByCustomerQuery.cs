

namespace Ordering.Application.Orders.Queries.GetOrderByCustomer
{
    public record GetOrderByCustomerQuery(Guid CustomreId) : IQuery<GetOrderByCustomerResult>;

    public record GetOrderByCustomerResult(IEnumerable<OrderDto> Orders);


}

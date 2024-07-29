
namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductRequest(Guid Id) : IQuery<DeleteProductResponse>;
    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid Id,ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(Id));

                var resonse = result.Adapt<DeleteProductResponse>();    

                return Results.Ok(resonse);
            });
        }
    }
}

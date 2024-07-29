
namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    // Validation
    public class DeleteProductCommanValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommanValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }
    internal class DeleteProductCommandHandler(IDocumentSession session)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {            

            session.Delete<Product>(command.Id);

            await session.SaveChangesAsync();

            return new DeleteProductResult(true);

        }
    }
}

﻿
namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name,List<string> Category,string Description,string ImageFile,decimal Price)
        :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateProductCommadnHandler(IDocumentSession session) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                Imagefile = command.ImageFile,
                Price = command.Price,
            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);            
        }
    }
}

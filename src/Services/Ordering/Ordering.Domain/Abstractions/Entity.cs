
namespace Ordering.Domain.Abstractions
{
    public abstract class Entity<T> : IEntity
    {
        public T Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModfied { get; set; }
        public string? LastModfiedBy { get; set; }
    }
}


namespace Ordering.Domain.Abstractions
{

    public interface IEntity<T> : IEntity
    {
        public T Id { get; set; }
    }
    public interface IEntity
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModfied { get; set; }
        public string? LastModfiedBy { get; set; }
    }
}

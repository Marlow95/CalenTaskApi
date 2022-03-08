using MongoDB.Driver;

namespace CalenTaskApi.Entities
{
    public record Todo 
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public bool IsComplete { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}
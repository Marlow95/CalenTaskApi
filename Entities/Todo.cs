namespace CalenTaskApi.Entities
{
    public record Todo 
    {
        public Guid TodoId { get; init; }
        public string Description { get; init; }
        public bool IsComplete { get; init; }
    }
}
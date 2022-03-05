namespace CalenTaskApi.Dtos
{
    public record TodoDto
    {
        public Guid TodoId { get; init; }
        public string Description { get; init; }
        public bool IsComplete { get; init; }
    }
}
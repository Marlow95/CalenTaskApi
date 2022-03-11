namespace CalenTaskApi.Dtos
{
    public record UpdateTodoDto
    {
        public string Description { get; init; }
        public bool IsComplete { get; init; }
    }
}
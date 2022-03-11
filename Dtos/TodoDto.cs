using System.ComponentModel.DataAnnotations;

namespace CalenTaskApi.Dtos
{
    public record TodoDto
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public bool IsComplete { get; init; }
        public string UserId { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}
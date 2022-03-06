using System.ComponentModel.DataAnnotations;

namespace CalenTaskApi.Dtos
{
    public record TodoDto
    {
        public Guid TodoId { get; init; }

        [Required]
        public string Description { get; init; }
        public bool IsComplete { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}
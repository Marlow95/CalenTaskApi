using System.ComponentModel.DataAnnotations;

namespace CalenTaskApi.Dtos
{
    public record PostTodoDto
    {
        [Required]
        public string Description { get; init; }
        public bool IsComplete { get; init; }
    }
}
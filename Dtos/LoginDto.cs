using System.ComponentModel.DataAnnotations;

namespace CalenTaskApi.Dtos
{
    public record LoginDto
    {
        [Required]
        public string UserName { get; init; }

        [Required]
        public string Password { get; init; }
    }
}
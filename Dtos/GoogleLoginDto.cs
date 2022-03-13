using System.ComponentModel.DataAnnotations;

namespace CalenTaskApi.Dtos
{
    public record GoogleLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
        [Required]
        public string PassWord { get; init; }

    }
}
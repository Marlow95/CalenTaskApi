using System.ComponentModel.DataAnnotations;

namespace CalenTaskApi.Dtos
{
    public record UpdateUsersDto
    {
        [Required]
        public string FirstName { get; init; }

        [Required]
        public string LastName { get; init; }

        [Required]
        public string UserName { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }
        public string About { get; init; }

    }
}
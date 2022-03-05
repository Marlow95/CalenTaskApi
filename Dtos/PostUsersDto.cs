using System.ComponentModel.DataAnnotations;

namespace CalenTaskApi.Dtos
{
    public record PostUsersDto
    {
        [Required]
        public Guid Id { get; init; }
        
        [Required]
        public string FirstName { get; init; }

        [Required]
        public string LastName { get; init; }

        [Required]
        public string UserName { get; init; }

        [Required]
        public string PassWord {get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }
        public string About { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset LastLogin { get; init;}

    }
}
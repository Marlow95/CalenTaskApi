namespace CalenTaskApi.Entities
{
    public record Users 
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public byte[] PasswordHash { get; init; }
        public byte[] PasswordSalt { get; init; }
        public string About { get; init; }
        public string Token { get; set; }
        public string Role { get; init; }
        public bool IsSuccess { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset LastLogin { get; set;}

    }
}
namespace CalenTaskApi.Entities
{
    public record Users 
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string About { get; init; }
        public Enum Status { get; init; }
        public Enum Role { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset LastLogin { get; init;}

    }
}
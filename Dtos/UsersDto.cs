namespace CalenTaskApi.Dtos
{
    public record UsersDto
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string About { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset LastLogin { get; set;}

    }
}
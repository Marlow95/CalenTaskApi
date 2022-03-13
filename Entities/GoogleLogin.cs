namespace CalenTaskApi.Entities
{
    public record GoogleLogin
    {
        public string Email { get; init; }
        public string PassWord { get; init; }
        public string ReturnUrl { get; init;}

    }
}
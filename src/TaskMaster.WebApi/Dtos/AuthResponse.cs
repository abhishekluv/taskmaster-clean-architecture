namespace TaskMaster.WebApi.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}

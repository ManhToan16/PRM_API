namespace PRM_API.DTO
{
    public class UsersDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
        public int RoleType { get; set; }
    }
}

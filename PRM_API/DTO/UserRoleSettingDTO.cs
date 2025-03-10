namespace PRM_API.DTO
{
    public class UserRoleSettingDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
        public string SettingName { get; set; } = null!;
        public int SettingID { get; set; }
        public int? CoursesNumber { get; set; }
        public int? TotalPoint { get; set; }
    }
}

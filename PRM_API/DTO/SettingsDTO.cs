namespace PRM_API.DTO
{
    public class SettingsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Status { get; set; }
        public string? Note { get; set; }
    }
}

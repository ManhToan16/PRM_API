using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public string? Note { get; set; }
        public int UserId { get; set; }
        public int SettingId { get; set; }
        public int RoleType { get; set; }

        public virtual Setting Setting { get; set; } = null!;
        public virtual User User { get; set; } = null!;

    }
}

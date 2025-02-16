using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class Setting
    {
        public Setting()
        {
            Exercises = new HashSet<Exercise>();
            InverseParent = new HashSet<Setting>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Status { get; set; }
        public string? Note { get; set; }
        public string Value { get; set; } = null!;
        public int? ParentId { get; set; }

        public virtual Setting? Parent { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual ICollection<Setting> InverseParent { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class User
    {
        public User()
        {
            StudentCourses = new HashSet<StudentCourse>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}

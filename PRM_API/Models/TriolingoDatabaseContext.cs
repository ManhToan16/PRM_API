using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRM_API.Models
{
    public partial class TriolingoDatabaseContext : DbContext
    {
        public TriolingoDatabaseContext()
        {
        }

        public TriolingoDatabaseContext(DbContextOptions<TriolingoDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Exercise> Exercises { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<StudentCourse> StudentCourses { get; set; } = null!;
        public virtual DbSet<StudentLesson> StudentLessons { get; set; } = null!;
        public virtual DbSet<Unit> Units { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("MyCnn");
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("Exercise");

                entity.HasIndex(e => e.LessonId, "IX_Exercise_LessonId");

                entity.HasIndex(e => e.TypeId, "IX_Exercise_TypeId");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.LessonId);

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lesson");

                entity.HasIndex(e => e.UnitId, "IX_Lesson_UnitId");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.UnitId);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.HasIndex(e => e.ExerciseId, "IX_Question_ExerciseId");

                entity.Property(e => e.Question1).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.ExerciseId);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

                entity.HasIndex(e => e.ParentId, "IX_Setting_ParentId");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Value).HasMaxLength(250);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.ToTable("StudentCourse");

                entity.HasIndex(e => e.CourseId, "IX_StudentCourse_CourseId");

                entity.HasIndex(e => e.StudentId, "IX_StudentCourse_StudentId");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.StudentCourses)
                    .HasForeignKey(d => d.CourseId);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentCourses)
                    .HasForeignKey(d => d.StudentId);
            });

            modelBuilder.Entity<StudentLesson>(entity =>
            {
                entity.ToTable("StudentLesson");

                entity.HasIndex(e => e.LessionId, "IX_StudentLesson_LessionId");

                entity.HasIndex(e => e.StudentCourseId, "IX_StudentLesson_StudentCourseId");

                entity.HasOne(d => d.Lession)
                    .WithMany(p => p.StudentLessons)
                    .HasForeignKey(d => d.LessionId);

                entity.HasOne(d => d.StudentCourse)
                    .WithMany(p => p.StudentLessons)
                    .HasForeignKey(d => d.StudentCourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit");

                entity.HasIndex(e => e.CourseId, "IX_Unit_CourseId");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.CourseId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.HasIndex(e => e.SettingId, "IX_UserRole_SettingId");

                entity.HasIndex(e => e.UserId, "IX_UserRole_UserId");

                entity.HasOne(d => d.Setting)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.SettingId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

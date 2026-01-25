using System;
using course_work.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace course_work.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserType> UserTypes { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;
    public DbSet<CourseAuthors> CourseAuthors { get; set; } = null!;
    public DbSet<CourseStudents> CourseStudents { get; set; } = null!;
    public DbSet<Test> Tests { get; set; } = null!;
    public DbSet<TestQuestion> TestQuestions { get; set; } = null!;
    public DbSet<TestQuestionOption> TestQuestionOptions { get; set; } = null!;
    public DbSet<TestResult> TestResults { get; set; } = null!;
    public DbSet<CourseReview> CourseReviews { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<LessonProgress> LessonProgresses { get; set; } = null!;
    
    public DbSet<UserComplaint> UserComplaints { get; set; } = null!;
    
    public DbSet<CourseComplaint> CourseComplaints { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(DbConfig.ConnectionString, DbConfig.ServerVersion);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserType>().HasData(new UserType()
        {
            Id = 1,
            Name = "Читатель"
        });
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<LessonProgress>()
            .HasKey(lp => lp.Id);

        modelBuilder.Entity<LessonProgress>()
            .HasOne(lp => lp.User)
            .WithMany(u => u.LessonProgresses)
            .HasForeignKey(lp => lp.UserId);

        modelBuilder.Entity<LessonProgress>()
            .HasOne(lp => lp.Lesson)
            .WithMany(l => l.LessonProgresses)
            .HasForeignKey(lp => lp.LessonId);
        
        modelBuilder.Entity<UserComplaint>()
            .HasKey(c => c.Id);
        
        modelBuilder.Entity<UserComplaint>()
            .HasOne(c => c.FromUser)
            .WithMany(u => u.ComplaintsSent)
            .HasForeignKey(c => c.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserComplaint>()
            .HasOne(c => c.ToUser)
            .WithMany(u => u.ComplaintsReceived)
            .HasForeignKey(c => c.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserComplaint>()
            .HasIndex(c => new { c.FromUserId, c.ToUserId })
            .IsUnique();
        
        /*modelBuilder.Entity<CourseReview>()
            .HasIndex(r => new { r.CourseId, r.UserId })
            .IsUnique();*/ // 1 пользователь — 1 отзыв на курс

        modelBuilder.Entity<CourseReview>()
            .HasOne(r => r.Course)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CourseReview>()
            .HasOne(r => r.User)
            .WithMany(u => u.CourseReviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ---------- CourseComplaint ----------
        modelBuilder.Entity<CourseComplaint>()
            .HasOne(c => c.Course)
            .WithMany(c => c.Complaints)
            .HasForeignKey(c => c.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CourseComplaint>()
            .HasOne(c => c.User)
            .WithMany(u => u.CourseComplaints)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}
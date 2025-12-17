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
    }
}
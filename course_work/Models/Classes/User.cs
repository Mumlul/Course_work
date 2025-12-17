using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("User")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Column("login")]
    public string Login { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }

    [Column("type")]
    public int UserTypeId { get; set; }

    [ForeignKey("UserTypeId")]
    public UserType UserType { get; set; } = null!;

    public ICollection<CourseAuthors> CoursesAuthored { get; set; } = new List<CourseAuthors>();
    public ICollection<CourseStudents> CoursesEnrolled { get; set; } = new List<CourseStudents>();
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    public ICollection<CourseReview> Reviews { get; set; } = new List<CourseReview>();
}
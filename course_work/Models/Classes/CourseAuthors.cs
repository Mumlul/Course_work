using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace course_work.Models.Classes;

[Table("Course_authors")]
public class CourseAuthors
{
    [Key]
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
using course_work.Models.Classes;

namespace course_work.Models;

public enum SearchType
{
    Course,
    Author
}


public class SearchBlock
{
    public SearchType Type { get; init; }
    public string SeacrchText { get; init; } = string.Empty;
    public Course? Course { get; init; }
    public User? Author { get; init; }
}
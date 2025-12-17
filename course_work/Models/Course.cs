namespace course_work.Models;

public class Course
{
    #region Private
    private string _title;
    private string _description;
    private string _author;
    #endregion
    
    #region Public
    public string Title { get=>_title; set =>_title=value; }
    public string Description { get=>_description; set =>_description=value; }
    public string Author { get=>_author; set =>_author=value; }
    #endregion
}
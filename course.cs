using System.Collections.Generic;

public class Course
{
    public int CourseId { get; }
    public string Title { get; }
    public string Description { get; }
    public Instructor? Instructor { get; set; }  // optional link

    public Course(int id, string title, string description)
    {
        CourseId = id;
        Title = title;
        Description = description;
    }
}

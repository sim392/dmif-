public class Enrollment
{
    public Student Student { get; }
    public int CourseId { get; }
    public DateTime EnrolledAt { get; } = DateTime.UtcNow;

    public Enrollment(Student student, int courseId)
    {
        Student = student;
        CourseId = courseId;
    }
}
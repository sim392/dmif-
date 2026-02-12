using System;
using System.Collections.Generic;
using System.Linq;

public class Instructor : User
{
    public List<int> CourseIds { get; } = new List<int>();  // courses this instructor teaches

    public Instructor(string username, string email, string password)
        : base(username, email, password)
    {
    }

    public void AddCourse(int courseId)
    {
        if (!CourseIds.Contains(courseId))
        {
            CourseIds.Add(courseId);
            Console.WriteLine($"Course {courseId} added to your teaching list.");
        }
        else
        {
            Console.WriteLine($"You are already teaching course {courseId}.");
        }
    }

    public void RemoveCourse(int courseId)
    {
        if (CourseIds.Remove(courseId))
        {
            Console.WriteLine($"Course {courseId} removed from your teaching list.");
        }
        else
        {
            Console.WriteLine($"You are not teaching course {courseId}.");
        }
    }

    public void ShowMyCourses()
    {
        if (CourseIds.Count == 0)
        {
            Console.WriteLine("You are not teaching any courses yet.");
            return;
        }
        Console.WriteLine("\n=== My Teaching Courses ===");
        foreach (int id in CourseIds) Console.WriteLine($"Course ID: {id}");
    }

    public int GetStudentCount(List<Enrollment> enrollments)
    {
        return enrollments.Count(e => CourseIds.Contains(e.CourseId));
    }

    public Course? GetCourseById(int id, List<Course> courses)
    {
        if (!CourseIds.Contains(id)) return null;
        return courses.Find(c => c.CourseId == id);
    }
}
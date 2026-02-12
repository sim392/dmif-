using System;
using System.Collections.Generic;
using System.Linq;

public class Student : User
{
    public List<int> EnrolledCourseIds { get; } = new List<int>();
    public Dictionary<int, int> CourseProgress { get; } = new Dictionary<int, int>(); // courseId → progress %

    public Student(string username, string email, string password)
        : base(username, email, password)
    {
    }

    public void EnrollInCourse(int courseId)
    {
        if (EnrolledCourseIds.Contains(courseId))
        {
            Console.WriteLine("Already enrolled in this course.");
            return;
        }
        EnrolledCourseIds.Add(courseId);
        CourseProgress[courseId] = 0;
        Console.WriteLine($"Successfully enrolled in course {courseId}.");
    }

    public void UpdateProgress(int courseId, int percentage)
    {
        if (percentage < 0 || percentage > 100)
        {
            Console.WriteLine("Progress must be between 0 and 100.");
            return;
        }
        if (!CourseProgress.ContainsKey(courseId))
        {
            Console.WriteLine("Not enrolled in this course.");
            return;
        }
        CourseProgress[courseId] = percentage;
        Console.WriteLine($"Progress for course {courseId} updated to {percentage}%.");
    }

    public void DropCourse(int courseId)
    {
        if (EnrolledCourseIds.Remove(courseId))
        {
            CourseProgress.Remove(courseId);
            Console.WriteLine($"Course {courseId} dropped successfully.");
        }
        else
        {
            Console.WriteLine("You are not enrolled in this course.");
        }
    }

    public void ShowEnrolledCourses()
    {
        if (EnrolledCourseIds.Count == 0)
        {
            Console.WriteLine("You haven't enrolled in any courses yet.");
            return;
        }

        Console.WriteLine("\n=== My Enrolled Courses ===");
        foreach (int id in EnrolledCourseIds)
        {
            int prog = CourseProgress.GetValueOrDefault(id, 0);
            Console.WriteLine($"Course {id,-6} : {prog,3}% complete");
        }
    }

    public List<int> GetCompletedCourses()
    {
        return CourseProgress
            .Where(kv => kv.Value == 100)
            .Select(kv => kv.Key)
            .ToList();
    }

    public double GetAverageProgress()
    {
        if (CourseProgress.Count == 0) return 0;
        return CourseProgress.Values.Average();
    }
}
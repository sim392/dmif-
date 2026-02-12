using System;
using System.Collections.Generic;
using System.Linq;

public class Admin : User
{
    public bool CanManageUsers { get; set; } = true;
    public bool CanManageCourses { get; set; } = true;

    public Admin(string username, string email, string password)
        : base(username, email, password)
    {
    }

    public void DisplayPermissions()
    {
        Console.WriteLine("\n=== Admin Permissions ===");
        Console.WriteLine($"Manage Users  : {(CanManageUsers ? "Yes" : "No")}");
        Console.WriteLine($"Manage Courses: {(CanManageCourses ? "Yes" : "No")}");
    }

    public void ViewAllUsers(List<User> users)
    {
        Console.WriteLine("\n=== All Registered Users ===");
        foreach (var user in users)
        {
            string type = user.GetType().Name;
            Console.WriteLine($"{user.Username,-12} {user.Email,-25} {type,-10} Active: {user.IsActive}");
        }
    }

    public void DeactivateUser(User user)
    {
        if (user == this)
        {
            Console.WriteLine("Admins cannot deactivate themselves.");
            return;
        }
        user.Deactivate();
    }

    public void GetSystemStats(List<User> users, List<Course> courses)
    {
        int students = users.OfType<Student>().Count();
        int instructors = users.OfType<Instructor>().Count();
        int admins = users.OfType<Admin>().Count();
        int active = users.Count(u => u.IsActive);

        Console.WriteLine("\n=== System Statistics ===");
        Console.WriteLine($"Total Users     : {users.Count}");
        Console.WriteLine($"  • Students    : {students}");
        Console.WriteLine($"  • Instructors : {instructors}");
        Console.WriteLine($"  • Admins      : {admins}");
        Console.WriteLine($"Active Accounts : {active}");
        Console.WriteLine($"Total Courses   : {courses.Count}");
    }
}
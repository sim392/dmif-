using System;
using System.Collections.Generic;

class Program
{
    private static readonly List<User> users = new List<User>();
    private static readonly List<Course> courses = new List<Course>();
    private static readonly List<Enrollment> enrollments = new List<Enrollment>();

    private static User? currentUser = null;
    private static bool isLoggedIn = false;

    static void Main()
    {
        
        SeedData();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SmartLearn LMS ===");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("\nChoice: ");

            string? choice = Console.ReadLine();

            if (choice == "1") RegisterUser();
            else if (choice == "2") LoginUser();
            else if (choice == "3") break;
            else Console.WriteLine("Invalid choice.");
        }
    }

    static void SeedData()
    {
        courses.Add(new Course(101, "C# Basics", "Introduction to C#"));
        courses.Add(new Course(102, "OOP in C#", "Inheritance & Polymorphism"));
    }

    static void RegisterUser()
    {
        Console.Write("Username: "); string? username = Console.ReadLine();
        Console.Write("Email   : "); string? email = Console.ReadLine();
        Console.Write("Password: "); string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("All fields are required.");
            return;
        }

        Console.WriteLine("\nSelect role:");
        Console.WriteLine("1. Student");
        Console.WriteLine("2. Instructor");
        Console.WriteLine("3. Admin");
        string? role = Console.ReadLine();

        User newUser;

        if (role == "1")
        {
            newUser = new Student(username, email, password);
            Console.WriteLine("Student account created.");
        }
        else if (role == "2")
        {
            newUser = new Instructor(username, email, password);
            Console.WriteLine("Instructor account created.");
        }
        else if (role == "3")
        {
            newUser = new Admin(username, email, password);
            Console.WriteLine("Admin account created.");
        }
        else
        {
            Console.WriteLine("Invalid role selected.");
            return;
        }

        users.Add(newUser);
    }

    static void LoginUser()
    {
        Console.Write("Username: "); string? username = Console.ReadLine();
        Console.Write("Password: "); string? password = Console.ReadLine();

        User? found = users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (found == null || !found.ValidatePassword(password))
        {
            Console.WriteLine("Invalid username or password.");
            return;
        }

        if (!found.IsActive)
        {
            Console.WriteLine("Account is deactivated.");
            return;
        }

        currentUser = found;
        isLoggedIn = true;

        Console.WriteLine($"\nWelcome, {found.Username}!");

        if (found is Student student)
        {
            ShowStudentDashboard(student);
        }
        else if (found is Instructor instructor)
        {
            ShowInstructorDashboard(instructor);
        }
        else if (found is Admin admin)
        {
            ShowAdminDashboard(admin);
        }
    }

    

    static void ShowStudentDashboard(Student student)
    {
        while (isLoggedIn)
        {
            Console.Clear();
            Console.WriteLine($"=== Student Dashboard - {student.Username} ===");
            Console.WriteLine("1. Enroll in Course");
            Console.WriteLine("2. View My Courses");
            Console.WriteLine("3. Update Progress");
            Console.WriteLine("4. Drop Course");
            Console.WriteLine("5. View Stats");
            Console.WriteLine("6. Logout");
            Console.Write("\nChoice: ");

            string? opt = Console.ReadLine();

            if (opt == "1")
            {
                Console.Write("Enter course ID: ");
                if (int.TryParse(Console.ReadLine(), out int cid))
                {
                    student.EnrollInCourse(cid);
                    if (student.EnrolledCourseIds.Contains(cid))
                    {
                        enrollments.Add(new Enrollment(student, cid));
                    }
                }
            }
            else if (opt == "2") student.ShowEnrolledCourses();
            else if (opt == "3")
            {
                Console.Write("Course ID: ");
                if (int.TryParse(Console.ReadLine(), out int cid) &&
                    int.TryParse(Console.ReadLine(), out int percent))
                {
                    student.UpdateProgress(cid, percent);
                }
            }
            else if (opt == "4")
            {
                Console.Write("Course ID to drop: ");
                if (int.TryParse(Console.ReadLine(), out int cid))
                {
                    student.DropCourse(cid);
                    enrollments.RemoveAll(e => e.Student == student && e.CourseId == cid);
                }
            }
            else if (opt == "5")
            {
                Console.WriteLine($"\nAverage progress: {student.GetAverageProgress():F1}%");
                var completed = student.GetCompletedCourses();
                Console.WriteLine($"Completed courses: {completed.Count} ({string.Join(", ", completed)})");
            }
            else if (opt == "6")
            {
                Logout();
                return;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void ShowInstructorDashboard(Instructor instructor)
    {
        while (isLoggedIn)
        {
            Console.Clear();
            Console.WriteLine($"=== Instructor Dashboard - {instructor.Username} ===");
            Console.WriteLine("1. View My Courses");
            Console.WriteLine("2. Add Course (to teaching list)");
            Console.WriteLine("3. Show total students (all courses)");
            Console.WriteLine("4. Logout");
            Console.Write("\nChoice: ");

            string? opt = Console.ReadLine();

            if (opt == "1") instructor.ShowMyCourses();
            else if (opt == "2")
            {
                Console.Write("Course ID to teach: ");
                if (int.TryParse(Console.ReadLine(), out int cid))
                    instructor.AddCourse(cid);
            }
            else if (opt == "3")
            {
                int count = instructor.GetStudentCount(enrollments);
                Console.WriteLine($"Total students across your courses: {count}");
            }
            else if (opt == "4")
            {
                Logout();
                return;
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }

    static void ShowAdminDashboard(Admin admin)
    {
        while (isLoggedIn)
        {
            Console.Clear();
            Console.WriteLine($"=== Admin Dashboard - {admin.Username} ===");
            Console.WriteLine("1. View All Users");
            Console.WriteLine("2. View System Statistics");
            Console.WriteLine("3. Deactivate User");
            Console.WriteLine("4. Logout");
            Console.Write("\nChoice: ");

            string? opt = Console.ReadLine();

            if (opt == "1") admin.ViewAllUsers(users);
            else if (opt == "2") admin.GetSystemStats(users, courses);
            else if (opt == "3")
            {
                Console.Write("Username to deactivate: ");
                string? target = Console.ReadLine();
                var user = users.Find(u => u.Username.Equals(target, StringComparison.OrdinalIgnoreCase));
                if (user != null) admin.DeactivateUser(user);
                else Console.WriteLine("User not found.");
            }
            else if (opt == "4")
            {
                Logout();
                return;
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }

    static void Logout()
    {
        isLoggedIn = false;
        currentUser = null;
        Console.WriteLine("Logged out successfully.");
    }
}
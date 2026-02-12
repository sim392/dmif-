using System;

public abstract class User
{
    public string Username { get; }
    public string Email { get; }
    public string Password { get; private set; }  
    public bool IsActive { get; private set; } = true;
    public DateTime DateRegistered { get; } = DateTime.UtcNow;

    protected User(string username, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username required");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email required");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password required");

        Username = username.Trim();
        Email = email.Trim();
        Password = password;
    }

    public bool ValidatePassword(string inputPassword)
    {
        return Password == inputPassword;  // In real app → use hashing!
    }

    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("New password cannot be empty");
        Password = newPassword;
        Console.WriteLine("Password changed successfully.");
    }

    public void Deactivate()
    {
        IsActive = false;
        Console.WriteLine($"Account '{Username}' has been deactivated.");
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Username : {Username}");
        Console.WriteLine($"Email    : {Email}");
        Console.WriteLine($"Active   : {(IsActive ? "Yes" : "No")}");
        Console.WriteLine($"Registered: {DateRegistered:yyyy-MM-dd HH:mm}");
    }
}
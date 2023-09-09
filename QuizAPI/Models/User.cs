namespace QuizAPI.Models;

public class User :Users
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizAPI.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int CorrectAnswer { get; set; }
        public int WrongAnswer { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }

    }
}

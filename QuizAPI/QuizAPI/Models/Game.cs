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

    public class GameDTO
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int CorrectAnswer { get; set; }
        public int WrongAnswer { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }

    }
    public static class GameExtensions
    {
        public static GameDTO ToDto(this Game item)
        {
            return new GameDTO
            {
                Id = item.Id,
                Score = item.Score,
                CorrectAnswer = item.CorrectAnswer,
                WrongAnswer = item.WrongAnswer,
                Email = item.Email,
                Date = item.Date

            };
        }

    }
}

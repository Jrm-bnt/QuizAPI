using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizAPI.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Quest { get; set; }
        public string Options { get; set; }
        public int CorrectAnswer { get; set; }
    }
}

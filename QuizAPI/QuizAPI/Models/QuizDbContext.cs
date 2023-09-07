using Microsoft.EntityFrameworkCore;

namespace QuizAPI.Models
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
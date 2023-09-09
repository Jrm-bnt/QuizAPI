#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{
    [Route("api/Game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly QuizDbContext _contextGame;

        public GameController(QuizDbContext context)
        {
            _contextGame = context;
        }

        // GET: api/Game
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            if (_contextGame.Games == null)
            {
                return NotFound();
            }
            return await _contextGame.Games.ToListAsync();
        }

        // GET: api/Game/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            if (_contextGame.Games == null)
            {
                return NotFound();
            }
            var game = await _contextGame.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }
            return game;
        }


        // POST: api/Game
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _contextGame.Games.Add(game);
            await _contextGame.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

    }
}

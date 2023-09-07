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
        public async Task<ActionResult<Game>> GetGame(long id)
        {
            //Vérfifier si la table existe
            if (_contextGame.Games == null)
            {
                return NotFound();
            }
            //Cherche l'objet
            var game = await _contextGame.Games.FindAsync(id);

            //On vérifie si l'objet existe
            if (game == null)
            {
                return NotFound();
            }

            //On retourne l'objet
            return game;
        }


        // POST: api/Game
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            if (_contextGame.Games == null)
            {
                return Problem("Entity set 'TodoContext.TodoItems'  is null.");
            }
            _contextGame.Games.Add(game);
            await _contextGame.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }



        private bool GameExists(long id)
        {
            return (_contextGame.Games?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

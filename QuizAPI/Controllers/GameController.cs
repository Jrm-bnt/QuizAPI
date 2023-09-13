#nullable disable // Désactive la vérification de la nullabilité pour ce fichier

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
        /// <summary>
        /// Récupère la liste des jeux.
        /// </summary>
        /// <param></param>
        /// <returns>Une reponse HTTP contenant la liste des jeux.</returns>
        /// <response code="200">La liste des jeux a été récupérée avec succès.</response>
        /// <response code="404">Si la liste des jeux est introuvable.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="401">Si l'utilisateur n'est pas authentifié.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            // Vérifie si la liste de jeux existe
            if (_contextGame.Games == null)
            {
                return NotFound();
            }
            return await _contextGame.Games.ToListAsync();
        }

        // GET: api/Game/{id}
        /// <summary>
        /// Récupère un jeu en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du jeu à récupérer.</param>
        /// <returns>Une réponse HTTP contenant le jeu demandé.</returns>
        /// <response code="200">Le jeu a été trouvé et renvoyé avec succès.</response>
        /// <response code="404">Si le jeu est introuvable.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="401">Si l'utilisateur n'est pas authentifié.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            // Vérifie si la liste de jeux existe
            if (_contextGame.Games == null)
            {
                return NotFound();
            }
            var game = await _contextGame.Games.FindAsync(id);

            // Si le jeu n'est pas trouvé, retourne une réponse NotFound
            if (game == null)
            {
                return NotFound();
            }
            return game;
        }

        // POST: api/Game
        /// <summary>
        /// Crée un nouveau jeu.
        /// </summary>
        /// <param name="game">Le jeu à créer.</param>
        /// <returns>Une réponse HTTP indiquant que le jeu a été créé avec succès.</returns>
        /// <response code="201">Le jeu a été créé avec succès.</response>
        /// <response code="400">Si le jeu est null.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="401">Si l'utilisateur n'est pas authentifié.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            // Ajoute un nouveau jeu à la base de données
            _contextGame.Games.Add(game);
            await _contextGame.SaveChangesAsync();

            // Retourne une réponse Created avec l'URL de l'objet créé
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

    }
}

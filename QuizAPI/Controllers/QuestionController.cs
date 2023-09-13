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
    [Route("api/Questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuizDbContext _contextQuestion;

        public QuestionsController(QuizDbContext context)
        {
            _contextQuestion = context;
        }

        // GET: api/Questions
        /// <summary>
        /// Récupère la liste des questions
        /// </summary>
        /// <returns>Une réponse HTTP contenant la liste des questions.</returns>
        /// <response code="200">La liste des questions a été trouvée et renvoyée avec succès.</response>
        /// <response code="404">Si la liste des questions est introuvable.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="401">Si l'utilisateur n'est pas authentifié.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionItems()
        {
            // Vérifie si la liste de questions existe
            if (_contextQuestion.Questions == null)
            {
                return NotFound();
            }
            return await _contextQuestion.Questions.ToListAsync();
        }

        // GET: api/Questions/{id}
        /// <summary>
        /// Récupère une question en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la question à récupérer.</param>
        /// <returns>Une réponse HTTP contenant la question demandée.</returns>
        /// <response code="200">La question a été trouvée et renvoyée avec succès.</response>
        /// <response code="404">Si la question est introuvable.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="401">Si l'utilisateur n'est pas authentifié.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestionItem(int id)
        {
            // Vérifie si la liste de questions existe
            if (_contextQuestion.Questions == null)
            {
                return NotFound();
            }
            var questionItem = await _contextQuestion.Questions.FindAsync(id);

            // Si la question n'est pas trouvée, retourne une réponse NotFound
            if (questionItem == null)
            {
                return NotFound();
            }

            return questionItem;
        }

        // PUT: api/Questions/{id}
        /// <summary>
        /// Met à jour une question en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la question à mettre à jour.</param>
        /// <param name="questionItem">Les nouvelles données de la question.</param>
        /// <returns>Une réponse HTTP indiquant le succès de la mise à jour.</returns>
        /// <response code="204">La question a été mise à jour avec succès.</response>
        /// <response code="400">Si l'identifiant de la question ne correspond pas à celui dans les données.</response>
        /// <response code="404">Si la question est introuvable.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="401">Si l'utilisateur n'est pas authentifié.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionItem(int id, Question questionItem)
        {
            if (id != questionItem.Id)
            {
                return BadRequest();
            }

            _contextQuestion.Entry(questionItem).State = EntityState.Modified;

            try
            {
                await _contextQuestion.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Questions
        /// <summary>
        /// Crée une nouvelle question.
        /// </summary>
        /// <param name="questionItem">La question à créer.</param>
        /// <returns>Une réponse HTTP indiquant que la question a été créée avec succès.</returns>
        /// <response code="201">La question a été créée avec succès.</response>
        /// <response code="400">Si la question est null.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="401">Si l'utilisateur n'est pas authentifié.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestionItem(Question questionItem)
        {
            _contextQuestion.Questions.Add(questionItem);
            await _contextQuestion.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestionItem), new { id = questionItem.Id }, questionItem);
        }

        // DELETE: api/Questions/{id}
        /// <summary>
        /// Supprime une question en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la question à supprimer.</param>
        /// <returns>Une réponse HTTP indiquant le succès de la suppression.</returns>
        /// <response code="204">La question a été supprimée avec succès.</response>
        /// <response code="404">Si la question est introuvable.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="401">Si l'utilisateur n'est pas authentifié.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionItem(int id)
        {
            if (_contextQuestion.Questions == null)
            {
                return NotFound();
            }
            var questionItem = await _contextQuestion.Questions.FindAsync(id);
            if (questionItem == null)
            {
                return NotFound();
            }

            _contextQuestion.Questions.Remove(questionItem);
            await _contextQuestion.SaveChangesAsync();

            return NoContent();
        }

        // Méthode auxiliaire pour vérifier l'existence de la question
        private bool QuestionItemExists(int id)
        {
            return (_contextQuestion.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

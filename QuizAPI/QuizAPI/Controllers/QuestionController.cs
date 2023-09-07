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
    [Route("api/Questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuizDbContext _contextQuestion;

        public QuestionsController(QuizDbContext context)
        {
            _contextQuestion = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionItems()
        {
            if (_contextQuestion.Questions == null)
            {
                return NotFound();
            }
            return await _contextQuestion.Questions.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestionItem(long id)
        {
            //Vérfifier si la table existe
            if (_contextQuestion.Questions == null)
            {
                return NotFound();
            }
            //Cherche l'objet
            var questionItem = await _contextQuestion.Questions.FindAsync(id);

            //On vérifie si l'objet existe
            if (questionItem == null)
            {
                return NotFound();
            }

            //On retourne l'objet
            return questionItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, Question questionItem)
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

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Créer une tâche à faire 
        /// </summary>
        /// <remarks>
        /// Sample request : 
        /// 
        ///     POST /TodoItems
        ///     {
        ///         "id":1,
        ///         "name":"Item1",
        ///         "isComplete":true
        ///     }
        ///     
        /// </remarks>
        /// <param name="questionItem"></param>
        /// <returns>A newly created todo items</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        public async Task<ActionResult<Question>> PostTodoItem(Question questionItem)
        {
            if (_contextQuestion.Questions == null)
            {
                return Problem("Entity set 'TodoContext.TodoItems'  is null.");
            }
            _contextQuestion.Questions.Add(questionItem);
            await _contextQuestion.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestionItem), new { id = questionItem.Id }, questionItem);
        }

        // DELETE: api/TodoItems/5
        /// <summary>
        /// Cette méthode sert à supprimer
        /// </summary>
        /// <param name="id">ID de l'item</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionItem(long id)
        {
            if (_contextQuestion.Questions == null)
            {
                return NotFound();
            }
            var todoItem = await _contextQuestion.Questions.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _contextQuestion.Questions.Remove(todoItem);
            await _contextQuestion.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionItemExists(long id)
        {
            return (_contextQuestion.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

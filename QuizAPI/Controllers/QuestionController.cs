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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionItems()
        {
            if (_contextQuestion.Questions == null)
            {
                return NotFound();
            }
            return await _contextQuestion.Questions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestionItem(int id)
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

            return questionItem;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Créer une tâche à faire 
        /// </summary>
        /// <remarks>
        /// Sample request : 
        /// 
        ///     POST /QuestionItems
        ///     {
        ///         "id":1,
        ///         "name":"Item1",
        ///         "isComplete":true
        ///     }
        ///     
        /// </remarks>
        /// <param name="questionItem"></param>
        /// <returns>A newly created question items</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestionItem(Question questionItem)
        {

            _contextQuestion.Questions.Add(questionItem);
            await _contextQuestion.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestionItem), new { id = questionItem.Id }, questionItem);
        }

        /// <summary>
        /// Cette méthode sert à supprimer
        /// </summary>
        /// <param name="id">ID de l'item</param>
        /// <returns></returns>
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

        private bool QuestionItemExists(int id)
        {
            return (_contextQuestion.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

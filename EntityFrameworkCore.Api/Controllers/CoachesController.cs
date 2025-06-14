using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachesController : ControllerBase
    {
        private readonly ICoachService _coachService;

        public CoachesController(ICoachService coachService)
        {
            _coachService = coachService;
        }

        // GET: api/Coaches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoachReadDto>>> GetCoaches()
        {
            return Ok(await _coachService.GetAllCoachesAsync());
        }

        // GET: api/Coaches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoachReadInfoDto>> GetCoach(int id)
        {
            var coach = await _coachService.GetCoachByIdAsync(id);
            if (coach == null)
            {
                return NotFound();
            }
            return Ok(coach);
        }

        // DELETE: api/Coaches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoach(int id)
        {
            await _coachService.DeleteCoachByIdAsync(id);

            return NoContent();
        }

        // POST: api/Coaches
        [HttpPost]
        public async Task<ActionResult<CoachReadDto>> PostCoach(CoachCreateDto coach)
        {
            CoachReadDto createdCoach;
            try
            {
                createdCoach = await _coachService.AddCoachAsync(coach);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(GetCoach), new { id = createdCoach.Id }, createdCoach);
        }

        // PUT: api/Coaches/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCoach(int id, CoachCreateDto coach)
        {
            try
            {
                await _coachService.UpdateCoachAsync(id, coach);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("A concurrency conflict occurred while updating the coach.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

            return NoContent();
        }
    }
}

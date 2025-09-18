using EntityFrameworkCore.Application.Coaches.Commands;
using EntityFrameworkCore.Application.Coaches.Queries;
using EntityFrameworkCore.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoachesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoachesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Coaches
        [Authorize(Roles = "user,mod")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoachReadDto>>> GetCoaches()
        {
            var query = new GetCoachesQuery();
            var coaches = await _mediator.Send(query);
            return Ok(coaches);
        }

        // GET: api/Coaches/5
        [Authorize(Roles = "user,mod")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CoachReadInfoDto>> GetCoach(int id)
        {
            var query = new GetCoachByIdQuery(id);
            var coach = await _mediator.Send(query);
            if (coach == null)
            {
                return NotFound();
            }
            return Ok(coach);
        }

        // DELETE: api/Coaches/5
        [Authorize(Roles = "mod")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoach(int id)
        {
            var command = new DeleteCoachCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        // POST: api/Coaches
        [Authorize(Roles = "mod")]
        [HttpPost]
        public async Task<ActionResult<CoachReadDto>> PostCoach(CoachCreateDto coach)
        {
            var command = new CreateCoachCommand(coach);
            CoachReadDto createdCoach;
            try
            {
                createdCoach = await _mediator.Send(command);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(GetCoach), new { id = createdCoach.Id }, createdCoach);
        }

        // PUT: api/Coaches/5
        [Authorize(Roles = "mod")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutCoach(int id, CoachCreateDto coach)
        {
            var command = new UpdateCoachCommand(id, coach);
            try
            {
                await _mediator.Send(command);
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

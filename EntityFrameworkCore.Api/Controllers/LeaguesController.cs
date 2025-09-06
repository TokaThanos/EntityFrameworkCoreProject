using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Leagues.Commands;
using EntityFrameworkCore.Application.Leagues.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaguesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaguesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Leagues
        [Authorize(Roles = "user,mod")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueReadDto>>> GetLeagues()
        {
            var query = new GetLeaguesQuery();
            var coaches = await _mediator.Send(query);
            return Ok(coaches);
        }

        // GET: api/Leagues/5
        [Authorize(Roles = "user,mod")]
        [HttpGet("{id}")]
        public async Task<ActionResult<LeagueReadInfoDto>> GetLeague(int id)
        {
            var query = new GetLeagueByIdQuery(id);
            var league = await _mediator.Send(query);
            if (league == null)
            {
                return NotFound();
            }
            return Ok(league);
        }

        // PUT: api/Leagues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "mod")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeague(int id, LeagueCreateDto league)
        {
            var command = new UpdateLeagueCommand(id, league);
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
                return Conflict("A concurrency conflict occurred while updating the league.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

            return NoContent();
        }

        // POST: api/Leagues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "mod")]
        [HttpPost]
        public async Task<ActionResult<LeagueReadDto>> PostLeague(LeagueCreateDto league)
        {
            LeagueReadDto createdLeague;
            var command = new CreateLeagueCommand(league);
            try
            {
                createdLeague = await _mediator.Send(command);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetLeague), new { id = createdLeague.Id }, createdLeague);
        }

        // DELETE: api/Leagues/5
        [Authorize(Roles = "mod")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            var command = new DeleteLeagueCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}

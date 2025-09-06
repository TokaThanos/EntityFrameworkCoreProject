using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Teams.Commands;
using EntityFrameworkCore.Application.Teams.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Teams
        [Authorize(Roles = "user,mod")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamReadDto>>> GetTeams()
        {
            var query = new GetTeamsQuery();
            var teams = await _mediator.Send(query);
            return Ok(teams);
        }

        // GET: api/Teams/5
        [Authorize(Roles = "user,mod")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamReadInfoDto>> GetTeam(int id)
        {
            var query = new GetTeamByIdQuery(id);
            var team = await _mediator.Send(query);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "mod")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamUpdateDto team)
        {
            var command = new UpdateTeamCommand(id, team);
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("A concurrency conflict occurred while updating the team.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "mod")]
        [HttpPost]
        public async Task<ActionResult<TeamReadDto>> PostTeam(TeamCreateDto team)
        {
            TeamReadDto createdTeam;
            var command = new CreateTeamCommand(team);
            try
            {
                createdTeam = await _mediator.Send(command);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetTeam), new { id = createdTeam.Id }, createdTeam);
        }

        // DELETE: api/Teams/5
        [Authorize(Roles = "mod")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var command = new DeleteTeamCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}

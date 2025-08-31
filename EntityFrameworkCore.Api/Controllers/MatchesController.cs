using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Matches.Commands;
using EntityFrameworkCore.Application.Matches.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "user,mod")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchReadDto>>> GetMatches()
        {
            var query = new GetMatchesQuery();
            var matches = await _mediator.Send(query);
            return Ok(matches);
        }

        [Authorize(Roles = "user,mod")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MatchReadInfoDto>> GetMatch(int id)
        {
            var query = new GetMatchByIdQuery(id);
            var match = await _mediator.Send(query);
            if (match is null)
            {
                return NotFound();
            }
            return Ok(match);
        }

        [Authorize(Roles = "mod")]
        [HttpPost]
        public async Task<ActionResult<MatchReadDto>> PostMatch(MatchCreateDto matchCreateDto)
        {
            var command = new CreateMatchCommand(matchCreateDto);
            MatchReadDto createdMatch;
            try
            {
                createdMatch = await _mediator.Send(command);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(GetMatch), new {id = createdMatch.Id}, createdMatch);
        }

        [Authorize(Roles = "mod")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutMatch(int id, MatchUpdateDto matchUpdateDto)
        {
            UpdateMatchCommand command = new UpdateMatchCommand(id, matchUpdateDto);
            try
            {
                await _mediator.Send(command);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [Authorize(Roles = "mod")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(int id)
        {
            var command = new DeleteMatchCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}

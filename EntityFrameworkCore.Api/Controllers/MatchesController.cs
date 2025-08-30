using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Matches.Commands;
using EntityFrameworkCore.Application.Matches.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MatchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchReadDto>>> GetMatches()
        {
            var query = new GetMatchesQuery();
            var matches = await _mediator.Send(query);
            return Ok(matches);
        }

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

        [HttpPost]
        public async Task<ActionResult<MatchReadDto>> PostMatch(MatchCreateDto matchCreateDto)
        {
            var command = new CreateMatchCommand(matchCreateDto);
            MatchReadDto cretedMatch;
            try
            {
                cretedMatch = await _mediator.Send(command);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return CreatedAtAction(nameof(GetMatch), new {id = cretedMatch.Id}, cretedMatch);
        }

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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(int id)
        {
            var command = new DeleteMatchCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}

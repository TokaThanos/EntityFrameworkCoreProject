using EntityFrameworkCore.Application.Dtos;
using EntityFrameworkCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaguesController : ControllerBase
    {
        private readonly ILeagueService _leagueService;

        public LeaguesController(ILeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        // GET: api/Leagues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueReadDto>>> GetLeagues()
        {
            return Ok(await _leagueService.GetAllLeaguesAsync());
        }

        // GET: api/Leagues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeagueReadInfoDto>> GetLeague(int id)
        {
            var league = await _leagueService.GetLeagueByIdAsync(id);
            if (league == null)
            {
                return NotFound();
            }
            return Ok(league);
        }

        // PUT: api/Leagues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeague(int id, LeagueCreateDto league)
        {
            try
            {
                await _leagueService.UpdateLeagueAsync(id, league);
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
        [HttpPost]
        public async Task<ActionResult<LeagueReadDto>> PostLeague(LeagueCreateDto league)
        {
            LeagueReadDto createdLeague;
            try
            {
                createdLeague = await _leagueService.AddLeagueAsync(league);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetLeague), new { id = createdLeague.Id }, createdLeague);
        }

        // DELETE: api/Leagues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            await _leagueService.DeleteLeagueByIdAsync(id);
            return NoContent();
        }
    }
}

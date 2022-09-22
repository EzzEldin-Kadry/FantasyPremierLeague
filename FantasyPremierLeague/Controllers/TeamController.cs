using AutoMapper;
using FantasyPremierLeague.Dto;
using FantasyPremierLeague.Models;
using FantasyPremierLeague.Repository;
using FantasyPremierLeague.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FantasyPremierLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get_Methods

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<TeamDto>))]
        public IActionResult GetTeams()
        {
            var teams = _mapper.Map<List<TeamDto>>(_unitOfWork.Team.GetTeamsSortedbyNames());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(teams); 
        }
        
        [HttpGet("{teamId}")]
        [ProducesResponseType(200,Type = typeof(TeamDto))]
        [ProducesResponseType(400)]
        public IActionResult GetTeambyId(int teamId)
        {
            if (!_unitOfWork.Team.HasTeam(teamId))
                return NotFound();
            var team = _mapper.Map<TeamDto>(_unitOfWork.Team.GetTeam(teamId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(team);
        }

        #endregion

        #region Post_Methods

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTeam([FromBody] TeamDto teamCreate)
        {
            if (teamCreate is null)
                return BadRequest(ModelState);

            var team = _unitOfWork.Team.GetAll(x => x.Name.Trim().ToUpper() == teamCreate.Name.Trim().ToUpper())
                                       .FirstOrDefault();

            if(team is not null)
            {
                ModelState.AddModelError("","Team already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teamMapped = _mapper.Map<Team>(teamCreate);

            if(!_unitOfWork.Team.CreateEntity(teamMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        #endregion

        #region Put_Methods

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateTeam([FromQuery] int teamId,[FromBody] TeamDetailsDto teamUpdate)
        {
            if (teamUpdate is null)
                return BadRequest(ModelState);

            if (teamUpdate.Id != teamId)
                return BadRequest("teamId not matched with record's teamId");

            if (!_unitOfWork.Team.HasTeam(teamId))
                return NotFound("teamId doesn't exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teamMapped = _mapper.Map<Team>(teamUpdate);

            if (!_unitOfWork.Team.UpdateEntity(teamMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }
        #endregion

        #region Delete_Methods

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteTeam([FromQuery] int teamId)
        {
            if (!_unitOfWork.Team.HasTeam(teamId))
                return NotFound("teamId doesn't exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var team = _unitOfWork.Team.GetTeam(teamId);

            if(!_unitOfWork.Team.DeleteEntity(team))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
        #endregion

    }
}

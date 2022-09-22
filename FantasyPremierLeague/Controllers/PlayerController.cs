using AutoMapper;
using FantasyPremierLeague.Dto;
using FantasyPremierLeague.Models;
using FantasyPremierLeague.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FantasyPremierLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlayerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Get_Methods
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Team>))]
        public IActionResult GetPlayers()
        {
            var players = _mapper.Map<List<PlayerDto>>(_unitOfWork.Player.GetPlayersSortedbyNames());
            //var players = _unitOfWork.Player.GetPlayersSortedbyNames();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(players);
        }

        [HttpGet("{playerId}/player")]
        [ProducesResponseType(200, Type = typeof(PlayerDto))]
        [ProducesResponseType(400)]
        public IActionResult GetPlayer(int playerId)
        {
            if (!_unitOfWork.Player.HasPlayer(playerId))
                return NotFound();

            var player = _mapper.Map<PlayerDto>(_unitOfWork.Player.GetPlayer(playerId));
            //var player = _unitOfWork.Player.GetPlayer(playerId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(player);
        }

        [HttpGet("{playerId}/Team")]
        [ProducesResponseType(200, Type = typeof(TeamDto))]
        [ProducesResponseType(400)]
        public IActionResult GetTeambyPlayer(int playerId)
        {
            if (!_unitOfWork.Player.HasPlayer(playerId))
                return NotFound();
            var team = _mapper.Map<TeamDto>(_unitOfWork.Player.GetTeambyPlayer(playerId));
            //var team = _unitOfWork.Player.GetTeambyPlayer(playerId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(team);
        }

        [HttpGet("{playerId}/Coaches")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CoachDetailsDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCoachesOfPlayer(int playerId)
        {
            if (!_unitOfWork.Player.HasPlayer(playerId))
                return NotFound();

            var coaches = _mapper.Map<IEnumerable<CoachDetailsDto>>(_unitOfWork.Player.GetCoachesOfPlayer(playerId));
            //var coach = _unitOfWork.Player.GetCoachesOfPlayer(playerId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(coaches);
        }
        #endregion

        #region Post_Methods

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePlayer([FromBody] PlayerDto playerCreate)
        {
            if (playerCreate is null)
                return BadRequest(ModelState);

            if(playerCreate.TeamId is not null && !_unitOfWork.Team.HasTeam((int)playerCreate.TeamId))
            {
                ModelState.AddModelError("","teamId doesn't exist");
                return NotFound(ModelState);
            }

            if (playerCreate.Statistics is null)
                playerCreate.Statistics = new();

            var playerMapped = _mapper.Map<Player>(playerCreate);

            if(playerCreate.TeamId is not null)
                playerMapped.Team = _unitOfWork.Team.GetTeam((int)playerCreate.TeamId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_unitOfWork.Player.CreateEntity(playerMapped))
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
        public IActionResult UpdatePlayer([FromQuery] int playerId, [FromBody] PlayerDto playerUpdate)
        {
            if (playerUpdate is null)
                return BadRequest(ModelState);

            if (playerUpdate.Id != playerId)
                return BadRequest("playerId not matched with record's playerId");

            if (!_unitOfWork.Player.HasPlayer(playerId))
                return NotFound("playerId doesn't exist");

            if(!_unitOfWork.Team.HasTeam((int)playerUpdate.TeamId))
                return NotFound("teamId doesn't exist");

            if(playerUpdate.Statistics is not null)
            {
                if(_unitOfWork.Player.GetStatistics(playerId).Id != playerUpdate.Statistics.Id)
                    return BadRequest("StatisticsId not matched with this player's StatisticsId");
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var playerMapped = _mapper.Map<Player>(playerUpdate);

            if (!_unitOfWork.Player.UpdateEntity(playerMapped))
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
        public IActionResult DeletePlayer([FromQuery] int playerId)
        {
            if (!_unitOfWork.Player.HasPlayer(playerId))
                return NotFound("playerId doesn't exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var player = _unitOfWork.Player.GetPlayer(playerId);

            if (!_unitOfWork.Player.DeleteEntity(player))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }

        #endregion

    }
}

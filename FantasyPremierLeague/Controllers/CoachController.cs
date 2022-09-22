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
    public class CoachController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CoachController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Get_Methods
        
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CoachDetailsDto>))]
        public IActionResult GetCoachs()
        {
            var coaches = _mapper.Map<IEnumerable<CoachDetailsDto>>(_unitOfWork.Coach.GetAll());
            //var coaches = _unitOfWork.Coach.GetAll();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(coaches);
        }

        [HttpGet("{coachId}/coach")]
        [ProducesResponseType(200, Type = typeof(CoachDto))]
        [ProducesResponseType(400)]
        public IActionResult GetCoach(int coachId)
        {
            if (!_unitOfWork.Coach.HasCoach(coachId))
                return NotFound();

            var coach = _mapper.Map<CoachDto>(_unitOfWork.Coach.GetCoach(coachId));
            //var coach = _unitOfWork.Coach.GetCoach(coachId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(coach);
        }

        [HttpGet("{coachId}/teamOfCoach")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<PlayerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTeamOfCoach(int coachId)
        {
            if (!_unitOfWork.Coach.HasCoach(coachId))
                return NotFound();

            var players = _mapper.Map<IEnumerable<PlayerDto>>(_unitOfWork.Coach.GetTeamOfCoach(coachId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(players); 
        }
        #endregion


        #region Post_Methods
        
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCoach([FromBody] CoachDto coachCreate)
        {
            if (coachCreate is null)
                return BadRequest(ModelState);

            if(coachCreate.CoachPlayers is not null)
            {
                if(!ValidatePlayers(coachCreate.CoachPlayers))
                {
                    return StatusCode(422, ModelState);
                }
            }
            var coachMapped = _mapper.Map<Coach>(coachCreate);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_unitOfWork.Coach.CreateEntity(coachMapped))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPost("player_IDs")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddPlayersToCoachPlayers([FromQuery]int coachId, [FromQuery]IList<int> player_IDs)
        {
            if (!_unitOfWork.Coach.HasCoach(coachId))
            {
                ModelState.AddModelError("", "coachId doesn't exist");
                return NotFound(ModelState);
            }

            if(player_IDs is null)
            {
                ModelState.AddModelError("", "No playerIds where entered");
                return NotFound(ModelState);
            }

            if(!ValidatePlayers(player_IDs))
                return StatusCode(422, ModelState);

            if (_unitOfWork.Coach.AlreadyHasPlayers(coachId, player_IDs))
            {
                ModelState.AddModelError("", "Some playerIds already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_unitOfWork.Coach.AddPlayersToCoach(coachId, player_IDs))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added");
        }

        #endregion

        #region Put_Methods

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCoach([FromQuery] int coachId, [FromBody] CoachDetailsDto coachUpdate)
        {
            if (coachUpdate is null)
                return BadRequest(ModelState);

            if (coachUpdate.Id != coachId)
                return BadRequest("coachId not matched with record's coachId");

            if (!_unitOfWork.Coach.HasCoach(coachId))
                return NotFound("playerId doesn't exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var coachMapped = _mapper.Map<Coach>(coachUpdate);

            if (!_unitOfWork.Coach.UpdateEntity(coachMapped))
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
        public IActionResult DeleteCoach([FromQuery] int coachId)
        {
            if (!_unitOfWork.Coach.HasCoach(coachId))
                return NotFound("coachId doesn't exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var coach = _unitOfWork.Coach.GetCoach(coachId);

            if (!_unitOfWork.Coach.DeleteEntity(coach))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }

        [HttpDelete("player_IDs")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeletePlayersFromCoach([FromQuery] int coachId, [FromQuery] IList<int> player_IDs)
        {
            if (!_unitOfWork.Coach.HasCoach(coachId))
                return NotFound("coachId doesn't exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (player_IDs is null)
            {
                ModelState.AddModelError("", "No playerIds where entered");
                return NotFound(ModelState);
            }

            if (!ValidatePlayers(player_IDs))
                return StatusCode(422, ModelState);

            if (!_unitOfWork.Coach.AllPlayersMatched(coachId, player_IDs))
            {
                ModelState.AddModelError("", "Some playerIds doesn't exist in coach's players");
                return StatusCode(422, ModelState);
            }

            if (!_unitOfWork.Coach.DeletePlayersFromCoach(coachId, player_IDs))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }

        #endregion

        #region Validations

        bool ValidatePlayers(IList<CoachPlayersDto> coachPlayers)
        {
            HashSet<int> playerIds = new HashSet<int>();
            foreach(var item in coachPlayers)
            {
                if (!_unitOfWork.Player.HasPlayer(item.PlayerId))
                {
                    ModelState.AddModelError("", $"playerId ({item.PlayerId}) doesn't exist");
                    return false;
                }
                if (playerIds.Contains(item.PlayerId))
                {
                    ModelState.AddModelError("", $"Coach Players already has PlayerId ({item.PlayerId})");
                    return false;
                }
                playerIds.Add(item.PlayerId);
            }
            return true;
        }
        bool ValidatePlayers(IList<int> players)
        {
            HashSet<int> playerIds = new HashSet<int>();
            foreach (var item in players)
            {
                if (!_unitOfWork.Player.HasPlayer(item))
                {
                    ModelState.AddModelError("", $"playerId ({item}) doesn't exist");
                    return false;
                }
                if (playerIds.Contains(item))
                {
                    ModelState.AddModelError("", $"Players already has PlayerId ({item})");
                    return false;
                }
                playerIds.Add(item);
            }
            return true;
        }

        #endregion
    }
}

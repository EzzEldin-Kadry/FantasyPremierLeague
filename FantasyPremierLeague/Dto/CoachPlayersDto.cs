using FantasyPremierLeague.Models;

namespace FantasyPremierLeague.Dto
{
    public class CoachPlayersDto
    {
        public int PlayerId { get; set; }
        public PlayerDto? Player { get; set; }
    }
}

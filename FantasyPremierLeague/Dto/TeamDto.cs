using FantasyPremierLeague.Models;

namespace FantasyPremierLeague.Dto
{
    public class TeamDto
    {
        public int Id { get; set; }        
        public int Number_of_Players { get; set; }
        public string Name { get; set; }
        public ICollection<PlayerDto>? Players { get; set; }
    }
}

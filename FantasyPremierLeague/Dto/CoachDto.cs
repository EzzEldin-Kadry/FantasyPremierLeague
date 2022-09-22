using FantasyPremierLeague.Models;

namespace FantasyPremierLeague.Dto
{
    public class CoachDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Points { get; set; }
        public int? TotalPoints { get; set; }
        public IList<CoachPlayersDto>? CoachPlayers { get; set; }
    }
}

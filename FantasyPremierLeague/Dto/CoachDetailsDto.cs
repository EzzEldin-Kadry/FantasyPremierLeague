using FantasyPremierLeague.Models;

namespace FantasyPremierLeague.Dto
{
    public class CoachDetailsDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Points { get; set; }
        public int? TotalPoints { get; set; }
    }
}

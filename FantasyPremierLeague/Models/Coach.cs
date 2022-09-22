using System.ComponentModel.DataAnnotations;

namespace FantasyPremierLeague.Models
{
    public class Coach
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Points { get; set; }
        public int? TotalPoints { get; set; }
        public IList<CoachPlayers>? CoachPlayers { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyPremierLeague.Models
{
    public class Statistics
    {
        [Key]
        public int Id { get; set; }
        public int? TotalPoints { get; set; } = 0;
        public int? MinutesPlayed { get; set; } = 0;
        public int? GoalsScored { get; set; } = 0;
        public int? GoalsConceded { get; set; } = 0;
        public int? Assists { get; set; } = 0;
        public int? CleanSheets { get; set; } = 0;
        public int? Saves { get; set; } = 0;
        public int? PlayerId { get; set; }
        public Player Player { get; set; }
    }
}

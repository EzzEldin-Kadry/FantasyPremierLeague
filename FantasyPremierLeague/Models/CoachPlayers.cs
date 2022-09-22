using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyPremierLeague.Models
{
    public class CoachPlayers
    {
        public int CoachId { get; set; }
        public Coach? Coach { get; set; }
        public int PlayerId { get; set; }
        public Player? Player { get; set; }
    }
}

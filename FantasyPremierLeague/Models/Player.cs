using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyPremierLeague.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Range(14, 60, ErrorMessage = "Enter a valid Age")]
        public int Age { get; set; } = 18;
        public int? Points { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public Statistics? Statistics { get; set; }
    }
}

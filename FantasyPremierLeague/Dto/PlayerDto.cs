using FantasyPremierLeague.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FantasyPremierLeague.Dto
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Range(14, 60, ErrorMessage = "Enter a valid Age")]
        public int Age { get; set; } = 18;
        public int? Points { get; set; }
        public int? TeamId { get; set; }
        public string? TeamName { get; set; }
        public StatisticsDto? Statistics { get; set; }
    }
}

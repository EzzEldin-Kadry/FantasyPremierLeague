using AutoMapper;
using FantasyPremierLeague.Dto;
using FantasyPremierLeague.Models;

namespace FantasyPremierLeague.Utility
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Statistics, StatisticsDto>().ReverseMap();

            CreateMap<Player, PlayerDto>()
               .ForMember(x => x.TeamId, opt => opt.MapFrom(x => x.Team.Id))
               .ForMember(x => x.TeamName, opt => opt.MapFrom(x => x.Team.Name))
               .ForMember(x => x.Statistics, opt => opt.MapFrom(x => x.Statistics));

            CreateMap<Team, TeamDto>()
                .ForMember(x => x.Number_of_Players, opt => opt.Ignore())
                .ForMember(x => x.Number_of_Players, opt => opt.MapFrom(x => x.Players.Count))
                .ForMember(x => x.Players, opt => opt.MapFrom(x => x.Players)).ReverseMap();

            CreateMap<CoachPlayers, CoachPlayersDto>()
                .ForMember(x => x.Player, opt => opt.MapFrom(x => x.Player)).ReverseMap();

            CreateMap<Coach, CoachDto>()
                .ForMember(x => x.CoachPlayers, opt => opt.MapFrom(x => x.CoachPlayers)).ReverseMap();

            CreateMap<PlayerDto, Player>();
            CreateMap<CoachDetailsDto, Coach>().ReverseMap();
            CreateMap<TeamDetailsDto, Team>();

        }
    }
}

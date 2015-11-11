using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntities;

namespace LogicInterfaces
{
    public interface ITeamManipulations
    {
        List<TeamType> GetTeams();
        List<TeamType> GetTeamsforGame(GameType game);
        bool AddOrUpdateTeam(TeamType team);
        List<GameType> GetGamesForTeam(TeamType team);
        List<MatchType> GetMatchesForTeam(TeamType team);
        List<MatchType> GetGameMatchesForTeam(GameType game, TeamType team);
    }
}

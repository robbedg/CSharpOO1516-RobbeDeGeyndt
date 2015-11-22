using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntities;
using DataAccessInterfaces;
using DataAccessImplementation;

namespace LogicImplementation
{
    public class TeamManipulations : ITeamManipulations
    {
        IGameRankingDataAccess grda = new GameRankingDataAccess();
        public bool AddOrUpdateTeam(TeamType team)
        {
            if (grda.Teams.Contains(team))
            {
                grda.Teams.Remove(team);
            }
           grda.Teams.Add(team);
            return true;
        }

        public List<MatchType> GetGameMatchesForTeam(GameType game, TeamType team)
        {
            throw new NotImplementedException();
        }

        public List<GameType> GetGamesForTeam(TeamType team)
        {
            throw new NotImplementedException();
        }

        public List<MatchType> GetMatchesForTeam(TeamType team)
        {
            throw new NotImplementedException();
        }

        public List<TeamType> GetTeams()
        {
            return grda.Teams;
        }

        public List<TeamType> GetTeamsforGame(GameType game)
        {
            throw new NotImplementedException();
        }
    }
}

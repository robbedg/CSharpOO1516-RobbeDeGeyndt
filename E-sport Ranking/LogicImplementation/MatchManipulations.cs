using DataAccessImplementation;
using DataAccessInterfaces;
using DataEntities;
using LogicInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicImplementation
{
    public class MatchManipulations : IMatchManipulations
    {
        IGameRankingDataAccess grda = new GameRankingDataAccess();

        public List<MatchType> GetMatches(GameType game, ParticipantTypes soloOrTeam, MatchCategories matchCategory)
        {
            List<MatchType> filtered = new List<MatchType>();
            filtered = grda.MatchList;

            for (int i = 0; i < grda.MatchList.Count; i++)
            {
                if (grda.MatchList[i].GameID.Equals(game) && grda.MatchList[i].Category.Equals(matchCategory))
                {
                    //solo or team?
                }
            }


        }
        public List<MatchType> GetMatchesAll(GameType game)
        {
            return grda.MatchList;
        }
        public void AddOrUpdateSoloMatch(SoloMatch match)
        {

        }
        public void AddOrUpdateTeamMatch(TeamMatch match)
        {

        }
    }
}

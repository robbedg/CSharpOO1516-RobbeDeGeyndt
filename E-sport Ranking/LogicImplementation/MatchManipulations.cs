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
                    if ((grda.MatchList[i] is SoloMatch) && (soloOrTeam == ParticipantTypes.Solo))
                    {
                        filtered.Add(grda.MatchList[i]);
                    }
                    else if ((grda.MatchList[i] is TeamMatch) && (soloOrTeam == ParticipantTypes.Team))
                    {
                        filtered.Add(grda.MatchList[i]);
                    }
                    else if (soloOrTeam == ParticipantTypes.All)
                    {
                        filtered.Add(grda.MatchList[i]);
                    }
                    
                }
               
            }
            return filtered;

        }
        public List<MatchType> GetMatchesAll(GameType game)
        {
            List<MatchType> matches = new List<MatchType>();
            for (int i = 0; i < grda.MatchList.Count; i++) {
                if (grda.MatchList[i].GameID.Equals(game))
                {
                    matches.Add(grda.MatchList[i]);
                }
            }
            return matches;
        }
        public void AddOrUpdateSoloMatch(SoloMatch match)
        {
            grda.MatchList.Add(match);
            grda.SubmitmatchListChanges();
        }
        public void AddOrUpdateTeamMatch(TeamMatch match)
        {
            grda.MatchList.Add(match);
            grda.SubmitmatchListChanges();
        }
    }
}

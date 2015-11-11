using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntities;


namespace LogicInterfaces
{
    public interface IMatchManipulations
    {
        List<MatchType> GetMatches(GameType game, ParticipantTypes soloOrTeam, MatchCategories matchCategory);
        List<MatchType> GetMatchesAll(GameType game);
        void AddOrUpdateSoloMatch(SoloMatch match);
        void AddOrUpdateTeamMatch(TeamMatch match);
    }
}

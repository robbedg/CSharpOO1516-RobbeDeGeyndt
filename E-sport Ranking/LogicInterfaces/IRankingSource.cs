using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntities;

namespace LogicInterfaces
{
    public interface IRankingSource
    {
        List<PlayerGameRankingType> GetGameRankingsAll(GameType game, ParticipantTypes soloOrTeam);
        List<PlayerGameRankingType> GetGameRankings(GameType game, ParticipantTypes soloOrTeam, Ranks rank);
    }
}

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
    public class RankingSource : IRankingSource
    {
        IGameRankingDataAccess grda = new GameRankingDataAccess();
        public List<PlayerGameRankingType> GetGameRankings(GameType game, ParticipantTypes soloOrTeam, Ranks rank)
        {
            throw new NotImplementedException();
        }

        public List<PlayerGameRankingType> GetGameRankingsAll(GameType game, ParticipantTypes soloOrTeam)
        {
            throw new NotImplementedException();
        }
    }
}

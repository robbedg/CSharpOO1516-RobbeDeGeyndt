using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntities;

namespace DataAccessInterfaces
{
    public interface IGameRankingDataAccess
    {
        List<GameType> Games { get; set; }
        List<PlayerType> Players { get; set; }
        List<TeamType> Teams { get; set; }
        List<MatchType> MatchList { get; set; }
        List<PlayerGameRankingType> RankingList { get; set; }

        void SubmitGameListChanges();
        void SubmitPlayerListChanges();
        void SubmitTeamListChanges();
        void SubmitmatchListChanges();
        void SubmitRankingListChanges();

	// for testing purposes
	void ClearAllData();
    }
}

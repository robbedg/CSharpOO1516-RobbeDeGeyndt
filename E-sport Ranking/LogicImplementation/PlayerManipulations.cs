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
    public class PlayerManipulations : IPlayerManipulations

    {
        IGameRankingDataAccess grda = new GameRankingDataAccess();
        public void AddOrUpdatePlayer(PlayerType player)
        {
            if (grda.Players.Contains(player)) {
                grda.Players.Remove(player);
            }
            grda.Players.Add(player);
            grda.SubmitPlayerListChanges();
        }

        public List<MatchType> GetGameMatchesForPlayer(GameType game, PlayerType player)
        {
            throw new NotImplementedException();
        }

        public List<GameType> GetGamesForPlayer(PlayerType player)
        {
            throw new NotImplementedException();
        }

        public List<MatchType> GetMatchesForPlayer(PlayerType player)
        {
            throw new NotImplementedException();
        }

        public List<PlayerType> GetPlayers()
        {
            return grda.Players;
        }

        public List<PlayerType> GetPlayersForgame(GameType game)
        {
            throw new NotImplementedException();
        }
    }
}

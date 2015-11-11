using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntities;

namespace LogicInterfaces
{
    public interface IPlayerManipulations
    {
        List<PlayerType> GetPlayers();
        List<PlayerType> GetPlayersForgame(GameType game);
        void AddOrUpdatePlayer(PlayerType player);
        List<GameType> GetGamesForPlayer(PlayerType player);
        List<MatchType> GetMatchesForPlayer(PlayerType player);
        List<MatchType> GetGameMatchesForPlayer(GameType game, PlayerType player);
    }
}

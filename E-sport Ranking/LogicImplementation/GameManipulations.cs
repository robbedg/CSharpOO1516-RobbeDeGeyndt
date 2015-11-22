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
    public class GameManipulations : IGameManipulations
    {
        IGameRankingDataAccess grda = new GameRankingDataAccess();

        public List<GameType> GetGames()
        {
            return grda.Games;
        }
        public void AddOrUpdateGame(GameType game)
        {
            if (grda.Games.Contains(game))
            {
                grda.Games.Remove(game);
            }
            grda.Games.Add(game);
            grda.SubmitGameListChanges();
        }
    }
}

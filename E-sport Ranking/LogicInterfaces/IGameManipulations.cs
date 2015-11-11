using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataEntities;

namespace LogicInterfaces
{
    public interface IGameManipulations
    {
        List<GameType> GetGames();
        void AddOrUpdateGame(GameType game);
    }
}

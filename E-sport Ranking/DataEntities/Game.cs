using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntities
{
    public abstract class Game
    {
        private String name { get; set; }
        private String category { get; set; }
        public Game(String name, String category)
        {
            this.name = name;
            this.category = category;
        }
        public Game() { }
    }

    public class GameSingle : Game
    {
        private List<Player> players { get; set; }
    }

    public class GameTeam : Game
    {
        private List<Team> teams { get; set; }
    }
}

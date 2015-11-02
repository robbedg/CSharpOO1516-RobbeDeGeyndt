using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntities
{
    public class Team
    {
        private String name { get; set; }
        private List<Player> players { get; set; }
        public Team(String name, List<Player> players)
        {
            this.name = name;
            this.players = players;
        }
    }
}

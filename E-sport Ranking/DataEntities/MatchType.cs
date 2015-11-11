using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntities
{
    public abstract class MatchType : IEquatable<MatchType>
    {
        public MatchCategories Category { get; set; }
        public DateTime dateTime { get; set; }
        public GameType GameID { get; set; }

        public static bool operator ==(MatchType a, MatchType b)
        {
            return ((a.Category == b.Category) && (a.dateTime == b.dateTime) && (a.GameID == b.GameID));
        }

        public static bool operator !=(MatchType a, MatchType b)
        {
            return !(a == b);
        }

        public bool Equals(MatchType other)
        {
            return (this == other);
        }

        public override bool Equals(object other)
        {
            if (!(other is MatchType))
            {
                return false;
            }
            else
            {
                return (this.Equals((MatchType)other));
            }
        }

        public override string ToString()
        {
            return $"{this.GameID} - {this.Category} - {this.dateTime}";
        }
    }

    public class TeamMatch : MatchType
    {
        public List<int> Scores { get; set; }
        public List<TeamType> Teams { get; set; }
    }

    public class SoloMatch : MatchType
    {
        public List<PlayerType> Players { get; set; }
        public List<int> Scores { get; set; }
    }
}

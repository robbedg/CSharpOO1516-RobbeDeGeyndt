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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TeamMatch a, TeamMatch b)
        {
            bool testa = a.Category == a.Category;
            bool testb = a.dateTime == b.dateTime;
            bool testc = a.GameID == b.GameID;
            bool testd = a.Scores.All(b.Scores.Contains);
            bool teste = a.Teams.All(b.Teams.Contains);

            return testa && testb && testc && testd && teste;
        }

        public static bool operator !=(TeamMatch a, TeamMatch b)
        {
            return !(a == b);
        }

        public override bool Equals(object other)
        {
            if (!(other is TeamMatch))
            {
                return false;
            }
            else
            {
                return (this.Equals((TeamMatch)other));
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class SoloMatch : MatchType
    {
        public List<PlayerType> Players { get; set; }
        public List<int> Scores { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(SoloMatch a, SoloMatch b)
        {
            bool testa = a.Category == a.Category;
            bool testb = a.dateTime == b.dateTime;
            bool testc = a.GameID == b.GameID;
            bool testd = a.Players.All(b.Players.Contains);
            bool teste = a.Scores.All(b.Scores.Contains);

            return testa && testb && testc && testd && teste;
        }

        public static bool operator !=(SoloMatch a, SoloMatch b)
        {
            return !(a == b);
        }

        public override bool Equals(object other)
        {
            if (!(other is SoloMatch))
            {
                return false;
            }
            else
            {
                return (this.Equals((SoloMatch)other));
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

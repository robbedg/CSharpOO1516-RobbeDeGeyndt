using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntities
{
    public struct PlayerGameRankingType : IEquatable<PlayerGameRankingType>, IComparable<PlayerGameRankingType>
    {
        public GameType Game { get; set; }
        public PlayerType Player { get; set; }
        public int Points { get; set; }
        public Ranks Ranking { get; set; }

        public static bool operator ==(PlayerGameRankingType a, PlayerGameRankingType b)
        {
            return ((a.Game == b.Game) && (a.Player == b.Player) && (a.Points == b.Points));
        }

        public static bool operator !=(PlayerGameRankingType a, PlayerGameRankingType b)
        {
            return !(a == b);
        }

        public bool Equals(PlayerGameRankingType other)
        {
            return (this == other);
        }

        public override bool Equals(object other)
        {
            if (!(other is PlayerGameRankingType))
            {
                return false;
            }
            else
            {
                return (this.Equals((PlayerGameRankingType)other));
            }
        }

        public override string ToString()
        {
            return $"{this.Player} - {this.Game} - {this.Ranking} - {this.Points}";
        }

        public int CompareTo(PlayerGameRankingType other)
        {
            if (!(this.Game.Equals(other.Game)))
            {
                throw new ArgumentException("GameType doesn't match.");
            }

            if (this.Points > other.Points)
            {
                return -1;
            }
            else if (this.Points < other.Points)
            {
                return 1;
            }

            return 0;
        }
    }
}

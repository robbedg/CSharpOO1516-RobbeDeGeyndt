using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntities
{
    public struct GameType : IEquatable<GameType>
    {
        public String Name { get; set; }
        public ParticipantTypes ParticipantType { get; set; }

        public static bool operator ==(GameType a, GameType b)
        {
            return ((a.ParticipantType == b.ParticipantType) && (a.Name == b.Name));
        }

        public static bool operator !=(GameType a, GameType b)
        {
            return !(a == b);
        }

        public bool Equals(GameType other)
        {
            return (this == other);
        }

        public override bool Equals(object other)
        {
            if (!(other is GameType))
            {
                return false;
            }
            else
            {
                return (this.Equals((GameType)other));
            }
        }

        public override string ToString()
        {
            return $"{this.Name} - {this.ParticipantType}";
        }
    }
}

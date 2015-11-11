using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntities
{
    public struct PlayerType : IEquatable<PlayerType>
    {
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }


        public static bool operator ==(PlayerType a, PlayerType b)
        {
            return ((a.Mail == b.Mail)&&(a.Name == b.Name)); 
        }

        public static bool operator !=(PlayerType a, PlayerType b)
        {
            return !(a== b);
        }
        
        public bool Equals(PlayerType other)
        {
            return (this == other);
        }

        public override bool Equals(object other)
        {
            if (!(other is PlayerType))
            {
                return false;
            }
            else
            {
                return (this.Equals((PlayerType)other));
            }
        }

        public override string ToString()
        {
            return $"{this.Name} - {this.Mail} - {this.Tag}";
        }
    }
}

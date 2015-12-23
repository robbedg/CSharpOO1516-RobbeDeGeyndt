using LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicImplementation
{
    class MD5CollisionCalculator : IMD5CollisionCalculator
    {
        public int NrOfWorkerTasks { get; set; }

        public event Action<string> CollisionFound;
        public event Action<decimal> ProgressChanged;

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void StartCalculatingMD5Collision(string hash, int passwordLength)
        {
            throw new NotImplementedException();
        }
    }
}

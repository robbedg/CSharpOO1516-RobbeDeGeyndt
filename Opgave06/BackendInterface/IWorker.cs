using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicInterface;
using GlobalTools;

namespace BackendInterface
{
    public interface IWorker
    {
        void GetCollisions(string hash, ConcurrentQueue<string> buffer);

        /// <summary>
        ///  Stop working and cancel task;
        /// </summary>
        void Abort();

        /// <summary>
        /// returns nr Of Combinations tested, called aprox.every 50 msec
        /// </summary>
        event Action<ulong> ProgressChanged;
        
        /// <summary>
        /// returns a found collision
        /// </summary>
        event Action<string> CollisionFound;

        /// <summary>
        /// signals buffer is empty, thread will sleep for 1..5 ms
        /// </summary>
        event Action Stalled;

    }
}

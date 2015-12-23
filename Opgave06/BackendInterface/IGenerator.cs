using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendInterface
{
    public interface IGenerator
    {
        /// <summary>
        /// Starts generating 'all capital alfa' passwords and
        /// passing them into the Queue thatis returned
        /// </summary>
        /// <param name="passWordLength">
        /// length of passwords generated in chars
        /// </param>
        /// /// <param name="maxQueueLength">
        /// Max capacity of the Queue
        /// </param>
        ConcurrentQueue<string> Start(int passWordLength, int maxQueueLength);

        /// <summary>
        /// Aborts password generation
        /// </summary>
        void Abort();

        /// <summary>
        /// Aborts password geenration (if active) and cancel task.
        /// </summary>
        void Close();

        /// <summary>
        /// returns total number of password combinations for the requested length
        /// </summary>
        /// <returns></returns>
        ulong MaxCount();

        /// <summary>
        /// returns total nr Of passwords generated since start, called aprox.every 50 msec
        /// </summary>
        event Action<ulong> ProgressChanged;

        /// <summary>
        /// signals generator has stopped because all passwords are generated
        /// </summary>
        event Action GeneratorFinished;

        /// <summary>
        /// signals generator has stalled because buffer is filled to capacity, will sleep for aprox. 1-5 ms
        /// </summary>
        event Action Stalled;

    }
}

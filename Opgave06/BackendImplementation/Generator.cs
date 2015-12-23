using BackendInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using GlobalTools;

namespace BackendImplementation
{
    class Generator : IGenerator
    {
        public event Action GeneratorFinished;
        public event Action<ulong> ProgressChanged;
        public event Action Stalled;

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public ulong MaxCount()
        {
            throw new NotImplementedException();
        }

        public ConcurrentQueue<string> Start(int passWordLength, int maxQueueLength)
        {
            object signal = new object();
            ConcurrentQueue<string> q = new ConcurrentQueue<string>();

            PasswordGenerator pg = new PasswordGenerator(passWordLength);

            foreach (string val in pg)
            {
                q.Enqueue(val);
            }

            lock (signal)
            {
                while (q.Count > maxQueueLength);
            }
            return q;
        }
    }
}

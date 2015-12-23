using BackendInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using GlobalTools;
using System.Threading;

namespace BackendImplementation
{
    public class Generator : IGenerator
    {
        PasswordGenerator pg = null;

        public ConcurrentQueue<string> qeue { get; set; }

        private bool stop = false;

        public event Action GeneratorFinished;
        public event Action<ulong> ProgressChanged;
        public event Action Stalled;

        public void Abort()
        {
            stop = true;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public ulong MaxCount()
        {
            return pg.Count();
        }

        public void Start(int passWordLength, int maxQueueLength)
        {
            pg = new PasswordGenerator(passWordLength);
            object signal = new object();
            qeue = new ConcurrentQueue<string>();

            

            foreach (string val in pg)
            {
                if (stop)
                {
                    break;
                }
                qeue.Enqueue(val);

                lock (signal)
                {
                    while (qeue.Count > maxQueueLength)
                    {
                        Thread.Sleep(5);
                    }
                }

            }

        }
    }
}

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
    public class Worker : IWorker
    {
        public event Action<string> CollisionFound;
        public event Action<decimal> ProgressChanged;
        public event Action Stalled;
        private string hashwoord;

        private bool stop = false;

        public void Abort()
        {
            stop = true;
        }

        public void GetCollisions(string hash, ConcurrentQueue<string> buffer)
        {
            decimal i = 0;
            do
            {
                if (buffer == null)
                {
                    Thread.Sleep(5);
                }
                else
                {
                    string woord;
                    bool test = buffer.TryDequeue(out woord);

                    if (!test)
                    {
                        //Stalled();
                        Thread.Sleep(5);
                    }

                    

                    hashwoord = MD5Calculator.GetHash(woord);
                    Console.WriteLine(hashwoord);
                    
                    if (hash.Equals(hashwoord))
                    {
                        
                        CollisionFound(woord);
                        
                    }
                    i++;
                    ProgressChanged(i);
                }
            } while (!stop);
        }
    }
}

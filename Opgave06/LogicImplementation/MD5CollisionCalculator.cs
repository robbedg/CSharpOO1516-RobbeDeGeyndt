using BackendInterface;
using BackendImplementation;
using LogicInterface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LogicImplementation
{
    public class MD5CollisionCalculator : IMD5CollisionCalculator
    {
        IGenerator generator = new Generator();
        IWorker worker = new Worker();

        public int NrOfWorkerTasks { get; set; }

        public event Action<string> CollisionFound;
        public event Action<decimal> ProgressChanged;

        public void Abort()
        {
            worker.Abort();
            generator.Abort();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void StartCalculatingMD5Collision(string hash, int passwordLength)
        {
         
            //quesize
            Task GeneratorResult = Task.Run(() => { generator.Start(passwordLength, 250); });

            Thread.Sleep(50);

            Task workerTask = Task.Run(() => { worker.GetCollisions(hash, generator.qeue); });
            worker.CollisionFound += CollisionFound;
            try
            {
                workerTask.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //worker.GetCollisions(hash, results);
        }
    }
}

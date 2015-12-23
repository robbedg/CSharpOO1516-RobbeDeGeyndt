using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalTools;
using BackendInterface;

namespace DemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string passWord = "ABCDEFGH";
            String hash = MD5Calculator.GetHash(passWord);
            Console.WriteLine($"Hash voor '{passWord}' = {hash}");

            var generator = new PasswordGenerator(8);
            Console.WriteLine($"\nAantal paswoorden van 8 hoofdletters: {generator.Count():N0}");
            Console.WriteLine($"\nEerste 10 paswoorden van 8 hoofdletters:\n");

            int count = 0;
            foreach (var password in generator)
            {
                Console.WriteLine($"{password}");
                count++;
                if (count == 10) break;
            }
            

            Console.WriteLine("\n\nPress <enter> to end");
            Console.ReadLine();


        }
    }
}

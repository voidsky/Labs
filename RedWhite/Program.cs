using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedWhite
{
    class Program
    {
        static public void WhiteThread(RedWhiteSynchronizer synchronizer)
        {
            synchronizer.StartWhite();

            for (int i = 0; i < 100; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("W ");
                Task.Delay(100);
            }

            synchronizer.StopWhite();
        }

        static public void RedThread(RedWhiteSynchronizer synchronizer)
        {
            synchronizer.StartRed();

            for (int i = 0; i < 100; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("R ");
                Task.Delay(100);
            }

            synchronizer.StopRed();
        }

        static void Main(string[] args)
        {
            RedWhiteSynchronizer rw = new RedWhiteSynchronizer();

            Console.WriteLine("Unsynchronized threads:");
            List<Task> tasks = new List<Task>();
            rw.Enabled = false;
            for (int i = 0; i < 100; i++)
                if (i%2==0)
                    tasks.Add(Task.Run(() => WhiteThread(rw)));
                else
                    tasks.Add(Task.Run(() => RedThread(rw)));

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("\nPress enter to do synchronized tasks:");
            Console.ReadLine();

            rw.Enabled = true;
            tasks = new List<Task>();
            rw.Enabled = true;
            for (int i = 0; i < 100; i++)
                if (i % 2 == 0)
                    tasks.Add(Task.Run(() => WhiteThread(rw)));
                else
                    tasks.Add(Task.Run(() => RedThread(rw)));

            Task.WaitAll(tasks.ToArray());
            Console.ReadLine();

        }
    }
}

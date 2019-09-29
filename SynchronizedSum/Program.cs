using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizedSum
{
    class Program
    {
        const int totalItemsToSum = 100_000_001;
        const int splitSize = 100_000;

        static int[] items = Enumerable.Range(0, totalItemsToSum).ToArray();
        static int[] items2 = Enumerable.Range(0, totalItemsToSum).ToArray();
        static int[] itemsSum = new int[totalItemsToSum];

        //!!!
        static long sharedSum = 0;

        static object sharedSumLock = new object();

        static void SingleThreadedSum()
        {
            for (int i = 0; i < totalItemsToSum; i++)
            {
                itemsSum[i] = items[i] + items2[i];
                sharedSum += itemsSum[i];
            }
        }
        static void RangeSum(int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                // gijos nesikerta ir pasidalina pasyva
                itemsSum[i] = items[i] + items2[i];
                // taciau sharedSum sumuojasi nekorektiskai nes gijos nesinchrinzuoja
                // skaitymo/rasimo
                sharedSum += itemsSum[i];
            }
        }
        static void RangeSumLocked(int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i < toIndex; i++)
            {
                itemsSum[i] = items[i] + items2[i];
                // kiekviena kart sumuojant naudojam lock`a,
                // klaidos nebera, taciau labai neefektyvu
                lock (sharedSumLock)
                {
                    sharedSum += itemsSum[i];
                }
            }
        }
        static void RangeSumLockedClever(int fromIndex, int toIndex)
        {
            long localSum = 0;

            for (int i = fromIndex; i < toIndex; i++)
            {
                itemsSum[i] = items[i] + items2[i];
                localSum += itemsSum[i];
            }
            // sumavima atliekame lokaliai i lokalu kintamaji,
            // o lockinam tik rezultato sumavima i globalu kintamaji
            lock (sharedSumLock)
            {
                sharedSum = sharedSum + localSum;
            }
        }
        static void RangeSumInterlockedAdd(int fromIndex, int toIndex)
        {
            long localSum = 0;

            for (int i = fromIndex; i < toIndex; i++)
            {
                itemsSum[i] = items[i] + items2[i];
                localSum += itemsSum[i];
            }
            // Viskas tas pats kaip ir su liocked tik naudojama Interlocked
            // klase kuri sumavima alieka atominiam lygmenyje ir veikia daug efektyviau 
            Interlocked.Add(ref sharedSum, localSum);
        }

        static void LaunchSummingThreads(Action<int, int> someAction)
        {
            sharedSum = 0;
            itemsSum = new int[totalItemsToSum];

            List<Task> tasks = new List<Task>();
            int rangeStart = 0, rangeEnd = rangeStart + splitSize;

            while (rangeStart <= items.Length - 1)
            {
                int rs = rangeStart, re = rangeEnd;
                tasks.Add(Task.Run(() => someAction(rs, re)));
                rangeStart = rangeEnd;
                rangeEnd = rangeStart + splitSize;
                if (rangeEnd >= items.Length) rangeEnd = items.Length;
            }
            Task.WaitAll(tasks.ToArray());
        }

        static void Main(string[] args)
        {
            Action<int, int> sumMethodToUse;
            Stopwatch sw;


            Console.WriteLine($"Sumuojame viena gija...");
            sw = Stopwatch.StartNew();
            SingleThreadedSum();
            Console.WriteLine($"Rezultatas {sharedSum}, užtruko {sw.ElapsedMilliseconds}ms\n");

            Console.WriteLine($"Sumuojame naudojant {(items.Length) / splitSize} gijų, nesinchronizuotai.");
            sumMethodToUse = RangeSum;
            sw = Stopwatch.StartNew();
            LaunchSummingThreads(sumMethodToUse);
            Console.WriteLine($"Rezultatas {sharedSum}, užtruko {sw.ElapsedMilliseconds}ms\n");

            Console.WriteLine($"Sumuojame naudojant {(items.Length) / splitSize} gijų, sinchronizuotai su Lock`u.");
            sumMethodToUse = RangeSumLocked;
            sw = Stopwatch.StartNew();
            LaunchSummingThreads(sumMethodToUse);
            Console.WriteLine($"Rezultatas {sharedSum}, užtruko {sw.ElapsedMilliseconds}ms\n");

            Console.WriteLine($"Sumuojame naudojant {(items.Length) / splitSize} gijų, sinchronizuotai su GUDRESNIU Lock`u.");
            sumMethodToUse = RangeSumLockedClever;
            sw = Stopwatch.StartNew();
            LaunchSummingThreads(sumMethodToUse);
            Console.WriteLine($"Rezultatas {sharedSum}, užtruko {sw.ElapsedMilliseconds}ms\n");

            Console.WriteLine($"Sumuojame naudojant {(items.Length) / splitSize} gijų, sinchronizuotai su Interlocked klase.");
            sumMethodToUse = RangeSumInterlockedAdd;
            sw = Stopwatch.StartNew();
            LaunchSummingThreads(sumMethodToUse);
            Console.WriteLine($"Rezultatas {sharedSum}, užtruko {sw.ElapsedMilliseconds}ms\n");

            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RedWhite
{
    public class RedWhiteSynchronizer
    {
        static readonly object locker = new object();
        static int whiteThreads = 0;
        static int redThreads = 0;
        static bool enabled = true;

        public bool Enabled { get => enabled; set => enabled = value; }

        public void StartRed()
        {
            if (!Enabled) return;

            lock (locker)
            {
                while (whiteThreads>0) Monitor.Wait(locker);
                redThreads++;
                Monitor.PulseAll(locker);
            }
        }

        public void StopRed()
        {
            if (!Enabled) return;

            lock (locker)
            {
                redThreads--;
                Monitor.PulseAll(locker);
            }
        }

        public void StartWhite()
        {
            if (!Enabled) return;

            lock (locker)
            {
                while (redThreads>0) Monitor.Wait(locker);
                whiteThreads++;
                Monitor.PulseAll(locker);
            }
        }

        public void StopWhite()
        {
            if (!Enabled) return;

            lock (locker)
            {
                whiteThreads--;
                Monitor.PulseAll(locker);
            }
        }

    }
}

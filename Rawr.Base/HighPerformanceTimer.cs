using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Rawr
{
    internal class HighPerformanceTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        private long _startTime = 0;
        private long _stopTime = 0;
        private double _freqency;

        public HighPerformanceTimer()
        {
            long freq;
            QueryPerformanceFrequency(out freq);
            _freqency = (double) freq;
        }

        public void Start()
        {
            Thread.Sleep(0);
            QueryPerformanceCounter(out _startTime);
        }

        public void Stop()
        {
            QueryPerformanceCounter(out _stopTime);
            Hits++;
            TotalTime += Ticks;
        }

        public void Reset()
        {
            Hits = 0;
            TotalTime = 0;
        }

        public long TotalTime{get;set;}
        public long Hits{get;set;}

        public double Average
        {
            get
            {
                return ((double) TotalTime / Math.Max(Hits, 1)) / _freqency;
            }
        }

        public long Ticks
        {
            get
            {
                return (_stopTime - _startTime);
            }
        }

        public double Duration
        {
            get
            {
                return (_stopTime - _startTime) / _freqency;
            }
        }
    }
}
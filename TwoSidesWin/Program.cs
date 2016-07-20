using System;
using System.IO;
using System.Diagnostics;

namespace TwoSides
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static bool isStart = false;
        static Stopwatch sw = new Stopwatch();

        public static void StartSw()
        {
            if (!isStart)
            {
                isStart = true;
            }
            sw.Reset();
            sw.Start();
        }

        public static string StopSw(string funcName)
        {
            sw.Stop();
            string log = funcName + ":" + sw.ElapsedTicks.ToString() + "ticks " + sw.ElapsedMilliseconds + "ms";
            sw.Reset();
            return log;
        }
        public static Game1 game;
        static void Main(string[] args)
        {
            using (game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}


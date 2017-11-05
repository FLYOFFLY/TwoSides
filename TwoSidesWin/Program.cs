using System.Diagnostics;
using System.Globalization;

namespace TwoSides
{
#if WINDOWS || XBOX
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static bool _isStart;
        static readonly Stopwatch Sw = new Stopwatch();

        public static void StartSw()
        {
            if (!_isStart)
            {
                _isStart = true;
            }
            if(Sw.ElapsedTicks >0)Sw.Reset();
            Sw.Start();
        }

        public static string StopSw(string funcName)
        {
            Sw.Stop();
            //string log = funcName + ":" + sw.ElapsedTicks.ToString() + "ticks " + sw.ElapsedMilliseconds + "ms";

            return Sw.ElapsedTicks.ToString(CultureInfo.InvariantCulture);
        }
        public static Game1 Game;
        static void Main()
        {
            using (Game = new Game1())
            {
                Game.Run();
            }
        }
    }
#endif
}


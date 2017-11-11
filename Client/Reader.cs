using System;
using System.Threading;

namespace Coinche.Client
{
    /// <summary>
    /// Reader.
    /// </summary>
    static class Reader
    {
        /// <summary>
        /// The input thread.
        /// </summary>
        private static Thread inputThread;

        private static readonly AutoResetEvent gotInput;
        private static readonly AutoResetEvent getInput;
        private static string input;

        static Reader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);
            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private static void reader()
        {
            while (true)
            {
                getInput.WaitOne();
                input = Console.ReadLine();
                gotInput.Set();
            }
        }

        /// <summary>
        /// Tries to read a line.
        /// </summary>
        /// <returns><c>true</c>, if read line was tryed, <c>false</c> otherwise.</returns>
        /// <param name="line">Line.</param>
        /// <param name="timeOutMillisecs">Time out millisecs.</param>
        public static bool TryReadLine(out string line, int timeOutMillisecs = Timeout.Infinite)
        {
            getInput.Set();
            bool success = gotInput.WaitOne(timeOutMillisecs);
            if (success)
                line = input;
            else
                line = null;
            return success;
        }
    }
}

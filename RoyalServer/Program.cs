﻿using System;
using System.Threading;

namespace RoyalServer
{
#if WINDOWS || LINUX

public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {

            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}

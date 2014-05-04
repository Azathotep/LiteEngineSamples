using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireFlies
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game())
                game.Run();
        }
    }
}

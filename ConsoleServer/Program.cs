using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;
using System.Threading;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.StartServer();
            
            List<star> playerlist = new List<star>() {new star(null) };
            
            server.ReadMessages(playerlist);
        }
    }
}

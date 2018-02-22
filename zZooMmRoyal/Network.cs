using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMmRoyal
{
    class Client
    {
        private NetClient client;

        public void StartClient()
        {
            var config = new NetPeerConfiguration("hej");
            config.AutoFlushSendQueue = false;
            client = new NetClient(config);
            client.Start();

            string ip = "localhost";
            int port = 14242;
            client.Connect(ip, port);
        }

        public void SendMessage(string text)
        {
            NetOutgoingMessage message = client.CreateMessage(text);

            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();
        }

        public void Disconnect()
        {
            client.Disconnect("Bye");
        }
    }
}

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
        public void GetInfo(Player player, Queue<String> msglist)
        {

            while (true) {

                NetIncomingMessage info = client.ReadMessage();
                if ((info != null))
                {
                    switch (info.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                String data = info.ReadString();
                                String[] mas = data.Split();
                                if (mas[0] == "id")
                                {
                                    player._id = mas[1]; continue;
                                }
                                if (mas[1]=="Player")
                                player.Changes(data);

                                //msg dlya objects
                                //if (mas[1] == "Objects") ;
                                //drugaya super slojnaya func
                            }
                            break;

                        default:
                            break;
                    }
                }
                else //recconect
                    continue;
               
              //  System.Threading.Thread.Sleep(10);
            }
        }
        public void Disconnect()
        {
            client.Disconnect("Bye");
        }
    }
}

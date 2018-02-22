﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;

namespace RoyalServer
{
    public class Server
    {
        public NetServer server;
        public List<NetPeer> clients;

        public void StartServer()
        {
            var config = new NetPeerConfiguration("zzoomm") { Port = 14242 };
            server = new NetServer(config);
            server.Start();

            clients = new List<NetPeer>();
        }

        public void ReadMessages(List<star> playerlist)
        {
            NetIncomingMessage message;
            var stop = false;

            while (!stop)
            {
                while ((message = server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                var data = message.ReadString();
                                String[] mas= data.Split();
                                foreach (var player in playerlist)
                                {
                                    if (mas[0] == player._Type)
                                    {
                                        player.buttons.Add(mas[1]);
                                    }
                                }

                                break;
                            }
                            /* case NetIncomingMessageType.DebugMessage:
                                 Console.WriteLine(message.ReadString());
                                 break;
                             case NetIncomingMessageType.StatusChanged:
                                 Console.WriteLine(message.SenderConnection.Status);
                                 if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                                 {
                                     clients.Add(message.SenderConnection.Peer);
                                     Console.WriteLine("{0} has connected.", message.SenderConnection.Peer.Configuration.LocalAddress);
                                 }
                                 if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                                 {
                                     clients.Remove(message.SenderConnection.Peer);
                                     Console.WriteLine("{0} has disconnected.", message.SenderConnection.Peer.Configuration.LocalAddress);
                                 }
                                 break;*/
                             default:
                                 break;
                    }
                    server.Recycle(message);
                }
            }
        }
    }
}
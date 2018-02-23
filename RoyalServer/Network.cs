using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RoyalServer
{
    public class Server
    {
        public NetServer server;
        public List<NetPeer> clients;

        public void StartServer()
        {
            var config = new NetPeerConfiguration("hej") { Port = 14242 };
            server = new NetServer(config);
            server.Start();

            clients = new List<NetPeer>();
        }

        public void ReadMessages(List<PlayerS> playerlist,Texture2D Player_Texture_Std,int idcounter)
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
                        
                                if (mas[0] == "give_id")
                                {
                                    PlayerS tmpP = new PlayerS(Player_Texture_Std)
                                    {
                                        _id = idcounter.ToString(),
                                        _name = mas[1],
                                        _Size = new Vector2(0.5f, 0.5f),
                                        _Type = "Player",
                                        buttons = new PlayerS.PressedButtons(),
                                        Origin = new Vector2(Player_Texture_Std.Width / 2, Player_Texture_Std.Height / 2),
                                        _rotation = 0,

                                    };
                                    tmpP.RandPos();
                                    playerlist.Add(tmpP);
                                    idcounter++;
                                    NetOutgoingMessage inform = server.CreateMessage("id "+ Convert.ToString(idcounter - 1));
                                    server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);
                                    continue;
                                }
                                if(mas[1]== "giveINFO")
                                foreach (var player in playerlist)
                                {
                                    if (mas[0] == player._id)
                                    {
                                        //menyat bool znachenia knopok
                                                //"_id _Player _PosX _PosY Rotation"
                                            NetOutgoingMessage inform = server.CreateMessage(player._id+" Player "+
                                            Convert.ToString(playerlist.ToArray()[0]._position.X) +
                                            " " + Convert.ToString(playerlist.ToArray()[0]._position.Y+" "+
                                            Convert.ToString(player._rotation)
                                            )
                                            );
                                            server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);
                                            continue;

                                        }
                                }
                                if(mas[1]== "ButtonChange")
                                {
                                    PlayerS tmpPlayer = null;
                                    foreach (var player in playerlist)
                                    {
                                        if (mas[0] == player._id) tmpPlayer = player;
                                    }
                                    if (tmpPlayer != null) {
                                        switch (mas[2])
                                        {
                                            case "Left_D": { tmpPlayer.buttons.left = true; } break;
                                            case "Left_U": { tmpPlayer.buttons.left = false; } break;
                                            case "Right_D": { tmpPlayer.buttons.right = true; } break;
                                            case "Right_U": { tmpPlayer.buttons.right = false; } break;
                                            case "Up_D": { tmpPlayer.buttons.up = true; } break;
                                            case "Up_U": { tmpPlayer.buttons.up = false; } break;
                                            case "Down_D": { tmpPlayer.buttons.down = true; } break;
                                            case "Down_U": { tmpPlayer.buttons.down = false; } break;

                                            default:
                                                break;
                                        }
                                    }
                                }
                                if (mas[1] == "MousePos")
                                {
                                    PlayerS tmpPlayer=null;
                                    foreach (var player in playerlist)
                                    {
                                        if (mas[0] == player._id) tmpPlayer = player;
                                    }
                                    if (tmpPlayer != null) { 
                                    tmpPlayer._mPosition.X = Convert.ToSingle(mas[2]);
                                        tmpPlayer._mPosition.Y= Convert.ToSingle(mas[3]);
                                    }
                                }
                            }
                            break;
                        default:
                                 break;
                    }
                    server.Recycle(message);
                }
            }
        }
    }
}

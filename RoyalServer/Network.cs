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
                                        buttons = new List<String>(),
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
                                            Convert.ToString(player._position.X) +
                                            " " + Convert.ToString(player._position.Y+" "+
                                            Convert.ToString(player._rotation)
                                            )
                                            );
                                            server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);
                                            //send info about other players
                                            String Tmps = "" + player._id + " Objects";
                                            
                                            NetOutgoingMessage statistic = server.CreateMessage(CreateMsgAboutPlayers(Tmps, playerlist, player._id));
                                            server.SendMessage(statistic, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);
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
                                       // tmpPlayer.buttons.Add(mas[2]);
                                        tmpPlayer.MoveButtons(mas[2]);
                                       
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

        public String CreateMsgAboutPlayers(String msg, List<PlayerS> playerlist,String currentid)
        {
            int objcounter = 0;
            String toadding = "";
            foreach (var obj in playerlist)
            {
                if (obj._Type == "Player" && obj._id != currentid)
                {
                    objcounter++;
                    String s = "Other_Player ";
                    s +=obj._position.X + " " + obj._position.Y+" "+obj._rotation;
                    toadding += s;
                }
                //other obj
            }
            if (objcounter != 0)
            {
                msg += " "+objcounter.ToString()+" " + toadding;
                return msg;
            }
            else
            {
                msg += " error"; return msg;
            }
        }
    }
}

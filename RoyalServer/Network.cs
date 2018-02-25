using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RoyalServer.MOB_S;

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
        public void ReadMessagesNew(Game1 game)
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
                                String[] mas = data.Split();
                                if (mas[1] == "disconnect") {
                                    foreach (var player in game.playerlist)
                                    {
                                        if (mas[0] == player._id) player._isAlive = false;
                                    }
                                }
                                #region WaitConnections
                                if (game.currentState == ServerState.waitingPlayers)
                                {
                                    game.currentState = ServerState.lobby;
                                }
                                #endregion

                                #region LobbyMsg

                                if (game.currentState == ServerState.lobby)
                                {
                                    if (mas[0] == "give_id")
                                    {
                                        PlayerS tmpP = new PlayerS(game.Player_Texture_Std)
                                        {
                                            _id = game.idcounter.ToString(),
                                            _name = mas[1],
                                            _Size = new Vector2(0.5f, 0.5f),
                                            _Type = "Player",
                                            buttons = new PlayerS.PressedButtons(),
                                            Origin = new Vector2(game.Player_Texture_Std.Width / 2, game.Player_Texture_Std.Height / 2),
                                            _rotation = 0,

                                        };
                                        tmpP.RandPos();
                                        game.playerlist.Add(tmpP);
                                        game.idcounter++;
                                        NetOutgoingMessage inform = server.CreateMessage("id " + Convert.ToString(game.idcounter - 1));
                                        server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);
                                        continue;
                                    }
                                    if (mas[1] == "startgamepls")
                                    {
                                        game.currentState = ServerState.gameplay;
                                        continue;
                                    }
                                    if (mas[1] == "giveINFO")
                                            foreach (var player in game.playerlist)
                                            {
                                                if (mas[0] == player._id)
                                                {
                                                  
                                                    NetOutgoingMessage inform = server.CreateMessage( );
                                                    server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                                                    //send info about other players
                                                    String Tmps = CreateMsgAboutLobby(game.playerlist);
                                                    NetOutgoingMessage statistic = server.CreateMessage(Tmps);
                                                    //send playerlist in lobby
                                                    server.SendMessage(statistic, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);
                                                    continue;
                                                }
                                            }
                                }

                                #endregion

                                #region GamePlay
                                else if (game.currentState == ServerState.gameplay)
                                {
                                    if (mas[1] == "giveINFO")
                                        foreach (var player in game.playerlist)
                                        {
                                            if (mas[0] == player._id)
                                            {
                                                //menyat bool znachenia knopok
                                                //"_id _Player _PosX _PosY Rotation"
                                                NetOutgoingMessage inform = server.CreateMessage(player._id + " Player " +
                                                Convert.ToString(player._position.X) +
                                                " " + Convert.ToString(player._position.Y + " " +
                                                Convert.ToString(player._rotation)));
                                                server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                                                //send info about other players
                                                String Tmps = "" + player._id + " Objects";
                                                String Sosat = CreateMsgAboutPlayers(Tmps, game.playerlist, player._id, game.zombielist);
                                                NetOutgoingMessage statistic = server.CreateMessage(Sosat);
                                                server.SendMessage(statistic, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                                                //send info about Zombies

                                                //String TmpZombie = "" + player._id + " All_Zombie";

                                                //NetOutgoingMessage infoZombie = server.CreateMessage(CreateMsgAboutZombies(TmpZombie, zombielist, player._id));
                                                //server.SendMessage(infoZombie, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                                                continue;

                                            }
                                        }
                                    if (mas[1] == "ButtonChange")
                                    {
                                        PlayerS tmpPlayer = null;
                                        foreach (var player in game.playerlist)
                                        {
                                            if (mas[0] == player._id) tmpPlayer = player;
                                        }
                                        if (tmpPlayer != null&&tmpPlayer._isAlive)
                                        {
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
                                        PlayerS tmpPlayer = null;
                                        foreach (var player in game.playerlist)
                                        {
                                            if (mas[0] == player._id) tmpPlayer = player;
                                        }
                                        if (tmpPlayer != null && tmpPlayer._isAlive)
                                        {
                                            tmpPlayer._mPosition.X = Convert.ToSingle(mas[2]);
                                            tmpPlayer._mPosition.Y = Convert.ToSingle(mas[3]);
                                        }
                                    }
                                }
                                #endregion
                                
                            }
                            break;
                        default:
                            break;
                    }
                    server.Recycle(message);
                }
            }

        }
        public void ReadMessages(List<ZombieS> zombielist, List<PlayerS> playerlist,Texture2D Player_Texture_Std,int idcounter)
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
                                            Convert.ToString(player._position.X) +
                                            " " + Convert.ToString(player._position.Y+" "+
                                            Convert.ToString(player._rotation) ));
                                            server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                                            //send info about other players
                                            String Tmps = "" + player._id + " Objects";
                                            String Sosat = CreateMsgAboutPlayers(Tmps, playerlist, player._id, zombielist);
                                            NetOutgoingMessage statistic = server.CreateMessage(Sosat);
                                            server.SendMessage(statistic, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                                            //send info about Zombies

                                            //String TmpZombie = "" + player._id + " All_Zombie";

                                            //NetOutgoingMessage infoZombie = server.CreateMessage(CreateMsgAboutZombies(TmpZombie, zombielist, player._id));
                                            //server.SendMessage(infoZombie, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

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
        public String CreateMsgAboutLobby(List<PlayerS> playerlist)
        {
            String tmps = "";
            tmps += "PlayersCount " + playerlist.Count.ToString();
            foreach (var player in playerlist)
            {
                tmps += " Player " + player._name;
            }

            return tmps;
        }
        public String CreateMsgAboutPlayers(String msg, List<PlayerS> playerlist,String currentid, List<ZombieS> zombieslist)
        {
            int objcounter = 0;
            String toadding = "";
            foreach (var obj in playerlist)
            {
                if (obj._Type == "Player" && obj._id != currentid)
                {
                    objcounter++;
                    String s = "Other_Player ";
                    s +=obj._position.X + " " + obj._position.Y+" "+obj._rotation+" ";
                    toadding += s;
                }
                //other obj
            }
            foreach (var obj in zombieslist)
            {
                if (obj._Type == "Zombie")
                {
                    objcounter++;
                    String s = "Mob_Zombie ";
                    s += obj._position.X + " " + obj._position.Y + " " + obj._rotation + " ";
                    toadding += s;
                }
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

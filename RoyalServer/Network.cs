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
using VelcroPhysics.Utilities;

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
                                        PlayerS tmpP = new PlayerS(game.textures.Player_1,game._world)
                                        {
                                            _id = game.idcounter.ToString(),
                                            _name = mas[1],
                                            centreScreen = new Vector2(1280 / 2, 720 / 2),

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
                                                  
                                                    //NetOutgoingMessage inform = server.CreateMessage( );
                                                    //server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

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

                                                Vector2 Player_Position = ConvertUnits.ToDisplayUnits(player.body.Position);

                                                NetOutgoingMessage inform = server.CreateMessage(player._id + " Player " +
                                                Convert.ToString(Player_Position.X) +
                                                " " + Convert.ToString(Player_Position.Y + " " +
                                                Convert.ToString(player.body.Rotation)));
                                                server.SendMessage(inform, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                                                //send info about other players
                                                foreach (var pl in game.playerlist)
                                                {
                                                    pl.TakeNear(game);
                                                }
                                                String Tmps = "" + player._id + " Objects";
                                                String Sosat = CreateMsgAboutPlayers(Tmps, game, player._id);
                                                NetOutgoingMessage statistic = server.CreateMessage(Sosat);
                                                server.SendMessage(statistic, message.SenderConnection, NetDeliveryMethod.ReliableUnordered);

                                                
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
                                            Vector2 mRotation = new Vector2(Convert.ToSingle(mas[2]), Convert.ToSingle(mas[3]));
                                            tmpPlayer.currentMouseState = mRotation;
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
        public String CreateMsgAboutPlayers(String msg,Game1 game,String currentid)
        {
            int objcounter = 0;
            String toadding = "";
            foreach (var Item in game.playerlist)
            {
                if(Item._id == currentid)
                {
                    foreach (var obj in Item.nearobj)
                    {
                        if ((string)obj.body.UserData == "Player")
                        {
                            objcounter++;
                            String s = "Other_Player ";

                            s += ConvertUnits.ToDisplayUnits(obj.body.Position.X) + " " + ConvertUnits.ToDisplayUnits(obj.body.Position.Y) + " " + obj.body.Rotation +  " ";
                            toadding += s;
                        }
                        //other obj
                    }
                    foreach (var obj in Item.nearobj)
                    {
                        if ((string)obj.body.UserData == "Zombie")
                        {
                            objcounter++;
                            String s = "Mob_Zombie ";
                            s += ConvertUnits.ToDisplayUnits(obj.body.Position.X) + " " + ConvertUnits.ToDisplayUnits(obj.body.Position.Y) + " " + obj.body.Rotation + " ";
                            toadding += s;
                        }
                    }
                    foreach (var item in Item.nearobj)
                    {
                        #region GameObject
                        if ((string)item.body.UserData == "Box_2")
                        {

                            objcounter++;
                            String s = "Box_2 ";
                            s += ConvertUnits.ToDisplayUnits(item.body.Position.X) + " " + ConvertUnits.ToDisplayUnits(item.body.Position.Y) + " " + item.body.Rotation + " ";
                            toadding += s;
                        }
                        #endregion

                    }

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
    public enum ServerState
    {
        waitingPlayers,
        lobby,
        gameplay
    }
}

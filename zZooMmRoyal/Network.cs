using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMmRoyal
{
    public class Client
    {
        private NetClient client;

        public void StartClient()
        {
            var config = new NetPeerConfiguration("hej");
            config.AutoFlushSendQueue = false;
            client = new NetClient(config);
            client.Start();
            //ip host 31.132.141.5

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
        public void GetInfoNew(Game1 game)
        {

            while (true)
            {

                NetIncomingMessage info = client.ReadMessage();
                if ((info != null))
                {
                    switch (info.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                String data = info.ReadString();

                                String[] mas = data.Split();
                                //server send " " msg, for what?????
                                if (mas.Length == 1) continue;
                                #region MenuState(no msg)
                                if (game.currentState == ClientState.mainmenu)
                                {
                                    continue;
                                }
                                #endregion

                                #region LobbyState
                                else if (game.currentState == ClientState.lobby)
                                {
                                    if (mas[0] == "id")
                                    {
                                        game.player._id = mas[1]; continue;
                                    }
                                    //catch lobby info
                                    int currentSdvig = 3;
                                    if (mas[0]== "PlayersCount")
                                    {
                                        game.LobbyPlayersList.Clear();
                                        for (int i = 0; i < Convert.ToInt32(mas[1]); i++)
                                        {
                                            String tmp = "";
                                            tmp += mas[currentSdvig];
                                            game.LobbyPlayersList.Add(tmp);
                                            currentSdvig += 2;
                                        }
                                        continue;
                                    }
                                }
                                #endregion

                                #region GamePlayState
                                else if (game.currentState == ClientState.gameplay)
                                {
                                    if (mas[1] == "Player")
                                        game.player.Changes(data);


                                    //msg dlya objects
                                    if (mas[1] == "Objects")
                                    {
                                        if (mas[2] != "error")
                                        {
                                            game.objlist.Clear();
                                            int objcountr = Convert.ToInt32(mas[2]);
                                            int currentSdvig = 3;
                                            for (int i = 0; i < objcountr; i++)
                                            {
                                                if (mas[currentSdvig] == "Mob_Zombie")
                                                {
                                                    Object tmpobj = new Object();
                                                    tmpobj._Type = mas[currentSdvig];
                                                    tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                    tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                    game.objlist.Add(tmpobj);
                                                }
                                                if (mas[currentSdvig] == "Other_Player")
                                                {
                                                    Object tmpobj = new Object();
                                                    tmpobj._Type = mas[currentSdvig];
                                                    tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                    tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                    game.objlist.Add(tmpobj);
                                                }

                                                currentSdvig += 4;
                                            }
                                        }
                                    }
                                    //msg dlya Zobie
                                    if (mas[1] == "All_Zombie")
                                    {
                                        if (mas[2] != "error")
                                        {
                                            game.objlist.Clear();
                                            int MOBcountr = Convert.ToInt32(mas[2]);
                                            int currentSdvig = 3;
                                            for (int i = 0; i < MOBcountr; i++)
                                            {
                                                if (mas[currentSdvig] == "Mob_Zombie")
                                                {
                                                    Object tmpobj = new Object();
                                                    tmpobj._Type = mas[currentSdvig];
                                                    tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                    tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                    game.objlist.Add(tmpobj);
                                                }
                                                currentSdvig += 4;
                                            }
                                        }
                                    }

                                    //drugaya super slojnaya func

                                }
                                #endregion

                            }
                            break;

                        default:
                            break;
                    }
                }
                else //recconect
                    continue;

            }
        }
        public void GetInfo(Player player, Queue<String> msglist, List<Object> objlist)
        {

            while (true)
            {

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
                                if (mas[1] == "Player")
                                    player.Changes(data);

                                //if(mas[0]== "Zombie")
                                //    zombie.Changes(data);

                                //msg dlya objects
                                if (mas[1] == "Objects")
                                {
                                    if (mas[2] != "error")
                                    {
                                        objlist.Clear();
                                        int objcountr = Convert.ToInt32(mas[2]);
                                        int currentSdvig = 3;
                                        for (int i = 0; i < objcountr; i++)
                                        {
                                            if (mas[currentSdvig] == "Mob_Zombie")
                                            {
                                                Object tmpobj = new Object();
                                                tmpobj._Type = mas[currentSdvig];
                                                tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                objlist.Add(tmpobj);
                                            }
                                            if (mas[currentSdvig] == "Other_Player")
                                            {
                                                Object tmpobj = new Object();
                                                tmpobj._Type = mas[currentSdvig];
                                                tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                objlist.Add(tmpobj);
                                            }

                                            currentSdvig += 4;
                                        }
                                    }
                                }


                                //msg dlya Zobie
                                if (mas[1] == "All_Zombie")
                                {
                                    if (mas[2] != "error")
                                    {
                                        objlist.Clear();
                                        int MOBcountr = Convert.ToInt32(mas[2]);
                                        int currentSdvig = 3;
                                        for (int i = 0; i < MOBcountr; i++)
                                        {
                                            if (mas[currentSdvig] == "Mob_Zombie")
                                            {
                                                Object tmpobj = new Object();
                                                tmpobj._Type = mas[currentSdvig];
                                                tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                objlist.Add(tmpobj);
                                            }
                                            currentSdvig += 4;
                                        }
                                    }
                                }

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

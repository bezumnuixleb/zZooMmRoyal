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
            //ip host 192.168.88.242
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
                                            game.flag = true;
                                            game.objlist.Clear();
                                           
                                            int objcountr = Convert.ToInt32(mas[2]);
                                            int currentSdvig = 3;
                                            for (int i = 0; i < objcountr; i++)
                                            {
                                                #region UnderPlayerObjects

                                                #endregion

                                                #region PlayerLevelObjects
                                                if (mas[currentSdvig] == "Mob_Zombie")
                                                {
                                                    Object tmpobj = new Object();
                                                    tmpobj._Type = mas[currentSdvig];
                                                    tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                    tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                    tmpobj._size = 0.5f;
                                                    game.objlist.Add(tmpobj);
                                                    currentSdvig += 4;
                                                }
                                                if (mas[currentSdvig] == "Other_Player")
                                                {
                                                    Object tmpobj = new Object();
                                                    tmpobj._Type = mas[currentSdvig];
                                                    tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                    tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                  
                                                    game.objlist.Add(tmpobj);
                                                    currentSdvig += 4;
                                                }
                                                if (mas[currentSdvig] == "Box_2")
                                                {
                                                    Object tmpobj = new Object();
                                                    tmpobj._Type = mas[currentSdvig];
                                                    tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                    tmpobj._rotation = Convert.ToSingle(mas[currentSdvig + 3]);
                                                    tmpobj._size =5f;
                                                    game.objlist.Add(tmpobj);

                                                    currentSdvig += 4;
                                                }
                                                if (mas[currentSdvig] == "Tile_Grass")
                                                {
                                                    Object tmpobj = new Object();
                                                    tmpobj._Type = mas[currentSdvig];
                                                    tmpobj._position = new Vector2(Convert.ToSingle(mas[currentSdvig + 1]), Convert.ToSingle(mas[currentSdvig + 2]));
                                                    tmpobj._rotation = 0f;
                                                    tmpobj._size = 1f;
                                                    //game.objlist.Add(tmpobj);
                                                }
                                                #endregion

                                           
                                            }
                                            game.flag = false;
                                        }
                                    }
                                 

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
        
        public void Disconnect()
        {
            client.Disconnect("Bye");
        }
    }
}

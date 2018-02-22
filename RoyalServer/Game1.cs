using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Lidgren.Network;
using System.Threading;
using System;

namespace RoyalServer
{
    public class Game1 : Game
    {
       
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public List<star> playerlist=new List<star>();
        public List<Object> objlist = new List<Object>();
        public List<Msg> mslist = new List<Msg>();
        // public Server server = new Server();
        public Server server = new Server();
        Texture2D text;
    
        //server.Start();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // server.StartServer();
            
            server.StartServer();
            Thread msgchecker = new Thread(() => server.ReadMessages(playerlist));
            msgchecker.Start();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //gdeto vzyal kolvo igrokov
            /*for (int i = 0; i < kolvoigrokov; i++)
            {

            }
            */

            text = Content.Load<Texture2D>("test");
            star tmpPl = new star(text) { _Type = "star" };
            tmpPl.RandPos();
            playerlist.Add(tmpPl);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //polychenie msg
            //zapysti menya v potok
           //start other thread

            
            //other thread

            foreach (var player  in playerlist)
            {
                player.Update(gameTime, objlist);
                player.buttons.Clear();
            }
            // main update

            // send positions
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(text, playerlist.ToArray()[0]._position, null, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void msgcheck(NetIncomingMessage msg, List<star> playerlist)
        {
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.ErrorMessage:
                case NetIncomingMessageType.Data:
                    {
                        var data = msg.ReadString();
                        String[] mas = data.Split();
                        foreach (var player in playerlist)
                        {
                            if (mas[0] == player._Type)
                            {
                                player.buttons.Add(mas[1]);
                            }
                        }break;
                    }
                   
                default:

                    break;
            }
        }
    }
}

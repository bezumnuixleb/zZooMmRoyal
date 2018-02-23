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
        public List<PlayerS> playerlist=new List<PlayerS>();
        public List<Object> objlist = new List<Object>();
        public List<String> mslist = new List<String>();
        public int idcounter = 1;
        // public Server server = new Server();
        public Server server = new Server();
        Texture2D Player_Texture_Std;
    
        //server.Start();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // server.StartServer();
         
            server.StartServer();
           
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

            Player_Texture_Std = Content.Load<Texture2D>("test");
            Thread msgchecker = new Thread(() => server.ReadMessages(playerlist, Player_Texture_Std, idcounter));
            msgchecker.Start();

            PlayerS tmpP = new PlayerS(Player_Texture_Std)
            {
                _id = idcounter.ToString(),
                _name = "Doven",
                _Size = new Vector2(0.5f, 0.5f),
                _Type = "Player",
                buttons = new PlayerS.PressedButtons(),
                Origin = new Vector2(Player_Texture_Std.Width / 2, Player_Texture_Std.Height / 2),
                _rotation = 0,

            };
            tmpP.RandPos();
            playerlist.Add(tmpP);
            idcounter++;
            //star tmpPl = new star(text) { _Type = "star" };
            //tmpPl.RandPos();
            //playerlist.Add(tmpPl);

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
          //  spriteBatch.Begin();
          //  spriteBatch.Draw(Player_Texture_Std, playerlist.ToArray()[0]._position, null, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 0f);
         //   spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

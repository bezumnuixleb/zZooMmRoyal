using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Lidgren.Network;
using System.Threading;

namespace RoyalServer
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public List<star> playerlist=new List<star>();
        public List<Object> objlist = new List<Object>();
        public List<Msg> mslist = new List<Msg>();
        public Server server = new Server();
       
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            Texture2D text = Content.Load<Texture2D>("test");
            star tmpPl = new star(text) { _Type = "star" };
            tmpPl.RandPos();
            playerlist.Add(tmpPl);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // polychenie msg
            //zapysti menya v potok
            server.ReadMessages(playerlist);


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


            base.Draw(gameTime);
        }
    }
}

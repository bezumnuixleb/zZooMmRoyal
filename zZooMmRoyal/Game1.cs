using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace zZooMmRoyal
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Queue<String> msglist = new Queue<String>();
        Player player;
        Texture2D text;
        Client client=new Client();
        List<Object> objlist;
        Thread msgchecker;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            client.StartClient();
            player = new Player()
            {
                _position = new Vector2(0, 0),
                _Type = "Player",
                _input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S },
                _name = "Dodek",
                _Size = new Vector2(0.5f, 0.5f),
            };
            objlist = new List<Object>();
            msgchecker = new Thread(() => client.GetInfo(player, msglist,objlist));
            msgchecker.Start();
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            text = Content.Load<Texture2D>("test");
            player._texture = text;
            player.Origin = new Vector2(player._texture.Width / 2, player._texture.Height / 2);
            client.SendMessage("give_id " + player._name);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // client.GetInfo(player);
            //give coords
            //zagruzka s servera
         
            client.SendMessage(player._id + " " + "giveINFO"); 
            player.Update(gameTime, objlist, Keyboard.GetState(), msglist);

            foreach (var msg in msglist)
            {
                if(player._id!=null)
                client.SendMessage(msg);

            }
            msglist.Clear();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            msgchecker.Suspend();
            foreach (var obj in objlist)
            {
                if (obj._Type == "Other_Player")
                {
                    obj._texture = text;
                    obj.Origin = new Vector2(obj._texture.Width / 2, obj._texture.Height / 2);

                    spriteBatch.Draw(obj._texture, obj._position, null, Color.White, obj._rotation, obj.Origin, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);

                }

            }
            msgchecker.Resume();
            // TODO: Add your drawing code here
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using zZooMmRoyal.MOB;
using zZooMmRoyal.States;

namespace zZooMmRoyal
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Queue<String> msglist = new Queue<String>();
        public Player player;
        public Texture2D text;
        public Texture2D texture_Zombie;
        public Client client =new Client();
        public List<Object> objlist;
        public Thread msgchecker;
        public List<String> LobbyPlayersList;
        public ClientState currentState = ClientState.mainmenu;
        public ClientState previousState;
        public int idchecker = 0;
        // MENU
        private State _currentState;

        private State _nextState;

        public void ChangesState(State state)
        {
            _nextState = state;
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 768;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            player = new Player()
            {
                _position = new Vector2(0, 0),
                _Type = "Player",
                _input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S },
                _name = "Dodek",
                _Size = new Vector2(0.5f, 0.5f),
                _id="null"
            };
            objlist = new List<Object>();
            //msgchecker = new Thread(() => client.GetInfo(player, msglist,objlist));
            msgchecker = new Thread(() => client.GetInfoNew(this));

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
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
            texture_Zombie = Content.Load<Texture2D>("Zombie");
            player._texture = text;
            player.Origin = new Vector2(player._texture.Width / 2, player._texture.Height / 2);

            LobbyPlayersList = new List<String>();
            //Menu
            _currentState = new MenuState(this, graphics.GraphicsDevice, Content);

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
         
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_nextState !=null)
            {
                _currentState = _nextState;

                _nextState = null;
            }

            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //MENU
            _currentState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}

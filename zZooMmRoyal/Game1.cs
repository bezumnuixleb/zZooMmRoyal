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
using zZooMmRoyal.States;

namespace zZooMmRoyal
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public Camera camera;

        public bool flag=false;
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Queue<String> msglist = new Queue<String>();
        public Player player;
        public struct TextureList
        {
            public Texture2D Player_1;
            public Texture2D Zombie_1;
            public Texture2D Box_2;
            public Texture2D Tile_1;
            public Texture2D Сannon_1;
            public Texture2D Bullet_Сannon;
        }
        public TextureList textures;
        public Client client =new Client();
        public List<Object> objlist;
        public List<Object> objlist2;
        public List<Object> backobjlist;
        public List<Object> frontobjlist;
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
            textures = new TextureList();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            player = new Player()
            {
                _position = new Vector2(0, 0),
                _Type = "Player",
                _input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S },
                _name = "Dodek",
                _id="null"
            };
            backobjlist = new List<Object>();
            objlist = new List<Object>();
            objlist2 = new List<Object>();
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
            camera = new Camera(GraphicsDevice.Viewport);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            textures.Player_1= Content.Load<Texture2D>("New_1");
            textures.Zombie_1= Content.Load<Texture2D>("Zombie");
            textures.Box_2= Content.Load<Texture2D>("graphics/level/enviroment/boxes/box_2");
            textures.Tile_1 = Content.Load<Texture2D>("graphics/level/ground/grass_tile");
            textures.Bullet_Сannon = Content.Load<Texture2D>("Bullet/bullet");
            textures.Сannon_1 = Content.Load<Texture2D>("Cannon");

            player._texture = textures.Player_1;
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
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //MENU
            _currentState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}

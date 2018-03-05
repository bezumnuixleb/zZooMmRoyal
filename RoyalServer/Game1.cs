using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Lidgren.Network;
using System.Threading;
using System;
using RoyalServer.MOB_S;
using zZooMmRoyal.States;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Shared;
using VelcroPhysics.Utilities;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Factories;
using RoyalServer.Game_objects;
using RoyalServer.Game_objects.MOB_S;

namespace RoyalServer
{
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        //Camera camera;
        //private List<Plain> _Plain;
        //private List<MobZombie> _Zombie;
        private Vector2 _screenCenter;//
        public Color defColor;
        public readonly World _world;//
        //gameobj list
        public List<PlayerS> playerlist = new List<PlayerS>();
        public List<ZombieS> zombielist = new List<ZombieS>();
        public List<ObjectS> solidlist = new List<ObjectS>();
        public List<Bullet> bulletlist = new List<Bullet>();
        public List<СannonS> Cannonlist = new List<СannonS>();

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
        

        public ServerState currentState;
        public ServerState previousState;

        public int counterToEndGame = 0;

        public List<ObjectS> objlist = new List<ObjectS>();
        public List<String> mslist = new List<String>();
        public int idcounter = 1;
        public Server server = new Server();
        
        public Thread msgchecker;

        private State _currentState;

        private State _nextState;

        public void ChangesState(State state)
        {
            _nextState = state;
        }
        public Game1()
        {
            defColor = Color.CornflowerBlue;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            _world = new World(new Vector2(0, 0));//
            textures = new TextureList();
            Content.RootDirectory = "Content";
            currentState = new ServerState();
            currentState = ServerState.waitingPlayers;
            previousState = new ServerState();
            previousState = ServerState.waitingPlayers;
            server.StartServer();
            _currentState=new WaitingPlayersState(this, graphics.GraphicsDevice, Content);
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            #region TextDownload
            textures.Player_1= Content.Load<Texture2D>("Player/New_1");
            textures.Zombie_1 = Content.Load<Texture2D>("Zombie");
            textures.Box_2 = Content.Load<Texture2D>("graphics/level/enviroment/boxes/box_2");

            textures.Bullet_Сannon = Content.Load<Texture2D>("Cannon");
            textures.Сannon_1 = Content.Load<Texture2D>("Bullet/bullet");

            #endregion
            //msgchecker = new Thread(() => server.ReadMessages(zombielist,playerlist, Player_Texture_Std, idcounter));
            msgchecker = new Thread(() => server.ReadMessagesNew(this));
             msgchecker.Start();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.IsMouseVisible = true;
            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            ConvertUnits.SetDisplayUnitToSimUnitRatio(200f);

            //var groundtexture = Content.Load<Texture2D>("grass_tile");
   
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }
          
            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);

            previousState = currentState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(defColor);

            //  spriteBatch.Begin();
            //  spriteBatch.Draw(Player_Texture_Std, playerlist.ToArray()[0]._position, null, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 0f);
            //   spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

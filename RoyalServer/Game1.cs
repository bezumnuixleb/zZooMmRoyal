using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Lidgren.Network;
using System.Threading;
using System;
using RoyalServer.MOB_S;
using zZooMmRoyal.States;

namespace RoyalServer
{
    public class Game1 : Game
    {
        public ServerState currentState;
        public ServerState previousState;

        public int counterToEndGame = 0;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public List<PlayerS> playerlist=new List<PlayerS>();
        public List<ZombieS> zombielist = new List<ZombieS>();

        public List<Object> objlist = new List<Object>();
        public List<String> mslist = new List<String>();
        public int idcounter = 1;
        public Server server = new Server();
        public Texture2D Player_Texture_Std;
        public Texture2D Zombie_Texture_Std;
        public Thread msgchecker;

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
       
            Player_Texture_Std = Content.Load<Texture2D>("test");
            Zombie_Texture_Std = Content.Load<Texture2D>("Zombie");

            //msgchecker = new Thread(() => server.ReadMessages(zombielist,playerlist, Player_Texture_Std, idcounter));
            msgchecker = new Thread(() => server.ReadMessagesNew(this));
             msgchecker.Start();


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

            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var player in playerlist)
            {
                foreach (var Zombie in zombielist)
                {
                    if (player.IsColliding(Zombie)) GraphicsDevice.Clear(Color.Red);
                }
            }
            //  spriteBatch.Begin();
            //  spriteBatch.Draw(Player_Texture_Std, playerlist.ToArray()[0]._position, null, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 0f);
            //   spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

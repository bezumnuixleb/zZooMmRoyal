using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RoyalServer;
using RoyalServer.MOB_S;

namespace zZooMmRoyal.States
{
    class LobbyState : State
    {
        public LobbyState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // mob generation
            for (int i = 0; i < 200; i++)
            {
                ZombieS tmpZ = new ZombieS(_game.Zombie_Texture_Std, _game._world);
                tmpZ.RandPos();
                _game.zombielist.Add(tmpZ);
            }
            //map generation
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //nothing to drawing 
            //or draw lobby
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //removing players list
            //clear map
            //etc
        }

        public override void Update(GameTime gameTime)
        {
            //lobby kek
            if (_game.currentState == ServerState.gameplay && _game.previousState == ServerState.lobby)
            {
                //change state
                _game.ChangesState(new GameState(_game, _graphicsDevice, _content));
            }
        }
    }
}

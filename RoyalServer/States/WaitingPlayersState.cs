using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RoyalServer;

namespace zZooMmRoyal.States
{
    public class WaitingPlayersState : State
    {
        

        public WaitingPlayersState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _game.counterToEndGame = 0;
            _game.playerlist.Clear();
            _game.zombielist.Clear();

            _game.objlist.Clear();
            _game.mslist.Clear();
            _game.idcounter = 1;
    }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //nothing to drawing 
            //or draw smth  
         
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // Remove sprites 
        }

        public override void Update(GameTime gameTime)
        {
            if (_game.currentState == ServerState.lobby && _game.previousState == ServerState.waitingPlayers)
            {
                //change state
                _game.ChangesState(new LobbyState(_game, _graphicsDevice, _content));
            }
        }

    }
}

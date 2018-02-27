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
            for (int i = 0; i < 7; i++)
            {
                ZombieS tmpZ = new ZombieS(_game.Zombie_Texture_Std)
                {
                    //_speed = 0f,
                    distance_Min = 300,
                    _Size = new Vector2(0.3f, 0.3f),
                    _Type = "Zombie",
                    Origin = new Vector2(_game.Player_Texture_Std.Width / 2, _game.Player_Texture_Std.Height / 2),
                    _rotation = 0,
                    phys = new PhysicZ.PhysZ(1f)
                    {

                    },
                    number = i,
                    // _position = new Vector2(900, 500),
                };
                tmpZ.RandPos(_game.playerlist);
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

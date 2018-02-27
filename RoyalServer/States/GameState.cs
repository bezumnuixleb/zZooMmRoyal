using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoyalServer;

namespace zZooMmRoyal.States
{
    public class GameState : State
    {

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {

        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {


        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void PostUpdate(GameTime gameTime)
        {
            //after end game session
        }

        public override void Update(GameTime gameTime)
        {
            //current update game
            bool someoneisAlive = false;
            foreach (var player in _game.playerlist)
            {
                player.Update(gameTime, _game.objlist);
                if (player._isAlive) someoneisAlive = true;
            }
            foreach (var Zombie in _game.zombielist)
            {
                Zombie.Update(gameTime, _game.objlist, _game.playerlist);
            }
            foreach (var ZombieMain in _game.zombielist)
            {
                foreach (var Zombie in _game.zombielist)
                {
                    if (ZombieMain.number != Zombie.number && Zombie.IsColliding(ZombieMain))
                    {

                        Zombie._position -= new Vector2(10, 10);
                        ZombieMain._position += new Vector2(10, 10);

                    }
                }
            }

            foreach (var player in _game.playerlist)
            {
                foreach (var Zombie in _game.zombielist)
                {
                    if (player.IsColliding(Zombie))
                    {
                        player.phys.Impulse += Zombie.getVectorSight() * 15;
                        Zombie.phys.Impulse -= Zombie.getVectorSight() * 5;

                    }
                }
           
            }


       
            // main update

            // send positions
            //_game.ChangesState(new MenuState(_game, _graphicsDevice, _content));

            if (!someoneisAlive)
            {
                _game.counterToEndGame++;
                if (_game.counterToEndGame++ > 100)
                {
                    _game.ChangesState(new WaitingPlayersState(_game, _graphicsDevice, _content));
                    _game.currentState = ServerState.waitingPlayers;
                }

            }

            //yslovie porajenia kek
            //пока что дисконнект=луз
            //если игроков нет сервак рестартится


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();

        }
    }
}

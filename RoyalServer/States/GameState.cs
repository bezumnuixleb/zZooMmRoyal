﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoyalServer;
using VelcroPhysics.Utilities;

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


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();

            foreach (var item in _game.playerlist)
            {
                item.Update(gameTime, _game);
                if (item._isAlive) someoneisAlive = true;
            }

            foreach (var item in _game.zombielist)
            {
                item.Update(gameTime, _game,_game.playerlist);
            }

            foreach (var item in _game.bulletlist.ToArray())
            {
                item.Update(gameTime, _game);
            }
            
            for (int i = 0; i < _game.bulletlist.Count; i++)
            {
                if (_game.bulletlist[i].isRemoved)
                {
                    _game.bulletlist.RemoveAt(i);
                    i--;
                }
            }

            foreach (var item in _game.Cannonlist)
            {
                item.Update(gameTime,_game, _game.playerlist, _game.bulletlist);
            }

            foreach (var player in _game.playerlist)
            {
                player.body.OnCollision += Body_OnCollision;
            }


            _game._world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

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

        private void Body_OnCollision(VelcroPhysics.Dynamics.Fixture fixtureA, VelcroPhysics.Dynamics.Fixture fixtureB, VelcroPhysics.Collision.ContactSystem.Contact contact)
        {
            if ((string)fixtureB.Body.UserData == (string)"Bullet_1")
            {
                
                fixtureA.Body.ApplyLinearImpulse(0.0025f*fixtureB.Body.GetLinearVelocityFromLocalPoint(fixtureA.Body.Position+ ConvertUnits.ToSimUnits(new Vector2(256,256))));
                fixtureB.Body.Position = new Vector2(1000f, 1000f);
            }

        }
    }
}

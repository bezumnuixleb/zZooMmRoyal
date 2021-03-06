﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RoyalServer;
using RoyalServer.Game_objects;
using RoyalServer.Game_objects.MOB_S;
using RoyalServer.MOB_S;
using VelcroPhysics.Utilities;

namespace zZooMmRoyal.States
{
    class LobbyState : State
    {
        public LobbyState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // mob generation
            game.defColor = Color.CornflowerBlue;
            for (int i = 0; i < 50; i++)
            {
                ZombieS tmpZ = new ZombieS(_game.textures.Zombie_1, _game._world);
                tmpZ.RandPos();
                _game.zombielist.Add(tmpZ);
            }
            for (int i = 0; i < 200; i++)
            {
                ObjectS tmpO = new ObjectS(_game.textures.Box_2);
                Thread.Sleep(20);
                Random random = new Random();
                int x=random.Next(-7400, 7400);
                int y=random.Next(-1100 * 6, 1100 * 6);
                //int size = random.Next(5,80);
                int size = 30;
                int rotation = random.Next(0,359);
                tmpO.body = BodyConstructor.CreateBody(TipTela.Box_2, _game._world, null, size * 0.1f,(float)rotation);
                tmpO._Size = new Vector2(size * 0.1f, size * 0.1f);
                tmpO.body.Position = new Vector2(ConvertUnits.ToSimUnits(x), ConvertUnits.ToSimUnits(y));
                _game.solidlist.Add(tmpO);
            }

            for (int i = 0; i < 20; i++)
            {
                СannonS tmpC = new СannonS(_game.textures.Сannon_1,_game.textures.Bullet_Сannon, _game._world);
                tmpC.body = BodyConstructor.CreateBody(TipTela.Mob_2, _game._world, tmpC, 0.2f);
                //tmpC.body.Position = new Vector2(15f, 5f*i);
                tmpC.RandPos();
                _game.Cannonlist.Add(tmpC);
            }


            //map generation


            game.defColor = Color.Red;
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

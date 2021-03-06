﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using zZooMmRoyal.Controls;

namespace zZooMmRoyal.States
{
    public class LobbyState : State
    {
        private List<Component> _components;
        public List<Component> _componentsPl;
        Texture2D buttonTexture;
        SpriteFont buttonFont;
        public LobbyState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            game.player._Size = new Vector2(0.5f, 0.5f);
            buttonTexture = _content.Load<Texture2D>("Controls/Buttons");
            buttonFont = _content.Load<SpriteFont>("Fonts/menu_font");

            var StartGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(_game.Window.ClientBounds.Width/2- _game.Window.ClientBounds.Width/10,
                _game.Window.ClientBounds.Height-_game.Window.ClientBounds.Height / 10),
                Text = "Start Game",
                _font = buttonFont,
            };

            StartGameButton.Click += StartGameButton_Click;
            _game.client.StartClient();
            if (_game.msgchecker.IsAlive==false)
            _game.msgchecker.Start();

            var DisconnectButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(0, 0),
                Text = "Disconnect",
                _font = buttonFont,
            };

            DisconnectButton.Click += DisconnectButton_Click;
            _components = new List<Component>()
            {
                StartGameButton,
                DisconnectButton
            };
            _componentsPl = new List<Component>();
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            _game.ChangesState(new MenuState(_game, _graphicsDevice, _content));
            _game.currentState = ClientState.mainmenu;
            _game.client.Disconnect();

        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangesState(new GameState(_game, _graphicsDevice, _content));
            _game.currentState = ClientState.gameplay;
            //msg about start game
            _game.client.SendMessage(_game.player._id + " " + "startgamepls");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach (var copmonent in _components)
                copmonent.Draw(gameTime, spriteBatch);
            foreach (var item in _componentsPl)
            {
                item.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (_game.player._id == "null" && _game.idchecker > 50) {
                _game.client.SendMessage("give_id " + _game.player._name);
                _game.idchecker = 0;
            }
            if (_game.player._id == "null" && _game.idchecker <= 50) {
                _game.idchecker++; }
            _game.client.SendMessage(_game.player._id + " " + "giveINFO");

            _game.backobjlist = new List<Object>();
            for (int i = -5; i < 5; i++)
            {
                for (int j = -6; j < 6; j++)
                {
                    Object tmp = new Object(_game.textures.Tile_1);
                    tmp._position = new Vector2(tmp._texture.Width * i, tmp._texture.Height* j);
                    _game.backobjlist.Add(tmp);

                }
            }

            int Counter = 0;
            _game.msgchecker.Suspend();
            _componentsPl.Clear();
            foreach (var player in _game.LobbyPlayersList)
            {
                PlayerInLobby tmp = new PlayerInLobby(buttonTexture, buttonFont)
                {
                    Position = new Vector2(_game.Window.ClientBounds.Width / 10,
                    _game.Window.ClientBounds.Height / 5+Counter* _game.Window.ClientBounds.Height / 10),
                    Text = player,
                    _font = buttonFont,
                };
                Counter++;
                _componentsPl.Add(tmp);
            }
            _game.msgchecker.Resume();

            foreach (var component in _components)
                component.Update(gameTime);

        }
    }
}

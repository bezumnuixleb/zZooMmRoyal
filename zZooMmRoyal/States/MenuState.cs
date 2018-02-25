using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using zZooMmRoyal.Controls;

namespace zZooMmRoyal.States
{
    public class MenuState : State
    {
        
        private List<Component> _components;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Buttons");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var BackText = _content.Load<Texture2D>("graphics/main_menu/main_menu_background");
            var Background = new Background(BackText)
            {
                Position = new Vector2(0, 0)
            };

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "New Game",
                _font = buttonFont,
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "load Game",
                _font = buttonFont,

            };

            loadGameButton.Click += loadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Quit",
                _font = buttonFont,

            };

            quitGameButton.Click += QuitButton_Click;

            _components = new List<Component>()
            { Background,
                newGameButton,
                loadGameButton,
                quitGameButton,
               
            };


            _game.player._position = new Vector2(0, 0);
            _game.player._Type = "Player";
            _game.player._input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S };
            _game.player._name = "Dodek";
            _game.player._Size = new Vector2(0.5f, 0.5f);
            _game.player._id = "null";
            _game.msglist.Clear();
            _game.objlist.Clear();
            _game.LobbyPlayersList.Clear();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var copmonent in _components)
                copmonent.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void loadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // Load new State\
            _game.ChangesState(new LobbyState(_game, _graphicsDevice, _content));
            _game.currentState = ClientState.lobby;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // Remove sprites 
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var copmonent in _components)
                copmonent.Update(gameTime);
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace zZooMm001
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        
        SpriteBatch spriteBatch;

        private List<Plain> _Plain;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {


            base.Initialize();
          
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            var texture = Content.Load<Texture2D>("plain");

            _Plain = new List<Plain>()
            {
                new Plain(texture) {_position = new Vector2(100,100), _speed = 3f, _input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S}, _Size = new Vector2(0.1f,0.15f), Origin = new Vector2(texture.Width/2,texture.Height/2),_rotation = 0},
                //new Plain(texture) {_position = new Vector2(0,0), _speed = 2f, _input = new Input { Left = Keys.Left, Right = Keys.Right, Up = Keys.Up, Down = Keys.Down}, _Size = new Vector2(1f,1f) },

            };

        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach (var plain in _Plain)
                plain.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var plain in _Plain)
                plain.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

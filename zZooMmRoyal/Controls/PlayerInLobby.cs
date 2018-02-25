using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zZooMmRoyal.Controls
{
    class PlayerInLobby : Component
    {
        private Texture2D _texture;
        public SpriteFont _font;

       public PlayerInLobby(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
        }

        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
        public string Text { get; set; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color.White);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);
                spriteBatch.DrawString(_font, Text, new Vector2(x, y), Color.Black);
                //spriteBatch.DrawString(_font, Text, new Vector2(100, 100), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //nothing
        }
    }
}

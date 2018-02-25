using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zZooMmRoyal.Controls
{
    class Background : Component
    {
        public Texture2D _texture;
        public Vector2 Position { get; set; }
        public Background(Texture2D texture)
        {
            _texture = texture;

        }

        public override void Draw(GameTime gamTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}

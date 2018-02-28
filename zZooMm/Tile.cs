using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMm001
{
    public class Tile
    {
        public Texture2D texture;
        public Vector2 position;
        public Tile(Texture2D text,Vector2 pos)
        {
            position = pos;
            texture = text;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalServer.Game_objects
{
    public class Tile 
    {
        public Vector2 Position;
        public String Type;
        public Tile(Vector2 pos,String type="Grass")
        {
            Position = pos;
            Type = type;
        }
    }
}

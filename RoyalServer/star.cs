using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RoyalServer
{
    public class star : Object
    {
        public float _speed=2f;
        public List<String> buttons;

        public star(Texture2D ntext) : base(ntext)
        {
            buttons = new List<String>();
        }
        public override void Update(GameTime gameTime, List<Object> gameobj)
        {
            foreach (var item in buttons)
            {
                switch (item)
                {
                    case "Left": { _position.X -= _speed; }break;
                    case "Right": { _position.X += _speed; } break;
                    case "Up": { _position.Y -= _speed; } break;
                    case "Down": { _position.Y += _speed; } break;
                    default:
                        break;
                }
            }
            //mouse pos getting
        }
        public void RandPos()
        {
            Random rnd = new Random();
            _position.X = rnd.Next(100, 600);
            _position.Y = rnd.Next(10, 400);
        }
    }

}

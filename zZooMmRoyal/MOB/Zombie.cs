using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMmRoyal.MOB
{
    class Zombie : Object
    {
        public Vector2 _Size;

        public Zombie(Texture2D text) : base(text)
        {

        }
        public Zombie() : base(null)
        {

        }
        public void Changes(String msg)
        {
            //" _Type _PosX _PosY Rotation"
            String[] mas = msg.Split();

            if (mas[0] != "Zombie") return;
            //check type
            _position.X = Convert.ToSingle(mas[1]);
            _position.Y = Convert.ToSingle(mas[2]);
            _rotation = Convert.ToSingle(mas[3]);
            //other changes
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, Origin, _Size, SpriteEffects.None, 0f);
        }

    }
}

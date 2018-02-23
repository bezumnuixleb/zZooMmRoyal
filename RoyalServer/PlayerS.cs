using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalServer
{
   public class PlayerS : Object
    {
        public String _name;
        public String _id;

        public Vector2 _Size;
        public Vector2 _mPosition;

        public List<String> buttons;


        public float _speed = 2f;
<<<<<<< HEAD
        public float _rotation = 3f;
=======
        public PressedButtons buttons;
>>>>>>> 6fae1eda53b63f08f07e5efc0cf64b3cdb89b57d
        public PlayerS(Texture2D texture):base(texture)
        {
            _mPosition = new Vector2(0,0);
        }
        public override void Update(GameTime gameTime, List<Object> gameobj)
        {
            //mouse pos getting
            Move();
            //MoveButtons();
        }
        public void MoveButtons(String msg)
        {

        
                switch (msg)
                {
                    case "Left": { _position.X -= _speed; } break;
                    case "Right": { _position.X += _speed; } break;
                    case "Up": { _position.Y -= _speed; } break;
                    case "Down": { _position.Y += _speed; } break;
                    default:
                        break;
                }


            //others buttons kek
        }
        public void Move()
        {
            /////////////////////////////////////////////////////
            //ПОВОРОТ ЗА МЫШКОЙ
            ///////////////////

            Vector2 direction = _mPosition - _position;
            direction.Normalize();

            _rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X) + MathHelper.ToRadians(90);// + 90 градусов из за картинки 
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, Origin, _Size, SpriteEffects.None, 0f);
        }
        public void RandPos()
        {
            Random rnd = new Random();
            _position.X = rnd.Next(100, 600);
            _position.Y = rnd.Next(10, 400);
        }
    }

}

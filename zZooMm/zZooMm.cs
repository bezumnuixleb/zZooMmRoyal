using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMm001
{
    class Plain
    {
        private Texture2D _texture;
        public Vector2 Origin;
        public Vector2 _position;
        public Vector2 _Size;

        public float _speed = 2f;
        public Input _input;

        public float _rotation = 3f;

        public Plain(Texture2D texture)
        {
            _texture = texture;
        }
        public void Update ()
        {
            Move();
        }

        public void Move()
        {
            /////////////////////////////////////////////////////
            //ПОВОРОТ ЗА МЫШКОЙ
            ///////////////////
            MouseState currentMouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            Vector2 direction = mousePosition - _position;
            direction.Normalize();

            _rotation =  (float)Math.Atan2((double)direction.Y, (double)direction.X) + MathHelper.ToRadians(90);// + 90 градусов из за картинки 

            ///////////////////
            //ПОВОРОТ ЗА МЫШКОЙ
            /////////////////////////////////////////////////////

            // var directory = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - _rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - _rotation));

            if (_input == null)
                return;
            if (Keyboard.GetState().IsKeyDown(_input.Left))
            {
                _position.X -= _speed;
            }
            if (Keyboard.GetState().IsKeyDown(_input.Right))
            {
                _position.X += _speed;
            }
            if (Keyboard.GetState().IsKeyDown(_input.Up))
            {
                _position.Y -= _speed;
            }
            if (Keyboard.GetState().IsKeyDown(_input.Down))
            {
                _position.Y +=  _speed;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, Origin, _Size, SpriteEffects.None,0f);
        }
    }
}

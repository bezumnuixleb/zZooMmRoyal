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
        public Vector2 _position;
        public Vector2 _Size;
        public float _speed = 2f;
        public Input _input;


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
                _position.Y += _speed;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, new Vector2(0,0), _Size, SpriteEffects.None,0f);
        }
    }
}

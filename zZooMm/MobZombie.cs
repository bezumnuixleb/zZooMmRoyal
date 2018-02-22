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
    class MobZombie
    {
        private Texture2D _texture;
        public Vector2 Origin;
        public Vector2 _position;
        public Vector2 _Size;

        public float _speed = 2f;

        public float _rotation = 3f;

        public MobZombie(Texture2D texture)
        {
            _texture = texture;
        }
        public void Update(List<Plain> Plain, float distanceMin)
        {

            foreach (var plain in Plain)
            {
                double formDistance = (double)((plain._position.X - _position.X) * (plain._position.X - _position.X) + (plain._position.Y - _position.Y) * (plain._position.Y - _position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distanceMin > distance)
                {
                    // поворот к персонажу 
                    Vector2 mousePosition = new Vector2(plain._position.X, plain._position.Y);

                    Vector2 direction = mousePosition - _position;
                    direction.Normalize();

                    _rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);

                    // движение к персонажу 

                    var directory = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - _rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - _rotation));
                    _position += direction * _speed;
                    //_position.X = _speed * (plain._position.X - _position.X) / distance;
                    //_position.Y = _speed * (plain._position.Y - _position.Y) / distance;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, Origin, _Size, SpriteEffects.None, 0f);
        }
    }
}

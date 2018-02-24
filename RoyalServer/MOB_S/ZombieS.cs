using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalServer.MOB_S
{
    public class ZombieS : Object
    {
        public Vector2 _Size;
        public float _speed = 1f;
        public float distance_Min;


        public ZombieS(Texture2D texture) : base(texture)
        {
        }

        public void Update(GameTime gameTime, List<Object> gameobj, List<PlayerS> playerlist)
        {
            foreach (var player in playerlist)
            {
                double formDistance = (double)((player._position.X - _position.X) * (player._position.X - _position.X) + (player._position.Y - _position.Y) * (player._position.Y - _position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distance_Min > distance)
                {
                    // поворот к персонажу 
                    Vector2 mousePosition = new Vector2(player._position.X, player._position.Y);

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, Origin, _Size, SpriteEffects.None, 0f);
        }
        public void RandPos(List<PlayerS> playerlist)
        {
            bool Flag = false;
            while (true)
            {
                Random rnd = new Random();
                Thread.Sleep(20);
                Flag = false;
                _position.X = rnd.Next(0, 500);
                _position.Y = rnd.Next(0, 300);
                foreach (var player in playerlist)
                {
                    if ((int)player._position.X == (int)_position.X || (int)player._position.Y == (int)_position.Y) Flag = true;
                }
                if (Flag == false) break;
            }
        }
    }
}

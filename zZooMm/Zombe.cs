using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rastating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMm001
{
    public class Zombe : CollidableObject
    {
        public Zombe(Texture2D texture, Vector2 position) : base(texture, position)
        {
        }

        public Zombe(Texture2D texture, Vector2 position, float rotation) : base(texture, position, rotation)
        {
        }
        public void UpdateZombe(GameTime gameTime,CollidableObject player)
        {
            Move(player);
           
            position = phys.Update(gameTime, position);
            Vector2 nulled = new Vector2(0, 0);
            phys.Force = nulled;

        }
        public void Move(CollidableObject player)
        {
            Vector2 mousePosition = new Vector2(player.position.X, player.position.Y);

            Vector2 direction = mousePosition - position;
            direction.Normalize();

            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
            // движение к персонажу 

            var directory = new Vector2((float)Math.Cos(MathHelper.ToRadians(90) - rotation), -(float)Math.Sin(MathHelper.ToRadians(90) - rotation));
            phys.Force.X = (float)Math.Sin(MathHelper.ToRadians(90) - rotation) * 4f;
            phys.Force.Y = (float)Math.Cos(MathHelper.ToRadians(90) - rotation) * 4f;


        }
        public Vector2 getVectorMove()
        {
            return new Vector2((float)Math.Sin(MathHelper.ToRadians(90) - rotation) * 4f, (float)Math.Cos(MathHelper.ToRadians(90) - rotation) * 4f);
        }
    }
}

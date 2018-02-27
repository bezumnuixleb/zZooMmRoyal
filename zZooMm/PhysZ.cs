using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMm001
{
   public class PhysZ
    {
        public Vector2 Force;
        public Vector2 Impulse;
        public Vector2 Velocity;
        public Vector2 CentrPos;
        public Vector2 PrevCentrPos;

        public float Mass = 1f;
        public float Radius = 1f;

        public Rectangle CurrentSize;

        public PhysZ(float mass)
        {
            Mass = mass;
            Impulse = new Vector2(0, 0);
            Force = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);

        }
        public Vector2 Update(GameTime gameTime,Vector2 pos)
        {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Impulse += Force*deltaT*40;
            Velocity = Impulse/Mass;
            Impulse *= 0.3f;
            if (Impulse.X < 0.01f&&Impulse.X>-0.01f) Impulse.X = 0;
            if (Impulse.Y < 0.01f && Impulse.Y > -0.01f) Impulse.Y = 0;
            return pos += Velocity;
        }



    }
}

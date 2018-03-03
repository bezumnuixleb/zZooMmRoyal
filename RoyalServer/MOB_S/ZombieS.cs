using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace RoyalServer.MOB_S
{
    public class ZombieS : Object
    {
        public int number;
        public float distance_Min;
        public ZombieS(Texture2D txt, World _world) : base(txt, new Vector2(0, 0))
        {
     

            distance_Min = 2000;
            texture = txt;
            _Size = new Vector2(0.5f, 0.5f);
            speed_rotation = 4f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            body = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(texture.Width / 2 * _Size.X), 0.2f, ConvertUnits.ToSimUnits(new Vector2(0,0)), BodyType.Dynamic);
            body.Restitution = 0.3f;
            body.Friction = 0.5f;
            body.UserData = "Zombie";

        }

        public void Update(GameTime gameTime, List<Object> gameobj, List<PlayerS> playerlist)
        {
            foreach (var player in playerlist)
            {
                Vector2 Player_Position = ConvertUnits.ToDisplayUnits(player.body.Position);
                Vector2 Mob_Position = ConvertUnits.ToDisplayUnits(body.Position);
                double formDistance = (double)((Player_Position.X - Mob_Position.X) * (Player_Position.X - Mob_Position.X) + (Player_Position.Y - Mob_Position.Y) * (Player_Position.Y - Mob_Position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distance_Min > distance)
                {
                    Vector2 Player_position = ConvertUnits.ToDisplayUnits(player.body.Position);

                    Vector2 direction = Player_position - ConvertUnits.ToDisplayUnits(body.Position);

                    direction.Normalize();

                    rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);

                    if (Math.Abs(rotation - body.Rotation) > MathHelper.ToRadians(180))
                    {
                        if (rotation > body.Rotation)
                        {
                            rotation -= MathHelper.ToRadians(360);
                        }
                        else
                        {
                            rotation += MathHelper.ToRadians(360);
                        }
                    }

                    if (body.Rotation > MathHelper.ToRadians(360)) body.Rotation -= MathHelper.ToRadians(360);
                    if (body.Rotation < MathHelper.ToRadians(0)) body.Rotation += MathHelper.ToRadians(360);

                    if (rotation > body.Rotation)
                    {
                        body.Rotation += MathHelper.ToRadians(speed_rotation);
                    }
                    if (rotation < body.Rotation)
                    {
                        body.Rotation -= MathHelper.ToRadians(speed_rotation);
                    }
                    // движение к персонажу 
                    body.ResetDynamics();

                    body.ApplyLinearImpulse(new Vector2((float)Math.Sin(MathHelper.ToRadians(90) - body.Rotation) * 0.2f, (float)Math.Cos(MathHelper.ToRadians(90) - body.Rotation) * 0.2f));
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(body.Position), null, Color.White, body.Rotation, origin, _Size, SpriteEffects.None, 0f);
        }
        public void RandPos()
        {
            Random rnd = new Random();
            Thread.Sleep(20);
            Vector2 Pos = ConvertUnits.ToSimUnits(new Vector2(rnd.Next(-1540 * 10, 1540 * 10), rnd.Next(-1190 * 10, 1190 * 10)));
            body.Position = Pos;
        }


    }
}

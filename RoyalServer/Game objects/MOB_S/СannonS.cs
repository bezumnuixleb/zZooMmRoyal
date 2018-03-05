using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

namespace RoyalServer.Game_objects.MOB_S
{
    public class СannonS: ObjectS
    {


        public int number;
        public float distance_Min;

        public Texture2D txt;

        public float pre_shot_timer;
        Bullet bullet;


        public СannonS(Texture2D txt,Texture2D txtBullet, World _world, TipTela _type = TipTela.Mob_1) : base(txt)
        {
            distance_Min = 1500;
            texture = txt;
            pre_shot_timer = 0;
            _Size = new Vector2(0.8f, 0.8f);
            speed_rotation = 4f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            body = BodyConstructor.CreateBody(_type, _world, this, 0.5f);

            bullet = new Bullet(txtBullet, _world);
        }

        public void Update(GameTime gameTime, Game1 game, List<PlayerS> playerlist, List<Bullet> bulletlist)
        {
            pre_shot_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            body.ResetDynamics();
            PlayerS tmp = playerlist[0];
            float MIN_distance = 1500;

            // находим наиближайшего персонажа 
            foreach (var player in playerlist)
            {
                Vector2 Player_Position = ConvertUnits.ToDisplayUnits(player.body.Position+ ConvertUnits.ToSimUnits(new Vector2(player.texture.Width/2,player.texture.Height/2)));
                Vector2 Mob_Position = ConvertUnits.ToDisplayUnits(body.Position);
                double formDistance = (double)((Player_Position.X - Mob_Position.X) * (Player_Position.X - Mob_Position.X) + (Player_Position.Y - Mob_Position.Y) * (Player_Position.Y - Mob_Position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distance < MIN_distance && distance < distance_Min )
                {
                    MIN_distance = distance;
                    tmp = player;
                }
            }

            if (MIN_distance != 1500)
            {
                
               
                //поворот к игроку
                rotation_Plyer(tmp);
                // выстрел
                if (pre_shot_timer >= 0.3f)
                {
                    var _bullet = bullet.Clone() as Bullet;
                    _bullet.rotation = body.Rotation;
                    _bullet.body.Rotation = body.Rotation;
                    _bullet.body.Position = body.Position;
                    bulletlist.Add(_bullet);
                    pre_shot_timer = 0;
                }
            }
            else
            {
                //обнуление таймера
                pre_shot_timer = 0;
            }
        }

       
        public void rotation_Plyer(PlayerS player)
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(body.Position), null, Color.White, body.Rotation, origin, _Size, SpriteEffects.None, 0f);
        }
        public void RandPos()
        {
            Random rnd = new Random();
            Thread.Sleep(20);
            Vector2 Pos = ConvertUnits.ToSimUnits(new Vector2(rnd.Next(-7400, 7400 ), rnd.Next(-1100 * 6, 1100 * 6)));
            body.Position = Pos;
        }
    }

}

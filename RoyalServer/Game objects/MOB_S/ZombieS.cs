using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyalServer.Game_objects;
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
    public class ZombieS : ObjectS
    {
        public int number;
        public float distance_Min;

        public String player_ID;
        public ZombieS(Texture2D txt, World _world,TipTela _type=TipTela.Mob_1) : base(txt)
        {

            player_ID = "";
            distance_Min = 2000;
            texture = txt;
            _Size = new Vector2(0.5f, 0.5f);
            speed_rotation = 4f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            //посчитать размеры офк норм для зомби
            body = BodyConstructor.CreateBody(_type, _world, this, 0.5f);
        }

        public void Update(GameTime gameTime, Game1 game, List<PlayerS> playerlist)
        {

            if (player_ID != "")
            {
                move_to_playe_ID(playerlist);
                if (player_ID != "") return;
            }

            PlayerS tmp = playerlist[0];
            float MIN_distance = 2000;
          
            // находим наиближайшего персонажа 
            foreach (var player in playerlist)
            {
                Vector2 Player_Position = ConvertUnits.ToDisplayUnits(player.body.Position);
                Vector2 Mob_Position = ConvertUnits.ToDisplayUnits(body.Position);
                double formDistance = (double)((Player_Position.X - Mob_Position.X) * (Player_Position.X - Mob_Position.X) + (Player_Position.Y - Mob_Position.Y) * (Player_Position.Y - Mob_Position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);       
                if (distance < MIN_distance && distance < distance_Min && player_ID == "")
                {
                    MIN_distance = distance;
                    tmp = player;
                    player_ID = player._id;
                }
            }

            if (MIN_distance != 2000)
            {
                //поворот к игроку
                rotation_Plyer(tmp);
                // движение к персонажу 
                body.ResetDynamics();
                body.ApplyLinearImpulse(new Vector2((float)Math.Sin(MathHelper.ToRadians(90) - body.Rotation) * 0.2f, (float)Math.Cos(MathHelper.ToRadians(90) - body.Rotation) * 0.2f));
            }
        }

        public void move_to_playe_ID(List<PlayerS> playerlist)
        {
            PlayerS tmp = playerlist[0];
            foreach (var Player in playerlist)
            {
                if (Player._id == player_ID) tmp = Player;
            }

            if (tmp._id != player_ID)// если такого челика нет в списке 
            {
                player_ID = "";
                return;
            }

            // если челик есть, проверим расстояние 
            Vector2 Player_Position = ConvertUnits.ToDisplayUnits(tmp.body.Position);
            Vector2 Mob_Position = ConvertUnits.ToDisplayUnits(body.Position);
            double formDistance = (double)((Player_Position.X - Mob_Position.X) * (Player_Position.X - Mob_Position.X) + (Player_Position.Y - Mob_Position.Y) * (Player_Position.Y - Mob_Position.Y));
            float distance = (float)Math.Sqrt((double)formDistance);

            if (distance < distance_Min)
            {
                //поворот к игроку
                rotation_Plyer(tmp);
                // движение к персонажу 
                body.ResetDynamics();
                body.ApplyLinearImpulse(new Vector2((float)Math.Sin(MathHelper.ToRadians(90) - body.Rotation) * 0.2f, (float)Math.Cos(MathHelper.ToRadians(90) - body.Rotation) * 0.2f));
            }
            else
            {
                player_ID = "";
                return;
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
            Vector2 Pos = ConvertUnits.ToSimUnits(new Vector2(rnd.Next(-7400, 7400), rnd.Next(-1100 * 6, 1100 * 6)));
            body.Position = Pos;
        }
    }
}

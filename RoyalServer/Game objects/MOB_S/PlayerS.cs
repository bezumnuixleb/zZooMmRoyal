﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoyalServer.Game_objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Shared;
using VelcroPhysics.Utilities;

namespace RoyalServer
{
   public class PlayerS : ObjectS
    {
        public String _name;
        public String _id;
        public bool _isAlive = true;
        public Vector2 currentMouseState;
        public List<ObjectS> nearobj;
        public class PressedButtons
        {
            public bool up,down,left,right;
            public PressedButtons()
            {
                up = false; down = false; left = false; right = false;
            }
        }
        public Vector2 centreScreen;

        public PressedButtons buttons;
        public PlayerS(Texture2D txt, World _world):base(txt)
        {
            buttons = new PressedButtons();
            nearobj = new List<ObjectS>();
            texture = txt;
            _Size = new Vector2(0.5f, 0.5f);
            //_SizeZ = new Vector2(0.5f, 0.5f);
            speed_rotation = 4f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            //пересчетать для плеера на 512х512 и поменять его размер
            body = BodyConstructor.CreateBody(TipTela.Player_1, _world,null,1f);

            RandPos();
        }
        public override void Update(GameTime gameTime,Game1 game)
        {
            //mouse pos getting
            Move();
            MoveButtons();
            




        }
        public void TakeNear(Game1 game)
        {
            Vector2 Player_Position = ConvertUnits.ToDisplayUnits(body.Position);
            nearobj.Clear();
            foreach (var p in game.playerlist)
            {
                if (p == this) continue;
                Vector2 p_Position = ConvertUnits.ToDisplayUnits(body.Position);
                double formDistance = (double)((Player_Position.X - p_Position.X) * (Player_Position.X - p_Position.X) + (Player_Position.Y - p_Position.Y) * (Player_Position.Y - p_Position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distance < 500)
                {
                    nearobj.Add(p);
                }
            }
            foreach (var zomb in game.zombielist)
            {
                Vector2 Mob_Position = ConvertUnits.ToDisplayUnits(body.Position);
                double formDistance = (double)((Player_Position.X - Mob_Position.X) * (Player_Position.X - Mob_Position.X) + (Player_Position.Y - Mob_Position.Y) * (Player_Position.Y - Mob_Position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distance < 500)
                {
                    nearobj.Add(zomb);
                }
            }
            foreach (var block in game.solidlist)
            {
                Vector2 block_Position = ConvertUnits.ToDisplayUnits(body.Position);
                double formDistance = (double)((Player_Position.X - block_Position.X) * (Player_Position.X - block_Position.X) + (Player_Position.Y - block_Position.Y) * (Player_Position.Y - block_Position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distance < 500)
                {
                    nearobj.Add(block);
                }
            }
            foreach (var a in game.Cannonlist)
            {
                Vector2 Mob_Position = ConvertUnits.ToDisplayUnits(body.Position);
                double formDistance = (double)((Player_Position.X - Mob_Position.X) * (Player_Position.X - Mob_Position.X) + (Player_Position.Y - Mob_Position.Y) * (Player_Position.Y - Mob_Position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distance < 500)
                {
                    nearobj.Add(a);
                }
            }
            foreach (var a in game.bulletlist.ToArray())
            {
                Vector2 Mob_Position = ConvertUnits.ToDisplayUnits(body.Position);
                double formDistance = (double)((Player_Position.X - Mob_Position.X) * (Player_Position.X - Mob_Position.X) + (Player_Position.Y - Mob_Position.Y) * (Player_Position.Y - Mob_Position.Y));
                float distance = (float)Math.Sqrt((double)formDistance);
                if (distance < 500)
                {
                    nearobj.Add(a);
                }
            }
      
        }
        public void MoveButtons()
        {
            if ( !buttons.up || !buttons.down || !buttons.left || !buttons.down) body.ResetDynamics();

            if (buttons.up)
            {
                body.ApplyLinearImpulse(new Vector2(0, -4));
            }
            if (buttons.down)
            {
                body.ApplyLinearImpulse(new Vector2(0, 4));
            }
            if (buttons.right)
            {
                body.ApplyLinearImpulse(new Vector2(4, 0));
            }
            if (buttons.left)
            {
                body.ApplyLinearImpulse(new Vector2(-4, 0));
            }

            //others buttons kek
        }
        public void Move()
        {
            /////////////////////////////////////////////////////
            //ПОВОРОТ ЗА МЫШКОЙ
            ///////////////////

            //Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            Vector2 direction = currentMouseState - centreScreen;
            direction.Normalize();
            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X) + MathHelper.ToRadians(90);// + 90 градусов из за картинки

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
            Vector2 Pos = ConvertUnits.ToSimUnits(new Vector2(rnd.Next(100, 600), rnd.Next(10, 400)));
            body.Position = Pos;
        }
    }

}

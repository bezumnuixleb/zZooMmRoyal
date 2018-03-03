using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
   public class PlayerS : Object
    {
        public String _name;
        public String _id;
        public bool _isAlive = true;
        public Vector2 currentMouseState;
        public class PressedButtons
        {
            public bool up,down,left,right;
            public PressedButtons()
            {
                up = false; down = false; left = false; right = false;
            }
        }
        public PressedButtons buttons;
        public PlayerS(Texture2D txt, World _world):base(txt, new Vector2(0, 0))
        {
            buttons = new PressedButtons();

            texture = txt;
            _Size = new Vector2(0.5f, 0.5f);
            //_SizeZ = new Vector2(0.5f, 0.5f);
            speed_rotation = 4f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);


            Vertices bodyvert = new Vertices(8);
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-90, 51)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-59, -49)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(22, -104)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(60, -63)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(86, 4)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(77, 81)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(9, 105)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-44, 109)));

            PolygonShape playershape = new PolygonShape(bodyvert, 2f);

            body = BodyFactory.CreateBody(_world);
            body.CreateFixture(playershape);

            body.BodyType = BodyType.Dynamic;
            body.UserData = "Player";
            body.Restitution = 0.3f;
            body.Friction = 0.5f;

            RandPos();
        }
        public override void Update(GameTime gameTime, List<Object> gameobj)
        {
            //mouse pos getting
            Move();
            MoveButtons();
            
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

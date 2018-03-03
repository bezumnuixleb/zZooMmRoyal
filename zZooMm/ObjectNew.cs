using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace zZooMm001
{
    public class ObjectNew
    {
        public Texture2D texture;
        public Body body;
        public Vector2 origin;
        public Vector2 _Size;
        public Vector2 _SizeZ;
        public Input input;

        public float rotation;
        public float speed_rotation;
        public Vector2 centreScreen;

        public ObjectNew(Texture2D txt,Vector2 position,World world)
        {
            texture = txt;
            _Size = new Vector2(0.3f, 0.3f);
            _SizeZ =new Vector2(0.5f, 0.5f);
            speed_rotation = 4f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            
          
        }
        public void UpdateKeys()
        {
            Update_Rotation();
            if(Keyboard.GetState().IsKeyUp(input.Left)|| Keyboard.GetState().IsKeyUp(input.Down) || Keyboard.GetState().IsKeyUp(input.Right) || Keyboard.GetState().IsKeyUp(input.Up))body.ResetDynamics();

            if (Keyboard.GetState().IsKeyDown(input.Left))
            {
                body.ApplyLinearImpulse(new Vector2(-4, 0));

            }
            if (Keyboard.GetState().IsKeyDown(input.Right))
            {
                body.ApplyLinearImpulse(new Vector2(4, 0));
            }
            if (Keyboard.GetState().IsKeyDown(input.Up))
            {
                body.ApplyLinearImpulse(new Vector2(0, -4));
            }
            if (Keyboard.GetState().IsKeyDown(input.Down))
            {
                body.ApplyLinearImpulse(new Vector2(0, 4));
            }
        }

        public void Update_Rotation()
        {

            MouseState currentMouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            Vector2 direction = mousePosition - centreScreen;
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

        public void MoveToPlayer(ObjectNew player)
        {
            Vector2 Player_Position = ConvertUnits.ToDisplayUnits(player.body.Position);

            Vector2 direction = Player_Position - ConvertUnits.ToDisplayUnits(body.Position);

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

            body.ApplyLinearImpulse(new Vector2((float)Math.Sin(MathHelper.ToRadians(90) - body.Rotation) * 0.2f,(float)Math.Cos(MathHelper.ToRadians(90) - body.Rotation) * 0.2f));


        }
    }
}

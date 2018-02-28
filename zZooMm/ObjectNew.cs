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
        public ObjectNew(Texture2D txt,Vector2 position,World world)
        {
            texture = txt;
            _Size = new Vector2(0.3f, 0.3f);
            _SizeZ =new Vector2(0.5f, 0.5f);
            
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            
          
        }
        public void UpdateKeys(Viewport NewViewport)
        {
            MouseState currentMouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            Vector2 centreScreen = new Vector2(NewViewport.Width / 2, NewViewport.Height / 2);

            //Vector2 mousePosition2 = new Vector2(ConvertUnits.ToDisplayUnits(body.Position.X) - currentMouseState.X, ConvertUnits.ToDisplayUnits(body.Position.Y) - currentMouseState.Y);


            // Vector2 Centre = new Vector2(NewViewport.Width / 2), NewViewport.Height / 2));
            Vector2 direction = mousePosition - centreScreen;
            direction.Normalize();

            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X) + MathHelper.ToRadians(90);// + 90 градусов из за картинки

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
        public void MoveToPlayer(ObjectNew player)
        {
            Vector2 mousePosition = ConvertUnits.ToDisplayUnits(player.body.Position);

            Vector2 direction = mousePosition - ConvertUnits.ToDisplayUnits(body.Position);
            direction.Normalize();

            rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
            // движение к персонажу 
            body.ResetDynamics();

            body.ApplyLinearImpulse(new Vector2((float)Math.Sin(MathHelper.ToRadians(90) - rotation) * 0.2f,(float)Math.Cos(MathHelper.ToRadians(90) - rotation) * 0.2f));


        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;

namespace RoyalServer
{
   public class ObjectS
    {
        public Texture2D texture;
        public Body body;
        public Vector2 origin;
        public Vector2 _Size;

        //public Input input;

        public float rotation;
        public float speed_rotation;


        public ObjectS(Texture2D txt,float Size=1f,String _type="null")
        {
            texture = txt;
            //_Size = new Vector2(0.3f, 0.3f);
            //_SizeZ = new Vector2(0.5f, 0.5f);
            _Size = new Vector2(Size,Size);
            speed_rotation = 4f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }



        public virtual void Update(GameTime gameTime, Game1 game)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }     

       
    

    }




}

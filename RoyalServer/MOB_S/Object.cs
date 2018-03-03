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
   public class Object
    {
        public Texture2D texture;
        public Body body;
        public Vector2 origin;
        public Vector2 _Size;
        //public Input input;

        public float rotation;
        public float speed_rotation;
        public Vector2 centreScreen;

        public Object(Texture2D txt, Vector2 position) : this(txt, position, 0.0f)
        {
            texture = txt;
            _Size = new Vector2(0.3f, 0.3f);
            //_SizeZ = new Vector2(0.5f, 0.5f);
            speed_rotation = 4f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public Object(Texture2D texture, Vector2 position, float rotation)
        {
            //this.LoadTexture(texture);
            //this.body.Position = position;
            //this.body.Rotation = rotation;
        }

        public virtual void Update(GameTime gameTime, List<Object> gameobj)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }     

        public void LoadTexture(Texture2D texture)
        {
            this.texture = texture;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void LoadTexture(Texture2D texture, Vector2 origin)
        {
            this.LoadTexture(texture);
            this.origin = origin;
        }

      

    }




}

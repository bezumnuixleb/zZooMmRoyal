using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace RoyalServer
{
    class Object
    {
        //[eqqeqeqeqwe 
        //sdfsdfds
        protected Texture2D _texture;
        public Vector2 _position;
        public String _Type;
        public Object(Texture2D ntext)
        {
            _texture = ntext;
        }
        public virtual void Update(GameTime gameTime,List<Object> gameobj)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        //yarik dcp
    }
}

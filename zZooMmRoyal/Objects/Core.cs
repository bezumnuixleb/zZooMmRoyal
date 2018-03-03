using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMmRoyal
{
    public class Object
    {
        public Texture2D _texture;
        public Vector2 _position;
        public String _Type;
        public Vector2 Origin;
        public float _rotation = 3f;
        public float _size;
        public Object()
        {
            _texture = null ;
        }
        public Object(Texture2D ntext)
        {
            _texture = ntext;
        }
        public virtual void Update(GameTime gameTime, List<Object> gameobj)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }

    public enum ClientState
    {
        mainmenu,
        lobby,
        gameplay
    }
    public class Input
    {
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Up { get; set; }
        public Keys Down { get; set; }
        /// public Keys Shoot { get; set; }

    }
}

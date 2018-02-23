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
    class Player : Object
    {
        public String _name;
        public String _id;
        public Input _input;
        public KeyboardState _current;
        public KeyboardState _prev;

        public float _rotation = 3f;

        public Vector2 Origin;
        public Vector2 _Size;
        public Vector2 _mPosition;
        public Player(Texture2D text) : base(text)
        {

        }
        public Player() : base(null)
        {

        }
        public void Update(GameTime gameTime, List<Object> gameobj, KeyboardState playerkeys, Queue<String> msglist)
        {
            _prev = _current;
            _current = playerkeys;
            Move(_current, msglist);
            MouseMove(msglist);
        }
        public void MouseMove(Queue<String> msglist)
        {
            MouseState currentMouseState = Mouse.GetState();
            Vector2 _mPosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            String tmp = ""; tmp += _id + " MousePos "+_mPosition.X.ToString()+" "+_mPosition.Y.ToString();
            msglist.Enqueue(tmp);
        }
        public void Changes(String msg)
        {
            //"id _Player _PosX _PosY Rotation"
            String[] mas = msg.Split();

          

            if (_id != mas[0]) {//error kappa
                return;
            }
            
            //check type
            _position.X = Convert.ToSingle(mas[2]);
            _position.Y = Convert.ToSingle(mas[3]);
            _rotation = Convert.ToSingle(mas[4]);
            //other changes
        }

        public void Move(KeyboardState keybord, Queue<String> msglist)
        {
            if (_input == null)
                return;
            String tmp = "";
            if (keybord.IsKeyDown(_input.Left))
            {
                tmp += _id+ " ButtonChange " + "Left";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyDown(_input.Right))
            {
               tmp += _id + " ButtonChange " + "Right";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyDown(_input.Up))
            {
               tmp += _id + " ButtonChange " + "Up";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyDown(_input.Down))
            {
                tmp += _id + " ButtonChange " + "Down";
                msglist.Enqueue(tmp);
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, Origin, _Size, SpriteEffects.None, 0f);
        }
    }
}

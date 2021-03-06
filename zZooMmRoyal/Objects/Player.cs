﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMmRoyal
{
    public class Player : Object
    {
        public String _name;
        public String _id;
        public Input _input;
        public KeyboardState _current;
        public KeyboardState _prev;


        public Vector2 _Size;
        public Vector2 _mPosition;
        public Player(Texture2D text) : base(text)
        {

        }
        public Player() : base(null)
        {

        }
        public void Update(GameTime gameTime, List<Object> gameobj, KeyboardState playerkeys, Queue<String> msglist, Camera camera)
        {
            _prev = _current;
            _current = playerkeys;
            Move(_current, msglist,camera);
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

        public void Move(KeyboardState keybord, Queue<String> msglist, Camera camera)
        {
            if (_input == null)
                return;
            String tmp = "";

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.ScrollWheelValue > camera.Scroll)
            {
                camera._zoom += 0.03f;
                camera.Scroll = currentMouseState.ScrollWheelValue;
            }
            if (currentMouseState.ScrollWheelValue < camera.Scroll)
            {
                camera._zoom -= 0.03f;
                camera.Scroll = currentMouseState.ScrollWheelValue;
            }

            if (keybord.IsKeyDown(_input.Left)&&_prev.IsKeyUp(_input.Left))
            {
                tmp = _id+ " ButtonChange " + "Left_D";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyUp(_input.Left) && _prev.IsKeyDown(_input.Left))
            {
               tmp= _id + " ButtonChange " + "Left_U";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyDown(_input.Right) && _prev.IsKeyUp(_input.Right))
            {
               tmp = _id + " ButtonChange " + "Right_D";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyUp(_input.Right) && _prev.IsKeyDown(_input.Right))
            {
                tmp = _id + " ButtonChange " + "Right_U";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyDown(_input.Up) && _prev.IsKeyUp(_input.Up))
            {
               tmp = _id + " ButtonChange " + "Up_D";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyUp(_input.Up) && _prev.IsKeyDown(_input.Up))
            {
               tmp = _id + " ButtonChange " + "Up_U";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyDown(_input.Down) && _prev.IsKeyUp(_input.Down))
            {
                tmp = _id + " ButtonChange " + "Down_D";
                msglist.Enqueue(tmp);
            }
            if (keybord.IsKeyUp(_input.Down) && _prev.IsKeyDown(_input.Down))
            {
                tmp = _id + " ButtonChange " + "Down_U";
                msglist.Enqueue(tmp);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, _rotation, Origin, _Size, SpriteEffects.None, 0.5f);
        }
    }
}

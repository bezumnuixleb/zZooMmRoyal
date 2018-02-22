﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace zZooMmRoyal
{
    class star : Object
    {
        Input _input;
        float _speed=2f;
        KeyboardState _current;
        KeyboardState _prev;

        public star(Texture2D ntext) : base(ntext)
        {
            
        }
        public void Update(GameTime gameTime, List<Object> gameobj, KeyboardState playerkeys,Queue<Msg> msglist)
        {
            _prev = _current;
            _current = playerkeys;
            Move(_current,msglist);
        }

        public void Move(KeyboardState keybord, Queue<Msg> msglist)
        {
            if (_input == null)
                return;
            if (keybord.IsKeyDown(_input.Left))
            {

                msglist.Enqueue(new Msg{_type="Left"});
            }
            if (keybord.IsKeyDown(_input.Right))
            {
                msglist.Enqueue(new Msg { _type = "Right" });
            }
            if (keybord.IsKeyDown(_input.Up))
            {
                msglist.Enqueue(new Msg { _type = "Up" });
            }
            if (keybord.IsKeyDown(_input.Down))
            {
                msglist.Enqueue(new Msg { _type = "Down" });
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
        }

    }
    class Input
    {
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Up { get; set; }
        public Keys Down { get; set; }
        /// public Keys Shoot { get; set; }

    }
}

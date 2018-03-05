using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using zZooMmRoyal.Controls;

namespace zZooMmRoyal.States
{
    public class GameState : State
    {
        private List<Component> _components;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
           
            var buttonTexture = _content.Load<Texture2D>("Controls/Buttons");
            var buttonFont = _content.Load<SpriteFont>("Fonts/menu_font");

            var DisconnectButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(0, 0),
                Text = "Disconnect",
                _font = buttonFont,
            };

            DisconnectButton.Click += DisconnectButton_Click;
            _components= new List<Component>()
            {
                DisconnectButton
            };
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            _game.client.SendMessage(_game.player._id + " " + "disconnect");
            _game.ChangesState(new MenuState(_game, _graphicsDevice, _content));
            _game.currentState = ClientState.mainmenu;
           
            //game ended
            
            _game.client.Disconnect();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_game.objlist.Count == 0) return;
                if (_game.flag) return;
            

            spriteBatch.Begin(SpriteSortMode.FrontToBack,
                             null,
                              null, null, null, null,
                              _game.camera._Ttansfor);


            foreach (var item in _game.backobjlist)
            {
               spriteBatch.Draw(item._texture, item._position, null, Color.White, 0f, new Vector2(item._texture.Width/2, item._texture.Height / 2), 1f, SpriteEffects.None, 0f);
               
            }
            foreach (var copmonent in _components)
                copmonent.Draw(gameTime, spriteBatch);

            _game.player.Draw(spriteBatch);
            _game.msgchecker.Suspend();
            if (_game.flag)
            {
                _game.msgchecker.Resume();
                // TODO: Add your drawing code here

                spriteBatch.End();
                return;
            }
            if (_game.objlist.Count != 0) {
                _game.GraphicsDevice.Clear(Color.Red);
                foreach (var obj in _game.objlist)
            {
                #region Objects

                if (obj._Type == "Other_Player")
                {
                    obj._texture = _game.textures.Player_1;
                    obj.Origin = new Vector2(obj._texture.Width / 2, obj._texture.Height / 2);

                    spriteBatch.Draw(obj._texture, obj._position, null, Color.White, obj._rotation, obj.Origin, obj._size, SpriteEffects.None, 0.5f);

                }
                if (obj._Type == "Mob_Zombie")
                {
                    obj._texture = _game.textures.Zombie_1;
                    obj.Origin = new Vector2(obj._texture.Width / 2, obj._texture.Height / 2);

                    spriteBatch.Draw(obj._texture, obj._position, null, Color.White, obj._rotation, obj.Origin,obj._size, SpriteEffects.None, 0.5f);

                }
                if (obj._Type == "Box_2")
                {
                    obj._texture = _game.textures.Box_2;
                    obj.Origin = new Vector2(obj._texture.Width / 2, obj._texture.Height / 2);

                    spriteBatch.Draw(obj._texture, obj._position, null, Color.White, obj._rotation, obj.Origin, obj._size, SpriteEffects.None, 0.5f);

                }
                
                if (obj._Type == "Tree_1")
                {
                    obj._texture = _game.textures.Tile_1;
                    obj.Origin = new Vector2(obj._texture.Width / 2, obj._texture.Height / 2);

                    spriteBatch.Draw(obj._texture, obj._position, null, Color.White, obj._rotation, obj.Origin, obj._size, SpriteEffects.None, 1f);

                }
                #endregion
            }
            }
            _game.msgchecker.Resume();
               // TODO: Add your drawing code here

            spriteBatch.End();

        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var copmonent in _components)
                copmonent.Update(gameTime);

            _game.client.SendMessage(_game.player._id + " " + "giveINFO");
            _game.player.Update(gameTime, _game.objlist, Keyboard.GetState(), _game.msglist, _game.camera);

            _game.camera.Update(_game.player._position);

            foreach (var msg in _game.msglist)
            {
                if (_game.player._id != null)
                    _game.client.SendMessage(msg);

            }
            _game.msglist.Clear();
        }
    }
}

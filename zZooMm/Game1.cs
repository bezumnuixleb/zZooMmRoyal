using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rastating;
using System;
using System.Collections.Generic;
using System.Threading;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

namespace zZooMm001
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        
        SpriteBatch spriteBatch;

        private List<Plain> _Plain;
        private List<MobZombie> _Zombie;

        private Matrix _view;//
        private Vector2 _cameraPosition;//
        private Vector2 _screenCenter;//
        private readonly World _world;//
        public List<ObjectNew> zombelest;
        public ObjectNew player;
        public ObjectNew zombie,zombie2;
        public Color changedColor;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            _world = new World(new Vector2(0, 0));//
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            zombelest = new List<ObjectNew>();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.IsMouseVisible = true;
            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);//
            _view = Matrix.Identity;//
            _cameraPosition = Vector2.Zero;//
            ConvertUnits.SetDisplayUnitToSimUnitRatio(200f);//

            var texture = Content.Load<Texture2D>("New_1");
            var textureZ = Content.Load<Texture2D>("Zombie");

            //_Plain = new List<Plain>()
            //{
            //    new Plain(texture) {_position = new Vector2(600,600), _speed = 5f, _input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S}, _Size = new Vector2(1.0f,1.0f), Origin = new Vector2(texture.Width/2,texture.Height/2),_rotation = 0},
            //};
            //_Zombie = new List<MobZombie>()
            //{
            //    new MobZombie(textureZ) {_position = new Vector2(100,100), _speed = 2f, _Size = new Vector2(1.0f,1.0f), Origin = new Vector2(textureZ.Width/2,textureZ.Height/2),_rotation = 0},
            //    new MobZombie(textureZ) {_position = new Vector2(0,0), _speed = 2f, _Size = new Vector2(1.0f,1.0f), Origin = new Vector2(textureZ.Width/2,textureZ.Height/2),_rotation = 0},
            //};
            player = new ObjectNew(texture, new Vector2(600, 600), _world)
            {
                _Size=new Vector2(0.3f,0.3f),
                input=new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S }
            };
            zombie = new ObjectNew(textureZ, new Vector2(200, 150),_world);
            zombie2 = new ObjectNew(textureZ, new Vector2(200, 150), _world);
            for (int i = 0; i < 6; i++)

            {
                Random rnd = new Random();
                Thread.Sleep(20);
                Vector2 _position = new Vector2();
                _position.X = rnd.Next(0, 500);
                _position.Y = rnd.Next(0, 300);
                zombie = new ObjectNew(textureZ, _position, _world);
                zombelest.Add(zombie);
            }
        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.UpdateKeys();
            zombie.MoveToPlayer(player);
            zombie2.MoveToPlayer(player);
            foreach (var item in zombelest)
            {
                item.MoveToPlayer(player);

            }
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);///////////////////
            _cameraPosition.X = ConvertUnits.ToDisplayUnits(player.body.Position.X) ;
            _cameraPosition.Y = ConvertUnits.ToDisplayUnits(player.body.Position.Y);
            _view = Matrix.CreateTranslation(
                -ConvertUnits.ToDisplayUnits(player.body.Position.X),
                -ConvertUnits.ToDisplayUnits(player.body.Position.Y), 0) * Matrix.CreateTranslation(_screenCenter.X, _screenCenter.Y, 0);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(changedColor);

            //spriteBatch.Begin(SpriteSortMode.Texture, null, null, null, null, null, _view);
            spriteBatch.Begin();
           spriteBatch.Draw(player.texture, ConvertUnits.ToDisplayUnits(player.body.Position), null, Color.White, player.rotation, player.origin,player._Size, SpriteEffects.None, 0f);
            spriteBatch.Draw(zombie.texture, ConvertUnits.ToDisplayUnits(zombie.body.Position), null, Color.White, zombie.rotation, zombie.origin, zombie._Size, SpriteEffects.None, 0f);
            spriteBatch.Draw(zombie2.texture, ConvertUnits.ToDisplayUnits(zombie2.body.Position), null, Color.White, zombie2.rotation, zombie2.origin, zombie2._Size, SpriteEffects.None, 0f);
            foreach (var zombie in zombelest)
            {
                spriteBatch.Draw(zombie.texture, ConvertUnits.ToDisplayUnits(zombie.body.Position), null, Color.White, zombie.rotation, zombie.origin, zombie._Size, SpriteEffects.None, 0f);

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

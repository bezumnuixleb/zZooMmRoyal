using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rastating;
using System;
using System.Collections.Generic;
using System.Threading;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace zZooMm001
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        
        SpriteBatch spriteBatch;
        Camera camera;
        private List<Plain> _Plain;
        private List<MobZombie> _Zombie;
        public float cooldown = 0f;
        private Matrix _view;//
        private Vector2 _cameraPosition;//
        private Vector2 _screenCenter;//
        private readonly World _world;//
        public List<ObjectNew> zombelest;
        public ObjectNew player;
        public ObjectNew zombie,zombie2;
        public Color changedColor;
        public List<Tile> tiles;
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
            camera = new Camera(GraphicsDevice.Viewport);
            zombelest = new List<ObjectNew>();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.IsMouseVisible = true;
            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);//
            _view = Matrix.Identity;//
            _cameraPosition = Vector2.Zero;//
            ConvertUnits.SetDisplayUnitToSimUnitRatio(200f);//

            var texture = Content.Load<Texture2D>("New_1");
            var groundtexture = Content.Load<Texture2D>("grass_tile");
            var textureZ = Content.Load<Texture2D>("ugnd");
            tiles = new List<Tile>();
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    Tile tmp = new Tile(groundtexture, new Vector2(groundtexture.Width*i,groundtexture.Height*j));
                    tiles.Add(tmp);
                }
            }
            player = new ObjectNew(texture, new Vector2(600, 600), _world)
            {
                _Size = new Vector2(0.3f, 0.3f),
                input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S },
                centreScreen = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2),

            };
            player.body= BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(texture.Width / 2 * player._Size.X), 3f, ConvertUnits.ToSimUnits(new Vector2(600, 600)), BodyType.Dynamic);//)
            player.body.UserData="Player";
            player.body.Restitution = 0.3f;
            player.body.Friction = 0.5f;
            for (int i = 0; i < 300; i++)

            {
                Random rnd = new Random();
                Thread.Sleep(20);
                Vector2 _position = new Vector2();
                _position.X = rnd.Next(-1540*10, 1540 * 10);
                _position.Y = rnd.Next(-1190*10, 1190 * 10);
               
                zombie = new ObjectNew(textureZ, _position, _world);
                zombie.body = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(zombie.texture.Width / 2 * zombie._Size.X), 0.2f, ConvertUnits.ToSimUnits(_position), BodyType.Dynamic);
                zombie.body.Restitution = 0.3f;
                zombie.body.Friction = 0.5f;
                zombelest.Add(zombie);
                zombie.body.UserData = "Zombie";
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
            foreach (var item in zombelest)
            {
                item.MoveToPlayer(player);

            }
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.ScrollWheelValue > camera.Scroll)
            {
                camera._zoom += 0.03f;
                camera.Scroll = currentMouseState.ScrollWheelValue;
            }
            if (currentMouseState.ScrollWheelValue <camera.Scroll)
            {
                camera._zoom -= 0.03f;
                camera.Scroll = currentMouseState.ScrollWheelValue;
            }
            cooldown++;
            changedColor = Color.CornflowerBlue;
            player.body.OnCollision += Body_OnCollision;
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);///////////////////
            camera.Update(new Vector2(ConvertUnits.ToDisplayUnits(player.body.Position.X), ConvertUnits.ToDisplayUnits(player.body.Position.Y)));
            _view = Matrix.CreateTranslation(
                -ConvertUnits.ToDisplayUnits(player.body.Position.X),
                -ConvertUnits.ToDisplayUnits(player.body.Position.Y), 0) * Matrix.CreateTranslation(_screenCenter.X, _screenCenter.Y, 0);
            base.Update(gameTime);
        }

        private void Body_OnCollision(Fixture fixtureA, Fixture fixtureB, VelcroPhysics.Collision.ContactSystem.Contact contact)
        {
           
            if ((string)fixtureB.Body.UserData== (string)"Zombie"&& cooldown>20)
            {
                cooldown = 0;
                Vector2 mousePosition = ConvertUnits.ToDisplayUnits(fixtureB.Body.Position);

                Vector2 direction = mousePosition - ConvertUnits.ToDisplayUnits(fixtureA.Body.Position);
                direction.Normalize();
                float rotation = fixtureB.Body.Rotation;
                // движение к персонажу 
                
                changedColor = Color.Red;
                fixtureB.Body.ApplyLinearImpulse(-(new Vector2((float)Math.Sin(rotation) * 10f, (float)Math.Cos(rotation) * 10f)));
                fixtureA.Body.ApplyLinearImpulse(new Vector2((float)Math.Sin(rotation) * 30f, (float)Math.Cos(rotation) *30f));
               // fixtureA.Body.ApplyLinearImpulse(fixtureB.Body.Rotation)
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(changedColor);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                              null,
                               null, null, null, null,
                               camera._Ttansfor);
            foreach (var tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
            spriteBatch.Draw(player.texture, ConvertUnits.ToDisplayUnits(player.body.Position), null, Color.White, player.current_rotation, player.origin,player._Size, SpriteEffects.None, 0f);
            foreach (var zombie in zombelest)
            {
                spriteBatch.Draw(zombie.texture, ConvertUnits.ToDisplayUnits(zombie.body.Position), null, Color.White, zombie.rotation, zombie.origin, zombie._Size, SpriteEffects.None, 0f);

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

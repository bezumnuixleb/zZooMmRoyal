using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rastating;
using System;
using System.Collections.Generic;
using System.Threading;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Shared;
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

        //dlya collis
        public struct TestS
        {
            public Texture2D Box_6txt;
            public Body Box_6;
            public Body Case;

        };
        public TestS test;

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
            _screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            _view = Matrix.Identity;
            _cameraPosition = Vector2.Zero;
            ConvertUnits.SetDisplayUnitToSimUnitRatio(200f);

            var texture = Content.Load<Texture2D>("New_1");
            var groundtexture = Content.Load<Texture2D>("grass_tile");
            var textureZ = Content.Load<Texture2D>("ugnd");
            tiles = new List<Tile>();
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    Tile tmp = new Tile(groundtexture, new Vector2(groundtexture.Width*i,groundtexture.Height*j));
                   // tiles.Add(tmp);
                }
            }
            //отрисовка только одного тайла
            Tile casetile = new Tile(groundtexture, new Vector2(0, 0));

            #region Ящики состоящие из двух 4х угольников с размером в 5 раз больше текстуры
            tiles.Add(casetile);
            test = new TestS();
            test.Box_6txt= Content.Load<Texture2D>("box_2");
            Vertices vertices1 = new Vertices(4);
            vertices1.Add(ConvertUnits.ToSimUnits(new Vector2(-72,-29)) * 5f);
            vertices1.Add(ConvertUnits.ToSimUnits(new Vector2(-24, -60)) * 5f);
            vertices1.Add(ConvertUnits.ToSimUnits(new Vector2(19, -8)) * 5f);
            vertices1.Add(ConvertUnits.ToSimUnits(new Vector2(-37, 29)) * 5f);
            Vertices vertices2 = new Vertices(4);
            vertices2.Add(ConvertUnits.ToSimUnits(new Vector2(56, -14)) * 5f);
            vertices2.Add(ConvertUnits.ToSimUnits(new Vector2(81, 42)) * 5f);
            vertices2.Add(ConvertUnits.ToSimUnits(new Vector2(22, 64)) * 5f);
            vertices2.Add(ConvertUnits.ToSimUnits(new Vector2(-6, 12)) * 5f);
           
            List<Vertices> boxlist = new List<Vertices>(2);
            boxlist.Add(vertices1);
            boxlist.Add(vertices2);
            test.Box_6 = BodyFactory.CreateCompoundPolygon(_world, boxlist, 2f);
            test.Box_6.BodyType = BodyType.Static;
            test.Box_6.Position = ConvertUnits.ToSimUnits(new Vector2(600, 300));
            #endregion

            #region Границы по размеру 1 тайла земли
            test.Case = BodyFactory.CreateBody(_world);
            {
                Vertices casevert = new Vertices();
                casevert.Add(ConvertUnits.ToSimUnits(new Vector2(0, 0)));
                casevert.Add(ConvertUnits.ToSimUnits(new Vector2(0, 1190)));
                casevert.Add(ConvertUnits.ToSimUnits(new Vector2(1540, 1190)));
                casevert.Add(ConvertUnits.ToSimUnits(new Vector2(1540, 0)));
                casevert.Add(ConvertUnits.ToSimUnits(new Vector2(0, 0)));

                for (int i = 0; i < casevert.Count - 1; ++i)
                {
                    FixtureFactory.AttachEdge(casevert[i], casevert[i + 1], test.Case);
                }
            }
            test.Case.BodyType = BodyType.Static;
            #endregion

            #region Игрок - 8угольная фигура
            player = new ObjectNew(texture, new Vector2(600, 600), _world)
            {
                _Size = new Vector2(0.5f, 0.5f),
                input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S },
                centreScreen = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2),

            };
            Vertices bodyvert = new Vertices(8);
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-90, 51)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-59, -49)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(22, -104)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(60, -63)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(86, 4)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(77,81)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(9, 105)));
            bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-44, 109)));

            PolygonShape playershape = new PolygonShape(bodyvert, 1f);

            player.body = BodyFactory.CreateBody(_world);
            player.body.CreateFixture(playershape);

            player.body.BodyType = BodyType.Dynamic;
            player.body.UserData = "Player";
            player.body.Restitution = 0.3f;
            player.body.Friction = 0.5f;
            player.body.Position = new Vector2(5f, 5f);
            #endregion

            #region Player(old collision)
            //player = new ObjectNew(texture, new Vector2(600, 600), _world)
            //{
            //    _Size = new Vector2(0.3f, 0.3f),
            //    input = new Input { Left = Keys.A, Right = Keys.D, Up = Keys.W, Down = Keys.S },
            //    centreScreen = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2),

            //};
            //player.body= BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(texture.Width / 2 * player._Size.X), 3f, ConvertUnits.ToSimUnits(new Vector2(600, 600)), BodyType.Dynamic);//)
            //player.body.UserData="Player";
            //player.body.Restitution = 0.3f;
            //player.body.Friction = 0.5f;
            #endregion

            #region Zombe(old collisions)
            for (int i = 0; i < 10; i++)

            {
                Random rnd = new Random();
                Thread.Sleep(20);
                Vector2 _position = new Vector2();
                //_position.X = rnd.Next(-1540*10, 1540 * 10);
                //_position.Y = rnd.Next(-1190*10, 1190 * 10);

                _position.X = rnd.Next(-100, 200);
                _position.Y = rnd.Next(-100, 200);

                zombie = new ObjectNew(textureZ, _position, _world);
                zombie.body = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(zombie.texture.Width / 2 * zombie._Size.X), 0.2f, ConvertUnits.ToSimUnits(_position), BodyType.Dynamic);
                zombie.body.Restitution = 0.3f;
                zombie.body.Friction = 0.5f;
                zombelest.Add(zombie);
                zombie.body.UserData = "Zombie";
            }
            #endregion
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

            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            
            //camera change
            camera.Update(new Vector2(ConvertUnits.ToDisplayUnits(player.body.Position.X), ConvertUnits.ToDisplayUnits(player.body.Position.Y)));
            _view = Matrix.CreateTranslation(
                -ConvertUnits.ToDisplayUnits(player.body.Position.X),
                -ConvertUnits.ToDisplayUnits(player.body.Position.Y), 0) * Matrix.CreateTranslation(_screenCenter.X, _screenCenter.Y, 0);
            base.Update(gameTime);
        }

        #region Толкание зомби и игрока при коллизии
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
        #endregion

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
            spriteBatch.Draw(player.texture, ConvertUnits.ToDisplayUnits(player.body.Position), null, Color.White, player.body.Rotation, player.origin,player._Size, SpriteEffects.None, 0f);

            //korobki blet
            spriteBatch.Draw(test.Box_6txt, ConvertUnits.ToDisplayUnits(test.Box_6.Position), null, Color.White, test.Box_6.Rotation, new Vector2(91,67),5f, SpriteEffects.None, 0f);
            foreach (var zombie in zombelest)
            {
                spriteBatch.Draw(zombie.texture, ConvertUnits.ToDisplayUnits(zombie.body.Position), null, Color.White, zombie.body.Rotation, zombie.origin, zombie._Size, SpriteEffects.None, 0f);

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

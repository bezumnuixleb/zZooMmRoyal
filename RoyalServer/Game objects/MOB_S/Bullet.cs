using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

namespace RoyalServer.Game_objects.MOB_S
{
    public class Bullet: ObjectS
    {
        public float _timer;
        public float LifeSpan = 4f;

        public Bullet(Texture2D txt, World _world,TipTela _type = TipTela.Bullet_1) : base(txt)
        {
            _timer = 0;
            texture = txt;
            //_Size = new Vector2(0.2f, 0.2f);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            body = BodyConstructor.CreateBody(_type, _world, this, 0.02f);
        }
        public override void Update(GameTime gameTime, Game1 game)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > LifeSpan)
                isRemoved = true;

            body.ResetDynamics();
            body.ApplyLinearImpulse(new Vector2((float)Math.Sin(MathHelper.ToRadians(90) - body.Rotation) * 100f, (float)Math.Cos(MathHelper.ToRadians(90) - body.Rotation) * 100f));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(body.Position), null, Color.White, body.Rotation, origin, _Size, SpriteEffects.None, 0f);
        }
    }
}

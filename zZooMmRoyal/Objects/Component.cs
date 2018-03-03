using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMmRoyal
{
    public abstract class Component
    {
        public abstract void Draw(GameTime gamTime, SpriteBatch sptiteBatch);

        public abstract void Update(GameTime gameTime);
    }
}

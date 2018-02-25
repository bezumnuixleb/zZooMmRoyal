using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace RoyalServer
{
   public class Object
    {
        public Texture2D _texture;
        public Vector2 _position;
        public String _Type;
        public Vector2 Origin;
        public float _rotation = 3f;

        public Color[] textureData;
        public Vector2 _Size;

        #region Properties



        #region Constructors


        public Object(Texture2D texture, Vector2 position) : this(texture, position, 0.0f)
        {
        }

        public Object(Texture2D texture, Vector2 position, float rotation)
        {
            this.LoadTexture(texture);
            this._position = position;
            this._rotation = rotation;
        }

        #endregion

        public Rectangle Rect
        {
            get { return new Rectangle(0, 0, this._texture.Width, this._texture.Height); }
        }
        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-this.Origin, 0.0f)) *
                    Matrix.CreateScale(_Size.X, _Size.Y, 1) *
                                        Matrix.CreateRotationZ(this._rotation) *
                                        Matrix.CreateTranslation(new Vector3(this._position, 0.0f));
            }
        }

        public Rectangle BoundingRectangle
        {
            get { return CalculateBoundingRectangle(this.Rect, this.Transform); }
        }

        #endregion


        public virtual void Update(GameTime gameTime,List<Object> gameobj)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }


        public void LoadTexture(Texture2D texture)
        {
            this._texture = texture;
            this.Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.textureData = new Color[texture.Width * texture.Height];
            this._texture.GetData(this.textureData);
        }

        public void LoadTexture(Texture2D texture, Vector2 origin)
        {
            this.LoadTexture(texture);
            this.Origin = origin;
        }

        #region PixelCollision

        public bool IsColliding(Object collidable)
        {
            bool retval = false;

            if (this.BoundingRectangle.Intersects(collidable.BoundingRectangle))
            {
                if (IntersectPixels(this.Transform, this._texture.Width, this._texture.Height, this.textureData, collidable.Transform, collidable._texture.Width, collidable._texture.Height, collidable.textureData))
                {
                    retval = true;
                }
            }

            return retval;
        }





        public static bool IntersectPixels(Matrix transformA, int widthA, int heightA, Color[] dataA, Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        #endregion
    }



    public enum ServerState
    {
        waitingPlayers,
        lobby,
        gameplay
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zZooMm001
{
    public class Camera
    {
        //matr
        private Matrix _transform;
        public int Scroll = 0;

        public Matrix _Ttansfor
        {
            get { return _transform; }
        }

        private Vector2 _centre;
        private Viewport _viewport;

        private float zoom = 1f;
        private float rotation_camera = 0;

        public float X
        {
            get { return _centre.X; }
            set { _centre.X = value; }
        }

        public float Y
        {
            get { return _centre.Y; }
            set { _centre.Y = value; }
        }

        public float _zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.5f) zoom = 0.5f;
                if (zoom > 1.5f) zoom = 1.5f;
                // проверка на макс приближение камеры 

            }
        }

        public float Rotation_Camera
        {
            get { return rotation_camera; }
            set { rotation_camera = value; }
        }

        public Camera(Viewport NewViewport)
        {
            _viewport = NewViewport;
        }

        public void Update(Vector2 position)
        {
        
            _centre = new Vector2(position.X, position.Y);

            _transform = Matrix.CreateTranslation(new Vector3(-_centre.X, -_centre.Y, 0)) * 
                Matrix.CreateRotationZ(rotation_camera) *
                Matrix.CreateScale(new Vector3 (zoom,zoom,0))*
                Matrix.CreateTranslation(new Vector3 (_viewport.Width/2, _viewport.Height/2,0));
        }
    }
}

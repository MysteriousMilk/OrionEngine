//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Orion.Core
//{
//    public class Camera
//    {
//        #region Fields
//        protected float _zoom;
//        protected Matrix _transform;
//        protected Matrix _inverseTransform;
//        protected Vector2 _pos;
//        protected float _rotation;
//        protected Viewport _viewport;
//        protected Int32 _scroll;
//        #endregion

//        #region Properties
//        public float Zoom
//        {
//            get { return _zoom; }
//            set { _zoom = value; }
//        }

//        /// <summary>
//        /// Camera View Matrix Property
//        /// </summary>
//        public Matrix Transform
//        {
//            get { return _transform; }
//            set { _transform = value; }
//        }

//        /// <summary>
//        /// Inverse of the view matrix, can be used to get objects screen coordinates
//        /// from its object coordinates
//        /// </summary>
//        public Matrix InverseTransform
//        {
//            get { return _inverseTransform; }
//        }

//        public Vector2 Position
//        {
//            get { return _pos; }
//            set { _pos = value; }
//        }

//        public float Rotation
//        {
//            get { return _rotation; }
//            set { _rotation = value; }
//        }
//        #endregion

//        #region Constructor
//        public Camera(Viewport viewport)
//        {
//            _zoom = 1.0f;
//            _scroll = 1;
//            _rotation = 0.0f;
//            _pos = Vector2.Zero;
//            _viewport = viewport;
//        }
//        #endregion

//        #region Methods
//        public void SetViewport(Viewport viewport)
//        {
//            _viewport = viewport;
//        }

//        /// <summary>
//        /// Update the camera view
//        /// </summary>
//        public void Update()
//        {
//            //Clamp zoom value
//            _zoom = MathHelper.Clamp(_zoom, 0.0f, 10.0f);

//            //Clamp rotation value
//            _rotation = ClampAngle(_rotation);

//            //Create view matrix
//            _transform =    Matrix.CreateRotationZ(_rotation) *
//                            Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
//                            Matrix.CreateTranslation(_pos.X + (_viewport.Width / 2f), _pos.Y + (_viewport.Height / 2f), 0);

//            //Update inverse matrix
//            _inverseTransform = Matrix.Invert(_transform);
//        }

//        /// <summary>
//        /// Clamps a radian value between -pi and pi
//        /// </summary>
//        /// <param name="radians">angle to be clamped</param>
//        /// <returns>clamped angle</returns>
//        protected float ClampAngle(float radians)
//        {
//            while (radians < -MathHelper.Pi)
//            {
//                radians += MathHelper.TwoPi;
//            }
//            while (radians > MathHelper.Pi)
//            {
//                radians -= MathHelper.TwoPi;
//            }
//            return radians;
//        }
//        #endregion

//        //public static Vector2 WorldToScreenCoords(Vector2 worldPos, GraphicsDeviceManager graphics)
//        //{
//        //    return worldPos - new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f,
//        //                                  graphics.GraphicsDevice.Viewport.Height / 2f);
//        //}
//    }
//}

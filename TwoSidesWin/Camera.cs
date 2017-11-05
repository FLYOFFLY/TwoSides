using Microsoft.Xna.Framework;

namespace TwoSides
{
    public class Camera
    {
        Matrix _translation;
        Vector2 _size;

        public Matrix GetViewTran(GraphicsDeviceManager graphics) => GetViewTran(graphics,Zoom);

        public Matrix GetViewTran(GraphicsDeviceManager graphics,float scale)
        {
            _size = new Vector2(graphics.PreferredBackBufferWidth / 2f,
                graphics.PreferredBackBufferHeight / 2f);
            _translation = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(90)) *
                Matrix.CreateScale(scale, scale, 1) *
                Matrix.CreateTranslation(new Vector3(_size, 0)); 
            _size.X = _size.X / Zoom;
            _size.Y = _size.Y / Zoom;
            return _translation;
        }
        public bool InView(Point target) => new Rectangle(GetLeftUpper.ToPoint(),_size.ToPoint()).Contains(target);
        public Matrix GetInverse() => Matrix.Invert(_translation);
        public Vector2 GetLeftUpper
        {
            get => new Vector2(Pos.X - _size.X, Pos.Y - _size.Y);
            set { Pos.X = value.X + _size.X;  Pos.Y = value.Y + _size.Y; }
           
        }
        public Vector2 GetRightDown
        {
            get => new Vector2(Pos.X + _size.X, Pos.Y + _size.Y);
            set { Pos.X = value.X - _size.X; Pos.Y = value.Y - _size.Y; }
        }
        public void LerpZoom(float newZoom, float t) => Zoom = MathHelper.Lerp(Zoom, newZoom, t);

        public float Zoom = 1f;

        public Vector2 Pos = new Vector2(0,0);
    }
}

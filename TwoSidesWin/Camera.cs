using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwoSides
{
    public class Camera
    {
        Matrix translation;
        Vector2 size;
        public Matrix getViewTran(GraphicsDeviceManager graphics) {
            return getViewTran(graphics,Zoom);
        }
        public Matrix getViewTran(GraphicsDeviceManager graphics,float scale)
        {
            size = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            translation = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) * Matrix.CreateRotationZ(MathHelper.ToRadians(0)) * Matrix.CreateScale(scale, scale, 1) * Matrix.CreateTranslation(new Vector3(size, 0)); ;
            size.X = size.X / Zoom;
            size.Y = size.Y / Zoom;
            return translation;
        }
        public bool inView(Point target)
        {
            Vector2 start = new Vector2(getLeftUpper.X,getLeftUpper.Y);
            int width = (int)(getRightDown.X-getLeftUpper.X);
            int height = (int)(getRightDown.Y-getLeftUpper.Y);
            Rectangle screen = new Rectangle((int)start.X, (int)start.Y, width, height);
            return screen.Contains(target);
        }
        public Matrix getInverse()
        {
            
            return Matrix.Invert(translation);
        }
        public Vector2 getLeftUpper
        {
            get{return new Vector2(pos.X - size.X, pos.Y - size.Y);}
            set { pos.X = value.X + size.X;  pos.Y = value.Y + size.Y; }
           
        }
        public Vector2 getRightDown
        {
            get{return new Vector2(pos.X + size.X, pos.Y + size.Y);}
            set { pos.X = value.X - size.X; pos.Y = value.Y - size.Y; }
        }
        public void lerpZoom(float newZoom, float t)
        {
            //System.out.println(a * (1 - t) + b * t);
            Zoom = MathHelper.Lerp(Zoom, newZoom, t);
        }
        public float Zoom = 1f;

        public Vector2 pos = new Vector2(0,0);
    }
}

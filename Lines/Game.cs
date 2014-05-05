using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.UI;
using LiteEngine.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines
{
    class Game : LiteXnaEngine
    {
        Camera2D _camera;
        protected override void Initialize(XnaRenderer renderer)
        {
            _camera = new Camera2D(new Vector2(0, 0), new Vector2(80, 60));
            renderer.SetScreenSize(800, 600, false);
        }

        float _lineLength = 5f;
        float _angle;
        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            Vector2 start = new Vector2(0,0);
            Vector2 end = start + Util.AngleToVector(_angle) * _lineLength;
            renderer.BeginDraw(_camera);
            
            renderer.DrawLine(start, end, Color.Blue, 0.5f);

            renderer.DrawPoint(start, 2f, Color.Green, 0f);
            renderer.DrawPoint(end, 2f, Color.Red, 0f);
            renderer.EndDraw();
        }

        protected override void UpdateFrame(GameTime gameTime)
        {
            _angle += 0.05f;
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            if (key == Keys.M)
                _lineLength += 1f;
            if (key == Keys.N)
                _lineLength -= 1f;
            if (key == Keys.Escape)
                Exit();
            return 0;
        }
    }
}

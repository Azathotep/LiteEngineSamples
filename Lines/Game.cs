using LiteEngine.Core;
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
            renderer.SetResolution(800, 600, false);
        }

        float _lineLength = 20f;
        float _angle;
        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.Clear(Color.White);
            Vector2 start = new Vector2(0,0);
            Vector2 end = start + Util.AngleToVector(_angle) * _lineLength;
            renderer.BeginDraw(_camera);

            renderer.DrawPoint(start, 3f, Color.Green, 1f);
            renderer.DrawPoint(end, 3f, Color.Red, 1f);
            renderer.DrawArrow(start, end, 0.5f, Color.Black, 1f);

            renderer.EndDraw();
        }

        protected override void UpdateFrame(GameTime gameTime)
        {
            _angle += 0.02f;
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

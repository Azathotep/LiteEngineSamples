using LiteEngine.Core;
using LiteEngine.Input;
using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireFlies
{
    class Game : LiteXnaEngine
    {
        List<FireFly> _fireFlies = new List<FireFly>();
        Camera2D _camera;
        
        protected override void Initialize(XnaRenderer renderer)
        {
            _camera = new Camera2D(new Vector2(0, 0), new Vector2(50, 50));
            renderer.SetScreenSize(1024, 768, false);
        }
        
        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.BeginDraw(_camera);
            foreach (FireFly fly in _fireFlies)
                fly.Draw(renderer);
            renderer.EndDraw();
        }

        protected override void UpdateFrame(GameTime gameTime)
        {
            foreach (FireFly fly in _fireFlies)
                fly.Update();
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            switch (key)
            {
                case Keys.Escape:
                    Exit();
                    break;
                case Keys.Z:
                    _camera.Zoom *= 1.1f;
                    break;
                case Keys.X:
                    _camera.Zoom *= 0.9f;
                    break;
                case Keys.F:
                    _fireFlies.Add(new FireFly());
                    return 0;
                case Keys.N:
                    for (int i=0;i<10;i++)
                        _fireFlies.Add(new FireFly());
                    return 0;
            }
            return 0;
        }
    }
}

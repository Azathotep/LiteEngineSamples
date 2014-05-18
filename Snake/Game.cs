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

namespace Snake
{
    class Game : LiteXnaEngine
    {
        Camera2D _camera;
        Snake _snake;
        WorldGrid _grid;

        protected override void Initialize(XnaRenderer renderer)
        {
            renderer.SetScreenSize(1024, 768, false);
            _camera = new Camera2D(new Vector2(0, 0), new Vector2(50, 50));
            _grid = new WorldGrid();

            _snake = new Snake();
        }

        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.BeginDraw(_camera);
            _grid.Draw(renderer);
            renderer.EndDraw();
        }

        protected override void UpdateFrame(GameTime gameTime)
        {
            
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            return 0;
        }
    }
}

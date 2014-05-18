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
            renderer.DrawDepth = 1f;
            _grid.Draw(renderer);
            renderer.DrawDepth = 0.5f;
            _snake.Draw(renderer);
            renderer.EndDraw();
        }

        int _lastMs;
        protected override void UpdateFrame(GameTime gameTime)
        {
            int delayMs = 100;
            double ms = gameTime.TotalGameTime.TotalMilliseconds;
            while (ms - _lastMs > delayMs)
            {
                _lastMs += delayMs;
                _snake.Update();
            }
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            switch (key)
            {
                case Keys.Up:
                    _snake.MoveDirection = CardinalDirection.North;
                    break;
                case Keys.Left:
                    _snake.MoveDirection = CardinalDirection.West;
                    break;
                case Keys.Right:
                    _snake.MoveDirection = CardinalDirection.East;
                    break;
                case Keys.Down:
                    _snake.MoveDirection = CardinalDirection.South;
                    break;
            }
            return 0;
        }
    }
}

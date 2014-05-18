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
            _grid = new WorldGrid(20,20);
            _camera = new Camera2D(new Vector2(10, 10), new Vector2(_grid.Size.Width, _grid.Size.Height));


            _snake = new Snake();
        }

        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.BeginDraw(_camera);
            renderer.DrawDepth = 1f;
            _grid.Draw(renderer);
            renderer.DrawDepth = 0.5f;
            renderer.Transformation = Matrix.Identity;
            _snake.Draw(renderer);
            renderer.EndDraw();
        }

        int _lastMs;
        int _delayMs = 100;

        protected override void UpdateFrame(GameTime gameTime)
        {
            double ms = gameTime.TotalGameTime.TotalMilliseconds;
            while (ms - _lastMs > _delayMs)
            {
                _lastMs += _delayMs;
                _snake.Update();
            }
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.W:
                    _snake.MoveDirection = CardinalDirection.North;
                    break;
                case Keys.Left:
                case Keys.A:
                    _snake.MoveDirection = CardinalDirection.West;
                    break;
                case Keys.Right:
                case Keys.D:
                    _snake.MoveDirection = CardinalDirection.East;
                    break;
                case Keys.Down:
                case Keys.S:
                    _snake.MoveDirection = CardinalDirection.South;
                    break;
                case Keys.Escape:
                    Exit();
                    break;
            }
            return 0;
        }
    }
}

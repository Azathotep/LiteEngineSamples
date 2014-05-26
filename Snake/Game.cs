using LiteEngine.Core;
using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Textures;
using LiteEngine.UI;
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
        Camera2D _screenCamera = new Camera2D(new Vector2(0, 0), new Vector2(800, 600));
        Camera2D _camera;
        Snake _snake;
        TileGrid _world;
        TextBox _txtBox;

        protected override void Initialize(XnaRenderer renderer)
        {
            renderer.SetResolution(1024, 768, false);
            TextureBook.AddSpriteSheet(@"textures\snake");
            _world = new TileGrid(20,20);
            _world.GetRandomEmptyTile().ContainsFood = true;
            _camera = new Camera2D(new Vector2(_world.Size.Width / 2, _world.Size.Height / 2), new Vector2(_world.Size.Width, _world.Size.Height));
            _snake = new Snake(this, _world);
            _snake.Place(_world.GetTile(10,10));
            _txtBox = new TextBox();
            _txtBox.Text = "5";
            _txtBox.TextScale = 5;
            _txtBox.AutoSize = true;
            _txtBox.TextColor = Color.Black;
            _txtBox.Position = new Vector2(0, 0);
            UserInterface.SetResolution(600, 480);
            this.UserInterface.AddChild(_txtBox);
        }

        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.BeginDraw(_camera);

            renderer.DrawDepth = 0.9f;
            _world.Draw(renderer);
            renderer.DrawDepth = 0.5f;
            renderer.Transformation = Matrix.Identity;
            _snake.Draw(renderer);

            
            renderer.EndDraw();
            
            
            //renderer.BeginDraw(_screenCamera);
            //renderer.DrawString("5", new Vector2(0, 0), Color.Black, 10);
            //renderer.EndDraw();
        }

        int _lastMs;
        int _delayMs = 100;

        protected override void UpdateFrame(GameTime gameTime)
        {
            double ms = gameTime.TotalGameTime.TotalMilliseconds;
            while (ms - _lastMs > _delayMs)
            {
                _lastMs += _delayMs;

                if (_snake.Length > 0)
                {
                    if (_snake.Length < 3)
                        _snake.ChopBody(this, _snake.Head);
                    else
                        _snake.Update(gameTime, this);
                }

                if (Dice.Next(10) == 1)
                    _world.GetRandomEmptyTile().ContainsFood = true;
            }
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.W:
                    _snake.ChangeDirection(CardinalDirection.North);
                    break;
                case Keys.Left:
                case Keys.A:
                    _snake.ChangeDirection(CardinalDirection.West);
                    break;
                case Keys.Right:
                case Keys.D:
                    _snake.ChangeDirection(CardinalDirection.East);
                    break;
                case Keys.Down:
                case Keys.S:
                    _snake.ChangeDirection(CardinalDirection.South);
                    break;
                case Keys.Escape:
                    Exit();
                    break;
            }
            return 0;
        }
    }
}

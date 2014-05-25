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

namespace Text
{
    class Game : LiteXnaEngine
    {
        Camera2D _camera;
        TextBox _textBox;
        protected override void Initialize(XnaRenderer renderer)
        {
            _camera = new Camera2D(new Vector2(100, 100), new Vector2(50, 80));
            //renderer.SetScreenSize(1024, 796, false);
            renderer.SetResolution(600, 480, false);
            _textBox = new TextBox();
            _textBox.Position = new Vector2(50, 50);
            _textBox.TextScale = 10f;
            _textBox.Text = "This is some text for the textbox";
            _textBox.TextColor = Color.White;
            
            _textBox.BorderWidth = 2f;
            UserInterface.AddChild(_textBox);
        }

        float _angle;
        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.BeginDraw(_camera);
            renderer.DrawPoint(new Vector2(100, 100) + Util.AngleToVector(_angle) * 20f, 5f, Color.Green, 1f);
            //_camera.Zoom = 0.5f;
            
            //renderer.DrawString("This is some text", new Vector2(0, 0), Color.Red);
            
            renderer.EndDraw();
        }

        protected override void UpdateFrame(GameTime gameTime)
        {
            _angle += 0.01f;
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            if (key == Keys.Escape)
                Exit();
            if (key == Keys.N)
            {
                _textBox.Size = new SizeF(_textBox.Size.Width - 5f, _textBox.Size.Height);
                return 0;
            }
            if (key == Keys.M)
            {
                _textBox.Size = new SizeF(_textBox.Size.Width + 5f, _textBox.Size.Height);
                return 0;
            }
            if (key == Keys.D1)
            {
                _textBox.TextScale = 1f;
                return -1;
            }
            if (key == Keys.D2)
            {
                _textBox.TextScale = 1.5f;
                return -1;
            }
            if (key == Keys.D3)
            {
                _textBox.TextScale = 2f;
                return -1;
            }
            if (key == Keys.D4)
            {
                _textBox.TextScale = 5f;
                return -1;
            }


            if (key == Keys.D7)
            {
                Renderer.SetResolution(640, 480, true);
                return -1;
            }
            if (key == Keys.D8)
            {
                Renderer.SetResolution(800, 600, true);
                return -1;
            }
            if (key == Keys.D9)
            {
                Renderer.SetResolution(1200, 1000, true);
                return -1;
            }
            if (key == Keys.D0)
            {
                Renderer.SetResolution(1600, 1200, true);
                return -1;
            }
            if (key == Keys.Space)
            {
                _textBox.Text += " ";
                return 20;
            }

            _textBox.Text += key.ToString();
            return 20;
        }
    }
}

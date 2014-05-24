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

namespace Text
{
    class Game : LiteXnaEngine
    {
        TextBox _textBox;
        protected override void Initialize(XnaRenderer renderer)
        {
            renderer.SetResolution(800, 600, false);
            _textBox = new TextBox();
            _textBox.Position = new Vector2(0, 0);
            _textBox.Text = "This is some text for the textbox";
            _textBox.TextColor = Color.White;
            _textBox.TextScale = 1f;
            _textBox.AutoSize = true;
            _textBox.BorderWidth = 2f;
            _textBox.Dock = DockPosition.Center;
            UserInterface.AddChild(_textBox);

            ImageControl image = new ImageControl(new Texture("point"));
            image.Size = new SizeF(300, 300);
            image.Dock = DockPosition.Center;
            UserInterface.AddChild(image);
        }

        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
        }

        protected override void UpdateFrame(GameTime gameTime)
        {
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            if (key == Keys.Escape)
                Exit();
            return 0;
        }
    }
}

using LiteEngine.Input;
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
            UserInterface.ShowMouseCursor = true;
            _textBox = new TextBox();
            _textBox.Text = "This textbox is docked in the center of the screen";
            _textBox.AutoSize = true;
            _textBox.BackgroundColor = Color.Azure;
            _textBox.BorderWidth = 2;
            _textBox.Dock = DockPosition.Center;
            UserInterface.AddChild(_textBox);

            TextBox topText = CreateTextBox();
            topText.Text = "This textbox is docked at the top of the screen";
            topText.Dock = DockPosition.Top;
            topText.AutoSize = true;
            UserInterface.AddChild(topText);

            TextBox rightText = CreateTextBox();
            rightText.Text = "This textbox is docked at the right of the screen";
            rightText.Dock = DockPosition.Right;
            rightText.Size = new SizeF(100, 0);
            UserInterface.AddChild(rightText);

            TextBox leftText = CreateTextBox();
            leftText.Text = "This textbox is docked at the left of the screen";
            leftText.Dock = DockPosition.Left;
            leftText.Size = new SizeF(100, 0);
            UserInterface.AddChild(leftText);

            TextBox bottomText = CreateTextBox();
            bottomText.Text = "This textbox is docked at the bottom of the screen";
            bottomText.Dock = DockPosition.Bottom;
            bottomText.AutoSize = true;
            UserInterface.AddChild(bottomText);

            TextBox topLeft = CreateTextBox();
            topLeft.Text = "This textbox is docked at the top left of the screen";
            topLeft.Size = new SizeF(150, 0);
            topLeft.Dock = DockPosition.TopLeft;
            UserInterface.AddChild(topLeft);

            TextBox topRight = CreateTextBox();
            topRight.Text = "This textbox is docked at the top right of the screen";
            topRight.Size = new SizeF(150, 0);
            topRight.Dock = DockPosition.TopRight;
            UserInterface.AddChild(topRight);

            TextBox bottomRight = CreateTextBox();
            bottomRight.Text = "This textbox is docked at the bottom right of the screen";
            bottomRight.Size = new SizeF(150, 0);
            bottomRight.Dock = DockPosition.BottomRight;
            UserInterface.AddChild(bottomRight);

            TextBox bottomLeft = CreateTextBox();
            bottomLeft.Text = "This textbox is docked at the bottom left of the screen";
            bottomLeft.Size = new SizeF(150, 0);
            bottomLeft.Dock = DockPosition.BottomLeft;
            UserInterface.AddChild(bottomLeft);

            Button button = new Button();
            button.Bounds = new RectangleF(200, 150, 150, 100);
            button.OnClick = () =>
            {
                topText.Text += "!";
            };
            UserInterface.AddChild(button);

            TextBox buttonText = new TextBox();
            buttonText.Dock = DockPosition.Center;
            buttonText.Text = "This textbox is in a button";
            buttonText.AutoSize = true;
            button.AddChild(buttonText);
            
            //ImageControl image = new ImageControl(new Texture("point"));
            //image.Size = new SizeF(300, 300);
            //image.Dock = DockPosition.Center;
            //image.BackgroundColor = Color.PaleGreen;
            //image.BorderWidth = 3f;
            //UserInterface.AddChild(image);
        }

        protected override void OnMouseClick(MouseButton button, Vector2 mousePosition)
        {
            
        }

        TextBox CreateTextBox()
        {
            TextBox ret = new TextBox();
            ret.BackgroundColor = Color.Azure;
            ret.BorderWidth = 2;
            return ret;
        }

        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            renderer.Clear(Color.White);
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

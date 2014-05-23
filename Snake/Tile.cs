using LiteEngine.Math;
using LiteEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Tile
    {
        Vector2I _position;
        public Tile(int x, int y)
        {
            _position = new Vector2I(x,y);
        }

        public Vector2I Position
        {
            get
            {
                return _position;
            }
        }

        public int Anim;

        public void Draw(XnaRenderer renderer)
        {
            renderer.DrawDepth = 1f;
            Color tileColor = Color.White;
            if (IsWall)
                tileColor = Color.SlateGray;
            renderer.DrawFilledRectangle(new RectangleF(-0.45f, -0.45f, 0.9f, 0.9f), tileColor);
            renderer.DrawDepth = 0.8f;
            if (ContainsFood)
                renderer.DrawPoint(new Vector2(0,0), 1f, Color.Green, 1f);

            if (IsEmpty && Anim > 0)
            {
                float width = (float)Anim / 50 * 0.45f;
                renderer.DrawFilledRectangle(new RectangleF(-width, -width, width * 2, width * 2), Color.Gray);
                Anim--;
            }
        }

        public bool IsWall;

        public bool ContainsFood;

        public bool IsEmpty
        {
            get
            {
                if (IsWall)
                    return false;
                if (ContainsFood)
                    return false;
                return true;
            }
        }
    }
}

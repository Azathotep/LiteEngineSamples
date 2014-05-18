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
    class WorldGrid
    {
        SizeI _size = new SizeI(20, 20);

        public void Draw(XnaRenderer renderer)
        {
            for (int y = 0; y < _size.Height; y++)
                for (int x = 0; x < _size.Width; x++)
                {
                    renderer.DrawFilledRectangle(new RectangleF(x, y, 0.9f, 0.9f), Color.Red);
                }
        }
    }
}

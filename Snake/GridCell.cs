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
    class GridCell
    {
        public void Draw(XnaRenderer renderer)
        {
            renderer.DrawFilledRectangle(new RectangleF(0, 0, 0.9f, 0.9f), Color.Gray);
        }
    }
}

using LiteEngine.Core;
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
        SizeI _size;
        GridCell[,] _grid;
        public WorldGrid(int width, int height)
        {
            _size = new SizeI(20, 20);
            _grid = new GridCell[width, height];
            GridHelper.Foreach(_grid, (x, y) => { _grid[x,y] = new GridCell(); });
        }

        public SizeI Size
        {
            get
            {
                return _size;
            }
        }

        public void Draw(XnaRenderer renderer)
        {
            GridHelper.Foreach(_grid, (x, y) =>
            {
                renderer.Transformation = Matrix.CreateTranslation(x, y, 0);
                _grid[x, y].Draw(renderer);
            });
        }
    }
}

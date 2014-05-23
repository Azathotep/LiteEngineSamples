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
    class TileGrid
    {
        SizeI _size;
        Tile[,] _grid;
        public TileGrid(int width, int height)
        {
            _size = new SizeI(20, 20);
            _grid = new Tile[width, height];
            GridHelper.Foreach(_grid, (x, y) => 
            { 
                _grid[x,y] = new Tile(x,y);
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    _grid[x, y].AddImpenetrableWall();
            });

            for (int i=0;i<10;i++)
                GetRandomEmptyTile().AddWall(1);
        }

        internal Tile GetNeighbour(Tile tile, CardinalDirection direction)
        {
            Vector2 v = Compass.GetVector2(direction);
            return GetTile(tile.Position.X + (int)v.X, tile.Position.Y + (int)v.Y);
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
                renderer.Transformation = Matrix.CreateTranslation(x + 0.5f, y + 0.5f, 0);
                _grid[x, y].Draw(renderer);
            });
        }

        internal Tile GetTile(int x, int y)
        {
            return _grid[x, y];
        }

        IEnumerable<Tile> AllTiles
        {
            get
            {
                for (int y = 0; y <= _grid.GetUpperBound(1); y++)
                    for (int x = 0; x <= _grid.GetUpperBound(0); x++)
                    {
                        yield return GetTile(x, y);
                    }
            }
        }

        public Tile GetRandomEmptyTile()
        {
            var emptyTiles = (from t in AllTiles where t.IsEmpty select t).ToList();
            if (emptyTiles.Count == 0)
                return null;
            Tile tile = Dice.RandomElement(emptyTiles);
            return tile;
        }
    }
}

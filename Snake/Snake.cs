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
    class Snake
    {
        CardinalDirection _moveDirection;
        Queue<Vector2I> _body = new Queue<Vector2I>();

        public Snake()
        {
            _body.Enqueue(new Vector2I(5, 5));
            _body.Enqueue(new Vector2I(6, 5));
            _body.Enqueue(new Vector2I(7, 5));
        }

        public void Draw(XnaRenderer renderer)
        {
            foreach (Vector2I part in _body)
            {
                renderer.DrawFilledRectangle(new RectangleF(part.X + 0.1f, part.Y + 0.1f, 0.8f, 0.8f), Color.Green);
            }
        }

        Vector2I Head
        {
            get
            {
                return _body.Last();
            }
        }

        public CardinalDirection MoveDirection
        {
            get
            {
                return _moveDirection;
            }
            set
            {
                _moveDirection = value;
            }
        }

        public void Update(TileGrid world)
        {
            Vector2 move = Compass.GetVector(_moveDirection);
            Vector2I newHeadPosition = Head + move;
            Tile tile = world.GetTile(newHeadPosition.X, newHeadPosition.Y);
            if (tile.IsWall)
                return;

            if (tile.ContainsFood)
                tile.ContainsFood = false;
            else
                _body.Dequeue();
            _body.Enqueue(newHeadPosition);
        }
    }
}

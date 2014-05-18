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
                renderer.DrawFilledRectangle(new RectangleF(part.X, part.Y, 0.9f, 0.9f), Color.Green);
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

        public void Update()
        {
            Vector2 move = Compass.GetVector(_moveDirection);
            Vector2I newHead = Head + move;
            _body.Dequeue();
            _body.Enqueue(newHead);
        }
    }
}

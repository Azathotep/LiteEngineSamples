﻿using LiteEngine.Math;
using LiteEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        Queue<Tile> _body = new Queue<Tile>();
        TileGrid _world;

        public Snake(TileGrid world)
        {
            _world = world;
        }

        public void Place(Tile tile)
        {
            _body.Enqueue(tile);
            tile = _world.GetNeighbour(tile, CardinalDirection.East);
            _body.Enqueue(tile);
            tile = _world.GetNeighbour(tile, CardinalDirection.East);
            _body.Enqueue(tile);
        }

        public void Draw(XnaRenderer renderer)
        {
            foreach (Tile part in _body)
            {
                renderer.DrawFilledRectangle(new RectangleF(part.X + 0.1f, part.Y + 0.1f, 0.8f, 0.8f), Color.Green);
            }
        }

        Tile Head
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
        }

        public void Update(Game game)
        {
            Tile nextTile = _world.GetNeighbour(Head, _moveDirection);
            if (nextTile.IsWall)
            {
                //shorten snake
                _body.Dequeue();
                game.GetSoundEffect(@"audio\collide").Play(0.5f, 1f, 0f);
                return;
            }
            if (nextTile.ContainsFood)
            {
                game.GetSoundEffect(@"audio\eat").Play(0.5f, 0f, 0f);
                nextTile.ContainsFood = false;
                _growth++;
            }
            if (_body.Contains(nextTile))
            {
                game.GetSoundEffect(@"audio\collide").Play(0.5f, 1f, 0f);

                //cut off tail
                while (true)
                {
                    Tile tail = _body.Dequeue();
                    tail.Anim = 50;
                    if (tail == nextTile)
                        break;
                }
                return;
            }
            MoveTo(nextTile);
        }

        int _growth = 0;

        void MoveTo(Tile tile)
        {
            if (_growth == 0)
                _body.Dequeue();
            else
                _growth--;
            _body.Enqueue(tile);
        }

        internal void ChangeDirection(CardinalDirection direction)
        {
            Tile moveTo = _world.GetNeighbour(Head, direction);
            if (moveTo == _body.ElementAt(_body.Count - 2))
                return;
            _moveDirection = direction;
        }
    }
}

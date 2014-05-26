using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Textures;
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
        Animation _headAnimation = new Animation();

        CardinalDirection _moveDirection;
        Queue<Tile> _body = new Queue<Tile>();
        TileGrid _world;
        Texture _bodyNsTexture;
        Texture _bodyNeTexture;
        Texture _tailTexture;
        public Snake(Game game, TileGrid world)
        {
            _world = world;
            _headAnimation.AddFrame(TextureBook.GetTexture(@"textures\snake.head"), 20);
            _headAnimation.AddFrame(TextureBook.GetTexture(@"textures\snake.head_eat"), 20);
            _headAnimation.Loop = true;
            _bodyNsTexture = TextureBook.GetTexture(@"textures\snake.bodyNS");
            _bodyNeTexture = TextureBook.GetTexture(@"textures\snake.bodyNE");
            _tailTexture = TextureBook.GetTexture(@"textures\snake.tail");
        }

        public void Place(Tile tile)
        {
            _body.Enqueue(tile);
            tile = _world.GetNeighbour(tile, CardinalDirection.East);
            _body.Enqueue(tile);
            tile = _world.GetNeighbour(tile, CardinalDirection.East);
            _body.Enqueue(tile);
        }

        Texture _headTexture = new Texture(@"textures\snakehead");

        public void Draw(XnaRenderer renderer)
        {
            Tile[] parts = _body.ToArray();
            for (int i = 0; i < parts.Length; i++)
            {
                Texture texture;
                float angle = 0;
                Tile part = parts[i];
                if (i == parts.Length - 1)
                {
                    texture = _headAnimation.CurrentTexture;
                    angle = Compass.GetAngle(MoveDirection);
                }
                else if (i == 0)
                {
                    texture = _tailTexture;
                    CardinalDirection directionOfLast = Compass.GetDirection(parts[i + 1].Position - parts[i].Position);
                    angle = Compass.GetAngle(directionOfLast);
                }
                else
                {
                    CardinalDirection directionOfNext = Compass.GetDirection(parts[i - 1].Position - parts[i].Position);
                    CardinalDirection directionOfLast = Compass.GetDirection(parts[i + 1].Position - parts[i].Position);
                    if (Compass.GetOppositeDirection(directionOfLast) == directionOfNext)
                    {
                        texture = _bodyNsTexture;
                        if (directionOfNext == CardinalDirection.East || directionOfNext == CardinalDirection.West)
                            angle = MathHelper.PiOver2;
                    }
                    else
                    {
                        texture = _bodyNeTexture;
                        if (directionOfLast == CardinalDirection.North && directionOfNext == CardinalDirection.West)
                            angle = MathHelper.PiOver2 * 3;
                        if (directionOfLast == CardinalDirection.East && directionOfNext == CardinalDirection.South)
                            angle = MathHelper.PiOver2;
                        if (directionOfLast == CardinalDirection.South && directionOfNext == CardinalDirection.East)
                            angle = MathHelper.PiOver2;
                        if (directionOfLast == CardinalDirection.South && directionOfNext == CardinalDirection.West)
                            angle = MathHelper.Pi;
                        if (directionOfLast == CardinalDirection.West && directionOfNext == CardinalDirection.South)
                            angle = MathHelper.Pi;
                        if (directionOfLast == CardinalDirection.West && directionOfNext == CardinalDirection.North)
                            angle = MathHelper.PiOver2 * 3;
                    }
                }
                renderer.DrawSprite(texture, new Vector2(part.Position.X+0.5f, part.Position.Y+0.5f), new Vector2(1f,1f), angle, Color.Green, 1f);
            }
        }

        public Tile Head
        {
            get
            {
                return _body.Last();
            }
        }

        Tile Tail
        {
            get
            {
                return _body.Peek();
            }
        }
        
        public CardinalDirection MoveDirection
        {
            get
            {
                return _moveDirection;
            }
        }

        public void Update(GameTime gameTime, Game game)
        {
            _headAnimation.Advance(gameTime.ElapsedGameTime.Milliseconds);

            Tile nextTile = _world.GetNeighbour(Head, _moveDirection);
            if (nextTile.IsWall)
            {
                //hit a wall
                ChopBody(game, Tail);
                nextTile.TakeDamage(1);
                return;
            }
            if (_body.Contains(nextTile))
            {
                //hit self
                ChopBody(game, nextTile);
                return;
            }
            if (nextTile.ContainsFood)
            {
                game.GetSoundEffect(@"audio\eat").Play(0.5f, 0f, 0f);
                nextTile.ContainsFood = false;
                _growth++;
            }
            MoveTo(nextTile);
        }

        /// <summary>
        /// Chops the body by removing all parts from the specified part down
        /// </summary>
        public void ChopBody(Game game, Tile part)
        {
            game.GetSoundEffect(@"audio\collide").Play(0.5f, 1f, 0f);
            while (true)
            {
                Tile tail = _body.Dequeue();
                tail.Anim = 50;
                if (tail == part)
                    break;
            }
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
            if (Length == 0)
                return;
            Tile moveTo = _world.GetNeighbour(Head, direction);
            if (moveTo == _body.ElementAt(_body.Count - 2))
                return;
            _moveDirection = direction;
        }

        public int Length
        {
            get
            {
                return _body.Count;
            }
        }
    }
}

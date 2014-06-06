using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Textures;
using LiteEngine.UI;
using LiteEngine.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        Camera2D _camera;
        VertexPositionColorTexture[] _vertexData = new VertexPositionColorTexture[1000];
        short[] _indexData = new short[1000];
        Animation _stickmanAnimation = new Animation();

        protected override void Initialize(XnaRenderer renderer)
        {
            _camera = new Camera2D(new Vector2(50, 50), new Vector2(100, 100));
            renderer.SetResolution(800, 600, false);


            _vertexData[0].Position = new Vector3(0, 0, 0);
            _vertexData[0].TextureCoordinate = new Vector2(0, 0);
            _vertexData[0].Color = Color.White;

            _vertexData[1].Position = new Vector3(1, 0, 0);
            _vertexData[1].TextureCoordinate = new Vector2(1, 0);
            _vertexData[1].Color = Color.White;

            _vertexData[2].Position = new Vector3(0, 1, 0);
            _vertexData[2].TextureCoordinate = new Vector2(0, 1);
            _vertexData[2].Color = Color.White;

            _vertexData[3].Position = new Vector3(1, 1, 0);
            _vertexData[3].TextureCoordinate = new Vector2(1, 1);
            _vertexData[3].Color = Color.White;

            _indexData[0] = 0;
            _indexData[1] = 1;
            _indexData[2] = 2;
            _indexData[3] = 2;
            _indexData[4] = 1;
            _indexData[5] = 3;

            TextureBook.AddSpriteSheet(@"Textures\spritesheet1");
            _stickmanAnimation.AddFrame(TextureBook.GetTexture(@"Textures\spritesheet1.man2"), 1000);
            _stickmanAnimation.AddFrame(TextureBook.GetTexture(@"Textures\spritesheet1.man1"), 500);
            _stickmanAnimation.AddFrame(TextureBook.GetTexture(@"Textures\spritesheet1.man3"), 500);
            _stickmanAnimation.AddFrame(TextureBook.GetTexture(@"Textures\spritesheet1.man4"), 500);
            _stickmanAnimation.AddFrame(TextureBook.GetTexture(@"Textures\spritesheet1.man5"), 500);
            _stickmanAnimation.Loop = true;
        }

        float _angle = 0;

        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            
            //Texture2D texture = renderer.ContentManager.Load<Texture2D>("solid");

            //Effect effect = renderer.ContentManager.Load<Effect>("basicshader.mgfxo");
            //effect.Parameters["xWorld"].SetValue(_camera.World);
            //effect.Parameters["xProjection"].SetValue(_camera.Projection);
            //effect.Parameters["xView"].SetValue(_camera.View);
            //effect.Parameters["TextureSampler"].SetValue(texture);
            //effect.Techniques["Basic"].Passes[0].Apply();

            //renderer.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertexData, 0, 4, _indexData, 0, 2, VertexPositionColorTexture.VertexDeclaration);

            renderer.BeginDraw(_camera);
            LiteEngine.Textures.Texture solidTexture = new LiteEngine.Textures.Texture("solid");

            renderer.DrawDepth = 0.9f;
            DrawTiles(renderer);


            renderer.DrawDepth = 0.7f;
            //this sprite will rotate around it's top corner (20,20)
            renderer.DrawSprite(solidTexture, new RectangleF(20, 20, 20, 2f), Color.Red, _angle);

            //this sprite will rotate around it's center placed at (30,30)
            renderer.DrawSprite(solidTexture, new Vector2(30, 30), new Vector2(20, 2), _angle);

            //this sprite will rotate around it's center placed at (70,20)
            renderer.DrawSprite(solidTexture, new RectangleF(70, 20, 20, 2f), renderer.DrawDepth, _angle, new Vector2(0.5f,0.5f), Color.Red);

            renderer.DrawPoint(new Vector2(0, 0), 1f, Color.White, 1f);

            renderer.DrawSprite(_stickmanAnimation.CurrentTexture, new RectangleF(10, 60, 30, 30));
            _stickmanAnimation.Advance(gameTime.ElapsedGameTime.Milliseconds);
            //renderer.DrawSprite(new LiteEngine.Textures.Texture("point"), new RectangleF(2, 0, 1, 1), 0.5f, _angle, new Vector2(0.5f,0.5f), Color.Blue);

            //renderer.DrawSprite(new LiteEngine.Textures.Texture("point", new RectangleI(0, 0, 32, 32)), new RectangleF(2, 0, 1, 1), 0.5f, _angle, new Vector2(0.5f, 0.5f), Color.Blue);

            //draw non-rectangular sprite centered and rotating around (50,50)
            DrawNonRectangular(renderer, new Vector2(50,50), new Vector2(0.5f,0.5f));

            //draw non-rectangular sprite rotating around its bottom right corner at (70,70)
            DrawNonRectangular(renderer, new Vector2(70, 70), new Vector2(1f, 1f));

            //draw a non rectangular texture 
            _angle += 0.01f;
            renderer.EndDraw();
        }

        void DrawTiles(XnaRenderer renderer)
        {
            LiteEngine.Textures.Texture solidTexture = new LiteEngine.Textures.Texture("solid");
            for (int y=0;y<10;y++)
                for (int x = 0; x < 10; x++)
                {
                    Color color = Color.BlanchedAlmond;
                    if ((x + y) % 2 == 0)
                        color = Color.PaleTurquoise;
                    renderer.DrawSprite(solidTexture, new RectangleF(x * 10f, y * 10f, 10f, 10f), color);
                }
        }

        void DrawNonRectangular(XnaRenderer renderer, Vector2 position, Vector2 origin)
        {
            renderer.DrawOffset = position;
            renderer.DrawDepth = 0.7f;
            renderer.DrawSprite(new LiteEngine.Textures.Texture("solid"), new Corners(new Vector2(0, 0), new Vector2(15, 0), new Vector2(2.5f, 10), new Vector2(12.5f, 10)), _angle, origin, Color.Red);
            renderer.DrawDepth = 0.5f;
            renderer.DrawSprite(new LiteEngine.Textures.Texture("zombie"), new Corners(new Vector2(0, 0), new Vector2(15, 0), new Vector2(2.5f, 10), new Vector2(12.5f, 10)), _angle, origin, Color.Red);
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

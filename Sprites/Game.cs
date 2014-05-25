using LiteEngine.Math;
using LiteEngine.Rendering;
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

        protected override void Initialize(XnaRenderer renderer)
        {
            _camera = new Camera2D(new Vector2(0, 0), new Vector2(10, 10));
            renderer.SetResolution(1000, 800, false);


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

            renderer.DrawSprite(new LiteEngine.Textures.Texture("solid"), new RectangleF(0, 0, 1, 1), Color.White);
            renderer.DrawSprite(new LiteEngine.Textures.Texture("solid"), new RectangleF(1, 0, 1, 1), Color.Red);

            renderer.DrawSprite(new LiteEngine.Textures.Texture("point", new RectangleI(0,0,32,32)), new Vector2(2.5f, 0.5f), new Vector2(1,1), _angle, Color.Blue, 1f);


            //renderer.DrawSprite(new LiteEngine.Textures.Texture("point"), new RectangleF(2, 0, 1, 1), 0.5f, _angle, new Vector2(0.5f,0.5f), Color.Blue);

            //renderer.DrawSprite(new LiteEngine.Textures.Texture("point", new RectangleI(0, 0, 32, 32)), new RectangleF(2, 0, 1, 1), 0.5f, _angle, new Vector2(0.5f, 0.5f), Color.Blue);

            renderer.DrawSprite(new LiteEngine.Textures.Texture("solid"), new Corners(new Vector2(0, 0), new Vector2(3, 0), new Vector2(0.5f, 2), new Vector2(2.5f, 2)), 0.5f, _angle, new Vector2(0f, 0f), Color.Red);


            renderer.DrawSprite(new LiteEngine.Textures.Texture("zombie"), new Corners(new Vector2(0, 0), new Vector2(3, 0), new Vector2(0.5f, 2), new Vector2(2.5f, 2)), 0.1f, _angle, new Vector2(0f,0f), Color.Red);
            //_angle += 0.01f;
            renderer.EndDraw();

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

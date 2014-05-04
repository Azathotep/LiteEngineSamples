using LiteEngine.Core;
using LiteEngine.Math;
using LiteEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireFlies
{
    class FireFly
    {
        Vector2 _position;
        Vector2 _velocity;
        Color _color;

        public FireFly()
        {
            Vector3 rgb = Dice.RandomVector3(1);
            rgb.Normalize();
            _color = Color.FromNonPremultiplied(new Vector4(rgb, 1f));
        }

        public void Update()
        {
            _position += _velocity;
            _velocity += Dice.RandomVector2(0.01f);
            if (_position.Length() > 20)
                _velocity -= _position * 0.0001f;
            if (_velocity.LengthSquared() > 0.01f)
                _velocity *= 0.9f;
        }

        internal void Draw(XnaRenderer renderer)
        {
            renderer.DrawPoint(_position, 10f, _color * 0.1f, 0f);
            renderer.DrawPoint(_position, 4f, _color * 0.2f, 0f);
            renderer.DrawPoint(_position, 2f, _color * 0.5f, 0f);
            renderer.DrawPoint(_position, 1f, Color.White, 1);
        }
    }
}

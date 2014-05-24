using LiteEngine.Input;
using LiteEngine.Math;
using LiteEngine.Rendering;
using LiteEngine.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collisions
{
    class Game : LiteXnaEngine
    {
        List<Polygon> _polygons = new List<Polygon>();
        Camera2D _camera;
        protected override void Initialize(XnaRenderer renderer)
        {
            _camera = new Camera2D(new Vector2(0, 0), new Vector2(80, 60));
            renderer.SetResolution(1200, 900, false);
            Polygon polygon = new Polygon(new Vector2[] { new Vector2(-4, 3), new Vector2(4, 3), new Vector2(0, -3) });
            _polygons.Add(polygon);
            polygon.Position = new Vector2(4f, 3f);
            polygon.Angle = 0.1f;
            polygon = new Polygon(new Vector2[] { new Vector2(-8, 3), new Vector2(8, 3), new Vector2(0, 0) });
            polygon.Position = new Vector2(7, 7);
            _polygons.Add(polygon);
        }

        float _cursorSize = 2f;
        protected override void DrawFrame(GameTime gameTime, XnaRenderer renderer)
        {
            Vector2 mp = base.GetMousePosition();
            mp = _camera.ViewToWorld(mp);


            renderer.BeginDraw(_camera);
            foreach (Polygon p in _polygons)
                p.Draw(renderer);

            renderer.DrawPoint(mp, _cursorSize, Color.Lerp(Color.Green, Color.Cyan, (_cursorSize - 2)/2), 1f);

            if (_stage == 0)
                renderer.DrawArrow(_forceStart, _forceEnd, 0.2f, Color.Green);

            foreach (Arrow arrow in ArrowCollection.Arrows.Values)
                renderer.DrawArrow(arrow.Start, arrow.End, 0.1f, arrow.Color);

            renderer.DrawPoint(Vector2.Zero, 0.2f, Color.Red, 0.5f);

            renderer.EndDraw();
        }

        bool _pause = true;
        protected override void UpdateFrame(GameTime gameTime)
        {
            if (_pause)
                return;
            if (_cursorSize > 2f)
                _cursorSize -= 0.1f;
            foreach (Polygon p in _polygons)
                p.Update(_polygons);
        }

        Vector2 _forceStart;
        Vector2 _forceEnd;
        int _stage = 0;

        protected override void OnMouseClick(MouseButton button, Vector2 mousePosition)
        {
            mousePosition = _camera.ViewToWorld(mousePosition);
            if (_stage == 0)
                _forceStart = mousePosition;
            else
                _forceEnd = mousePosition;
            _stage = (_stage + 1) % 2;

            //
            //_polygon.ApplyForce(mousePosition, 
            //_polygon.Velocity += (_polygon.Position + _polygon.CenterOfMass - mousePosition) * 0.01f;
            //_cursorSize = 4;
        }

        protected override int OnKeyPress(Keys key, GameTime gameTime)
        {
            if (key == Keys.Escape)
                Exit();
            if (key == Keys.Space)
            {
                //if (_pause)
                //    _polygons[0].ApplyForce(_forceEnd, (_forceEnd - _forceStart));
                _pause = !_pause;
                return -1;
            }
            switch (key)
            {
                case Keys.Z:
                    _camera.Zoom *= 1.1f;
                    break;
                case Keys.X:
                    _camera.Zoom *= 0.9f;
                    break;
                case Keys.E:
                    _polygons[0].ApplyForce(_forceEnd, (_forceEnd - _forceStart));
                    break;
            }
            return 0;
        }

        public static ArrowCollection ArrowCollection = new ArrowCollection();
    }

    class ArrowCollection
    {
        Dictionary<string, Arrow> _arrows = new Dictionary<string, Arrow>();
        public void AddArrow(string name, Vector2 start, Vector2 end, Color color)
        {
            Arrow arrow = new Arrow(start, end, color);
            _arrows[name] = arrow;
        }

        public Dictionary<string, Arrow> Arrows
        {
            get
            {
                return _arrows;
            }
        }
    }

    struct Arrow
    {
        Vector2 _start;
        Vector2 _end;
        Color _color;
        public Arrow(Vector2 start, Vector2 end, Color color)
        {
            _start = start;
            _end = end;
            _color = color;
        }

        public Vector2 Start
        {
            get
            {
                return _start;
            }
        }

        public Vector2 End
        {
            get
            {
                return _end;
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
        }
    }

    class Polygon
    {
        public float AngularVelocity;
        public float Angle;
        Vector2[] _points;
        Vector2[] _normals;
        
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 _centerOfMass;

        public Polygon(Vector2[] points)
        {
            _points = points;
            foreach (var p in _points)
                _centerOfMass += p;
            _centerOfMass /= _points.Length;

            _normals = new Vector2[points.Length];
            for (int i = 0; i < _points.Length; i++)
            {
                Vector2 start = _points[i];
                Vector2 end = _points[(i + 1) % _points.Length];
                _normals[i] = new Vector2(-(end.Y - start.Y), (end.X - start.X));
                _normals[i].Normalize();
            }

            //Vector2 lineStart = new Vector2(25,-3);
            //Vector2 lineEnd = new Vector2(-10, 10);

            //for (float f = 0; f < Math.PI*2; f+=0.1f)
            //{
            //    Vector2 v = Util.AngleToVector(f) * 10f + new Vector2(16, -7);
            //    Vector2 v2 = Util.AngleToVector(f + 0.1f) * 10f + new Vector2(16, -7);
            //    Game.ArrowCollection.AddArrow("f" + f, v, v2, Color.Violet);
            //}

            //Game.ArrowCollection.AddArrow("testline", lineStart, lineEnd, Color.Orange);
            //Vector2 intersect1, intersect2;

            ////float fraction;
            ////LineSegmentIntersectCircle(lineStart, lineEnd, new Vector2(16, -7), 10f, out fraction);

            //var r = TestLineIntersectsCircle(lineStart, lineEnd, new Vector2(16, -7), 10f);
            //{
            //    Game.ArrowCollection.AddArrow("intersectcircle1", r.Intersect1, r.Intersect1, Color.White);
            //    Game.ArrowCollection.AddArrow("intersectcircle2", r.Intersect2, r.Intersect2, Color.White);
            //}
        }

        /// <summary>
        /// Apply force
        /// </summary>
        /// <param name="position">position in world coordinates</param>
        public void ApplyForce(Vector2 applicationPoint, Vector2 force)
        {
            //Game.ArrowCollection.AddArrow("fforce", applicationPoint, applicationPoint + force * 10f, Color.Green);

            //moment arm is the line from the center of mass to the point of application of the force
            Vector2 momentArm = applicationPoint - CenterOfMass;
            //Game.ArrowCollection.AddArrow("momentumArm", CenterOfMass, CenterOfMass + momentArm, Color.Red);


            //calculate force parallel to the moment arm
            Vector2 parallelForce = Util.Project(force, momentArm);
            //calculate the force perpendicular to the moment arm
            //(force = parallelForce + perpForce)
            Vector2 perpForce = force - parallelForce;
            //Game.ArrowCollection.AddArrow("parallelForce", applicationPoint - force, applicationPoint - force + parallelForce, Color.Yellow);
            //Game.ArrowCollection.AddArrow("perpForce", applicationPoint - force, applicationPoint - force + perpForce, Color.Blue);

            //the perpendicular force causes a torque
            Vector2 torque = perpForce * momentArm.Length();
            //Game.ArrowCollection.AddArrow("torque", applicationPoint, applicationPoint + torque, Color.Cyan);
            //angular acceleration = torque / moment of inertia <-- skip this

            Vector2 angularAcceleration = torque / 1000;

            Vector3 c = Vector3.Cross(new Vector3(angularAcceleration, 0f), new Vector3(momentArm, 0f));

            //c = Vector3.Cross(new Vector3(force, 0f), new Vector3(momentArm, 0f));

            AngularVelocity -= c.Z * 0.01f;  // angularAcceleration.Length();

            Velocity += force * 0.001f;

            //Velocity += (Position + CenterOfMass - applicationPoint) * 0.01f;
        }

        public Vector2 CenterOfMass
        {
            get
            {
                return ToWorld(_centerOfMass);
            }
        }

        public Vector2 WorldPoint(int i)
        {
            return ToWorld(_points[i]);
        }

        Vector2 ToWorld(Vector2 point)
        {
            Matrix tranform = Matrix.CreateTranslation(-_centerOfMass.X, -_centerOfMass.Y, 0) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(_centerOfMass.X, _centerOfMass.Y, 0) * Matrix.CreateTranslation(Position.X, Position.Y, 0);
            return Vector2.Transform(point, tranform);
        }

        public void Draw(XnaRenderer renderer)
        {
            //renderer.DrawPoint(ToWorld(_centerOfMass), 1f, Color.Red, 1f);
            renderer.Transformation = Matrix.CreateTranslation(-_centerOfMass.X, -_centerOfMass.Y, 0) * Matrix.CreateRotationZ(Angle) * Matrix.CreateTranslation(_centerOfMass.X, _centerOfMass.Y, 0) * Matrix.CreateTranslation(Position.X, Position.Y, 0);
            renderer.DrawPoint(_centerOfMass, 1f, Color.Red, 1f);
            
            for (int i = 0; i < _points.Length; i++)
            {
                renderer.DrawPoint(_points[i], 1f, Color.White, 1f);
                
                //draw edge
                Vector2 start = _points[i];
                Vector2 end = _points[(i + 1) % _points.Length];
                renderer.DrawLine(start, end, 0.1f, Color.White, 1f);
                //renderer.DrawArrow((start + end) * 0.5f, (start + end) * 0.5f + _normals[i] * 1f, 0.1f, Color.DarkGray, 0.6f);
            }

            renderer.Transformation = Matrix.Identity;
            renderer.DrawArrow(CenterOfMass, CenterOfMass + Velocity * 50f, 0.2f, Color.Blue);
        }

        /// <summary>
        /// Tests whether any of the vertices of this polygon collide with a specified line segment when this polygon is
        /// moved by a linear amount
        /// </summary>
        /// <param name="moveBy"></param>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        CollisionInfo TestCollisionWithLineSegment(Vector2 moveBy, Vector2 lineStart, Vector2 lineEnd)
        {
            CollisionInfo ret = new CollisionInfo();
            for (int i=0;i<_points.Length;i++)
            {
                Vector2 p = _points[i];
                //test line intersection between p->p+moveBy
                Vector2 start = ToWorld(p);
                Vector2 intersectionPoint;
                if (FarseerPhysics.Common.LineTools.LineIntersect(start, start + moveBy, lineStart, lineEnd, out intersectionPoint))
                {
                    float f = (intersectionPoint - start).Length() / moveBy.Length();
                    if (!ret.IsCollision || f < ret.Fraction)
                    {
                        ret.IsCollision = true;
                        ret.Fraction = f;
                        //holds the index of the vertex that collided
                        ret.IndexOfStriking = i;
                        ret.Impact = intersectionPoint;
                        ret.Type = CollisionType.VertexStrikesEdge;
                    }
                }
            }
            return ret;
        }

        //bool LineSegmentIntersectCircle(Vector2 lineStart, Vector2 lineEnd, Vector2 circleCenter, float circleRadius, out float fraction)
        //{
        //    fraction = 0f;
        //    Vector2 intersect1, intersect2;
        //    if (!LineIntersectCircle(lineStart, lineEnd, circleCenter, circleRadius, out intersect1, out intersect2))
        //        return false;
        //    Vector2 vFraction = (intersect1 - lineStart) / (lineEnd - lineStart);
        //    float fraction1 = Math.Max(vFraction.X, vFraction.Y);

        //    vFraction = (intersect2 - lineStart) / (lineEnd - lineStart);
        //    float fraction2 = Math.Max(vFraction.X, vFraction.Y);
            
        //    //epsilon!
        //    if (fraction1 < 0f || fraction1 > 1f)
        //        fraction1 = fraction2;
        //    if (fraction2 >= 0f && fraction2 <= 1f)
        //        if (fraction2 < fraction1)
        //            fraction1 = fraction2;
        //    if (fraction1 < 0f || fraction1 > 1f)
        //        return false;
        //    fraction = Math.Min(fraction1, fraction2);
        //    return true;
        //}

        struct LineIntersectCircleResult
        {
            public bool Intersects;
            //fractional distance to intersections
            public float T1;
            public float T2;
            public Vector2 Intersect1;
            public Vector2 Intersect2;
        }

        /// <summary>
        /// Returns whether a line intersects a circle and the points at which it intersects
        /// </summary>
        LineIntersectCircleResult TestLineIntersectsCircle(Vector2 lineStart, Vector2 lineEnd, Vector2 circleCenter, float circleRadius)
        {
            LineIntersectCircleResult ret = new LineIntersectCircleResult();
            float x1 = lineStart.X - circleCenter.X;
            float x2 = lineEnd.X - circleCenter.X;
            float y1 = lineStart.Y - circleCenter.Y;
            float y2 = lineEnd.Y - circleCenter.Y;
            float a = (float)Math.Pow(y2 - y1, 2) + (float)Math.Pow(x2 - x1, 2);
            float b = 2 * (x1 * (x2 - x1) + y1 * (y2 - y1));
            float c = x1 * x1 + y1 * y1 - circleRadius * circleRadius;
            //t = (-b +- Sqrt(b^2 - 4ac)) / (2a)
            float D = b * b - 4 * a * c;
            if (D < 0)
                return ret; //no intersect
            float sqD = (float)Math.Sqrt(D);
            ret.T1 = (-b + sqD) / (2 * a);
            ret.T2 = (-b - sqD) / (2 * a);
            ret.Intersect1 = lineStart + (lineEnd - lineStart) * ret.T1;
            ret.Intersect2 = lineStart + (lineEnd - lineStart) * ret.T2;
            ret.Intersects = true;
            return ret;
        }

        /// <summary>
        /// Tests whether any of the vertices of this polygon collide with a specified line segment when this polygon 
        /// is rotated
        /// </summary>
        /// <param name="moveBy"></param>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        CollisionInfo TestCollisionWithLineSegment(float rotateBy, Vector2 lineStart, Vector2 lineEnd)
        {
            CollisionInfo ret = new CollisionInfo();
            ret.Fraction = 1;
            for (int i = 0; i < _points.Length; i++)
            {
                Vector2 p = _points[i];

                float length = (p - _centerOfMass).Length();
                LineIntersectCircleResult info = TestLineIntersectsCircle(lineStart, lineEnd, CenterOfMass, length);

                //Game.ArrowCollection.AddArrow("test4" + i, Vector2.Zero, Vector2.Zero, Color.Orange);
                if (info.Intersects)
                {
                    //Game.ArrowCollection.AddArrow("test" + i, info.Intersect1, info.Intersect1, Color.Green);
                    //Game.ArrowCollection.AddArrow("test2" + i, info.Intersect2, info.Intersect2, Color.Green);

                    for (int a = 0; a < 2; a++)
                    {
                        float t;
                        Vector2 intersection;
                        if (a == 0)
                        {
                            t = info.T1;
                            intersection = info.Intersect1;
                        }
                        else
                        {
                            t = info.T2;
                            intersection = info.Intersect2;
                        }

                        if (t >= 0 && t < 1)
                        {
                            Vector2 v = ToWorld(p) - ToWorld(Vector2.Zero);
                            //Game.ArrowCollection.AddArrow("sadad", ToWorld(Vector2.Zero), ToWorld(p), Color.RoyalBlue);
                            //Game.ArrowCollection.AddArrow("sadad2", ToWorld(Vector2.Zero), intersection, Color.GreenYellow);

                            float angleToIntersect = Util.AngleBetween(CenterOfMass, intersection);
                            float angleToPoint = Util.AngleBetween(CenterOfMass, ToWorld(p));

                            float angleBeforeHit = Util.AngleBetween(angleToPoint, angleToIntersect);

                            float fractionIntersect = angleBeforeHit / rotateBy;
                            if (fractionIntersect > 0f && fractionIntersect < 1f && fractionIntersect < ret.Fraction)
                            {
                                ret.IsCollision = true;
                                ret.Fraction = fractionIntersect;
                                ret.IndexOfStriking = i;
                                ret.Impact = intersection;
                                ret.Type = CollisionType.VertexStrikesEdge;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Tests whether any of the points of this polygon collide with the edges of another polygon when rotated
        /// </summary>
        CollisionInfo TestPointsCollideWithPolygon(float rotateBy, Polygon other)
        {
            CollisionInfo ret = new CollisionInfo();
            //test each of the other polygon's edges
            for (int i = 0; i < other._points.Length; i++)
            {
                //TODO optimize? find out whether can discard certain edges like in linear movement case.

                Vector2 lineStart = other.WorldPoint(i);
                Vector2 lineEnd = other.WorldPoint((i + 1) % other._points.Length);
                CollisionInfo info = TestCollisionWithLineSegment(rotateBy, lineStart, lineEnd);
                if (info.IsCollision)
                {
                    if (!ret.IsCollision || info.Fraction < ret.Fraction)
                    {
                        ret = info;
                        //index of the edge of the struck polygon
                        ret.IndexOfStruck = i;
                    }
                }
            }
            return ret;
        }


        CollisionInfo TestCollideWithPolygon(float rotateBy, Polygon other)
        {
            //test collision of this polygon's points against edges of other polygon
            CollisionInfo ret = TestPointsCollideWithPolygon(rotateBy, other);
            CollisionInfo collisionOther = other.TestPointsCollideWithPolygon(-rotateBy, this);
            if (collisionOther.IsCollision && (!ret.IsCollision || collisionOther.Fraction < ret.Fraction))
            {
                ret = collisionOther;
                //reverse the collision so it's from this polygon's point of view
                ret.Type = CollisionType.EdgeStrikesVertex;
                int tmp = ret.IndexOfStriking;
                ret.IndexOfStriking = collisionOther.IndexOfStruck;
                ret.IndexOfStruck = tmp;
                //collided with point of other, but the collision info holds the point of intersection with self
                //so reset it to the point of the other that we struck
                ret.Impact = other.ToWorld(other._points[ret.IndexOfStruck]);
            }
            return ret;
        }

        /// <summary>
        /// Tests whether any of the points of this polygon would collide with the edges of another polygon if this polygon
        /// was moved by the specified amount
        /// </summary>
        /// <param name="moveBy">amount to move the polygon</param>
        /// <param name="other">other polygon to test against</param>
        /// <returns></returns>
        CollisionInfo TestPointsCollideWithPolygon(Vector2 moveBy, Polygon other)
        {
            CollisionInfo ret = new CollisionInfo();
            for (int i = 0; i < other._points.Length; i++)
            {
                //if the other polygon's edge is facing away from the direction of movement then no collision can occur
                //so skip it
                Vector2 otherNormal = Util.RotateVector(other._normals[i], other.Angle);
                Vector2 nStart = (other.ToWorld(other._points[i]) + other.ToWorld(other._points[(i+1) % other._points.Length])) * 0.5f;
                //Game.ArrowCollection.AddArrow("" + other._points[0].X + i, nStart, nStart + otherNormal * 3f, Color.Orange);

                if (Vector2.Dot(moveBy, otherNormal) > 0)
                    continue;

                Vector2 lineStart = other.WorldPoint(i);
                Vector2 lineEnd = other.WorldPoint((i + 1) % other._points.Length);
                CollisionInfo info = TestCollisionWithLineSegment(moveBy, lineStart, lineEnd);
                if (info.IsCollision)
                {
                    if (!ret.IsCollision || info.Fraction < ret.Fraction)
                    {
                        ret = info;
                        //index of the edge of the struck polygon
                        ret.IndexOfStruck = i;
                    }
                }
            }
            return ret;
        }

        CollisionInfo TestCollideWithPolygon(Vector2 moveBy, Polygon other)
        {
            //test collision of this polygon's points against edges of other polygon
            CollisionInfo ret = TestPointsCollideWithPolygon(moveBy, other);
            CollisionInfo collisionOther = other.TestPointsCollideWithPolygon(-moveBy, this);
            if (collisionOther.IsCollision && (!ret.IsCollision || collisionOther.Fraction < ret.Fraction))
            {
                ret = collisionOther;
                //reverse the collision so it's from this polygon's point of view
                ret.Type = CollisionType.EdgeStrikesVertex;
                int tmp = ret.IndexOfStriking;
                ret.IndexOfStriking = collisionOther.IndexOfStruck;
                ret.IndexOfStruck = tmp;
                //collided with point of other, but the collision info holds the point of intersection with self
                //so reset it to the point of the other that we struck
                ret.Impact = other.ToWorld(other._points[ret.IndexOfStruck]);
            }
            return ret;
        }

        /// <summary>
        /// Info about a collision between two polygons
        /// One polygon is called the striking polygon (the one that moved)
        /// and the other the struck polygon
        /// </summary>
        struct CollisionInfo
        {
            public bool IsCollision;

            public CollisionType Type;
            //vertex or edge index of striking polygon
            public int IndexOfStriking;
            //vertex or edge index of struck polygon
            public int IndexOfStruck;   

            //fraction of movement completed before impact (between 0 and 1)
            public float Fraction;
            //impact point
            public Vector2 Impact;
        }

        enum CollisionType
        {
            VertexStrikesEdge, //Vertex of striking polygon has hit an edge of the struck polygon
            EdgeStrikesVertex //Edge of striking polygon has hit a vertex of the struck polygon 
            //TODO edge strike edge?
        }

        public void Update(List<Polygon> others)
        {
            if (this == others[0])
            {
                Velocity.Y += 0.001f;

                CollisionInfo collision = TestCollideWithPolygon(Velocity, others[1]);
                if (collision.IsCollision)
                {
                    //the impact will either be a point of this polygon striking an edge of the other polygon
                    //or it will be a point of the other polygon striking an edge of this polygon
                    if (collision.Fraction > 0.5f)
                    {
                        Position += Velocity * collision.Fraction * 0.95f;
                        Vector2 impactNormal = Vector2.Zero;
                        if (collision.Type == CollisionType.VertexStrikesEdge)
                        {
                            int edgeIndex = collision.IndexOfStruck;
                            impactNormal = others[1]._normals[edgeIndex];
                            impactNormal = Util.RotateVector(impactNormal, others[1].Angle);
                        }
                        if (collision.Type == CollisionType.EdgeStrikesVertex)
                        {
                            int edgeIndex = collision.IndexOfStriking;
                            impactNormal = _normals[edgeIndex];
                            impactNormal = Util.RotateVector(impactNormal, Angle);
                        }

                        
                        Vector2 normalForce = -Util.Project(Velocity, impactNormal) * 1.1f;

                        Velocity = Vector2.Zero;
                        
                        ApplyForce(collision.Impact, normalForce * 1000f);
                        Game.ArrowCollection.AddArrow("normalForceP", collision.Impact, collision.Impact + normalForce * 2000, Color.Blue);
                        
                    }
                    else
                    {
                        Velocity = Vector2.Zero;
                    }
                }
                
            }
            Position += Velocity;

            if (this == others[0])
            {
                CollisionInfo collision = TestCollideWithPolygon(AngularVelocity, others[1]);
                if (collision.IsCollision)
                {
                    if (collision.Fraction < 0.5f)
                    {
                        AngularVelocity = 0;
                    }
                    else
                    {

                        Angle += AngularVelocity * collision.Fraction * 0.9f;

                        Vector2 impactNormal = Vector2.Zero;
                        if (collision.Type == CollisionType.VertexStrikesEdge)
                        {
                            int edgeIndex = collision.IndexOfStruck;
                            impactNormal = others[1]._normals[edgeIndex];
                            impactNormal = Util.RotateVector(impactNormal, others[1].Angle);
                        }
                        if (collision.Type == CollisionType.EdgeStrikesVertex)
                        {
                            int edgeIndex = collision.IndexOfStriking;
                            impactNormal = _normals[edgeIndex];
                            impactNormal = Util.RotateVector(impactNormal, Angle);
                        }
                        Vector2 normalForce = impactNormal * Math.Abs(AngularVelocity) * 1.5f; // -Util.Project(Velocity, impactNormal) * 1.1f;
                        AngularVelocity = 0;
                        ApplyForce(collision.Impact, normalForce * 1000f);
                        Game.ArrowCollection.AddArrow("normalForce", collision.Impact, collision.Impact + normalForce * 2000, Color.Yellow);
                    }
                }
            }

            Angle += AngularVelocity;




            //create movement hull
            //foreach point in polygon
            //check both neighbouring edges
            //if both edges face away from the direction of movement then create new point in hull as identical copy of this point
            //if both edges face towards the direction of movement then create new point in hull that is this point + velocity
            //if one edge faces towards and one away then copy self into hull and also add a new point that is point + velocity
            //TODO these last two added points are different way round depending on the side
            //maybe go through clockwise

            //now test movement hull to see if there is a collision with nearby obstacles
            //FarseerPhysics.Collision.Collision.TestOverlap(null, null, null, null, null, null
            
            //FarseerPhysics.Common.LineTools.LineIntersect(.LineSegmentVerticesIntersect(

            //Angle += AngularVelocity;
        }
    }
}

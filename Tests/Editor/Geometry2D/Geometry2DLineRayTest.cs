using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DLineRayTest : TestBase
    {
        private const string format = "{0:F8}\n{1}";

        #region Distance

        [Test]
        public void Distance_Collinear()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    line.origin = origin;
                    line.direction = direction;

                    ray.direction = direction;

                    ray.origin = origin - direction*50;
                    AreEqual_Distance(line, ray);
                    ray.origin = origin - direction;
                    AreEqual_Distance(line, ray);
                    ray.origin = origin;
                    AreEqual_Distance(line, ray);
                    ray.origin = origin + direction;
                    AreEqual_Distance(line, ray);
                    ray.origin = origin + direction*50;
                    AreEqual_Distance(line, ray);
                }
            }
        }

        [Test]
        public void Distance_Parallel()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    ray.direction = direction;

                    ray.origin = origin + perpendicular - direction*30;
                    AreEqual_Distance(line, ray, 1);
                    ray.origin = origin + perpendicular - direction;
                    AreEqual_Distance(line, ray, 1);
                    ray.origin = origin + perpendicular;
                    AreEqual_Distance(line, ray, 1);
                    ray.origin = origin + perpendicular + direction;
                    AreEqual_Distance(line, ray, 1);
                    ray.origin = origin + perpendicular + direction*30;
                    AreEqual_Distance(line, ray, 1);
                }
            }
        }

        [Test]
        public void Distance_Diagonal()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    for (int rayAngle = 1; rayAngle < 180; rayAngle += 10)
                    {
                        Vector2 rayDirection = direction.RotateCW(rayAngle).normalized;
                        ray.direction = rayDirection;

                        ray.origin = origin;
                        AreEqual_Distance(line, ray);
                        ray.origin = origin + direction;
                        AreEqual_Distance(line, ray);
                        ray.origin = origin - direction;
                        AreEqual_Distance(line, ray);

                        ray.origin = origin + perpendicular;
                        AreEqual_Distance(line, ray, 1);
                        ray.origin = origin + perpendicular + direction;
                        AreEqual_Distance(line, ray, 1);
                        ray.origin = origin + perpendicular - direction;
                        AreEqual_Distance(line, ray, 1);

                        ray.origin = origin - rayDirection;
                        AreEqual_Distance(line, ray);
                        ray.origin = origin + direction - rayDirection;
                        AreEqual_Distance(line, ray);
                        ray.origin = origin - direction - rayDirection;
                        AreEqual_Distance(line, ray);
                    }
                }
            }
        }

        private void AreEqual_Distance(Line2 line, Ray2D ray, float expected = 0)
        {
            string message = string.Format(format, line, ray.ToString("F8"));
            AreEqual(Distance.LineRay(line, ray), expected, message);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_Collinear()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    line.origin = origin;
                    line.direction = direction;

                    ray.direction = direction;

                    ray.origin = origin - direction*50;
                    AreEqual_ClosestPoints(line, ray, ray.origin);
                    ray.origin = origin - direction;
                    AreEqual_ClosestPoints(line, ray, ray.origin);
                    ray.origin = origin;
                    AreEqual_ClosestPoints(line, ray, ray.origin);
                    ray.origin = origin + direction;
                    AreEqual_ClosestPoints(line, ray, ray.origin);
                    ray.origin = origin + direction*50;
                    AreEqual_ClosestPoints(line, ray, ray.origin);
                }
            }
        }

        [Test]
        public void ClosestPoints_Parallel()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    ray.direction = direction;

                    ray.origin = origin + perpendicular - direction*30;
                    AreEqual_ClosestPoints(line, ray, line.GetPoint(-30), ray.origin);
                    ray.origin = origin + perpendicular - direction;
                    AreEqual_ClosestPoints(line, ray, origin - direction, ray.origin);
                    ray.origin = origin + perpendicular;
                    AreEqual_ClosestPoints(line, ray, origin, ray.origin);
                    ray.origin = origin + perpendicular + direction;
                    AreEqual_ClosestPoints(line, ray, origin + direction, ray.origin);
                    ray.origin = origin + perpendicular + direction*30;
                    AreEqual_ClosestPoints(line, ray, line.GetPoint(30), ray.origin);
                }
            }
        }

        [Test]
        public void ClosestPoints_Diagonal()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    for (int rayAngle = 1; rayAngle < 180; rayAngle += 10)
                    {
                        Vector2 rayDirection = direction.RotateCW(rayAngle).normalized;
                        ray.direction = rayDirection;

                        ray.origin = origin;
                        AreEqual_ClosestPoints(line, ray, ray.origin);
                        ray.origin = origin + direction;
                        AreEqual_ClosestPoints(line, ray, ray.origin);
                        ray.origin = origin - direction;
                        AreEqual_ClosestPoints(line, ray, ray.origin);

                        ray.origin = origin + perpendicular;
                        AreEqual_ClosestPoints(line, ray, origin, ray.origin);
                        ray.origin = origin + perpendicular + direction;
                        AreEqual_ClosestPoints(line, ray, origin + direction, ray.origin);
                        ray.origin = origin + perpendicular - direction;
                        AreEqual_ClosestPoints(line, ray, origin - direction, ray.origin);

                        ray.origin = origin - rayDirection;
                        AreEqual_ClosestPoints(line, ray, origin);
                        ray.origin = origin + direction - rayDirection;
                        AreEqual_ClosestPoints(line, ray, origin + direction);
                        ray.origin = origin - direction - rayDirection;
                        AreEqual_ClosestPoints(line, ray, origin - direction);
                    }
                }
            }
        }

        private void AreEqual_ClosestPoints(Line2 line, Ray2D ray, Vector2 expected)
        {
            AreEqual_ClosestPoints(line, ray, expected, expected);
        }

        private void AreEqual_ClosestPoints(Line2 line, Ray2D ray, Vector2 lineExpected, Vector2 rayExpected)
        {
            string message = string.Format(format, line, ray.ToString("F8"));
            Closest.LineRay(line, ray, out Vector2 linePoint, out Vector2 rayPoint);
            AreEqual(linePoint, lineExpected, message);
            AreEqual(rayPoint, rayExpected, message);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_Collinear()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    line.origin = origin;
                    line.direction = direction;

                    ray.direction = direction;

                    ray.origin = origin - direction*50;
                    IsTrue_IntersectRay(line, ray);
                    ray.origin = origin - direction;
                    IsTrue_IntersectRay(line, ray);
                    ray.origin = origin;
                    IsTrue_IntersectRay(line, ray);
                    ray.origin = origin + direction;
                    IsTrue_IntersectRay(line, ray);
                    ray.origin = origin + direction*50;
                    IsTrue_IntersectRay(line, ray);
                }
            }
        }

        [Test]
        public void Intersect_Parallel()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    ray.direction = direction;

                    ray.origin = origin + perpendicular - direction*50;
                    IsFalse_IntersectSwap(line, ray);
                    ray.origin = origin + perpendicular - direction;
                    IsFalse_IntersectSwap(line, ray);
                    ray.origin = origin + perpendicular;
                    IsFalse_IntersectSwap(line, ray);
                    ray.origin = origin + perpendicular + direction;
                    IsFalse_IntersectSwap(line, ray);
                    ray.origin = origin + perpendicular + direction*50;
                    IsFalse_IntersectSwap(line, ray);
                }
            }
        }

        [Test]
        public void Intersect_Diagonal()
        {
            var line = new Line2();
            var ray = new Ray2D();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    for (int rayAngle = 1; rayAngle < 180; rayAngle += 10)
                    {
                        Vector2 rayDirection = direction.RotateCW(rayAngle).normalized;
                        ray.direction = rayDirection;

                        ray.origin = origin;
                        IsTrue_IntersectPoint(line, ray, ray.origin);
                        //ray.origin = origin + direction;
                        //IsTrue_IntersectPoint(line, ray, ray.origin);
                        //ray.origin = origin - direction;
                        //IsTrue_IntersectPoint(line, ray, ray.origin);

                        ray.origin = origin + perpendicular;
                        IsFalse_Intersect(line, ray);
                        ray.origin = origin + perpendicular + direction;
                        IsFalse_Intersect(line, ray);
                        ray.origin = origin + perpendicular - direction;
                        IsFalse_Intersect(line, ray);

                        ray.origin = origin - rayDirection;
                        IsTrue_IntersectPoint(line, ray, origin);
                        ray.origin = origin + direction - rayDirection;
                        IsTrue_IntersectPoint(line, ray, origin + direction);
                        ray.origin = origin - direction - rayDirection;
                        IsTrue_IntersectPoint(line, ray, origin - direction);
                    }
                }
            }
        }

        private void IsTrue_IntersectPoint(Line2 line, Ray2D ray, Vector2 expected)
        {
            string message = string.Format(format, line, ray.ToString("F8"));
            Assert.IsTrue(Intersect.LineRay(line, ray, out IntersectionLineRay2 intersection), message);
            Assert.AreEqual(IntersectionType.Point, intersection.type);
            AreEqual(intersection.point, expected);
        }

        private void IsTrue_IntersectRay(Line2 line, Ray2D ray)
        {
            string message = string.Format(format, line, ray.ToString("F8"));
            Assert.IsTrue(Intersect.LineRay(line.origin, line.direction, ray.origin, ray.direction, out IntersectionLineRay2 intersection), message);
            Assert.AreEqual(IntersectionType.Ray, intersection.type, message);
            AreEqual(intersection.point, ray.origin, message);
            Assert.IsTrue(Intersect.LineRay(line.origin, line.direction, ray.origin, -ray.direction, out intersection), message);
            Assert.AreEqual(IntersectionType.Ray, intersection.type, message);
            AreEqual(intersection.point, ray.origin, message);
        }

        private void IsFalse_IntersectSwap(Line2 line, Ray2D ray)
        {
            string message = string.Format(format, line, ray.ToString("F8"));
            Assert.IsFalse(Intersect.LineRay(line.origin, line.direction, ray.origin, ray.direction, out _), message);
            Assert.IsFalse(Intersect.LineRay(line.origin, line.direction, ray.origin, -ray.direction, out _), message);
        }

        private void IsFalse_Intersect(Line2 line, Ray2D ray)
        {
            string message = string.Format(format, line, ray.ToString("F8"));
            Assert.IsFalse(Intersect.LineRay(line, ray, out IntersectionLineRay2 intersection), message);
        }

        #endregion Intersect
    }
}

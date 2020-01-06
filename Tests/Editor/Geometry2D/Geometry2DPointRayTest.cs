using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DPointRayTest : TestBase
    {
        private const string format = "{0}\n{1}";
        private const float offset = 100;

        #region Distance

        [Test]
        public void Distance_PointOnLine()
        {
            var ray = new Ray2D(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    AreEqual_Distance(ray, origin);
                    AreEqual_Distance(ray, origin + direction*10);
                    AreEqual_Distance(ray, origin - direction*10, 10);
                }
            }
        }

        [Test]
        public void Distance_PointNotOnLine()
        {
            float expected = Mathf.Sqrt(1 + offset*offset);
            var ray = new Ray2D(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    AreEqual_Distance(ray, origin + perpendicular, 1);
                    AreEqual_Distance(ray, origin + perpendicular + direction*offset, 1);
                    AreEqual_Distance(ray, origin + perpendicular - direction*offset, expected);
                }
            }
        }

        private void AreEqual_Distance(Ray2D ray, Vector2 point, float expected = 0)
        {
            string message = string.Format(format, ray.ToString("F8"), point.ToString("F8"));
            AreEqual(Distance.PointRay(point, ray), expected, message);
        }

        #endregion Distance

        #region ClosestPoint

        [Test]
        public void ClosestPoint_PointOnLine()
        {
            var ray = new Ray2D(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    AreEqual_ClosestPoint(ray, origin);
                    AreEqual_ClosestPoint(ray, origin + direction*10);
                    AreEqual_ClosestPoint(ray, origin - direction*10, origin);
                }
            }
        }

        [Test]
        public void ClosestPoint_PointNotOnLine()
        {
            var ray = new Ray2D(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    AreEqual_ClosestPoint(ray, origin + perpendicular, origin);
                    AreEqual_ClosestPoint(ray, origin + perpendicular + direction*offset, ray.GetPoint(offset));
                    AreEqual_ClosestPoint(ray, origin + perpendicular - direction*offset, origin);
                }
            }
        }

        private void AreEqual_ClosestPoint(Ray2D ray, Vector2 point)
        {
            AreEqual_ClosestPoint(ray, point, point);
        }

        private void AreEqual_ClosestPoint(Ray2D ray, Vector2 point, Vector2 expected)
        {
            string message = string.Format(format, ray.ToString("F8"), point.ToString("F8"));
            AreEqual(Closest.PointRay(point, ray), expected, message);
        }

        #endregion ClosestPoint

        #region Intersect

        [Test]
        public void Intersect_PointOnLine()
        {
            var ray = new Ray2D(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    True_Intersect(ray, origin);
                    True_Intersect(ray, origin + direction*10);
                    False_Intersect(ray, origin - direction*10);
                }
            }
        }

        [Test]
        public void Intersect_PointNotOnLine()
        {
            var ray = new Ray2D(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    False_Intersect(ray, origin + perpendicular, 1);
                    False_Intersect(ray, origin + perpendicular + direction*offset, 1);
                    False_Intersect(ray, origin + perpendicular - direction*offset, 1);
                }
            }
        }

        private void True_Intersect(Ray2D ray, Vector2 point)
        {
            string message = string.Format(format, ray.ToString("F8"), point.ToString("F8"));
            Assert.True(Intersect.PointRay(point, ray.origin, ray.direction), message);
            Assert.True(Intersect.PointRay(point, ray.origin, ray.direction, out int side), message);
            Assert.AreEqual(0, side, message);
        }

        private void False_Intersect(Ray2D ray, Vector2 point, int expected = 0)
        {
            string message = string.Format(format, ray.ToString("F8"), point.ToString("F8"));
            Assert.False(Intersect.PointRay(point, ray.origin, ray.direction), message);
            Assert.False(Intersect.PointRay(point, ray.origin, ray.direction, out int side), message);
            Assert.AreEqual(expected, side, message);
        }

        #endregion Intersect
    }
}

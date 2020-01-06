using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DRaySegmentTest : TestBase
    {
        private const string format = "{0}\n{1:F8}";

        #region Distance

        [Test]
        public void Distance_Collinear()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = origin - direction*50;
                    segment.b = origin - direction;
                    AreEqual_Distance(ray, segment, 1);
                    segment.a = origin - direction*2;
                    segment.b = origin;
                    AreEqual_Distance(ray, segment);
                    segment.a = origin - direction;
                    segment.b = origin + direction;
                    AreEqual_Distance(ray, segment);
                    segment.a = origin;
                    segment.b = origin + direction*2;
                    AreEqual_Distance(ray, segment);
                    segment.a = origin + direction;
                    segment.b = origin + direction*50;
                    AreEqual_Distance(ray, segment);
                }
            }
        }

        [Test]
        public void Distance_Parallel()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = origin + perpendicular - direction*30;
                    segment.b = origin + perpendicular - direction;
                    AreEqual_Distance(ray, segment, PTUtils.Sqrt2);
                    segment.a = origin + perpendicular - direction*2;
                    segment.b = origin + perpendicular;
                    AreEqual_Distance(ray, segment, 1);
                    segment.a = origin + perpendicular - direction;
                    segment.b = origin + perpendicular + direction;
                    AreEqual_Distance(ray, segment, 1);
                    segment.a = origin + perpendicular;
                    segment.b = origin + perpendicular + direction*2;
                    AreEqual_Distance(ray, segment, 1);
                    segment.a = origin + perpendicular + direction;
                    segment.b = origin + perpendicular + direction*30;
                    AreEqual_Distance(ray, segment, 1);
                }
            }
        }

        [Test]
        public void Distance_Diagonal()
        {
            float length = 3;
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    for (int segmentAngle = 15; segmentAngle < 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment.a = origin;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment);
                        segment.a = origin + direction;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment);
                        segment.a = origin - direction;
                        segment.b = segment.a + segmentDirection.RotateCW90()*length;
                        AreEqual_Distance(ray, segment, 1);

                        segment.a = origin + perpendicular;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment, 1);
                        segment.a = origin + perpendicular + direction;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment, 1);
                        segment.a = origin + perpendicular*2 + direction;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment, 2);
                        segment.a = origin + perpendicular + direction*2;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment, 1);
                        segment.a = origin + perpendicular*2 + direction*2;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment, 2);

                        segment.a = origin - segmentDirection;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment);
                        segment.a = origin + direction - segmentDirection;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment);
                    }

                    Vector2 diagonal = direction.RotateCW45();
                    for (int segmentAngle = 0; segmentAngle <= 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = diagonal.RotateCW(segmentAngle);
                        segment.a = origin + perpendicular - direction;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment, PTUtils.Sqrt2);
                        segment.a = origin + perpendicular*length - direction*length;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_Distance(ray, segment, PTUtils.Sqrt2*length);
                    }

                    segment.a = origin + perpendicular - direction - diagonal*2;
                    segment.b = segment.a + diagonal*4;
                    AreEqual_Distance(ray, segment, PTUtils.Sqrt2);
                    segment.a = origin + perpendicular - direction;
                    segment.b = segment.a + diagonal*4;
                    AreEqual_Distance(ray, segment, PTUtils.Sqrt2);
                    segment.a = origin + perpendicular + direction;
                    segment.b = segment.a + diagonal.RotateCW90()*4;
                    AreEqual_Distance(ray, segment, 1);
                }
            }
        }

        [Test]
        public void Distance_DegenerateSegment()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = segment.b = origin;
                    AreEqual_Distance(ray, segment);
                    segment.a = segment.b = origin + direction*50;
                    AreEqual_Distance(ray, segment);
                    segment.a = segment.b = origin - direction*50;
                    AreEqual_Distance(ray, segment, 50);

                    segment.a = segment.b = origin + perpendicular;
                    AreEqual_Distance(ray, segment, 1);
                    segment.a = segment.b = origin + perpendicular + direction;
                    AreEqual_Distance(ray, segment, 1);
                    segment.a = segment.b = origin + perpendicular - direction;
                    AreEqual_Distance(ray, segment, PTUtils.Sqrt2);
                }
            }
        }

        private void AreEqual_Distance(Ray2D ray, Segment2 segment, float expected = 0)
        {
            string message = string.Format(format, ray.ToString("F8"), segment);
            AreEqual(Distance.RaySegment(ray.origin, ray.direction, segment.a, segment.b), expected, message);
            AreEqual(Distance.RaySegment(ray.origin, ray.direction, segment.b, segment.a), expected, message);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_Collinear()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = origin - direction*50;
                    segment.b = origin - direction;
                    AreEqual_ClosestPoints(ray, segment, origin, segment.b);
                    segment.a = origin - direction*2;
                    segment.b = origin;
                    AreEqual_ClosestPoints(ray, segment, origin);
                    segment.a = origin - direction;
                    segment.b = origin + direction;
                    AreEqual_ClosestPoints(ray, segment, origin);
                    segment.a = origin;
                    segment.b = origin + direction*2;
                    AreEqual_ClosestPoints(ray, segment, origin);
                    segment.a = origin + direction;
                    segment.b = origin + direction*50;
                    AreEqual_ClosestPoints(ray, segment, segment.a);
                }
            }
        }

        [Test]
        public void ClosestPoints_Parallel()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = origin + perpendicular - direction*50;
                    segment.b = origin + perpendicular - direction;
                    AreEqual_ClosestPoints(ray, segment, origin, segment.b);
                    segment.a = origin + perpendicular - direction*2;
                    segment.b = origin + perpendicular;
                    AreEqual_ClosestPoints(ray, segment, origin, segment.b);
                    segment.a = origin + perpendicular - direction;
                    segment.b = origin + perpendicular + direction;
                    AreEqual_ClosestPoints(ray, segment, origin, origin + perpendicular);
                    segment.a = origin + perpendicular;
                    segment.b = origin + perpendicular + direction*2;
                    AreEqual_ClosestPoints(ray, segment, origin, segment.a);
                    segment.a = origin + perpendicular + direction;
                    segment.b = origin + perpendicular + direction*50;
                    AreEqual_ClosestPoints(ray, segment, origin + direction, segment.a);
                }
            }
        }

        [Test]
        public void ClosestPoints_Diagonal()
        {
            float length = 3;
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    for (int segmentAngle = 15; segmentAngle < 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment.a = origin;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, segment.a);
                        segment.a = origin + direction;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, segment.a);
                        segment.a = origin - direction;
                        segment.b = segment.a + segmentDirection.RotateCW90()*length;
                        AreEqual_ClosestPoints(ray, segment, origin, segment.a);

                        segment.a = origin + perpendicular;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin, segment.a);
                        segment.a = origin + perpendicular + direction;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin + direction, segment.a);
                        segment.a = origin + perpendicular*2 + direction;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin + direction, segment.a);
                        segment.a = origin + perpendicular + direction*2;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin + direction*2, segment.a);
                        segment.a = origin + perpendicular*2 + direction*2;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin + direction*2, segment.a);

                        segment.a = origin - segmentDirection;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin);
                        segment.a = origin + direction - segmentDirection;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin + direction);
                    }

                    Vector2 diagonal = direction.RotateCW45();
                    for (int segmentAngle = 0; segmentAngle <= 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = diagonal.RotateCW(segmentAngle);
                        segment.a = origin + perpendicular - direction;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin, segment.a);
                        segment.a = origin + perpendicular*length - direction*length;
                        segment.b = segment.a + segmentDirection*length;
                        AreEqual_ClosestPoints(ray, segment, origin, segment.a);
                    }

                    segment.a = origin + perpendicular - direction - diagonal*2;
                    segment.b = segment.a + diagonal*4;
                    AreEqual_ClosestPoints(ray, segment, origin, origin + perpendicular - direction);
                    segment.a = origin + perpendicular - direction;
                    segment.b = segment.a + diagonal*4;
                    AreEqual_ClosestPoints(ray, segment, origin, segment.a);
                    segment.a = origin + perpendicular + direction;
                    segment.b = segment.a + diagonal.RotateCW90()*4;
                    AreEqual_ClosestPoints(ray, segment, origin + direction, segment.a);
                }
            }
        }

        [Test]
        public void ClosestPoints_DegenerateSegment()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = segment.b = origin;
                    AreEqual_ClosestPoints(ray, segment, segment.a);
                    segment.a = segment.b = origin + direction*50;
                    AreEqual_ClosestPoints(ray, segment, segment.a);
                    segment.a = segment.b = origin - direction*50;
                    AreEqual_ClosestPoints(ray, segment, origin, segment.a);

                    segment.a = segment.b = origin + perpendicular;
                    AreEqual_ClosestPoints(ray, segment, origin, segment.a);
                    segment.a = segment.b = origin + perpendicular + direction;
                    AreEqual_ClosestPoints(ray, segment, origin + direction, segment.a);
                    segment.a = segment.b = origin + perpendicular - direction;
                    AreEqual_ClosestPoints(ray, segment, origin, segment.a);
                }
            }
        }

        private void AreEqual_ClosestPoints(Ray2D ray, Segment2 segment, Vector2 expected)
        {
            AreEqual_ClosestPoints(ray, segment, expected, expected);
        }

        private void AreEqual_ClosestPoints(Ray2D ray, Segment2 segment, Vector2 expectedRay, Vector2 expectedSegment)
        {
            string message = string.Format(format, ray.ToString("F8"), segment);
            Closest.RaySegment(ray.origin, ray.direction, segment.a, segment.b, out Vector2 rayPoint, out Vector2 segmentPoint);
            AreEqual(rayPoint, expectedRay, message);
            AreEqual(segmentPoint, expectedSegment, message);
            Closest.RaySegment(ray.origin, ray.direction, segment.b, segment.a, out rayPoint, out segmentPoint);
            AreEqual(rayPoint, expectedRay, message);
            AreEqual(segmentPoint, expectedSegment, message);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_Collinear()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = origin - direction*50;
                    segment.b = origin - direction;
                    IsFalse_Intersect(ray, segment);
                    segment.a = origin - direction*2;
                    segment.b = origin;
                    IsTrue_IntersectPoint(ray, segment, origin);
                    segment.a = origin - direction;
                    segment.b = origin + direction;
                    IsTrue_IntersectSegment(ray, segment, origin, segment.b);
                    segment.a = origin;
                    segment.b = origin + direction*2;
                    IsTrue_IntersectSegment(ray, segment, segment.a, segment.b);
                    segment.a = origin + direction;
                    segment.b = origin + direction*50;
                    IsTrue_IntersectSegment(ray, segment, segment.a, segment.b);
                }
            }
        }

        [Test]
        public void Intersect_Parallel()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = origin + perpendicular - direction*50;
                    segment.b = origin + perpendicular - direction;
                    IsFalse_Intersect(ray, segment);
                    segment.a = origin + perpendicular - direction*2;
                    segment.b = origin + perpendicular;
                    IsFalse_Intersect(ray, segment);
                    segment.a = origin + perpendicular - direction;
                    segment.b = origin + perpendicular + direction;
                    IsFalse_Intersect(ray, segment);
                    segment.a = origin + perpendicular;
                    segment.b = origin + perpendicular + direction*2;
                    IsFalse_Intersect(ray, segment);
                    segment.a = origin + perpendicular + direction;
                    segment.b = origin + perpendicular + direction*50;
                    IsFalse_Intersect(ray, segment);
                }
            }
        }

        [Test]
        public void Intersect_Diagonal()
        {
            float length = 3;
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    for (int segmentAngle = 15; segmentAngle < 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment.a = origin;
                        segment.b = segment.a + segmentDirection*length;
                        IsTrue_IntersectPoint(ray, segment, segment.a);
                        segment.a = origin + direction;
                        segment.b = segment.a + segmentDirection*length;
                        IsTrue_IntersectPoint(ray, segment, segment.a);
                        segment.a = origin - direction;
                        segment.b = segment.a + segmentDirection.RotateCW90()*length;
                        IsFalse_Intersect(ray, segment);

                        segment.a = origin + perpendicular;
                        segment.b = segment.a + segmentDirection*length;
                        IsFalse_Intersect(ray, segment);
                        segment.a = origin + perpendicular + direction;
                        segment.b = segment.a + segmentDirection*length;
                        IsFalse_Intersect(ray, segment);
                        segment.a = origin + perpendicular*2 + direction;
                        segment.b = segment.a + segmentDirection*length;
                        IsFalse_Intersect(ray, segment);
                        segment.a = origin + perpendicular + direction*2;
                        segment.b = segment.a + segmentDirection*length;
                        IsFalse_Intersect(ray, segment);
                        segment.a = origin + perpendicular*2 + direction*2;
                        segment.b = segment.a + segmentDirection*length;
                        IsFalse_Intersect(ray, segment);

                        segment.a = origin - segmentDirection;
                        segment.b = segment.a + segmentDirection*length;
                        IsTrue_IntersectPoint(ray, segment, origin);
                        segment.a = origin + direction - segmentDirection;
                        segment.b = segment.a + segmentDirection*length;
                        IsTrue_IntersectPoint(ray, segment, origin + direction);
                    }

                    Vector2 diagonal = direction.RotateCW45();
                    for (int segmentAngle = 0; segmentAngle <= 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = diagonal.RotateCW(segmentAngle);
                        segment.a = origin + perpendicular - direction;
                        segment.b = segment.a + segmentDirection*length;
                        IsFalse_Intersect(ray, segment);
                        segment.a = origin + perpendicular*length - direction*length;
                        segment.b = segment.a + segmentDirection*length;
                        IsFalse_Intersect(ray, segment);
                    }

                    segment.a = origin + perpendicular - direction - diagonal*2;
                    segment.b = segment.a + diagonal*4;
                    IsFalse_Intersect(ray, segment);
                    segment.a = origin + perpendicular - direction;
                    segment.b = segment.a + diagonal*4;
                    IsFalse_Intersect(ray, segment);
                    segment.a = origin + perpendicular + direction;
                    segment.b = segment.a + diagonal.RotateCW90()*4;
                    IsFalse_Intersect(ray, segment);
                }
            }
        }

        [Test]
        public void Intersect_DegenerateSegment()
        {
            var ray = new Ray2D();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    ray.origin = origin;
                    ray.direction = direction;

                    segment.a = segment.b = origin;
                    IsTrue_IntersectPoint(ray, segment, segment.a);
                    segment.a = segment.b = origin + direction*50;
                    IsTrue_IntersectPoint(ray, segment, segment.a);
                    segment.a = segment.b = origin - direction*50;
                    IsFalse_Intersect(ray, segment);

                    segment.a = segment.b = origin + perpendicular;
                    IsFalse_Intersect(ray, segment);
                    segment.a = segment.b = origin + perpendicular + direction;
                    IsFalse_Intersect(ray, segment);
                    segment.a = segment.b = origin + perpendicular - direction;
                    IsFalse_Intersect(ray, segment);
                }
            }
        }

        private void IsTrue_IntersectSegment(Ray2D ray, Segment2 segment, Vector2 expectedA, Vector2 expectedB)
        {
            string message = string.Format(format, ray.ToString("F8"), segment);
            Assert.IsTrue(Intersect.RaySegment(ray.origin, ray.direction, segment.a, segment.b, out IntersectionRaySegment2 intersection), message);
            Assert.AreEqual(IntersectionType.Segment, intersection.type, message);
            AreEqual(intersection.pointA, expectedA, message);
            AreEqual(intersection.pointB, expectedB, message);
            Assert.IsTrue(Intersect.RaySegment(ray.origin, ray.direction, segment.b, segment.a, out intersection), message);
            Assert.AreEqual(IntersectionType.Segment, intersection.type);
            AreEqual(intersection.pointA, expectedA, message);
            AreEqual(intersection.pointB, expectedB, message);
        }

        private void IsTrue_IntersectPoint(Ray2D ray, Segment2 segment, Vector2 expected)
        {
            string message = string.Format(format, ray.ToString("F8"), segment);
            Assert.IsTrue(Intersect.RaySegment(ray.origin, ray.direction, segment.a, segment.b, out IntersectionRaySegment2 intersection), message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
            Assert.IsTrue(Intersect.RaySegment(ray.origin, ray.direction, segment.b, segment.a, out intersection), message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
        }

        private void IsFalse_Intersect(Ray2D ray, Segment2 segment)
        {
            string message = string.Format(format, ray.ToString("F8"), segment);
            Assert.IsFalse(Intersect.RaySegment(ray.origin, ray.direction, segment.a, segment.b, out _), message);
            Assert.IsFalse(Intersect.RaySegment(ray.origin, ray.direction, segment.b, segment.a, out _), message);
        }

        #endregion Intersect
    }
}

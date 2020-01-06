using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DPointSegmentTest : TestBase
    {
        private const string format = "{0:F8}\n{1}";
        private const float offset = 100;
        private const float length = 3;

        #region Distance

        [Test]
        public void Distance_PointOnLine()
        {
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    AreEqual_Distance(segment, segment.a);
                    AreEqual_Distance(segment, segment.a + direction);
                    AreEqual_Distance(segment, segment.a - direction*offset, offset);
                }
            }
        }

        [Test]
        public void Distance_PointNotOnLine()
        {
            float expected = Mathf.Sqrt(1 + offset*offset);
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    AreEqual_Distance(segment, segment.a + perpendicular, 1);
                    AreEqual_Distance(segment, segment.a + perpendicular + direction, 1);
                    AreEqual_Distance(segment, segment.a + perpendicular - direction*offset, expected);
                }
            }
        }

        [Test]
        public void Distance_Degenerate()
        {
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    segment.a = origin;
                    segment.b = origin;

                    AreEqual_Distance(segment, origin);
                    AreEqual_Distance(segment, origin + direction, 1);
                }
            }
        }

        private void AreEqual_Distance(Segment2 segment, Vector2 point, float expected = 0)
        {
            string message = string.Format(format, segment, point.ToString("F8"));
            AreEqual(Distance.PointSegment(point, segment.a, segment.b), expected, message);
            AreEqual(Distance.PointSegment(point, segment.b, segment.a), expected, message);
        }

        #endregion Distance

        #region ClosestPoint

        [Test]
        public void ClosestPoint_PointOnLine()
        {
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    AreEqual_ClosestPoint(segment, segment.a);
                    AreEqual_ClosestPoint(segment, segment.a + direction);
                    AreEqual_ClosestPoint(segment, segment.a - direction*offset, segment.a);
                }
            }
        }

        [Test]
        public void ClosestPoint_PointNotOnLine()
        {
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    AreEqual_ClosestPoint(segment, segment.a + perpendicular, segment.a);
                    AreEqual_ClosestPoint(segment, segment.a + perpendicular + direction, segment.a + direction);
                    AreEqual_ClosestPoint(segment, segment.a + perpendicular - direction*offset, segment.a);
                }
            }
        }

        [Test]
        public void ClosestPoint_Degenerate()
        {
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    segment.a = origin;
                    segment.b = origin;

                    AreEqual_ClosestPoint(segment, origin, origin);
                    AreEqual_ClosestPoint(segment, origin + direction, origin);
                }
            }
        }

        private void AreEqual_ClosestPoint(Segment2 segment, Vector2 point)
        {
            AreEqual_ClosestPoint(segment, point, point);
        }

        private void AreEqual_ClosestPoint(Segment2 segment, Vector2 point, Vector2 expected)
        {
            string message = string.Format(format, segment, point.ToString("F8"));
            AreEqual(Closest.PointSegment(point, segment.a, segment.b), expected, message);
            AreEqual(Closest.PointSegment(point, segment.b, segment.a), expected, message);
        }

        #endregion ClosestPoint

        #region Intersect

        [Test]
        public void Intersect_PointOnLine()
        {
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    True_Intersect(segment, segment.a);
                    True_Intersect(segment, segment.a + direction);
                    False_Intersect(segment, segment.a - direction*30);
                }
            }
        }

        [Test]
        public void Intersect_PointNotOnLine()
        {
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    False_Intersect(segment, segment.a + perpendicular, 1);
                    False_Intersect(segment, segment.a + perpendicular + direction, 1);
                    False_Intersect(segment, segment.a + perpendicular - direction*offset, 1);
                }
            }
        }

        [Test]
        public void Intersect_Degenerate()
        {
            var segment = new Segment2(Vector2.zero, Vector2.up);
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    segment.a = origin;
                    segment.b = origin;

                    True_Intersect(segment, origin);
                    False_Intersect(segment, origin + direction);
                }
            }
        }

        private void True_Intersect(Segment2 segment, Vector2 point)
        {
            string message = string.Format(format, segment, point.ToString("F8"));
            Assert.True(Intersect.PointSegment(point, segment.a, segment.b), message);
            Assert.True(Intersect.PointSegment(point, segment.a, segment.b, out int side), message);
            Assert.AreEqual(0, side, message);
            Assert.True(Intersect.PointSegment(point, segment.b, segment.a), message);
            Assert.True(Intersect.PointSegment(point, segment.b, segment.a, out side), message);
            Assert.AreEqual(0, side, message);
        }

        private void False_Intersect(Segment2 segment, Vector2 point, int expected = 0)
        {
            string message = string.Format(format, segment, point.ToString("F8"));
            Assert.False(Intersect.PointSegment(point, segment.a, segment.b), message);
            Assert.False(Intersect.PointSegment(point, segment.a, segment.b, out int side), message);
            Assert.AreEqual(expected, side, message);
            Assert.False(Intersect.PointSegment(point, segment.b, segment.a), message);
            Assert.False(Intersect.PointSegment(point, segment.b, segment.a, out side), message);
            Assert.AreEqual(-expected, side, message);
        }

        #endregion Intersect
    }
}

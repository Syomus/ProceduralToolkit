using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DLineSegmentTest : TestBase
    {
        private const string format = "{0:F8}\n{1:F8}";
        private const float length = 3;

        #region Distance

        [Test]
        public void Distance_Collinear()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = origin - direction*50;
                    segment.b = origin - direction;
                    AreEqual_Distance(line, segment);
                    segment.a = origin - direction*2;
                    segment.b = origin;
                    AreEqual_Distance(line, segment);
                    segment.a = origin - direction;
                    segment.b = origin + direction;
                    AreEqual_Distance(line, segment);
                    segment.a = origin;
                    segment.b = origin + direction*2;
                    AreEqual_Distance(line, segment);
                    segment.a = origin + direction;
                    segment.b = origin + direction*50;
                    AreEqual_Distance(line, segment);
                }
            }
        }

        [Test]
        public void Distance_Parallel()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = origin + perpendicular - direction*30;
                    segment.b = origin + perpendicular - direction;
                    AreEqual_Distance(line, segment, 1);
                    segment.a = origin + perpendicular - direction*2;
                    segment.b = origin + perpendicular;
                    AreEqual_Distance(line, segment, 1);
                    segment.a = origin + perpendicular - direction;
                    segment.b = origin + perpendicular + direction;
                    AreEqual_Distance(line, segment, 1);
                    segment.a = origin + perpendicular;
                    segment.b = origin + perpendicular + direction*2;
                    AreEqual_Distance(line, segment, 1);
                    segment.a = origin + perpendicular + direction;
                    segment.b = origin + perpendicular + direction*30;
                    AreEqual_Distance(line, segment, 1);
                }
            }
        }

        [Test]
        public void Distance_Diagonal()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    for (int segmentAngle = 1; segmentAngle < 180; segmentAngle += 10)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment.a = origin;
                        segment.b = origin + segmentDirection*length;
                        AreEqual_Distance(line, segment);
                        segment.a = origin + direction;
                        segment.b = origin + direction + segmentDirection*length;
                        AreEqual_Distance(line, segment);
                        segment.a = origin - direction;
                        segment.b = origin - direction + segmentDirection*length;
                        AreEqual_Distance(line, segment);

                        segment.a = origin + perpendicular;
                        segment.b = origin + perpendicular + segmentDirection*length;
                        AreEqual_Distance(line, segment, 1);
                        segment.a = origin + perpendicular + direction;
                        segment.b = origin + perpendicular + direction + segmentDirection*length;
                        AreEqual_Distance(line, segment, 1);
                        segment.a = origin + perpendicular - direction;
                        segment.b = origin + perpendicular - direction + segmentDirection*length;
                        AreEqual_Distance(line, segment, 1);
                    }
                }
            }
        }

        [Test]
        public void Distance_DegenerateSegment()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = segment.b = origin;
                    AreEqual_Distance(line, segment);
                    segment.a = segment.b = origin + direction*50;
                    AreEqual_Distance(line, segment);
                    segment.a = segment.b = origin - direction*50;
                    AreEqual_Distance(line, segment);

                    segment.a = segment.b = origin + perpendicular;
                    AreEqual_Distance(line, segment, 1);
                    segment.a = segment.b = origin + perpendicular + direction;
                    AreEqual_Distance(line, segment, 1);
                    segment.a = segment.b = origin + perpendicular - direction;
                    AreEqual_Distance(line, segment, 1);
                }
            }
        }

        private void AreEqual_Distance(Line2 line, Segment2 segment, float expected = 0)
        {
            string message = string.Format(format, line, segment);
            AreEqual(Distance.LineSegment(line.origin, line.direction, segment.a, segment.b), expected, message);
            AreEqual(Distance.LineSegment(line.origin, line.direction, segment.b, segment.a), expected, message);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_Collinear()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = origin - direction*50;
                    segment.b = origin - direction;
                    AreEqual_ClosestPoints(line, segment, segment.a);
                    segment.a = origin - direction*2;
                    segment.b = origin;
                    AreEqual_ClosestPoints(line, segment, segment.a);
                    segment.a = origin - direction;
                    segment.b = origin + direction;
                    AreEqual_ClosestPoints(line, segment, segment.a);
                    segment.a = origin;
                    segment.b = origin + direction*2;
                    AreEqual_ClosestPoints(line, segment, segment.a);
                    segment.a = origin + direction;
                    segment.b = origin + direction*50;
                    AreEqual_ClosestPoints(line, segment, segment.a);
                }
            }
        }

        [Test]
        public void ClosestPoints_Parallel()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = origin + perpendicular - direction*50;
                    segment.b = origin + perpendicular - direction;
                    AreEqual_ClosestPoints(line, segment, line.GetPoint(-50), segment.a);
                    segment.a = origin + perpendicular - direction*2;
                    segment.b = origin + perpendicular;
                    AreEqual_ClosestPoints(line, segment, line.GetPoint(-2), segment.a);
                    segment.a = origin + perpendicular - direction;
                    segment.b = origin + perpendicular + direction;
                    AreEqual_ClosestPoints(line, segment, line.GetPoint(-1), segment.a);
                    segment.a = origin + perpendicular;
                    segment.b = origin + perpendicular + direction*2;
                    AreEqual_ClosestPoints(line, segment, line.origin, segment.a);
                    segment.a = origin + perpendicular + direction;
                    segment.b = origin + perpendicular + direction*50;
                    AreEqual_ClosestPoints(line, segment, line.GetPoint(1), segment.a);
                }
            }
        }

        [Test]
        public void ClosestPoints_Diagonal()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    for (int segmentAngle = 1; segmentAngle < 180; segmentAngle += 10)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment.a = origin;
                        segment.b = origin + segmentDirection*length;
                        AreEqual_ClosestPoints(line, segment, origin);
                        segment.a = origin + direction;
                        segment.b = origin + direction + segmentDirection*length;
                        AreEqual_ClosestPoints(line, segment, origin + direction);
                        segment.a = origin - direction;
                        segment.b = origin - direction + segmentDirection*length;
                        AreEqual_ClosestPoints(line, segment, origin - direction);

                        segment.a = origin + perpendicular;
                        segment.b = origin + perpendicular + segmentDirection*length;
                        AreEqual_ClosestPoints(line, segment, origin, segment.a);
                        segment.a = origin + perpendicular + direction;
                        segment.b = origin + perpendicular + direction + segmentDirection*length;
                        AreEqual_ClosestPoints(line, segment, origin + direction, segment.a);
                        segment.a = origin + perpendicular - direction;
                        segment.b = origin + perpendicular - direction + segmentDirection*length;
                        AreEqual_ClosestPoints(line, segment, origin - direction, segment.a);
                    }
                }
            }
        }

        [Test]
        public void ClosestPoints_DegenerateSegment()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = segment.b = origin;
                    AreEqual_ClosestPoints(line, segment, segment.a);
                    segment.a = segment.b = origin + direction*50;
                    AreEqual_ClosestPoints(line, segment, segment.a);
                    segment.a = segment.b = origin - direction*50;
                    AreEqual_ClosestPoints(line, segment, segment.a);

                    segment.a = segment.b = origin + perpendicular;
                    AreEqual_ClosestPoints(line, segment, line.origin, segment.a);
                    segment.a = segment.b = origin + perpendicular + direction;
                    AreEqual_ClosestPoints(line, segment, line.origin + direction, segment.a);
                    segment.a = segment.b = origin + perpendicular - direction;
                    AreEqual_ClosestPoints(line, segment, line.origin - direction, segment.a);
                }
            }
        }

        private void AreEqual_ClosestPoints(Line2 line, Segment2 segment, Vector2 expected)
        {
            AreEqual_ClosestPoints(line, segment, expected, expected);
        }

        private void AreEqual_ClosestPoints(Line2 line, Segment2 segment, Vector2 expectedLine, Vector2 expectedSegment)
        {
            string message = string.Format(format, line, segment);
            Closest.LineSegment(line.origin, line.direction, segment.a, segment.b, out Vector2 linePoint, out Vector2 segmentPoint);
            AreEqual(linePoint, expectedLine, message);
            AreEqual(segmentPoint, expectedSegment, message);
            Closest.LineSegment(line.origin, line.direction, segment.b, segment.a, out linePoint, out segmentPoint);
            AreEqual(linePoint, expectedLine, message);
            AreEqual(segmentPoint, expectedSegment, message);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_Collinear()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = origin - direction*50;
                    segment.b = origin - direction;
                    IsTrue_IntersectSegment(line, segment);
                    segment.a = origin - direction*2;
                    segment.b = origin;
                    IsTrue_IntersectSegment(line, segment);
                    segment.a = origin - direction;
                    segment.b = origin + direction;
                    IsTrue_IntersectSegment(line, segment);
                    segment.a = origin;
                    segment.b = origin + direction*2;
                    IsTrue_IntersectSegment(line, segment);
                    segment.a = origin + direction;
                    segment.b = origin + direction*50;
                    IsTrue_IntersectSegment(line, segment);
                }
            }
        }

        [Test]
        public void Intersect_Parallel()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = origin + perpendicular - direction*50;
                    segment.b = origin + perpendicular - direction;
                    IsFalse_Intersect(line, segment);
                    segment.a = origin + perpendicular - direction*2;
                    segment.b = origin + perpendicular;
                    IsFalse_Intersect(line, segment);
                    segment.a = origin + perpendicular - direction;
                    segment.b = origin + perpendicular + direction;
                    IsFalse_Intersect(line, segment);
                    segment.a = origin + perpendicular;
                    segment.b = origin + perpendicular + direction*2;
                    IsFalse_Intersect(line, segment);
                    segment.a = origin + perpendicular + direction;
                    segment.b = origin + perpendicular + direction*50;
                    IsFalse_Intersect(line, segment);
                }
            }
        }

        [Test]
        public void Intersect_Diagonal()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    for (int segmentAngle = 1; segmentAngle < 180; segmentAngle += 10)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment.a = origin;
                        segment.b = origin + segmentDirection*length;
                        IsTrue_IntersectPoint(line, segment, origin);
                        segment.a = origin + direction;
                        segment.b = origin + direction + segmentDirection*length;
                        IsTrue_IntersectPoint(line, segment, origin + direction);
                        segment.a = origin - direction;
                        segment.b = origin - direction + segmentDirection*length;
                        IsTrue_IntersectPoint(line, segment, origin - direction);

                        segment.a = origin + perpendicular;
                        segment.b = origin + perpendicular + segmentDirection*length;
                        IsFalse_Intersect(line, segment);
                        segment.a = origin + perpendicular + direction;
                        segment.b = origin + perpendicular + direction + segmentDirection*length;
                        IsFalse_Intersect(line, segment);
                        segment.a = origin + perpendicular - direction;
                        segment.b = origin + perpendicular - direction + segmentDirection*length;
                        IsFalse_Intersect(line, segment);
                    }
                }
            }
        }

        [Test]
        public void Intersect_DegenerateSegment()
        {
            var line = new Line2();
            var segment = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    line.origin = origin;
                    line.direction = direction;

                    segment.a = segment.b = origin;
                    IsTrue_IntersectPoint(line, segment, segment.a);
                    segment.a = segment.b = origin + direction*50;
                    IsTrue_IntersectPoint(line, segment, segment.a);
                    segment.a = segment.b = origin - direction*50;
                    IsTrue_IntersectPoint(line, segment, segment.a);

                    segment.a = segment.b = origin + perpendicular;
                    IsFalse_Intersect(line, segment);
                    segment.a = segment.b = origin + perpendicular + direction;
                    IsFalse_Intersect(line, segment);
                    segment.a = segment.b = origin + perpendicular - direction;
                    IsFalse_Intersect(line, segment);
                }
            }
        }

        private void IsTrue_IntersectSegment(Line2 line, Segment2 segment)
        {
            string message = string.Format(format, line, segment);
            Assert.IsTrue(Intersect.LineSegment(line.origin, line.direction, segment.a, segment.b, out IntersectionLineSegment2 intersection),
                message);
            Assert.AreEqual(IntersectionType.Segment, intersection.type, message);
            AreEqual(intersection.pointA, segment.a, message);
            AreEqual(intersection.pointB, segment.b, message);
            Assert.IsTrue(Intersect.LineSegment(line.origin, line.direction, segment.b, segment.a, out intersection), message);
            Assert.AreEqual(IntersectionType.Segment, intersection.type, message);
            AreEqual(intersection.pointA, segment.a, message);
            AreEqual(intersection.pointB, segment.b, message);
        }

        private void IsTrue_IntersectPoint(Line2 line, Segment2 segment, Vector2 expected)
        {
            string message = string.Format(format, line, segment);
            Assert.IsTrue(Intersect.LineSegment(line.origin, line.direction, segment.a, segment.b, out IntersectionLineSegment2 intersection),
                message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
            Assert.IsTrue(Intersect.LineSegment(line.origin, line.direction, segment.b, segment.a, out intersection), message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
        }

        private void IsFalse_Intersect(Line2 line, Segment2 segment)
        {
            Assert.IsFalse(Intersect.LineSegment(line.origin, line.direction, segment.a, segment.b, out _), format, line, segment);
            Assert.IsFalse(Intersect.LineSegment(line.origin, line.direction, segment.b, segment.a, out _), format, line, segment);
        }

        #endregion Intersect
    }
}

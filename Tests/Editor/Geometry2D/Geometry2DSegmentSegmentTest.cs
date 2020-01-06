using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DSegmentSegmentTest : TestBase
    {
        private const string format = "{0:F8}\n{1:F8}";
        private const float length = 3;

        #region Distance

        [Test]
        public void Distance_Collinear()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    // Coincident
                    segment1.a = segment2.a = origin;
                    segment1.b = segment2.b = origin + direction;
                    AreEqual_Distance(segment1, segment2);
                    segment1.b = segment2.b = origin + direction*100;
                    AreEqual_Distance(segment1, segment2);

                    // Segment
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2.5f;
                    AreEqual_Distance(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2;
                    AreEqual_Distance(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2.5f;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2;
                    AreEqual_Distance(segment1, segment2);

                    // Point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*2;
                    segment2.b = origin + direction*4;
                    AreEqual_Distance(segment1, segment2);

                    // No intersection
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*3;
                    segment2.b = origin + direction*5;
                    AreEqual_Distance(segment1, segment2, 1);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*4;
                    segment2.b = origin + direction*6;
                    AreEqual_Distance(segment1, segment2, 2);
                }
            }
        }

        [Test]
        public void Distance_Parallel()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();

                    segment1.a = origin;
                    segment1.b = origin + direction;
                    segment2.a = origin + perpendicular;
                    segment2.b = origin + perpendicular + direction;
                    AreEqual_Distance(segment1, segment2, 1);
                    segment1.a = origin;
                    segment1.b = origin + direction*30;
                    segment2.a = origin + perpendicular;
                    segment2.b = origin + perpendicular + direction*30;
                    AreEqual_Distance(segment1, segment2, 1);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2.5f;
                    AreEqual_Distance(segment1, segment2, 1);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2;
                    AreEqual_Distance(segment1, segment2, 1);
                    segment1.a = origin;
                    segment1.b = origin + direction*2.5f;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2;
                    AreEqual_Distance(segment1, segment2, 1);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*2;
                    segment2.b = origin + perpendicular + direction*4;
                    AreEqual_Distance(segment1, segment2, 1);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*3;
                    segment2.b = origin + perpendicular + direction*5;
                    AreEqual_Distance(segment1, segment2, PTUtils.Sqrt2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*4;
                    segment2.b = origin + perpendicular + direction*6;
                    AreEqual_Distance(segment1, segment2, Mathf.Sqrt(5));
                }
            }
        }

        [Test]
        public void Distance_Diagonal()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    segment1.a = origin;
                    segment1.b = origin + direction*10;

                    for (int segmentAngle = 15; segmentAngle < 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment2.a = origin;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2);
                        segment2.a = origin + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2);
                        segment2.a = origin - direction;
                        segment2.b = segment2.a + segmentDirection.RotateCW90()*length;
                        AreEqual_Distance(segment1, segment2, 1);

                        segment2.a = origin + perpendicular;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2, 1);
                        segment2.a = origin + perpendicular + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2, 1);
                        segment2.a = origin + perpendicular*2 + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2, 2);
                        segment2.a = origin + perpendicular + direction*2;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2, 1);
                        segment2.a = origin + perpendicular*2 + direction*2;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2, 2);

                        segment2.a = origin - segmentDirection;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2);
                        segment2.a = origin + direction - segmentDirection;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2);
                    }

                    Vector2 diagonal = direction.RotateCW45();
                    for (int segmentAngle = 0; segmentAngle <= 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = diagonal.RotateCW(segmentAngle);
                        segment2.a = origin + perpendicular - direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2, PTUtils.Sqrt2);
                        segment2.a = origin + perpendicular*length - direction*length;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_Distance(segment1, segment2, PTUtils.Sqrt2*length);
                    }

                    segment2.a = origin + perpendicular - direction - diagonal*2;
                    segment2.b = segment2.a + diagonal*4;
                    AreEqual_Distance(segment1, segment2, PTUtils.Sqrt2);
                    segment2.a = origin + perpendicular - direction;
                    segment2.b = segment2.a + diagonal*4;
                    AreEqual_Distance(segment1, segment2, PTUtils.Sqrt2);
                    segment2.a = origin + perpendicular + direction;
                    segment2.b = segment2.a + diagonal.RotateCW90()*4;
                    AreEqual_Distance(segment1, segment2, 1);
                }
            }
        }

        [Test]
        public void Distance_Degenerate()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();

                    // Two points
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin;
                    AreEqual_Distance(segment1, segment2);
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin + direction;
                    AreEqual_Distance(segment1, segment2, 1);
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin + direction*2;
                    AreEqual_Distance(segment1, segment2, 2);

                    // One collinear point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin;
                    AreEqual_Distance(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction;
                    AreEqual_Distance(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction*3;
                    AreEqual_Distance(segment1, segment2, 1);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction*4;
                    AreEqual_Distance(segment1, segment2, 2);

                    // One noncollinear point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular;
                    AreEqual_Distance(segment1, segment2, 1);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction;
                    AreEqual_Distance(segment1, segment2, 1);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction*3;
                    AreEqual_Distance(segment1, segment2, PTUtils.Sqrt2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction*4;
                    AreEqual_Distance(segment1, segment2, Mathf.Sqrt(5));
                }
            }
        }

        private void AreEqual_Distance(Segment2 segment1, Segment2 segment2, float expected = 0)
        {
            string message = string.Format(format, segment1, segment2);
            AreEqual(Distance.SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b), expected, message);
            AreEqual(Distance.SegmentSegment(segment1.a, segment1.b, segment2.b, segment2.a), expected, message);
            AreEqual(Distance.SegmentSegment(segment2.a, segment2.b, segment1.a, segment1.b), expected, message);
            AreEqual(Distance.SegmentSegment(segment2.a, segment2.b, segment1.b, segment1.a), expected, message);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_Collinear()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    // Coincident
                    segment1.a = segment2.a = origin;
                    segment1.b = segment2.b = origin + direction;
                    AreEqual_ClosestPoints_Point(segment1, segment2, segment1.a);
                    segment1.b = segment2.b = origin + direction*100;
                    AreEqual_ClosestPoints_Point(segment1, segment2, segment1.a);

                    // Segment
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2.5f;
                    AreEqual_ClosestPoints_Point(segment1, segment2, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2;
                    AreEqual_ClosestPoints_Point(segment1, segment2, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2.5f;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2;
                    AreEqual_ClosestPoints_Point(segment1, segment2, segment2.a);

                    // Point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*2;
                    segment2.b = origin + direction*4;
                    AreEqual_ClosestPoints_Point(segment1, segment2, segment2.a);

                    // No intersection
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*3;
                    segment2.b = origin + direction*5;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*4;
                    segment2.b = origin + direction*6;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);
                }
            }
        }

        [Test]
        public void ClosestPoints_Parallel()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();

                    segment1.a = origin;
                    segment1.b = origin + direction;
                    segment2.a = origin + perpendicular;
                    segment2.b = origin + perpendicular + direction;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.a, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*30;
                    segment2.a = origin + perpendicular;
                    segment2.b = origin + perpendicular + direction*30;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.a, segment2.a);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2.5f;
                    AreEqual_ClosestPoints(segment1, segment2, origin + direction*0.5f, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2;
                    AreEqual_ClosestPoints(segment1, segment2, origin + direction*0.5f, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2.5f;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2;
                    AreEqual_ClosestPoints(segment1, segment2, origin + direction*0.5f, segment2.a);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*2;
                    segment2.b = origin + perpendicular + direction*4;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*3;
                    segment2.b = origin + perpendicular + direction*5;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*4;
                    segment2.b = origin + perpendicular + direction*6;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);
                }
            }
        }

        [Test]
        public void ClosestPoints_Diagonal()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    segment1.a = origin;
                    segment1.b = origin + direction*10;

                    for (int segmentAngle = 15; segmentAngle < 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment2.a = origin;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints_Point(segment1, segment2, segment2.a);
                        segment2.a = origin + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints_Point(segment1, segment2, segment2.a);
                        segment2.a = origin - direction;
                        segment2.b = segment2.a + segmentDirection.RotateCW90()*length;
                        AreEqual_ClosestPoints(segment1, segment2, origin, segment2.a);

                        segment2.a = origin + perpendicular;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints(segment1, segment2, origin, segment2.a);
                        segment2.a = origin + perpendicular + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints(segment1, segment2, origin + direction, segment2.a);
                        segment2.a = origin + perpendicular*2 + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints(segment1, segment2, origin + direction, segment2.a);
                        segment2.a = origin + perpendicular + direction*2;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints(segment1, segment2, origin + direction*2, segment2.a);
                        segment2.a = origin + perpendicular*2 + direction*2;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints(segment1, segment2, origin + direction*2, segment2.a);

                        segment2.a = origin - segmentDirection;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints_Point(segment1, segment2, origin);
                        segment2.a = origin + direction - segmentDirection;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints_Point(segment1, segment2, origin + direction);
                    }

                    Vector2 diagonal = direction.RotateCW45();
                    for (int segmentAngle = 0; segmentAngle <= 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = diagonal.RotateCW(segmentAngle);
                        segment2.a = origin + perpendicular - direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints(segment1, segment2, origin, segment2.a);
                        segment2.a = origin + perpendicular*length - direction*length;
                        segment2.b = segment2.a + segmentDirection*length;
                        AreEqual_ClosestPoints(segment1, segment2, origin, segment2.a);
                    }

                    segment2.a = origin + perpendicular - direction - diagonal*2;
                    segment2.b = segment2.a + diagonal*4;
                    AreEqual_ClosestPoints(segment1, segment2, origin, origin + perpendicular - direction);
                    segment2.a = origin + perpendicular - direction;
                    segment2.b = segment2.a + diagonal*4;
                    AreEqual_ClosestPoints(segment1, segment2, origin, segment2.a);
                    segment2.a = origin + perpendicular + direction;
                    segment2.b = segment2.a + diagonal.RotateCW90()*4;
                    AreEqual_ClosestPoints(segment1, segment2, origin + direction, segment2.a);
                }
            }
        }

        [Test]
        public void ClosestPoints_Degenerate()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();

                    // Two points
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin;
                    AreEqual_ClosestPoints_Point(segment1, segment2, origin);
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin + direction;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.a, segment2.a);
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin + direction*2;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.a, segment2.a);

                    // One collinear point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin;
                    AreEqual_ClosestPoints_Point(segment1, segment2, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction;
                    AreEqual_ClosestPoints_Point(segment1, segment2, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction*3;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction*4;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);

                    // One noncollinear point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.a, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction;
                    AreEqual_ClosestPoints(segment1, segment2, origin + direction, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction*3;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction*4;
                    AreEqual_ClosestPoints(segment1, segment2, segment1.b, segment2.a);
                }
            }
        }

        private void AreEqual_ClosestPoints_Point(Segment2 segment1, Segment2 segment2, Vector2 expected)
        {
            AreEqual_ClosestPoints(segment1, segment2, expected, expected, expected, expected);
        }

        private void AreEqual_ClosestPoints(Segment2 segment1, Segment2 segment2, Vector2 expected1, Vector2 expected2)
        {
            AreEqual_ClosestPoints(segment1, segment2, expected1, expected2, expected2, expected1);
        }

        private void AreEqual_ClosestPoints(Segment2 segment1, Segment2 segment2, Vector2 expected1A, Vector2 expected2A,
            Vector2 expected1B, Vector2 expected2B)
        {
            string message = string.Format(format, segment1, segment2);
            Closest.SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out Vector2 point1, out Vector2 point2);
            AreEqual(point1, expected1A, message);
            AreEqual(point2, expected2A, message);
            Closest.SegmentSegment(segment1.a, segment1.b, segment2.b, segment2.a, out point1, out point2);
            AreEqual(point1, expected1A, message);
            AreEqual(point2, expected2A, message);
            Closest.SegmentSegment(segment2.a, segment2.b, segment1.a, segment1.b, out point1, out point2);
            AreEqual(point1, expected1B, message);
            AreEqual(point2, expected2B, message);
            Closest.SegmentSegment(segment2.a, segment2.b, segment1.b, segment1.a, out point1, out point2);
            AreEqual(point1, expected1B, message);
            AreEqual(point2, expected2B, message);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_Collinear()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    // Coincident
                    segment1.a = segment2.a = origin;
                    segment1.b = segment2.b = origin + direction;
                    True_IntersectSegment(segment1, segment2, segment1.a, segment1.b);
                    segment1.b = segment2.b = origin + direction*100;
                    True_IntersectSegment(segment1, segment2, segment1.a, segment1.b);

                    // Segment
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2.5f;
                    True_IntersectSegment(segment1, segment2, segment2.a, segment1.b);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2;
                    True_IntersectSegment(segment1, segment2, segment2.a, segment1.b);
                    segment1.a = origin;
                    segment1.b = origin + direction*2.5f;
                    segment2.a = origin + direction*0.5f;
                    segment2.b = origin + direction*2;
                    True_IntersectSegment(segment1, segment2, segment2.a, segment2.b);

                    // Point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*2;
                    segment2.b = origin + direction*4;
                    True_IntersectPoint(segment1, segment2, segment1.b);

                    // No intersection
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*3;
                    segment2.b = origin + direction*5;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + direction*4;
                    segment2.b = origin + direction*6;
                    False_Intersect(segment1, segment2);
                }
            }
        }

        [Test]
        public void Intersect_Parallel()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();

                    segment1.a = origin;
                    segment1.b = origin + direction;
                    segment2.a = origin + perpendicular;
                    segment2.b = origin + perpendicular + direction;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*30;
                    segment2.a = origin + perpendicular;
                    segment2.b = origin + perpendicular + direction*30;
                    False_Intersect(segment1, segment2);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2.5f;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2.5f;
                    segment2.a = origin + perpendicular + direction*0.5f;
                    segment2.b = origin + perpendicular + direction*2;
                    False_Intersect(segment1, segment2);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*2;
                    segment2.b = origin + perpendicular + direction*4;
                    False_Intersect(segment1, segment2);

                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*3;
                    segment2.b = origin + perpendicular + direction*5;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = origin + perpendicular + direction*4;
                    segment2.b = origin + perpendicular + direction*6;
                    False_Intersect(segment1, segment2);
                }
            }
        }

        [Test]
        public void Intersect_Diagonal()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();
                    segment1.a = origin;
                    segment1.b = origin + direction*10;

                    for (int segmentAngle = 15; segmentAngle < 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = direction.RotateCW(segmentAngle);

                        segment2.a = origin;
                        segment2.b = segment2.a + segmentDirection*length;
                        True_IntersectPoint(segment1, segment2, segment2.a);
                        segment2.a = origin + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        True_IntersectPoint(segment1, segment2, segment2.a);
                        segment2.a = origin - direction;
                        segment2.b = segment2.a + segmentDirection.RotateCW90()*length;
                        False_Intersect(segment1, segment2);

                        segment2.a = origin + perpendicular;
                        segment2.b = segment2.a + segmentDirection*length;
                        False_Intersect(segment1, segment2);
                        segment2.a = origin + perpendicular + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        False_Intersect(segment1, segment2);
                        segment2.a = origin + perpendicular*2 + direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        False_Intersect(segment1, segment2);
                        segment2.a = origin + perpendicular + direction*2;
                        segment2.b = segment2.a + segmentDirection*length;
                        False_Intersect(segment1, segment2);
                        segment2.a = origin + perpendicular*2 + direction*2;
                        segment2.b = segment2.a + segmentDirection*length;
                        False_Intersect(segment1, segment2);

                        segment2.a = origin - segmentDirection;
                        segment2.b = segment2.a + segmentDirection*length;
                        True_IntersectPoint(segment1, segment2, origin);
                        segment2.a = origin + direction - segmentDirection;
                        segment2.b = segment2.a + segmentDirection*length;
                        True_IntersectPoint(segment1, segment2, origin + direction);
                    }

                    Vector2 diagonal = direction.RotateCW45();
                    for (int segmentAngle = 0; segmentAngle <= 180; segmentAngle += 15)
                    {
                        Vector2 segmentDirection = diagonal.RotateCW(segmentAngle);
                        segment2.a = origin + perpendicular - direction;
                        segment2.b = segment2.a + segmentDirection*length;
                        False_Intersect(segment1, segment2);
                        segment2.a = origin + perpendicular*length - direction*length;
                        segment2.b = segment2.a + segmentDirection*length;
                        False_Intersect(segment1, segment2);
                    }

                    segment2.a = origin + perpendicular - direction - diagonal*2;
                    segment2.b = segment2.a + diagonal*4;
                    False_Intersect(segment1, segment2);
                    segment2.a = origin + perpendicular - direction;
                    segment2.b = segment2.a + diagonal*4;
                    False_Intersect(segment1, segment2);
                    segment2.a = origin + perpendicular + direction;
                    segment2.b = segment2.a + diagonal.RotateCW90()*4;
                    False_Intersect(segment1, segment2);
                }
            }
        }

        [Test]
        public void Intersect_Degenerate()
        {
            var segment1 = new Segment2();
            var segment2 = new Segment2();
            foreach (var origin in originPoints2)
            {
                foreach (var direction in directionPoints2)
                {
                    Vector2 perpendicular = direction.RotateCW90();

                    // Two points
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin;
                    True_IntersectPoint(segment1, segment2, segment1.a);
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin + direction;
                    False_Intersect(segment1, segment2);
                    segment1.a = segment1.b = origin;
                    segment2.a = segment2.b = origin + direction*2;
                    False_Intersect(segment1, segment2);

                    // One collinear point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin;
                    True_IntersectPoint(segment1, segment2, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction;
                    True_IntersectPoint(segment1, segment2, segment2.a);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction*3;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + direction*4;
                    False_Intersect(segment1, segment2);

                    // One noncollinear point
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction*3;
                    False_Intersect(segment1, segment2);
                    segment1.a = origin;
                    segment1.b = origin + direction*2;
                    segment2.a = segment2.b = origin + perpendicular + direction*4;
                    False_Intersect(segment1, segment2);
                }
            }
        }

        private void True_IntersectPoint(Segment2 segment1, Segment2 segment2, Vector2 expected)
        {
            string message = string.Format(format, segment1, segment2);
            Assert.IsTrue(Intersect.SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out IntersectionSegmentSegment2 intersection),
                message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
            Assert.IsTrue(Intersect.SegmentSegment(segment1.a, segment1.b, segment2.b, segment2.a, out intersection), message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
            Assert.IsTrue(Intersect.SegmentSegment(segment2.a, segment2.b, segment1.a, segment1.b, out intersection), message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
            Assert.IsTrue(Intersect.SegmentSegment(segment2.a, segment2.b, segment1.b, segment1.a, out intersection), message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
        }

        private void True_IntersectSegment(Segment2 segment1, Segment2 segment2, Vector2 expectedA, Vector2 expectedB)
        {
            True_IntersectSegment(segment1, segment2, expectedA, expectedB, expectedA, expectedB);
        }

        private void True_IntersectSegment(Segment2 segment1, Segment2 segment2, Vector2 expected1A, Vector2 expected1B, Vector2 expected2A,
            Vector2 expected2B)
        {
            string message = string.Format(format, segment1, segment2);
            Assert.IsTrue(Intersect.SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out IntersectionSegmentSegment2 intersection),
                message);
            Assert.AreEqual(IntersectionType.Segment, intersection.type, message);
            AreEqual(intersection.pointA, expected1A, message);
            AreEqual(intersection.pointB, expected1B, message);
            Assert.IsTrue(Intersect.SegmentSegment(segment1.a, segment1.b, segment2.b, segment2.a, out intersection), message);
            Assert.AreEqual(IntersectionType.Segment, intersection.type, message);
            AreEqual(intersection.pointA, expected1A, message);
            AreEqual(intersection.pointB, expected1B, message);
            Assert.IsTrue(Intersect.SegmentSegment(segment2.a, segment2.b, segment1.a, segment1.b, out intersection), message);
            Assert.AreEqual(IntersectionType.Segment, intersection.type, message);
            AreEqual(intersection.pointA, expected2A, message);
            AreEqual(intersection.pointB, expected2B, message);
            Assert.IsTrue(Intersect.SegmentSegment(segment2.a, segment2.b, segment1.b, segment1.a, out intersection), message);
            Assert.AreEqual(IntersectionType.Segment, intersection.type, message);
            AreEqual(intersection.pointA, expected2A, message);
            AreEqual(intersection.pointB, expected2B, message);
        }

        private void False_Intersect(Segment2 segment1, Segment2 segment2)
        {
            Assert.IsFalse(Intersect.SegmentSegment(segment1.a, segment1.b, segment2.a, segment2.b, out _), format, segment1, segment2);
            Assert.IsFalse(Intersect.SegmentSegment(segment1.a, segment1.b, segment2.b, segment2.a, out _), format, segment1, segment2);
            Assert.IsFalse(Intersect.SegmentSegment(segment2.a, segment2.b, segment1.a, segment1.b, out _), format, segment1, segment2);
            Assert.IsFalse(Intersect.SegmentSegment(segment2.a, segment2.b, segment1.b, segment1.a, out _), format, segment1, segment2);
        }

        #endregion Intersect
    }
}

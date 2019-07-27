using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry3D
{
    public class Geometry3DPointSegmentTest : TestBase
    {
        private const string format = "{0:F8}\n{1}";
        private const float offset = 100;
        private const float length = 3;

        #region Distance

        [Test]
        public void Distance_PointOnLine()
        {
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
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
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;

                        AreEqual_Distance(segment, segment.a + perpendicular, 1);
                        AreEqual_Distance(segment, segment.a + perpendicular + direction, 1);
                        AreEqual_Distance(segment, segment.a + perpendicular - direction*offset, expected);
                    }
                }
            }
        }

        [Test]
        public void Distance_Degenerate()
        {
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    segment.a = origin;
                    segment.b = origin;

                    AreEqual_Distance(segment, origin);
                    AreEqual_Distance(segment, origin + direction, 1);
                }
            }
        }

        private void AreEqual_Distance(Segment3 segment, Vector3 point, float expected = 0)
        {
            AreEqual(Distance.PointSegment(point, segment.a, segment.b), expected);
            AreEqual(Distance.PointSegment(point, segment.b, segment.a), expected);
        }

        #endregion Distance

        #region ClosestPoint

        [Test]
        public void ClosestPoint_PointOnLine()
        {
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
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
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;

                        AreEqual_ClosestPoint(segment, segment.a + perpendicular, segment.a);
                        AreEqual_ClosestPoint(segment, segment.a + perpendicular + direction, segment.a + direction);
                        AreEqual_ClosestPoint(segment, segment.a + perpendicular - direction*offset, segment.a);
                    }
                }
            }
        }

        [Test]
        public void ClosestPoint_Degenerate()
        {
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    segment.a = origin;
                    segment.b = origin;

                    AreEqual_ClosestPoint(segment, origin, origin);
                    AreEqual_ClosestPoint(segment, origin + direction, origin);
                }
            }
        }

        private void AreEqual_ClosestPoint(Segment3 segment, Vector3 point)
        {
            AreEqual_ClosestPoint(segment, point, point);
        }

        private void AreEqual_ClosestPoint(Segment3 segment, Vector3 point, Vector3 expected)
        {
            AreEqual(Closest.PointSegment(point, segment.a, segment.b), expected);
            AreEqual(Closest.PointSegment(point, segment.b, segment.a), expected);
        }

        #endregion ClosestPoint

        #region Intersect

        [Test]
        public void Intersect_PointOnLine()
        {
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    True_Intersect(segment, segment.a);
                    True_Intersect(segment, segment.a + direction);
                    False_Intersect(segment, segment.a - direction*offset);
                }
            }
        }

        [Test]
        public void Intersect_PointNotOnLine()
        {
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    segment.a = origin;
                    segment.b = origin + direction*length;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;

                        False_Intersect(segment, segment.a + perpendicular);
                        False_Intersect(segment, segment.a + perpendicular + direction);
                        False_Intersect(segment, segment.a + perpendicular - direction*offset);
                    }
                }
            }
        }

        [Test]
        public void Intersect_Degenerate()
        {
            var segment = new Segment3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    segment.a = origin;
                    segment.b = origin;

                    True_Intersect(segment, origin);
                    False_Intersect(segment, origin + direction);
                }
            }
        }

        private void False_Intersect(Segment3 segment, Vector3 point)
        {
            Assert.False(Intersect.PointSegment(point, segment.a, segment.b), format, segment, point);
            Assert.False(Intersect.PointSegment(point, segment.b, segment.a), format, segment, point);
        }

        private void True_Intersect(Segment3 segment, Vector3 point)
        {
            Assert.True(Intersect.PointSegment(point, segment.a, segment.b), format, segment, point);
            Assert.True(Intersect.PointSegment(point, segment.b, segment.a), format, segment, point);
        }

        #endregion Intersect
    }
}

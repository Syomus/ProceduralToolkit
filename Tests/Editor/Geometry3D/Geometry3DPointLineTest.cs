using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry3D
{
    public class Geometry3DPointLineTest : TestBase
    {
        private const string format = "{0:F8}\n{1}";
        private const float offset = 100;

        #region Distance

        [Test]
        public void Distance_PointOnLine()
        {
            var line = new Line3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    line.origin = origin;
                    line.direction = direction;

                    AreEqual_Distance(line, origin);
                    AreEqual_Distance(line, origin + direction*offset);
                    AreEqual_Distance(line, origin - direction*offset);
                }
            }
        }

        [Test]
        public void Distance_PointNotOnLine()
        {
            var line = new Line3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    line.origin = origin;
                    line.direction = direction;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        AreEqual_Distance(line, origin + perpendicular, 1);
                        AreEqual_Distance(line, origin + perpendicular + direction*offset, 1);
                        AreEqual_Distance(line, origin + perpendicular - direction*offset, 1);
                    }
                }
            }
        }

        private void AreEqual_Distance(Line3 line, Vector3 point, float expected = 0)
        {
            AreEqual(Distance.PointLine(point, line), expected);
        }

        #endregion Distance

        #region ClosestPoint

        [Test]
        public void ClosestPoint_PointOnLine()
        {
            var line = new Line3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    line.origin = origin;
                    line.direction = direction;

                    AreEqual_ClosestPoint(line, origin);
                    AreEqual_ClosestPoint(line, origin + direction*offset);
                    AreEqual_ClosestPoint(line, origin - direction*offset);
                }
            }
        }

        [Test]
        public void ClosestPoint_PointNotOnLine()
        {
            var line = new Line3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    line.origin = origin;
                    line.direction = direction;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        AreEqual_ClosestPoint(line, origin + perpendicular, origin);
                        AreEqual_ClosestPoint(line, origin + perpendicular + direction*offset, line.GetPoint(offset));
                        AreEqual_ClosestPoint(line, origin + perpendicular - direction*offset, line.GetPoint(-offset));
                    }
                }
            }
        }

        private void AreEqual_ClosestPoint(Line3 line, Vector3 point)
        {
            AreEqual_ClosestPoint(line, point, point);
        }

        private void AreEqual_ClosestPoint(Line3 line, Vector3 point, Vector3 expected)
        {
            AreEqual(Closest.PointLine(point, line), expected);
        }

        #endregion ClosestPoint

        #region Intersect

        [Test]
        public void Intersect_PointOnLine()
        {
            var line = new Line3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    line.origin = origin;
                    line.direction = direction;

                    True_Intersect(line, origin);
                    True_Intersect(line, origin + direction*offset*0.5f);
                    True_Intersect(line, origin - direction*offset*0.5f);
                }
            }
        }

        [Test]
        public void Intersect_PointNotOnLine()
        {
            var line = new Line3();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    line.origin = origin;
                    line.direction = direction;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        False_Intersect(line, origin + perpendicular);
                        False_Intersect(line, origin + perpendicular + direction*offset);
                        False_Intersect(line, origin + perpendicular - direction*offset);
                    }
                }
            }
        }

        private void False_Intersect(Line3 line, Vector3 point)
        {
            Assert.False(Intersect.PointLine(point, line), format, line, point);
        }

        private void True_Intersect(Line3 line, Vector3 point)
        {
            Assert.True(Intersect.PointLine(point, line), format, line, point);
        }

        #endregion Intersect
    }
}

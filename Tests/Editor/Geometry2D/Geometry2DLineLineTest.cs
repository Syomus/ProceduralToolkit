using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DLineLineTest : TestBase
    {
        private const string linesFormat = "{0:G9}\n{1:G9}";

        #region Distance

        [Test]
        public void Distance_Coincident()
        {
            var line = new Line2();
            foreach (var origin in originPoints2)
            {
                line.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    line.direction = direction;
                    AreEqual_Distance(line, line);
                }
            }
        }

        [Test]
        public void Distance_Collinear()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = lineB.direction = direction;
                    lineB.origin = lineA.GetPoint(100);
                    AreEqual_DistanceSwap(lineA, lineB);
                    lineB.direction = -lineB.direction;
                    AreEqual_DistanceSwap(lineA, lineB);
                }
            }
        }

        [Test]
        public void Distance_Parallel()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = lineB.direction = direction;
                    lineB.origin = lineA.GetPoint(30) + lineA.direction.RotateCCW90();
                    AreEqual_DistanceSwap(lineA, lineB, 1);
                    lineB.direction = -lineB.direction;
                    AreEqual_DistanceSwap(lineA, lineB, 1);
                }
            }
        }

        [Test]
        public void Distance_Diagonal()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = lineB.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = direction;
                    for (int bAngle = 1; bAngle < 180; bAngle += 20)
                    {
                        lineB.direction = lineA.direction.RotateCW(bAngle);
                        AreEqual_Distance(lineA, lineB);
                    }
                }
            }
        }

        private void AreEqual_DistanceSwap(Line2 lineA, Line2 lineB, float expected = 0)
        {
            string message = string.Format(linesFormat, lineA, lineB);
            AreEqual(Distance.LineLine(lineA, lineB), expected, message);
            AreEqual(Distance.LineLine(lineB, lineA), expected, message);
        }

        private void AreEqual_Distance(Line2 lineA, Line2 lineB, float expected = 0)
        {
            string message = string.Format(linesFormat, lineA, lineB);
            AreEqual(Distance.LineLine(lineA, lineB), expected, message);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_Coincident()
        {
            var line = new Line2();
            foreach (var origin in originPoints2)
            {
                line.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    line.direction = direction;
                    AreEqual_ClosestPoints(line, line, line.origin);
                }
            }
        }

        [Test]
        public void ClosestPoints_Collinear()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = lineB.direction = direction;
                    lineB.origin = lineA.GetPoint(50);
                    AreEqual_ClosestPoints(lineA, lineB, lineA.origin);
                    AreEqual_ClosestPoints(lineB, lineA, lineB.origin);
                    lineB.direction = -lineB.direction;
                    AreEqual_ClosestPoints(lineA, lineB, lineA.origin);
                    AreEqual_ClosestPoints(lineB, lineA, lineB.origin);
                }
            }
        }

        [Test]
        public void ClosestPoints_Parallel()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = lineB.direction = direction;
                    Vector2 perpendicular = lineA.direction.RotateCCW90();
                    lineB.origin = lineA.GetPoint(30) + perpendicular;
                    AreEqual_ClosestPoints(lineA, lineB, lineA.origin, lineA.origin + perpendicular);
                    AreEqual_ClosestPoints(lineB, lineA, lineB.origin, lineB.origin - perpendicular);
                    lineB.direction = -lineB.direction;
                    AreEqual_ClosestPoints(lineA, lineB, lineA.origin, lineA.origin + perpendicular);
                    AreEqual_ClosestPoints(lineB, lineA, lineB.origin, lineB.origin - perpendicular);
                }
            }
        }

        [Test]
        public void ClosestPoints_Diagonal()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = lineB.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = direction;
                    for (int bAngle = 1; bAngle < 180; bAngle += 20)
                    {
                        lineB.direction = lineA.direction.RotateCW(bAngle);
                        AreEqual_ClosestPoints(lineA, lineB, lineA.origin);
                        AreEqual_ClosestPoints(lineB, lineA, lineA.origin);
                    }
                }
            }
        }

        private void AreEqual_ClosestPoints(Line2 lineA, Line2 lineB, Vector2 expected)
        {
            AreEqual_ClosestPoints(lineA, lineB, expected, expected);
        }

        private void AreEqual_ClosestPoints(Line2 lineA, Line2 lineB, Vector2 expectedA, Vector2 expectedB)
        {
            string message = string.Format(linesFormat, lineA, lineB);
            Closest.LineLine(lineA, lineB, out Vector2 pointA, out Vector2 pointB);
            AreEqual(pointA, expectedA, message);
            AreEqual(pointB, expectedB, message);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_Coincident()
        {
            var line = new Line2();
            foreach (var origin in originPoints2)
            {
                line.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    line.direction = direction;
                    Intersect_Line(line, line);
                }
            }
        }

        [Test]
        public void Intersect_Collinear()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = lineB.direction = direction;
                    lineB.origin = lineA.GetPoint(50);
                    Intersect_LineSwap(lineA, lineB);
                    lineB.direction = -lineB.direction;
                    Intersect_LineSwap(lineA, lineB);
                }
            }
        }

        [Test]
        public void Intersect_Parallel()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = lineB.direction = direction;
                    Vector2 perpendicular = lineA.direction.RotateCCW90();
                    lineB.origin = lineA.GetPoint(30) + perpendicular;
                    IsFalse_Intersect(lineA, lineB);
                    lineB.direction = -lineB.direction;
                    IsFalse_Intersect(lineA, lineB);
                }
            }
        }

        [Test]
        public void Intersect_Diagonal()
        {
            var lineA = new Line2();
            var lineB = new Line2();
            foreach (var origin in originPoints2)
            {
                lineA.origin = lineB.origin = origin;
                foreach (var direction in directionPoints2)
                {
                    lineA.direction = direction;
                    for (int bAngle = 1; bAngle < 180; bAngle += 20)
                    {
                        lineB.direction = lineA.direction.RotateCW(bAngle);
                        Intersect_Point(lineA, lineB, lineA.origin);
                    }
                }
            }
        }

        private void Intersect_LineSwap(Line2 lineA, Line2 lineB)
        {
            Intersect_Line(lineA, lineB);
            Intersect_Line(lineB, lineA);
        }

        private void Intersect_Line(Line2 lineA, Line2 lineB)
        {
            string message = string.Format(linesFormat, lineA, lineB);
            Assert.IsTrue(Intersect.LineLine(lineA, lineB, out IntersectionLineLine2 intersection), linesFormat, lineA, lineB);
            Assert.AreEqual(intersection.type, IntersectionType.Line, message);
            AreEqual(intersection.point, lineA.origin, message);
        }

        private void Intersect_Point(Line2 lineA, Line2 lineB, Vector2 expected)
        {
            string message = string.Format(linesFormat, lineA, lineB);
            Assert.IsTrue(Intersect.LineLine(lineA, lineB, out IntersectionLineLine2 intersection), linesFormat, lineA, lineB);
            Assert.AreEqual(intersection.type, IntersectionType.Point, message);
            AreEqual(intersection.point, expected, message);
            Assert.IsTrue(Intersect.LineLine(lineB, lineA, out intersection), linesFormat, lineA, lineB);
            Assert.AreEqual(intersection.type, IntersectionType.Point, message);
            AreEqual(intersection.point, expected, message);
        }

        private void IsFalse_Intersect(Line2 lineA, Line2 lineB)
        {
            Assert.IsFalse(Intersect.LineLine(lineA, lineB, out _), linesFormat, lineA, lineB);
            Assert.IsFalse(Intersect.LineLine(lineB, lineA, out _), linesFormat, lineA, lineB);
        }

        #endregion Intersect
    }
}

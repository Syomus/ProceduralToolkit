using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DCircleCircleTest : TestBase
    {
        private const string format = "{0:F8}\n{1:F8}";

        #region Distance

        [Test]
        public void Distance_Coincident()
        {
            var circle = new Circle2();
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    AreEqual_Distance(circle, circle, -circle.radius*2);
                }
            }
        }

        [Test]
        public void Distance_InsideCentered()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            foreach (var center in originPoints2)
            {
                circleA.center = circleB.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;
                    AreEqual_Distance(circleA, circleB, -(circleA.radius + circleB.radius));
                }
            }
        }

        [Test]
        public void Distance_OutsideOnePoint()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            foreach (var center in originPoints2)
            {
                circleA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;

                    for (int angle = 0; angle < 360; angle += 10)
                    {
                        circleB.center = circleA.center + Geometry.PointOnCircle2(circleA.radius + circleB.radius, angle);
                        AreEqual_Distance(circleA, circleB);
                    }
                }
            }
        }

        [Test]
        public void Distance_Separate()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            float distance = 1;
            foreach (var center in originPoints2)
            {
                circleA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;

                    for (int angle = 0; angle < 360; angle += 10)
                    {
                        circleB.center = circleA.center + Geometry.PointOnCircle2(circleA.radius + circleB.radius + distance, angle);
                        AreEqual_Distance(circleA, circleB, distance);
                    }
                }
            }
        }

        private void AreEqual_Distance(Circle2 circleA, Circle2 circleB, float expected = 0)
        {
            AreEqual(Distance.CircleCircle(circleA, circleB), expected);
            AreEqual(Distance.CircleCircle(circleB, circleA), expected);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_Coincident()
        {
            var circle = new Circle2();
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    AreEqual_ClosestPoints(circle, circle, circle.center, circle.center);
                }
            }
        }

        [Test]
        public void ClosestPoints_InsideCentered()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            foreach (var center in originPoints2)
            {
                circleA.center = circleB.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;
                    AreEqual_ClosestPoints(circleA, circleB, circleA.center, circleA.center);
                }
            }
        }

        [Test]
        public void ClosestPoints_OutsideOnePoint()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            foreach (var center in originPoints2)
            {
                circleA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;

                    for (int angle = 0; angle < 360; angle += 10)
                    {
                        circleB.center = circleA.center + Geometry.PointOnCircle2(circleA.radius + circleB.radius, angle);
                        Vector2 expected = circleA.GetPoint(angle);
                        AreEqual_ClosestPoints(circleA, circleB, expected, expected);
                    }
                }
            }
        }

        [Test]
        public void ClosestPoints_Separate()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            float distance = 1;
            foreach (var center in originPoints2)
            {
                circleA.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;

                    for (int angle = 0; angle < 360; angle += 10)
                    {
                        circleB.center = circleA.center + Geometry.PointOnCircle2(circleA.radius + circleB.radius + distance, angle);
                        AreEqual_ClosestPoints(circleA, circleB, circleA.GetPoint(angle), circleB.GetPoint(angle - 180));
                    }
                }
            }
        }

        private void AreEqual_ClosestPoints(Circle2 circleA, Circle2 circleB, Vector2 expectedA, Vector2 expectedB)
        {
            Closest.CircleCircle(circleA, circleB, out Vector2 pointA, out Vector2 pointB);
            AreEqual(pointA, expectedA);
            AreEqual(pointB, expectedB);
            Closest.CircleCircle(circleB, circleA, out pointA, out pointB);
            AreEqual(pointA, expectedB);
            AreEqual(pointB, expectedA);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_Coincident()
        {
            var circle = new Circle2();
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    True_IntersectCircle(circle, circle);
                }
            }
        }

        [Test]
        public void Intersect_InsideCentered()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            foreach (var center in originPoints2)
            {
                circleA.center = circleB.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;

                    True_IntersectNone(circleA, circleB);
                }
            }
        }

        [Test]
        public void Intersect_InsideOffCenter()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            float offset = 1;
            foreach (var center in originPoints2)
            {
                circleA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius + 2;
                    circleB.radius = radius;

                    for (int angle = 0; angle < 360; angle += 10)
                    {
                        circleB.center = circleA.center + Geometry.PointOnCircle2(offset, angle);
                        True_IntersectNone(circleA, circleB);
                    }
                }
            }
        }

        [Test]
        public void Intersect_InsideOnePoint()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            float distance = 1;
            foreach (var center in originPoints2)
            {
                circleA.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    circleA.radius = radius + distance;
                    circleB.radius = radius;

                    for (int angle = 0; angle < 360; angle += 10)
                    {
                        circleB.center = circleA.center + Geometry.PointOnCircle2(distance, angle);
                        True_Intersect(circleA, circleB, circleA.GetPoint(angle));
                    }
                }
            }
        }

        [Test]
        public void Intersect_InsideTwoPoints()
        {
            var circleA = new Circle2(Vector2.zero, 1.5f);
            var circleB = new Circle2(Vector2.right, 1);
            True_Intersect(circleA, circleB, new Vector2(1.125f, -0.9921567f), new Vector2(1.125f, 0.9921567f));
        }

        [Test]
        public void Intersect_OutsideOnePoint()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            foreach (var center in originPoints2)
            {
                circleA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;

                    for (int angle = 0; angle < 360; angle += 10)
                    {
                        circleB.center = circleA.center + Geometry.PointOnCircle2(circleA.radius + circleB.radius, angle);
                        True_Intersect(circleA, circleB, circleA.GetPoint(angle));
                    }
                }
            }
        }

        [Test]
        public void Intersect_OutsideTwoPoints()
        {
            var circleA = new Circle2(Vector2.zero, 5);
            var circleB = new Circle2(Vector2.right*8, 5);
            True_Intersect(circleA, circleB, new Vector2(4, -3), new Vector2(4, 3));
        }

        [Test]
        public void Intersect_Separate()
        {
            var circleA = new Circle2();
            var circleB = new Circle2();
            float distance = 1;
            foreach (var center in originPoints2)
            {
                circleA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circleA.radius = radius;
                    circleB.radius = radius + 1;

                    for (int angle = 0; angle < 360; angle += 10)
                    {
                        circleB.center = circleA.center + Geometry.PointOnCircle2(circleA.radius + circleB.radius + distance, angle);
                        False_Intersect(circleA, circleB);
                    }
                }
            }
        }

        private void True_IntersectNone(Circle2 circleA, Circle2 circleB)
        {
            Assert.True(Intersect.CircleCircle(circleA, circleB), format, circleA, circleB);
            Assert.True(Intersect.CircleCircle(circleA, circleB, out IntersectionCircleCircle intersection), format, circleA, circleB);
            Assert.AreEqual(IntersectionType.None, intersection.type, format, circleA, circleB);
            Assert.True(Intersect.CircleCircle(circleB, circleA), format, circleA, circleB);
            Assert.True(Intersect.CircleCircle(circleB, circleA, out intersection), format, circleA, circleB);
            Assert.AreEqual(IntersectionType.None, intersection.type, format, circleA, circleB);
        }

        private void True_IntersectCircle(Circle2 circleA, Circle2 circleB)
        {
            Assert.True(Intersect.CircleCircle(circleA, circleB), format, circleA, circleB);
            Assert.True(Intersect.CircleCircle(circleA, circleB, out IntersectionCircleCircle intersection), format, circleA, circleB);
            Assert.AreEqual(IntersectionType.Circle, intersection.type, format, circleA, circleB);
            Assert.True(Intersect.CircleCircle(circleB, circleA), format, circleA, circleB);
            Assert.True(Intersect.CircleCircle(circleB, circleA, out intersection), format, circleA, circleB);
            Assert.AreEqual(IntersectionType.Circle, intersection.type, format, circleA, circleB);
        }

        private void True_Intersect(Circle2 circleA, Circle2 circleB, Vector2 expected)
        {
            Assert.True(Intersect.CircleCircle(circleA, circleB), format, circleA, circleB);
            Assert.True(Intersect.CircleCircle(circleA, circleB, out IntersectionCircleCircle intersection), format, circleA, circleB);
            Assert.AreEqual(IntersectionType.Point, intersection.type, format, circleA, circleB);
            AreEqual(intersection.pointA, expected);
            Assert.True(Intersect.CircleCircle(circleB, circleA), format, circleA, circleB);
            Assert.True(Intersect.CircleCircle(circleB, circleA, out intersection), format, circleA, circleB);
            Assert.AreEqual(IntersectionType.Point, intersection.type, format, circleA, circleB);
            AreEqual(intersection.pointA, expected);
        }

        private void True_Intersect(Circle2 circleA, Circle2 circleB, Vector2 expectedA, Vector2 expectedB)
        {
            Assert.True(Intersect.CircleCircle(circleA, circleB));
            Assert.True(Intersect.CircleCircle(circleA, circleB, out IntersectionCircleCircle intersection));
            Assert.AreEqual(IntersectionType.TwoPoints, intersection.type);
            AreEqual(intersection.pointA, expectedA);
            AreEqual(intersection.pointB, expectedB);
            Assert.True(Intersect.CircleCircle(circleB, circleA));
            Assert.True(Intersect.CircleCircle(circleB, circleA, out intersection));
            Assert.AreEqual(IntersectionType.TwoPoints, intersection.type);
            AreEqual(intersection.pointA, expectedB);
            AreEqual(intersection.pointB, expectedA);
        }

        private void False_Intersect(Circle2 circleA, Circle2 circleB)
        {
            Assert.False(Intersect.CircleCircle(circleA, circleB));
            Assert.False(Intersect.CircleCircle(circleA, circleB, out _));
            Assert.False(Intersect.CircleCircle(circleB, circleA));
            Assert.False(Intersect.CircleCircle(circleB, circleA, out _));
        }

        #endregion Intersect
    }
}

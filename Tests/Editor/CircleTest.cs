using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests
{
    public class CircleTest : TestBase
    {
        [Test]
        public void Constructor_Empty()
        {
            var circle = new Circle2();
            Assert.AreEqual(default(Vector2), circle.center);
            Assert.AreEqual(default(float), circle.radius);
        }

        [Test]
        public void Constructor_OneArgument()
        {
            for (int radius = 1; radius < 102; radius += 10)
            {
                var circle = new Circle2(radius);
                Assert.AreEqual(Vector2.zero, circle.center);
                Assert.AreEqual(radius, circle.radius);
            }
        }

        [Test]
        public void Constructor_TwoArguments()
        {
            for (int centerAngle = 0; centerAngle < 360; centerAngle += 10)
            {
                Vector2 center = Geometry.PointOnCircle2(100, centerAngle);
                for (int radius = 1; radius < 102; radius += 10)
                {
                    var circle = new Circle2(center, radius);
                    Assert.AreEqual(center, circle.center);
                    Assert.AreEqual(radius, circle.radius);
                }
            }
        }

        [Test]
        public void Equals()
        {
            for (int centerAngle = 0; centerAngle < 360; centerAngle += 10)
            {
                Vector2 center = Geometry.PointOnCircle2(100, centerAngle);
                for (int radius = 1; radius < 102; radius += 10)
                {
                    var circle = new Circle2(center, radius);
                    Assert.AreEqual(circle, new Circle2(center, radius));
                    Assert.AreNotEqual(circle, new Circle2(-center, radius));
                }
            }
        }

        [Test]
        public void GetPoint()
        {
            for (int centerAngle = 0; centerAngle < 360; centerAngle += 10)
            {
                Vector2 center = Geometry.PointOnCircle2(100, centerAngle);
                for (int radius = 1; radius < 32; radius += 10)
                {
                    var circle = new Circle2(center, radius);
                    Vector2 point0 = center + Vector2.up*radius;
                    AreEqual(circle.GetPoint(0), point0);
                    AreEqual(circle.GetPoint(360), point0);
                    AreEqual(circle.GetPoint(-360), point0);

                    Vector2 point90 = center + Vector2.right*radius;
                    AreEqual(circle.GetPoint(90), point90);
                    AreEqual(circle.GetPoint(450), point90);
                    AreEqual(circle.GetPoint(-270), point90);

                    Vector2 point180 = center + Vector2.down*radius;
                    AreEqual(circle.GetPoint(180), point180);
                    AreEqual(circle.GetPoint(540), point180);
                    AreEqual(circle.GetPoint(-180), point180);

                    Vector2 point270 = center + Vector2.left*radius;
                    AreEqual(circle.GetPoint(270), point270);
                    AreEqual(circle.GetPoint(630), point270);
                    AreEqual(circle.GetPoint(-90), point270);
                }
            }
        }

        [Test]
        public void Lerp()
        {
            for (int centerAngle = 0; centerAngle < 360; centerAngle += 10)
            {
                Vector2 center = Geometry.PointOnCircle2(100, centerAngle);
                for (int radius = 1; radius < 102; radius += 10)
                {
                    var circleA = new Circle2(center, radius);
                    var circleB = new Circle2(-center, -radius);

                    Assert.AreEqual(circleA, Circle2.Lerp(circleA, circleB, -1));
                    Assert.AreEqual(circleA, Circle2.Lerp(circleA, circleB, 0));
                    Assert.AreEqual(new Circle2(), Circle2.Lerp(circleA, circleB, 0.5f));
                    Assert.AreEqual(circleB, Circle2.Lerp(circleA, circleB, 1));
                    Assert.AreEqual(circleB, Circle2.Lerp(circleA, circleB, 2));
                }
            }
        }

        [Test]
        public void LerpUnclamped()
        {
            for (int centerAngle = 0; centerAngle < 360; centerAngle += 10)
            {
                Vector2 center = Geometry.PointOnCircle2(100, centerAngle);
                for (int radius = 1; radius < 102; radius += 10)
                {
                    var circleA = new Circle2(center, radius);
                    var circleB = new Circle2(-center, -radius);

                    Assert.AreEqual(new Circle2(center*3, radius*3), Circle2.LerpUnclamped(circleA, circleB, -1));
                    Assert.AreEqual(circleA, Circle2.LerpUnclamped(circleA, circleB, 0));
                    Assert.AreEqual(new Circle2(), Circle2.LerpUnclamped(circleA, circleB, 0.5f));
                    Assert.AreEqual(circleB, Circle2.LerpUnclamped(circleA, circleB, 1));
                    Assert.AreEqual(new Circle2(-center*3, -radius*3), Circle2.LerpUnclamped(circleA, circleB, 2));
                }
            }
        }
    }
}

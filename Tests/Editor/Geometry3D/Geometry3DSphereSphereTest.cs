using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry3D
{
    public class Geometry3DSphereSphereTest : TestBase
    {
        private const string format = "{0:F8}\n{1:F8}";

        #region Distance

        [Test]
        public void Distance_Coincident()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    AreEqual_Distance(sphere, sphere, -sphere.radius*2);
                }
            }
        }

        [Test]
        public void Distance_InsideCentered()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            foreach (var center in originPoints3)
            {
                sphereA.center = sphereB.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;
                    AreEqual_Distance(sphereA, sphereB, -(sphereA.radius + sphereB.radius));
                }
            }
        }

        [Test]
        public void Distance_OutsideOnePoint()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            foreach (var center in originPoints3)
            {
                sphereA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;

                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            sphereB.center = sphereA.center + Geometry.PointOnSphere(sphereA.radius + sphereB.radius,
                                                 horizontalAngle, verticalAngle);
                            AreEqual_Distance(sphereA, sphereB);
                        }
                    }
                }
            }
        }

        [Test]
        public void Distance_Separate()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            float distance = 1;
            foreach (var center in originPoints3)
            {
                sphereA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;

                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            sphereB.center = sphereA.center + Geometry.PointOnSphere(sphereA.radius + sphereB.radius + distance,
                                                 horizontalAngle, verticalAngle);
                            AreEqual_Distance(sphereA, sphereB, distance);
                        }
                    }
                }
            }
        }

        private void AreEqual_Distance(Sphere sphereA, Sphere sphereB, float expected = 0)
        {
            AreEqual(Distance.SphereSphere(sphereA, sphereB), expected);
            AreEqual(Distance.SphereSphere(sphereB, sphereA), expected);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_Coincident()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    AreEqual_ClosestPoints(sphere, sphere, sphere.center, sphere.center);
                }
            }
        }

        [Test]
        public void ClosestPoints_InsideCentered()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            foreach (var center in originPoints3)
            {
                sphereA.center = sphereB.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;
                    AreEqual_ClosestPoints(sphereA, sphereB, sphereA.center, sphereA.center);
                }
            }
        }

        [Test]
        public void ClosestPoints_OutsideOnePoint()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            foreach (var center in originPoints3)
            {
                sphereA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;

                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            sphereB.center = sphereA.center + Geometry.PointOnSphere(sphereA.radius + sphereB.radius,
                                                 horizontalAngle, verticalAngle);
                            Vector3 expected = sphereA.GetPoint(horizontalAngle, verticalAngle);
                            AreEqual_ClosestPoints(sphereA, sphereB, expected, expected);
                        }
                    }
                }
            }
        }

        [Test]
        public void ClosestPoints_Separate()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            float distance = 1;
            foreach (var center in originPoints3)
            {
                sphereA.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;

                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            sphereB.center = sphereA.center + Geometry.PointOnSphere(sphereA.radius + sphereB.radius + distance,
                                                 horizontalAngle, verticalAngle);
                            Vector3 expectedA = sphereA.GetPoint(horizontalAngle, verticalAngle);
                            Vector3 expectedB = sphereB.GetPoint(horizontalAngle - 180, -verticalAngle);
                            AreEqual_ClosestPoints(sphereA, sphereB, expectedA, expectedB);
                        }
                    }
                }
            }
        }

        private void AreEqual_ClosestPoints(Sphere sphereA, Sphere sphereB, Vector3 expectedA, Vector3 expectedB)
        {
            Closest.SphereSphere(sphereA, sphereB, out Vector3 pointA, out Vector3 pointB);
            AreEqual(pointA, expectedA);
            AreEqual(pointB, expectedB);
            Closest.SphereSphere(sphereB, sphereA, out pointA, out pointB);
            AreEqual(pointA, expectedB);
            AreEqual(pointB, expectedA);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_Coincident()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    True_IntersectSphere(sphere, sphere);
                }
            }
        }

        [Test]
        public void Intersect_InsideCentered()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            foreach (var center in originPoints3)
            {
                sphereA.center = sphereB.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;

                    True_IntersectNone(sphereA, sphereB);
                }
            }
        }

        [Test]
        public void Intersect_InsideOffCenter()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            float offset = 1;
            foreach (var center in originPoints3)
            {
                sphereA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius + 2;
                    sphereB.radius = radius;

                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            sphereB.center = sphereA.center + Geometry.PointOnSphere(offset, horizontalAngle, verticalAngle);
                            True_IntersectNone(sphereA, sphereB);
                        }
                    }
                }
            }
        }

        [Test]
        public void Intersect_InsideOnePoint()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            float distance = 1;
            foreach (var center in originPoints3)
            {
                sphereA.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphereA.radius = radius + distance;
                    sphereB.radius = radius;

                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            sphereB.center = sphereA.center + Geometry.PointOnSphere(distance, horizontalAngle, verticalAngle);
                            True_Intersect(sphereA, sphereB, sphereA.GetPoint(horizontalAngle, verticalAngle));
                        }
                    }
                }
            }
        }

        [Test]
        public void Intersect_InsideCircle()
        {
            var sphereA = new Sphere(Vector3.zero, PTUtils.Sqrt2);
            var sphereB = new Sphere(Vector3.right, 1);
            True_Intersect(sphereA, sphereB, new Circle3(Vector3.right, Vector3.right, 1));
        }

        [Test]
        public void Intersect_OutsideOnePoint()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            foreach (var center in originPoints3)
            {
                sphereA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;

                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            sphereB.center = sphereA.center + Geometry.PointOnSphere(sphereA.radius + sphereB.radius, horizontalAngle, verticalAngle);
                            True_Intersect(sphereA, sphereB, sphereA.GetPoint(horizontalAngle, verticalAngle));
                        }
                    }
                }
            }
        }

        [Test]
        public void Intersect_OutsideCircle()
        {
            var sphereA = new Sphere(Vector3.zero, 5);
            var sphereB = new Sphere(Vector3.right*8, 5);
            True_Intersect(sphereA, sphereB, new Circle3(Vector3.right*4, Vector3.right, 3));
        }

        [Test]
        public void Intersect_Separate()
        {
            var sphereA = new Sphere();
            var sphereB = new Sphere();
            float distance = 1;
            foreach (var center in originPoints3)
            {
                sphereA.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphereA.radius = radius;
                    sphereB.radius = radius + 1;

                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            sphereB.center = sphereA.center + Geometry.PointOnSphere(sphereA.radius + sphereB.radius + distance,
                                                 horizontalAngle, verticalAngle);
                            False_Intersect(sphereA, sphereB);
                        }
                    }
                }
            }
        }

        private void True_IntersectNone(Sphere sphereA, Sphere sphereB)
        {
            Assert.True(Intersect.SphereSphere(sphereA, sphereB), format, sphereA, sphereB);
            Assert.True(Intersect.SphereSphere(sphereA, sphereB, out IntersectionSphereSphere intersection), format, sphereA, sphereB);
            Assert.AreEqual(IntersectionType.None, intersection.type, format, sphereA, sphereB);
            Assert.True(Intersect.SphereSphere(sphereB, sphereA), format, sphereA, sphereB);
            Assert.True(Intersect.SphereSphere(sphereB, sphereA, out intersection), format, sphereA, sphereB);
            Assert.AreEqual(IntersectionType.None, intersection.type, format, sphereA, sphereB);
        }

        private void True_IntersectSphere(Sphere sphereA, Sphere sphereB)
        {
            Assert.True(Intersect.SphereSphere(sphereA, sphereB), format, sphereA, sphereB);
            Assert.True(Intersect.SphereSphere(sphereA, sphereB, out IntersectionSphereSphere intersection), format, sphereA, sphereB);
            Assert.AreEqual(IntersectionType.Sphere, intersection.type, format, sphereA, sphereB);
            Assert.True(Intersect.SphereSphere(sphereB, sphereA), format, sphereA, sphereB);
            Assert.True(Intersect.SphereSphere(sphereB, sphereA, out intersection), format, sphereA, sphereB);
            Assert.AreEqual(IntersectionType.Sphere, intersection.type, format, sphereA, sphereB);
        }

        private void True_Intersect(Sphere sphereA, Sphere sphereB, Vector3 expected)
        {
            Assert.True(Intersect.SphereSphere(sphereA, sphereB), format, sphereA, sphereB);
            Assert.True(Intersect.SphereSphere(sphereA, sphereB, out IntersectionSphereSphere intersection), format, sphereA, sphereB);
            Assert.AreEqual(IntersectionType.Point, intersection.type, format, sphereA, sphereB);
            AreEqual(intersection.point, expected);
            Assert.True(Intersect.SphereSphere(sphereB, sphereA), format, sphereA, sphereB);
            Assert.True(Intersect.SphereSphere(sphereB, sphereA, out intersection), format, sphereA, sphereB);
            Assert.AreEqual(IntersectionType.Point, intersection.type, format, sphereA, sphereB);
            AreEqual(intersection.point, expected);
        }

        private void True_Intersect(Sphere sphereA, Sphere sphereB, Circle3 expected)
        {
            Assert.True(Intersect.SphereSphere(sphereA, sphereB));
            Assert.True(Intersect.SphereSphere(sphereA, sphereB, out IntersectionSphereSphere intersection));
            Assert.AreEqual(IntersectionType.Circle, intersection.type);
            AreEqual(intersection.point, expected.center);
            AreEqual(intersection.normal, expected.normal);
            AreEqual(intersection.radius, expected.radius);
            Assert.True(Intersect.SphereSphere(sphereB, sphereA));
            Assert.True(Intersect.SphereSphere(sphereB, sphereA, out intersection));
            Assert.AreEqual(IntersectionType.Circle, intersection.type);
            AreEqual(intersection.point, expected.center);
            AreEqual(intersection.normal, -expected.normal);
            AreEqual(intersection.radius, expected.radius);
        }

        private void False_Intersect(Sphere sphereA, Sphere sphereB)
        {
            Assert.False(Intersect.SphereSphere(sphereA, sphereB));
            Assert.False(Intersect.SphereSphere(sphereA, sphereB, out _));
            Assert.False(Intersect.SphereSphere(sphereB, sphereA));
            Assert.False(Intersect.SphereSphere(sphereB, sphereA, out _));
        }

        #endregion Intersect
    }
}

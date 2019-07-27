using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry3D
{
    public class Geometry3DPointSphereTest : TestBase
    {
        private const string format = "{0:F8}\n{1}";

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
                    AreEqual_Distance(sphere, sphere.center, -sphere.radius);
                }
            }
        }

        [Test]
        public void Distance_OnSphere()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            AreEqual_Distance(sphere, sphere.GetPoint(horizontalAngle, verticalAngle));
                        }
                    }
                }
            }
        }

        [Test]
        public void Distance_Separate()
        {
            float distance = 1;
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            Vector3 point = sphere.center + Geometry.PointOnSphere(sphere.radius + distance, horizontalAngle, verticalAngle);
                            AreEqual_Distance(sphere, point, distance);
                        }
                    }
                }
            }
        }

        private void AreEqual_Distance(Sphere sphere, Vector3 point, float expected = 0)
        {
            AreEqual(Distance.PointSphere(point, sphere), expected);
        }

        #endregion Distance

        #region ClosestPoint

        [Test]
        public void ClosestPoint_Coincident()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    AreEqual_ClosestPoint(sphere, sphere.center, sphere.center);
                }
            }
        }

        [Test]
        public void ClosestPoint_OnSphere()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            Vector3 point = sphere.GetPoint(horizontalAngle, verticalAngle);
                            AreEqual_ClosestPoint(sphere, point, point);
                        }
                    }
                }
            }
        }

        [Test]
        public void ClosestPoint_Separate()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            Vector3 point = sphere.center + Geometry.PointOnSphere(sphere.radius + 1, horizontalAngle, verticalAngle);
                            Vector3 expected = sphere.GetPoint(horizontalAngle, verticalAngle);
                            AreEqual_ClosestPoint(sphere, point, expected);
                        }
                    }
                }
            }
        }

        private void AreEqual_ClosestPoint(Sphere sphere, Vector3 point, Vector3 expected)
        {
            AreEqual(Closest.PointSphere(point, sphere), expected);
        }

        #endregion ClosestPoint

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
                    True_Intersect(sphere, sphere.center);
                }
            }
        }

        [Test]
        public void Intersect_OffCenter()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            Vector3 point = sphere.center + Geometry.PointOnSphere(sphere.radius - 1, horizontalAngle, verticalAngle);
                            True_Intersect(sphere, point);
                        }
                    }
                }
            }
        }

        [Test]
        public void Intersect_OnSphere()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 11; radius < 42; radius += 10)
                {
                    sphere.radius = radius;
                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            True_Intersect(sphere, sphere.GetPoint(horizontalAngle, verticalAngle));
                        }
                    }
                }
            }
        }

        [Test]
        public void Intersect_Separate()
        {
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    sphere.radius = radius;
                    for (int verticalAngle = -90; verticalAngle < 90; verticalAngle += 15)
                    {
                        for (int horizontalAngle = 0; horizontalAngle < 360; horizontalAngle += 15)
                        {
                            Vector3 point = sphere.center + Geometry.PointOnSphere(sphere.radius + 1, horizontalAngle, verticalAngle);
                            False_Intersect(sphere, point);
                        }
                    }
                }
            }
        }

        private void True_Intersect(Sphere sphere, Vector3 point)
        {
            Assert.True(Intersect.PointSphere(point, sphere), format, sphere, point.ToString("F8"));
        }

        private void False_Intersect(Sphere sphere, Vector3 point)
        {
            Assert.False(Intersect.PointSphere(point, sphere), format, sphere, point.ToString("F8"));
        }

        #endregion Intersect
    }
}

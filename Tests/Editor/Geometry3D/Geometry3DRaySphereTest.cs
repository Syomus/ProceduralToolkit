using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry3D
{
    public class Geometry3DRaySphereTest : TestBase
    {
        private const string format = "{0}\n{1:F8}";

        #region Distance

        [Test]
        public void Distance_TwoPoints()
        {
            var ray = new Ray();
            var sphere = new Sphere();
            float offset = 1;
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        ray.direction = direction;
                        ray.origin = sphere.center - direction*sphere.radius;
                        AreEqual_Distance(ray, sphere);
                        ray.origin = sphere.center - direction*(sphere.radius + offset);
                        AreEqual_Distance(ray, sphere);
                    }
                }
            }
        }

        [Test]
        public void Distance_OnePoint()
        {
            var ray = new Ray();
            var sphere = new Sphere(Vector3.zero, 2);
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                foreach (var direction in directionPoints3)
                {
                    Vector3 origin = sphere.center + direction*sphere.radius;
                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 45)
                    {
                        ray.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        ray.origin = origin;
                        AreEqual_Distance(ray, sphere);
                        ray.origin = origin - ray.direction;
                        AreEqual_Distance(ray, sphere);
                    }

                    ray.direction = direction;
                    ray.origin = sphere.center;
                    AreEqual_Distance(ray, sphere);

                    ray.origin = sphere.center + direction*0.5f;
                    AreEqual_Distance(ray, sphere);
                }
            }
        }

        [Test]
        public void Distance_Separate()
        {
            var ray = new Ray();
            var sphere = new Sphere();
            float distance = 1;
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        Vector3 origin = sphere.center + direction*(sphere.radius + distance);
                        Vector3 tangent = GetTangent(direction);
                        for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 45)
                        {
                            ray.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                            ray.origin = origin;
                            AreEqual_Distance(ray, sphere, distance);
                            ray.origin = origin - ray.direction;
                            AreEqual_Distance(ray, sphere, distance);
                        }
                    }
                }
            }
        }

        private void AreEqual_Distance(Ray ray, Sphere sphere, float expected = 0)
        {
            AreEqual(Distance.RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius), expected);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_TwoPoints()
        {
            var ray = new Ray();
            var sphere = new Sphere();
            float offset = 1;
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        ray.direction = direction;
                        Vector3 point = sphere.center - direction*sphere.radius;
                        ray.origin = point;
                        AreEqual_ClosestPoints(ray, sphere, point, point);
                        ray.origin = sphere.center - direction*(sphere.radius + offset);
                        AreEqual_ClosestPoints(ray, sphere, point, point);
                    }
                }
            }
        }

        [Test]
        public void ClosestPoints_OnePoint()
        {
            var ray = new Ray();
            var sphere = new Sphere(Vector3.zero, 2);
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                foreach (var direction in directionPoints3)
                {
                    Vector3 origin = sphere.center + direction*sphere.radius;
                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 45)
                    {
                        ray.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        ray.origin = origin;
                        AreEqual_ClosestPoints(ray, sphere, origin, origin);
                        ray.origin = origin - ray.direction;
                        AreEqual_ClosestPoints(ray, sphere, origin, origin);
                    }

                    ray.direction = direction;
                    ray.origin = sphere.center;
                    Vector3 point = ray.GetPoint(sphere.radius);
                    AreEqual_ClosestPoints(ray, sphere, point, point);

                    ray.origin = sphere.center + direction*0.5f;
                    AreEqual_ClosestPoints(ray, sphere, point, point);
                }
            }
        }

        [Test]
        public void ClosestPoints_Separate()
        {
            var ray = new Ray();
            var sphere = new Sphere();
            float distance = 1;
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        Vector3 origin = sphere.center + direction*(sphere.radius + distance);
                        Vector3 spherePoint = sphere.center + direction*sphere.radius;
                        Vector3 tangent = GetTangent(direction);
                        for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 45)
                        {
                            ray.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                            ray.origin = origin;
                            AreEqual_ClosestPoints(ray, sphere, origin, spherePoint);
                            ray.origin = origin - ray.direction;
                            AreEqual_ClosestPoints(ray, sphere, origin, spherePoint);
                        }
                    }
                }
            }
        }

        private void AreEqual_ClosestPoints(Ray ray, Sphere sphere, Vector3 expectedRay, Vector3 expectedSphere)
        {
            Closest.RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out Vector3 rayPoint, out Vector3 centerPoint);
            AreEqual(rayPoint, expectedRay);
            AreEqual(centerPoint, expectedSphere);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_TwoPoints()
        {
            var ray = new Ray();
            var sphere = new Sphere();
            float offset = 1;
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        ray.direction = direction;
                        Vector3 point1 = sphere.center - direction*sphere.radius;
                        Vector3 point2 = sphere.center + direction*sphere.radius;
                        ray.origin = point1;
                        True_IntersectTwoPoints(ray, sphere, point1, point2);
                        ray.origin = sphere.center - direction*(sphere.radius + offset);
                        True_IntersectTwoPoints(ray, sphere, point1, point2);
                    }
                }
            }
        }

        [Test]
        public void Intersect_OnePoint()
        {
            var ray = new Ray();
            var sphere = new Sphere(Vector3.zero, 2);
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                foreach (var direction in directionPoints3)
                {
                    Vector3 origin = sphere.center + direction*sphere.radius;
                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 45)
                    {
                        ray.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        ray.origin = origin;
                        True_IntersectPoint(ray, sphere, origin);
                        ray.origin = origin - ray.direction;
                        True_IntersectPoint(ray, sphere, origin);
                    }

                    ray.direction = direction;
                    ray.origin = sphere.center;
                    Vector3 point = ray.GetPoint(sphere.radius);
                    True_IntersectPoint(ray, sphere, point);

                    ray.origin = sphere.center + direction*0.5f;
                    True_IntersectPoint(ray, sphere, point);
                }
            }
        }

        [Test]
        public void Intersect_Separate()
        {
            var ray = new Ray();
            var sphere = new Sphere();
            float distance = 1;
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        Vector3 origin = sphere.center + direction*(sphere.radius + distance);
                        Vector3 tangent = GetTangent(direction);
                        for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 45)
                        {
                            ray.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                            ray.origin = origin;
                            False_Intersect(ray, sphere);
                            ray.origin = origin - ray.direction;
                            False_Intersect(ray, sphere);
                        }
                    }
                }
            }
        }

        private void True_IntersectPoint(Ray ray, Sphere sphere, Vector3 expected)
        {
            Assert.True(Intersect.RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out IntersectionRaySphere intersection), format, ray, sphere);
            Assert.AreEqual(IntersectionType.Point, intersection.type, format, ray, sphere);
            AreEqual(intersection.pointA, expected);
        }

        private void True_IntersectTwoPoints(Ray ray, Sphere sphere, Vector3 expectedA, Vector3 expectedB)
        {
            Assert.True(Intersect.RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out IntersectionRaySphere intersection), format, ray, sphere);
            Assert.AreEqual(IntersectionType.TwoPoints, intersection.type, format, ray, sphere);
            AreEqual(intersection.pointA, expectedA);
            AreEqual(intersection.pointB, expectedB);
        }

        private void False_Intersect(Ray ray, Sphere sphere)
        {
            Assert.False(Intersect.RaySphere(ray.origin, ray.direction, sphere.center, sphere.radius, out IntersectionRaySphere intersection), format, ray, sphere);
        }

        #endregion Intersect
    }
}

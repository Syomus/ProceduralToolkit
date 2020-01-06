using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry3D
{
    public class Geometry3DLineSphereTest : TestBase
    {
        private const string format = "{0:F8}\n{1:F8}";

        #region Distance

        [Test]
        public void Distance_TwoPoints()
        {
            var line = new Line3();
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        line.direction = direction;
                        line.origin = center;
                        AreEqual_Distance(line, sphere);
                        line.origin = center - line.direction;
                        AreEqual_Distance(line, sphere);
                        line.origin = center + line.direction;
                        AreEqual_Distance(line, sphere);
                    }
                }
            }
        }

        [Test]
        public void Distance_OnePoint()
        {
            var line = new Line3();
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
                        line.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        line.origin = origin;
                        AreEqual_Distance(line, sphere);
                        line.origin = origin - line.direction;
                        AreEqual_Distance(line, sphere);
                        line.origin = origin + line.direction;
                        AreEqual_Distance(line, sphere);
                    }
                }
            }
        }

        [Test]
        public void Distance_Separate()
        {
            var line = new Line3();
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
                            line.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                            line.origin = origin;
                            AreEqual_Distance(line, sphere, distance);
                            line.origin = origin - line.direction;
                            AreEqual_Distance(line, sphere, distance);
                            line.origin = origin + line.direction;
                            AreEqual_Distance(line, sphere, distance);
                        }
                    }
                }
            }
        }

        private void AreEqual_Distance(Line3 line, Sphere sphere, float expected = 0)
        {
            AreEqual(Distance.LineSphere(line.origin, line.direction, sphere.center, sphere.radius), expected);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_TwoPoints()
        {
            var line = new Line3();
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        line.direction = direction;
                        line.origin = center;
                        Vector3 point = line.GetPoint(-radius);
                        AreEqual_ClosestPoints(line, sphere, point, point);
                        line.origin = center - line.direction;
                        AreEqual_ClosestPoints(line, sphere, point, point);
                        line.origin = center + line.direction;
                        AreEqual_ClosestPoints(line, sphere, point, point);
                    }
                }
            }
        }

        [Test]
        public void ClosestPoints_OnePoint()
        {
            var line = new Line3();
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
                        line.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        line.origin = origin;
                        AreEqual_ClosestPoints(line, sphere, origin, origin);
                        line.origin = origin - line.direction;
                        AreEqual_ClosestPoints(line, sphere, origin, origin);
                        line.origin = origin + line.direction;
                        AreEqual_ClosestPoints(line, sphere, origin, origin);
                    }
                }
            }
        }

        [Test]
        public void ClosestPoints_Separate()
        {
            var line = new Line3();
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
                            line.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                            line.origin = origin;
                            AreEqual_ClosestPoints(line, sphere, origin, spherePoint);
                            line.origin = origin - line.direction;
                            AreEqual_ClosestPoints(line, sphere, origin, spherePoint);
                            line.origin = origin + line.direction;
                            AreEqual_ClosestPoints(line, sphere, origin, spherePoint);
                        }
                    }
                }
            }
        }

        private void AreEqual_ClosestPoints(Line3 line, Sphere sphere, Vector3 expectedLine, Vector3 expectedSphere)
        {
            Closest.LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out Vector3 linePoint, out Vector3 centerPoint);
            AreEqual(linePoint, expectedLine);
            AreEqual(centerPoint, expectedSphere);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_TwoPoints()
        {
            var line = new Line3();
            var sphere = new Sphere();
            foreach (var center in originPoints3)
            {
                sphere.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    sphere.radius = radius;
                    foreach (var direction in directionPoints3)
                    {
                        line.direction = direction;
                        line.origin = center;
                        Vector3 point1 = line.GetPoint(-sphere.radius);
                        Vector3 point2 = line.GetPoint(sphere.radius);
                        True_Intersect(line, sphere, point1, point2);
                        line.origin = center - line.direction;
                        True_Intersect(line, sphere, point1, point2);
                        line.origin = center + line.direction;
                        True_Intersect(line, sphere, point1, point2);
                    }
                }
            }
        }

        [Test]
        public void Intersect_OnePoint()
        {
            var line = new Line3();
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
                        line.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        line.origin = origin;
                        True_Intersect(line, sphere, origin);
                        line.origin = origin - line.direction;
                        True_Intersect(line, sphere, origin);
                        line.origin = origin + line.direction;
                        True_Intersect(line, sphere, origin);
                    }
                }
            }
        }

        [Test]
        public void Intersect_Separate()
        {
            var line = new Line3();
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
                            line.direction = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                            line.origin = origin;
                            False_Intersect(line, sphere);
                            line.origin = origin - line.direction;
                            False_Intersect(line, sphere);
                            line.origin = origin + line.direction;
                            False_Intersect(line, sphere);
                        }
                    }
                }
            }
        }

        private void True_Intersect(Line3 line, Sphere sphere, Vector3 expected)
        {
            Assert.True(Intersect.LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out IntersectionLineSphere intersection), format, line, sphere);
            Assert.AreEqual(IntersectionType.Point, intersection.type, format, line, sphere);
            AreEqual(intersection.pointA, expected);
        }

        private void True_Intersect(Line3 line, Sphere sphere, Vector3 expectedA, Vector3 expectedB)
        {
            Assert.True(Intersect.LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out IntersectionLineSphere intersection), format, line, sphere);
            Assert.AreEqual(IntersectionType.TwoPoints, intersection.type, format, line, sphere);
            AreEqual(intersection.pointA, expectedA);
            AreEqual(intersection.pointB, expectedB);
        }

        private void False_Intersect(Line3 line, Sphere sphere)
        {
            Assert.False(Intersect.LineSphere(line.origin, line.direction, sphere.center, sphere.radius, out IntersectionLineSphere intersection), format, line, sphere);
        }

        #endregion Intersect
    }
}

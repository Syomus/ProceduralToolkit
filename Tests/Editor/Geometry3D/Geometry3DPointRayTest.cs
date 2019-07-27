using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry3D
{
    public class Geometry3DPointRayTest : TestBase
    {
        private const string format = "{0}\n{1}";
        private const float offset = 100;

        #region Distance

        [Test]
        public void Distance_PointOnLine()
        {
            var ray = new Ray();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    AreEqual_Distance(ray, origin);
                    AreEqual_Distance(ray, origin + direction*offset);
                    AreEqual_Distance(ray, origin - direction*offset, offset);
                }
            }
        }

        [Test]
        public void Distance_PointNotOnLine()
        {
            float expected = Mathf.Sqrt(1 + offset*offset);
            var ray = new Ray();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        AreEqual_Distance(ray, origin + perpendicular, 1);
                        AreEqual_Distance(ray, origin + perpendicular + direction*offset, 1);
                        AreEqual_Distance(ray, origin + perpendicular - direction*offset, expected);
                    }
                }
            }
        }

        private void AreEqual_Distance(Ray ray, Vector3 point, float expected = 0)
        {
            AreEqual(Distance.PointRay(point, ray), expected);
        }

        #endregion Distance

        #region ClosestPoint

        [Test]
        public void ClosestPoint_PointOnLine()
        {
            var ray = new Ray();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    AreEqual_ClosestPoint(ray, origin);
                    AreEqual_ClosestPoint(ray, origin + direction*offset);
                    AreEqual_ClosestPoint(ray, origin - direction*offset, origin);
                }
            }
        }

        [Test]
        public void ClosestPoint_PointNotOnLine()
        {
            var ray = new Ray();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        AreEqual_ClosestPoint(ray, origin + perpendicular, origin);
                        AreEqual_ClosestPoint(ray, origin + perpendicular + direction*offset, ray.GetPoint(offset));
                        AreEqual_ClosestPoint(ray, origin + perpendicular - direction*offset, origin);
                    }
                }
            }
        }

        private void AreEqual_ClosestPoint(Ray ray, Vector3 point)
        {
            AreEqual_ClosestPoint(ray, point, point);
        }

        private void AreEqual_ClosestPoint(Ray ray, Vector3 point, Vector3 expected)
        {
            AreEqual(Closest.PointRay(point, ray), expected);
        }

        #endregion ClosestPoint

        #region Intersect

        [Test]
        public void Intersect_PointOnLine()
        {
            var ray = new Ray();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    True_Intersect(ray, origin);
                    True_Intersect(ray, origin + direction*offset*0.5f);
                    False_Intersect(ray, origin - direction*offset);
                }
            }
        }

        [Test]
        public void Intersect_PointNotOnLine()
        {
            var ray = new Ray();
            foreach (var origin in originPoints3)
            {
                foreach (var direction in directionPoints3)
                {
                    ray.origin = origin;
                    ray.direction = direction;

                    Vector3 tangent = GetTangent(direction);
                    for (int perpendicularAngle = 0; perpendicularAngle < 360; perpendicularAngle += 10)
                    {
                        Vector3 perpendicular = Quaternion.AngleAxis(perpendicularAngle, direction)*tangent;
                        False_Intersect(ray, origin + perpendicular);
                        False_Intersect(ray, origin + perpendicular + direction*offset);
                        False_Intersect(ray, origin + perpendicular - direction*offset);
                    }
                }
            }
        }

        private void False_Intersect(Ray ray, Vector3 point)
        {
            Assert.False(Intersect.PointRay(point, ray), format, ray, point);
        }

        private void True_Intersect(Ray ray, Vector3 point)
        {
            Assert.True(Intersect.PointRay(point, ray), format, ray, point);
        }

        #endregion Intersect
    }
}

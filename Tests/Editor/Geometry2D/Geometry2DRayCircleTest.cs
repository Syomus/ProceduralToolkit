using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.Geometry2D
{
    public class Geometry2DRayCircleTest : TestBase
    {
        private const string format = "{0}\n{1:F8}";

        #region Distance

        [Test]
        public void Distance_TwoPoints()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            float offset = 1;
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    for (int rayAngle = 0; rayAngle < 360; rayAngle += 10)
                    {
                        Vector2 direction = Vector2.down.RotateCW(rayAngle);
                        ray.origin = circle.center - direction*circle.radius;
                        ray.direction = direction;
                        AreEqual_Distance(ray, circle);

                        ray.origin = circle.center - direction*(circle.radius + offset);
                        ray.direction = direction;
                        AreEqual_Distance(ray, circle);

                        Vector2 expectedA = circle.GetPoint(rayAngle + 45);
                        Vector2 expectedB = circle.GetPoint(rayAngle + 135);
                        direction = (expectedB - expectedA).normalized;
                        ray.origin = expectedA - direction*0.1f;
                        ray.direction = direction;
                        AreEqual_Distance(ray, circle);
                    }
                }
            }
        }

        [Test]
        public void Distance_OnePointOnCircle()
        {
            var ray = new Ray2D();
            var circle = new Circle2(Vector2.zero, 5);
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int originAngle = 0; originAngle < 360; originAngle += 10)
                {
                    Vector2 onCircle = circle.GetPoint(originAngle);
                    Vector2 left = Vector2.left.RotateCW(originAngle).normalized;
                    ray.origin = onCircle - left;
                    ray.direction = left;
                    AreEqual_Distance(ray, circle);

                    for (int directionAngle = 0; directionAngle <= 180; directionAngle += 10)
                    {
                        ray.origin = onCircle;
                        ray.direction = left.RotateCW(directionAngle);
                        AreEqual_Distance(ray, circle);
                    }
                }
            }
        }

        [Test]
        public void Distance_OnePointInCircle()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    for (int rayAngle = 0; rayAngle < 360; rayAngle += 10)
                    {
                        Vector2 down = Vector2.down.RotateCW(rayAngle);
                        ray.origin = circle.center;
                        ray.direction = down;
                        AreEqual_Distance(ray, circle);

                        ray.origin = circle.center + down*0.5f;
                        AreEqual_Distance(ray, circle);

                        Vector2 onCircle = circle.GetPoint(rayAngle + 135);
                        float distance = Mathf.Sqrt(2*circle.radius);
                        ray.origin = onCircle - down*distance*0.1f;
                        AreEqual_Distance(ray, circle);

                        ray.origin = onCircle - down*distance*0.9f;
                        AreEqual_Distance(ray, circle);
                    }
                }
            }
        }

        [Test]
        public void Distance_Separate()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            float distance = 1;
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    for (int originAngle = 0; originAngle < 360; originAngle += 10)
                    {
                        Vector2 origin = circle.center + Geometry.PointOnCircle2(circle.radius + distance, originAngle);
                        Vector2 left = Vector2.left.RotateCW(originAngle).normalized;
                        Vector2 right = -left;

                        ray.origin = origin + left;
                        ray.direction = right;
                        AreEqual_Distance(ray, circle, distance);

                        ray.origin = origin + right;
                        ray.direction = left;
                        AreEqual_Distance(ray, circle, distance);

                        for (int directionAngle = 0; directionAngle <= 180; directionAngle += 10)
                        {
                            ray.origin = origin;
                            ray.direction = left.RotateCW(directionAngle);
                            AreEqual_Distance(ray, circle, distance);
                        }
                    }
                }
            }
        }

        private void AreEqual_Distance(Ray2D ray, Circle2 circle, float expected = 0)
        {
            string message = string.Format(format, ray, circle);
            AreEqual(Distance.RayCircle(ray.origin, ray.direction, circle.center, circle.radius), expected, message);
        }

        #endregion Distance

        #region ClosestPoints

        [Test]
        public void ClosestPoints_TwoPoints()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            float offset = 1;
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    for (int rayAngle = 0; rayAngle < 360; rayAngle += 10)
                    {
                        Vector2 direction = Vector2.down.RotateCW(rayAngle);
                        ray.origin = circle.center - direction*circle.radius;
                        ray.direction = direction;
                        ClosestPoints_TwoPoints(ray, circle, ray.origin);

                        ray.origin = circle.center - direction*(circle.radius + offset);
                        ray.direction = direction;
                        ClosestPoints_TwoPoints(ray, circle, ray.GetPoint(offset));

                        Vector2 expectedA = circle.GetPoint(rayAngle + 45);
                        Vector2 expectedB = circle.GetPoint(rayAngle + 135);
                        direction = (expectedB - expectedA).normalized;
                        ray.origin = expectedA - direction*0.1f;
                        ray.direction = direction;
                        ClosestPoints_TwoPoints(ray, circle, expectedA);
                    }
                }
            }
        }

        private void ClosestPoints_TwoPoints(Ray2D ray, Circle2 circle, Vector2 point1)
        {
            AreEqual_ClosestPoints(ray, circle, point1, point1);
        }

        [Test]
        public void ClosestPoints_OnePointOnCircle()
        {
            var ray = new Ray2D();
            var circle = new Circle2(Vector2.zero, 4);
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int originAngle = 0; originAngle < 360; originAngle += 10)
                {
                    Vector2 onCircle = circle.GetPoint(originAngle);
                    Vector2 left = Vector2.left.RotateCW(originAngle).normalized;
                    ray.origin = onCircle - left;
                    ray.direction = left;
                    ClosestPoints_OnePoint(ray, circle, onCircle);

                    for (int directionAngle = 0; directionAngle <= 180; directionAngle += 10)
                    {
                        ray.origin = onCircle;
                        ray.direction = left.RotateCW(directionAngle);
                        ClosestPoints_OnePoint(ray, circle, onCircle);
                    }
                }
            }
        }

        [Test]
        public void ClosestPoints_OnePointInCircle()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    for (int rayAngle = 0; rayAngle < 360; rayAngle += 10)
                    {
                        Vector2 down = Vector2.down.RotateCW(rayAngle);
                        ray.origin = circle.center;
                        ray.direction = down;
                        Vector2 onCircle = ray.GetPoint(circle.radius);
                        ClosestPoints_OnePoint(ray, circle, onCircle);

                        ray.origin = circle.center + down*0.5f;
                        ClosestPoints_OnePoint(ray, circle, onCircle);

                        onCircle = circle.GetPoint(rayAngle + 135);
                        float distance = Mathf.Sqrt(2*circle.radius);
                        ray.origin = onCircle - down*distance*0.1f;
                        ClosestPoints_OnePoint(ray, circle, onCircle);

                        ray.origin = onCircle - down*distance*0.9f;
                        ClosestPoints_OnePoint(ray, circle, onCircle);
                    }
                }
            }
        }

        private void ClosestPoints_OnePoint(Ray2D ray, Circle2 circle, Vector2 expected)
        {
            AreEqual_ClosestPoints(ray, circle, expected, expected);
        }

        [Test]
        public void ClosestPoints_Separate()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            float distance = 1;
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    for (int originAngle = 0; originAngle < 360; originAngle += 10)
                    {
                        Vector2 origin = circle.center + Geometry.PointOnCircle2(circle.radius + distance, originAngle);
                        Vector2 left = Vector2.left.RotateCW(originAngle).normalized;
                        Vector2 right = -left;
                        Vector2 onCircle = circle.GetPoint(originAngle);

                        ray.origin = origin + left;
                        ray.direction = right;
                        AreEqual_ClosestPoints(ray, circle, origin, onCircle);

                        ray.origin = origin + right;
                        ray.direction = left;
                        AreEqual_ClosestPoints(ray, circle, origin, onCircle);

                        for (int directionAngle = 0; directionAngle <= 180; directionAngle += 10)
                        {
                            ray.origin = origin;
                            ray.direction = left.RotateCW(directionAngle);
                            AreEqual_ClosestPoints(ray, circle, ray.origin, onCircle);
                        }
                    }
                }
            }
        }

        private void AreEqual_ClosestPoints(Ray2D ray, Circle2 circle, Vector2 expectedRay, Vector2 expectedCircle)
        {
            string message = string.Format(format, ray, circle);
            Closest.RayCircle(ray.origin, ray.direction, circle.center, circle.radius, out Vector2 rayPoint, out Vector2 centerPoint);
            AreEqual(rayPoint, expectedRay, message);
            AreEqual(centerPoint, expectedCircle, message);
        }

        #endregion ClosestPoints

        #region Intersect

        [Test]
        public void Intersect_TwoPoints()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            float offset = 1;
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 12; radius += 10)
                {
                    circle.radius = radius;
                    for (int rayAngle = 0; rayAngle < 360; rayAngle += 10)
                    {
                        Vector2 direction = Vector2.down.RotateCW(rayAngle);
                        ray.origin = circle.center - direction*circle.radius;
                        ray.direction = direction;
                        True_IntersectTwoPoints(ray, circle, ray.origin, ray.GetPoint(circle.radius*2));

                        ray.origin = circle.center - direction*(circle.radius + offset);
                        ray.direction = direction;
                        True_IntersectTwoPoints(ray, circle, ray.GetPoint(offset), ray.GetPoint(offset + circle.radius*2));

                        Vector2 expectedA = circle.GetPoint(rayAngle + 45);
                        Vector2 expectedB = circle.GetPoint(rayAngle + 135);
                        direction = (expectedB - expectedA).normalized;
                        ray.origin = expectedA - direction*0.1f;
                        ray.direction = direction;
                        True_IntersectTwoPoints(ray, circle, expectedA, expectedB);
                    }
                }
            }
        }

        [Test]
        public void Intersect_OnePointOnCircle()
        {
            var ray = new Ray2D();
            var circle = new Circle2(Vector2.zero, 4);
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int originAngle = 0; originAngle < 360; originAngle += 10)
                {
                    Vector2 onCircle = circle.GetPoint(originAngle);
                    Vector2 left = Vector2.left.RotateCW(originAngle).normalized;
                    ray.origin = onCircle - left;
                    ray.direction = left;
                    True_IntersectPoint(ray, circle, onCircle);

                    for (int directionAngle = 0; directionAngle <= 180; directionAngle += 10)
                    {
                        ray.origin = onCircle;
                        ray.direction = left.RotateCW(directionAngle);
                        True_IntersectPoint(ray, circle, onCircle);
                    }
                }
            }
        }

        [Test]
        public void Intersect_OnePointInCircle()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    for (int rayAngle = 0; rayAngle < 360; rayAngle += 10)
                    {
                        Vector2 down = Vector2.down.RotateCW(rayAngle);
                        ray.origin = circle.center;
                        ray.direction = down;
                        Vector2 onCircle = ray.GetPoint(circle.radius);
                        True_IntersectPoint(ray, circle, onCircle);
                        ray.origin = circle.center + down*0.5f;
                        True_IntersectPoint(ray, circle, onCircle);
                        onCircle = circle.GetPoint(rayAngle + 135);
                        float distance = Mathf.Sqrt(2*circle.radius);
                        ray.origin = onCircle - down*distance*0.1f;
                        True_IntersectPoint(ray, circle, onCircle);
                        ray.origin = onCircle - down*distance*0.9f;
                        True_IntersectPoint(ray, circle, onCircle);
                    }
                }
            }
        }

        [Test]
        public void Intersect_Separate()
        {
            var ray = new Ray2D();
            var circle = new Circle2();
            float distance = 1;
            foreach (var center in originPoints2)
            {
                circle.center = center;
                for (int radius = 1; radius < 22; radius += 10)
                {
                    circle.radius = radius;
                    for (int originAngle = 0; originAngle < 360; originAngle += 10)
                    {
                        Vector2 origin = circle.center + Geometry.PointOnCircle2(circle.radius + distance, originAngle);
                        Vector2 left = Vector2.left.RotateCW(originAngle).normalized;
                        Vector2 right = -left;

                        ray.origin = origin + left;
                        ray.direction = right;
                        False_Intersect(ray, circle);

                        ray.origin = origin + left;
                        ray.direction = left;
                        False_Intersect(ray, circle);

                        ray.origin = origin + right;
                        ray.direction = right;
                        False_Intersect(ray, circle);

                        ray.origin = origin + right;
                        ray.direction = left;
                        False_Intersect(ray, circle);

                        for (int directionAngle = 0; directionAngle <= 180; directionAngle += 10)
                        {
                            ray.origin = origin;
                            ray.direction = left.RotateCW(directionAngle);
                            False_Intersect(ray, circle);
                        }
                    }
                }
            }
        }

        private void True_IntersectPoint(Ray2D ray, Circle2 circle, Vector2 expected)
        {
            string message = string.Format(format, ray.ToString("F8"), circle);
            Assert.True(Intersect.RayCircle(ray.origin, ray.direction, circle.center, circle.radius, out IntersectionRayCircle intersection), message);
            Assert.AreEqual(IntersectionType.Point, intersection.type, message);
            AreEqual(intersection.pointA, expected, message);
        }

        private void True_IntersectTwoPoints(Ray2D ray, Circle2 circle, Vector2 expectedA, Vector2 expectedB)
        {
            string message = string.Format(format, ray.ToString("F8"), circle);
            Assert.True(Intersect.RayCircle(ray.origin, ray.direction, circle.center, circle.radius, out IntersectionRayCircle intersection), message);
            Assert.AreEqual(IntersectionType.TwoPoints, intersection.type, message);
            AreEqual(intersection.pointA, expectedA, message);
            AreEqual(intersection.pointB, expectedB, message);
        }

        private void False_Intersect(Ray2D ray, Circle2 circle)
        {
            string message = string.Format(format, ray.ToString("F8"), circle);
            Assert.False(Intersect.RayCircle(ray.origin, ray.direction, circle.center, circle.radius, out IntersectionRayCircle intersection), message);
        }

        #endregion Intersect
    }
}

using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests
{
    public class TestBase
    {
        private const float Epsilon = Geometry.Epsilon*10;
        private const string vector2Format = "actual: {0} expected: {1}\ndelta: {2:F8}\n{3}";
        private const string floatFormat = "actual: {0:G9} expected: {1:G9}\ndelta: {2:F8}\n{3}";

        private List<Vector2> _directionPoints2;
        protected List<Vector2> directionPoints2
        {
            get { return _directionPoints2 ?? (_directionPoints2 = Geometry.PointsOnCircle2(1, 24)); }
        }
        private List<Vector2> _originPoints2;
        protected List<Vector2> originPoints2
        {
            get { return _originPoints2 ?? (_originPoints2 = Geometry.PointsOnCircle2(10, 24)); }
        }

        private List<Vector3> _directionPoints3;
        protected List<Vector3> directionPoints3
        {
            get { return _directionPoints3 ?? (_directionPoints3 = Geometry.PointsOnSphere(1, 100)); }
        }
        private List<Vector3> _originPoints3;
        protected List<Vector3> originPoints3
        {
            get { return _originPoints3 ?? (_originPoints3 = Geometry.PointsOnSphere(10, 100)); }
        }

        protected static void AreEqual(Vector2 actual, Vector2 expected, string message = null)
        {
            float delta = (actual - expected).magnitude;
            Assert.True(delta < Epsilon, vector2Format, actual.ToString("G9"), expected.ToString("G9"), delta, message);
        }

        protected static void AreEqual(float actual, float expected, string message = null)
        {
            float delta = Mathf.Abs(actual - expected);
            Assert.True(delta < Epsilon, floatFormat, actual, expected, delta, message);
        }

        protected static Vector3 GetTangent(Vector3 direction)
        {
            Vector3 tangent = direction;
            Vector3.OrthoNormalize(ref direction, ref tangent);
            return tangent;
        }
    }
}

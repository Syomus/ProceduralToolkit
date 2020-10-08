using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace ProceduralToolkit.Tests
{
    public class GeometryTest : TestBase
    {
        [Test]
        public void GetAngleBisector()
        {
            for (int originAngle = 0; originAngle < 360; originAngle += 10)
            {
                Vector2 current = Geometry.PointOnCircle2(30, originAngle);
                for (int directionAngle = 0; directionAngle < 360; directionAngle += 10)
                {
                    Vector2 direction = Vector2.up.RotateCW(directionAngle).normalized;

                    // C--PNB
                    GetAngleBisector(current + direction, current, current + direction, direction, 0);
                    // N B
                    // |/
                    // C--P
                    GetAngleBisector(current + direction, current, current + direction.RotateCCW90(), direction.RotateCCW45(), 90);
                    //    B
                    //    |
                    // N--C--P
                    GetAngleBisector(current + direction, current, current - direction, direction.RotateCCW90(), 180);
                    // B
                    //  \
                    //   C--P
                    //   |
                    //   N
                    GetAngleBisector(current + direction, current, current + direction.RotateCW90(), -direction.RotateCW45(), 270);
                }
            }
        }

        private void GetAngleBisector(Vector2 previous, Vector2 current, Vector2 next, Vector2 expectedBisector, float expectedAngle)
        {
            Vector2 bisector = Geometry.GetAngleBisector(previous, current, next, out float degrees);
            string message = string.Format("previous: {0}\ncurrent: {1}\nnext: {2}", previous, current, next);
            AreEqual(bisector, expectedBisector, message);
            AreEqual(degrees, expectedAngle, message);
        }

        [Test]
        public void OffsetPolygon_SquarePositive()
        {
            var verticesArray = new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
            };
            var reference = new Vector2[]
            {
                new Vector2(-2, -2),
                new Vector2(-2, 2),
                new Vector2(2, 2),
                new Vector2(2, -2),
            };
            OffsetPolygon(verticesArray, reference, 1);
        }

        [Test]
        public void OffsetPolygon_SquareNegative()
        {
            var verticesArray = new Vector2[]
            {
                new Vector2(-2, -2),
                new Vector2(-2, 2),
                new Vector2(2, 2),
                new Vector2(2, -2),
            };
            var reference = new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
            };
            OffsetPolygon(verticesArray, reference, -1);
        }

        [Test]
        public void OffsetPolygon_PlusPositive()
        {
            var verticesArray = new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-2, -1),
                new Vector2(-2, 1),
                new Vector2(-1, 1),
                new Vector2(-1, 2),
                new Vector2(1, 2),
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, -1),
                new Vector2(1, -1),
                new Vector2(1, -2),
                new Vector2(-1, -2),
            };
            var reference = new Vector2[]
            {
                new Vector2(-2, -2),
                new Vector2(-3, -2),
                new Vector2(-3, 2),
                new Vector2(-2, 2),
                new Vector2(-2, 3),
                new Vector2(2, 3),
                new Vector2(2, 2),
                new Vector2(3, 2),
                new Vector2(3, -2),
                new Vector2(2, -2),
                new Vector2(2, -3),
                new Vector2(-2, -3),
            };
            OffsetPolygon(verticesArray, reference, 1);
        }

        [Test]
        public void OffsetPolygon_PlusNegative()
        {
            var verticesArray = new Vector2[]
            {
                new Vector2(-2, -2),
                new Vector2(-3, -2),
                new Vector2(-3, 2),
                new Vector2(-2, 2),
                new Vector2(-2, 3),
                new Vector2(2, 3),
                new Vector2(2, 2),
                new Vector2(3, 2),
                new Vector2(3, -2),
                new Vector2(2, -2),
                new Vector2(2, -3),
                new Vector2(-2, -3),
            };
            var reference = new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-2, -1),
                new Vector2(-2, 1),
                new Vector2(-1, 1),
                new Vector2(-1, 2),
                new Vector2(1, 2),
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, -1),
                new Vector2(1, -1),
                new Vector2(1, -2),
                new Vector2(-1, -2),
            };
            OffsetPolygon(verticesArray, reference, -1);
        }

        [Test]
        public void OffsetPolygon_PentagonPositive()
        {
            var verticesArray = new Vector2[]
            {
                new Vector2(+0.00000f, +1.23607f),
                new Vector2(+1.17557f, +0.38197f),
                new Vector2(+0.72654f, -1.00000f),
                new Vector2(-0.72654f, -1.00000f),
                new Vector2(-1.17557f, +0.38197f),
            };
            var reference = new Vector2[]
            {
                new Vector2(+0.00000f, +2.47214f),
                new Vector2(+2.35115f, +0.76393f),
                new Vector2(+1.45309f, -2.00000f),
                new Vector2(-1.45309f, -2.00000f),
                new Vector2(-2.35115f, +0.76393f),
            };
            OffsetPolygon(verticesArray, reference, 1);
        }

        [Test]
        public void OffsetPolygon_PentagonNegative()
        {
            var verticesArray = new Vector2[]
            {
                new Vector2(+0.00000f, +2.47214f),
                new Vector2(+2.35115f, +0.76393f),
                new Vector2(+1.45309f, -2.00000f),
                new Vector2(-1.45309f, -2.00000f),
                new Vector2(-2.35115f, +0.76393f),
            };
            var reference = new Vector2[]
            {
                new Vector2(+0.00000f, +1.23607f),
                new Vector2(+1.17557f, +0.38197f),
                new Vector2(+0.72654f, -1.00000f),
                new Vector2(-0.72654f, -1.00000f),
                new Vector2(-1.17557f, +0.38197f),
            };
            OffsetPolygon(verticesArray, reference, -1);
        }

        [Test]
        public void OffsetPolygon_PentagramPositive()
        {
            var verticesArray = new Vector2[]
            {
                new Vector2(+0.00000f, +2.00000f),
                new Vector2(+0.58779f, +0.80902f),
                new Vector2(+1.90211f, +0.61803f),
                new Vector2(+0.95106f, -0.30902f),
                new Vector2(+1.17557f, -1.61803f),
                new Vector2(-0.00000f, -1.00000f),
                new Vector2(-1.17557f, -1.61803f),
                new Vector2(-0.95106f, -0.30902f),
                new Vector2(-1.90211f, +0.61803f),
                new Vector2(-0.58779f, +0.80902f),
            };
            var reference = new Vector2[]
            {
                new Vector2(+0.00000f, +4.25953f),
                new Vector2(+1.25185f, +1.72303f),
                new Vector2(+4.05105f, +1.31627f),
                new Vector2(+2.02554f, -0.65814f),
                new Vector2(+2.50369f, -3.44603f),
                new Vector2(-0.00000f, -2.12978f),
                new Vector2(-2.50369f, -3.44603f),
                new Vector2(-2.02554f, -0.65814f),
                new Vector2(-4.05105f, +1.31627f),
                new Vector2(-1.25185f, +1.72303f),
            };
            OffsetPolygon(verticesArray, reference, 1);
        }

        [Test]
        public void OffsetPolygon_PentagramNegative()
        {
            var verticesArray = new Vector2[]
            {
                new Vector2(+0.00000f, +4.25953f),
                new Vector2(+1.25185f, +1.72303f),
                new Vector2(+4.05105f, +1.31627f),
                new Vector2(+2.02554f, -0.65814f),
                new Vector2(+2.50369f, -3.44603f),
                new Vector2(-0.00000f, -2.12978f),
                new Vector2(-2.50369f, -3.44603f),
                new Vector2(-2.02554f, -0.65814f),
                new Vector2(-4.05105f, +1.31627f),
                new Vector2(-1.25185f, +1.72303f),
            };
            var reference = new Vector2[]
            {
                new Vector2(+0.00000f, +2.00000f),
                new Vector2(+0.58779f, +0.80902f),
                new Vector2(+1.90211f, +0.61803f),
                new Vector2(+0.95106f, -0.30902f),
                new Vector2(+1.17557f, -1.61803f),
                new Vector2(-0.00000f, -1.00000f),
                new Vector2(-1.17557f, -1.61803f),
                new Vector2(-0.95106f, -0.30902f),
                new Vector2(-1.90211f, +0.61803f),
                new Vector2(-0.58779f, +0.80902f),
            };
            OffsetPolygon(verticesArray, reference, -1);
        }

        private void OffsetPolygon(Vector2[] verticesArray, Vector2[] reference, float distance)
        {
            var verticesList = new List<Vector2>(verticesArray);
            var newVertices = Geometry.OffsetPolygon(verticesArray, distance);
            Geometry.OffsetPolygon(ref verticesArray, distance);
            Geometry.OffsetPolygon(ref verticesList, distance);
            for (int i = 0; i < verticesArray.Length; i++)
            {
                AreEqual(verticesArray[i], reference[i]);
                AreEqual(verticesList[i], reference[i]);
                AreEqual(newVertices[i], reference[i]);
            }
        }

        [Test]
        public void GetSignedArea_EmptyPolygon()
        {
            AreEqual(Geometry.GetSignedArea(new List<Vector2>()), 0);
        }

        [Test]
        public void GetSignedArea_ClockwiseRectangle()
        {
            AreEqual(Geometry.GetSignedArea(new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
            }), -4);
        }

        [Test]
        public void GetSignedArea_CounterClockwiseRectangle()
        {
            AreEqual(Geometry.GetSignedArea(new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(1, -1),
                new Vector2(1, 1),
                new Vector2(-1, 1),
            }), 4);
        }

        [Test]
        public void GetSignedArea_DegeneratePoint()
        {
            AreEqual(Geometry.GetSignedArea(new Vector2[]
            {
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
            }), 0);
        }

        [Test]
        public void GetOrientation_EmptyPolygon()
        {
            Assert.AreEqual(Orientation.NonOrientable, Geometry.GetOrientation(new List<Vector2>()));
        }

        [Test]
        public void GetOrientation_ClockwiseRectangle()
        {
            Assert.AreEqual(Orientation.Clockwise, Geometry.GetOrientation(new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(1, -1),
            }));
        }

        [Test]
        public void GetOrientation_CounterClockwiseRectangle()
        {
            Assert.AreEqual(Orientation.CounterClockwise, Geometry.GetOrientation(new Vector2[]
            {
                new Vector2(-1, -1),
                new Vector2(1, -1),
                new Vector2(1, 1),
                new Vector2(-1, 1),
            }));
        }

        [Test]
        public void GetOrientation_DegeneratePoint()
        {
            Assert.AreEqual(Orientation.NonOrientable, Geometry.GetOrientation(new Vector2[]
            {
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
            }));
        }
    }
}

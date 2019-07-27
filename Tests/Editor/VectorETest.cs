using NUnit.Framework;
using UnityEngine;

namespace ProceduralToolkit.Tests
{
    public class VectorETest : TestBase
    {
        [Test]
        public void SignedAngle()
        {
            Assert.AreEqual(0, VectorE.SignedAngle(Vector2.up, Vector2.up));
            Assert.AreEqual(45, VectorE.SignedAngle(Vector2.up, Vector2.one));
            Assert.AreEqual(90, VectorE.SignedAngle(Vector2.up, Vector2.right));
            Assert.AreEqual(135, VectorE.SignedAngle(Vector2.up, new Vector2(1, -1)));
            Assert.AreEqual(180, VectorE.SignedAngle(Vector2.up, Vector2.down));
            Assert.AreEqual(-45, VectorE.SignedAngle(Vector2.up, new Vector2(-1, 1)));
            Assert.AreEqual(-90, VectorE.SignedAngle(Vector2.up, Vector2.left));
            Assert.AreEqual(-135, VectorE.SignedAngle(Vector2.up, new Vector2(-1, -1)));
        }

        [Test]
        public void RotateCW()
        {
            for (int offset = -360; offset <= 360; offset += 360)
            {
                AreEqual(Vector2.up.RotateCW(offset + 0), Vector2.up);
                AreEqual(Vector2.up.RotateCW(offset + 45), new Vector2(1, 1).normalized);
                AreEqual(Vector2.up.RotateCW(offset + 90), Vector2.right);
                AreEqual(Vector2.up.RotateCW(offset + 135), new Vector2(1, -1).normalized);
                AreEqual(Vector2.up.RotateCW(offset + 180), Vector2.down);
                AreEqual(Vector2.up.RotateCW(offset + 225), new Vector2(-1, -1).normalized);
                AreEqual(Vector2.up.RotateCW(offset + 270), Vector2.left);
                AreEqual(Vector2.up.RotateCW(offset + 315), new Vector2(-1, 1).normalized);
                AreEqual(Vector2.up.RotateCW(offset + 360), Vector2.up);
                AreEqual(Vector2.up.RotateCW(offset - 360), Vector2.up);
            }
        }

        [Test]
        public void RotateCCW()
        {
            for (int offset = -360; offset <= 360; offset += 360)
            {
                AreEqual(Vector2.up.RotateCCW(offset + 0), Vector2.up);
                AreEqual(Vector2.up.RotateCCW(offset + 45), new Vector2(-1, 1).normalized);
                AreEqual(Vector2.up.RotateCCW(offset + 90), Vector2.left);
                AreEqual(Vector2.up.RotateCCW(offset + 135), new Vector2(-1, -1).normalized);
                AreEqual(Vector2.up.RotateCCW(offset + 180), Vector2.down);
                AreEqual(Vector2.up.RotateCCW(offset + 225), new Vector2(1, -1).normalized);
                AreEqual(Vector2.up.RotateCCW(offset + 270), Vector2.right);
                AreEqual(Vector2.up.RotateCCW(offset + 315), new Vector2(1, 1).normalized);
                AreEqual(Vector2.up.RotateCCW(offset + 360), Vector2.up);
                AreEqual(Vector2.up.RotateCCW(offset - 360), Vector2.up);
            }
        }

        [Test]
        public void RotateCW45()
        {
            AreEqual(Vector2.up.RotateCW45(), new Vector2(1, 1).normalized);
            AreEqual(Vector2.right.RotateCW45(), new Vector2(1, -1).normalized);
            AreEqual(Vector2.down.RotateCW45(), new Vector2(-1, -1).normalized);
            AreEqual(Vector2.left.RotateCW45(), new Vector2(-1, 1).normalized);
        }

        [Test]
        public void RotateCCW45()
        {
            AreEqual(Vector2.up.RotateCCW45(), new Vector2(-1, 1).normalized);
            AreEqual(Vector2.right.RotateCCW45(), new Vector2(1, 1).normalized);
            AreEqual(Vector2.down.RotateCCW45(), new Vector2(1, -1).normalized);
            AreEqual(Vector2.left.RotateCCW45(), new Vector2(-1, -1).normalized);
        }

        [Test]
        public void RotateCW90()
        {
            AreEqual(Vector2.up.RotateCW90(), Vector2.right);
            AreEqual(Vector2.right.RotateCW90(), Vector2.down);
            AreEqual(Vector2.down.RotateCW90(), Vector2.left);
            AreEqual(Vector2.left.RotateCW90(), Vector2.up);
        }

        [Test]
        public void RotateCCW90()
        {
            AreEqual(Vector2.up.RotateCCW90(), Vector2.left);
            AreEqual(Vector2.right.RotateCCW90(), Vector2.up);
            AreEqual(Vector2.down.RotateCCW90(), Vector2.right);
            AreEqual(Vector2.left.RotateCCW90(), Vector2.down);
        }
    }
}

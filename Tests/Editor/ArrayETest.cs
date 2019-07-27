using System.Collections.Generic;
using NUnit.Framework;

namespace ProceduralToolkit.Tests
{
    public class ArrayETest
    {
        public class GetLooped_Array
        {
            private readonly int[] array = new[] {0, 1, 2, 3};

            [Test]
            public void ZeroEqualsZero()
            {
                Assert.AreEqual(array[0], array.GetLooped(0));
            }

            [Test]
            public void LengthEqualsZero()
            {
                Assert.AreEqual(array[0], array.GetLooped(array.Length));
            }

            [Test]
            public void DoubleLengthEqualsZero()
            {
                Assert.AreEqual(array[0], array.GetLooped(array.Length*2));
            }

            [Test]
            public void MinusOneEqualsLast()
            {
                Assert.AreEqual(array[array.Length - 1], array.GetLooped(-1));
            }
        }

        public class GetLooped_List
        {
            private readonly List<int> list = new List<int> {0, 1, 2, 3};

            [Test]
            public void ZeroEqualsZero()
            {
                Assert.AreEqual(list[0], list.GetLooped(0));
            }

            [Test]
            public void LengthEqualsZero()
            {
                Assert.AreEqual(list[0], list.GetLooped(list.Count));
            }

            [Test]
            public void DoubleLengthEqualsZero()
            {
                Assert.AreEqual(list[0], list.GetLooped(list.Count*2));
            }

            [Test]
            public void MinusOneEqualsLast()
            {
                Assert.AreEqual(list[list.Count - 1], list.GetLooped(-1));
            }
        }

        public class SetLooped_Array
        {
            private readonly int[] array = new[] {0, 0, 0, 0};

            [Test]
            public void ZeroSetsZero()
            {
                array.SetLooped(0, 1);
                Assert.AreEqual(array[0], 1);
            }

            [Test]
            public void LengthSetsZero()
            {
                array.SetLooped(array.Length, 2);
                Assert.AreEqual(array[0], 2);
            }

            [Test]
            public void DoubleLengthSetsZero()
            {
                array.SetLooped(array.Length*2, 3);
                Assert.AreEqual(array[0], 3);
            }

            [Test]
            public void MinusOneSetsLast()
            {
                array.SetLooped(-1, 4);
                Assert.AreEqual(array[array.Length - 1], 4);
            }
        }

        public class SetLooped_List
        {
            private readonly List<int> list = new List<int> {0, 0, 0, 0};

            [Test]
            public void ZeroSetsZero()
            {
                list.SetLooped(0, 1);
                Assert.AreEqual(list[0], 1);
            }

            [Test]
            public void LengthSetsZero()
            {
                list.SetLooped(list.Count, 2);
                Assert.AreEqual(list[0], 2);
            }

            [Test]
            public void DoubleLengthSetsZero()
            {
                list.SetLooped(list.Count*2, 3);
                Assert.AreEqual(list[0], 3);
            }

            [Test]
            public void MinusOneSetsLast()
            {
                list.SetLooped(-1, 4);
                Assert.AreEqual(list[list.Count - 1], 4);
            }
        }
    }
}

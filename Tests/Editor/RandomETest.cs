using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.RandomETest
{
    public class RandomETest
    {
        public class GetRandom_IList
        {
            private readonly int[] nullArray = null;
            private readonly int[] emptyArray = new int[0];
            private readonly int[] array = new int[] {0, 1, 2, 3};
            private readonly List<int> nullList = null;
            private readonly List<int> emptyList = new List<int>();
            private readonly List<int> list = new List<int> {0, 1, 2, 3};

            [Test]
            public void NullListThrowsException()
            {
                Assert.Catch<ArgumentNullException>(() => nullArray.GetRandom());
                Assert.Catch<ArgumentNullException>(() => nullList.GetRandom());
            }

            [Test]
            public void EmptyListThrowsException()
            {
                Assert.Catch<ArgumentException>(() => emptyArray.GetRandom());
                Assert.Catch<ArgumentException>(() => emptyList.GetRandom());
            }

            [Test]
            public void ListContainsReturnedItem()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.Contains(array.GetRandom(), array);
                    Assert.Contains(list.GetRandom(), list);
                }
            }
        }

        public class GetRandom_Params
        {
            private readonly int[] nullArray = null;
            private readonly int[] emptyArray = new int[0];
            private readonly int[] array0123 = new int[] {0, 1, 2, 3};
            private readonly int[] array01 = new int[] {0, 1};
            private readonly int[] array23 = new int[] {2, 3};

            [Test]
            public void NoParams()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.Contains(RandomE.GetRandom(0, 1), array01);
                }
            }

            [Test]
            public void WithParams()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.Contains(RandomE.GetRandom(0, 1, 2, 3), array0123);
                }
            }

            [Test]
            public void WithArray()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.Contains(RandomE.GetRandom(0, 1, array23), array0123);
                }
            }

            [Test]
            public void NullArrayThrowsException()
            {
                Assert.Catch<ArgumentNullException>(() => RandomE.GetRandom(0, 1, nullArray));
            }

            [Test]
            public void WithEmptyArray()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.Contains(RandomE.GetRandom(0, 1, emptyArray), array01);
                }
            }
        }
    }
}

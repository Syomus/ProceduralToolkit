using System;
using System.Collections.Generic;
using NUnit.Framework;
using MRandom = Unity.Mathematics.Random;

namespace ProceduralToolkit.Tests.RandomETest
{
    public class RandomETest
    {
        public class GetRandom_IList
        {
            private readonly int[] nullArray = null;
            private readonly int[] emptyArray = new int[0];
            private readonly int[] array = {0, 1, 2, 3};
            private readonly List<int> nullList = null;
            private readonly List<int> emptyList = new();
            private readonly List<int> list = new() {0, 1, 2, 3};
            private MRandom random = new(1337);

            [Test]
            public void NullListThrowsException()
            {
                Assert.Catch<ArgumentNullException>(() => nullArray.GetRandom());
                Assert.Catch<ArgumentNullException>(() => nullArray.GetRandom(ref random));
                Assert.Catch<ArgumentNullException>(() => nullList.GetRandom());
                Assert.Catch<ArgumentNullException>(() => nullList.GetRandom(ref random));
            }

            [Test]
            public void EmptyListThrowsException()
            {
                Assert.Catch<ArgumentException>(() => emptyArray.GetRandom());
                Assert.Catch<ArgumentException>(() => emptyArray.GetRandom(ref random));
                Assert.Catch<ArgumentException>(() => emptyList.GetRandom());
                Assert.Catch<ArgumentException>(() => emptyList.GetRandom(ref random));
            }

            [Test]
            public void ListContainsReturnedItem()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.Contains(array.GetRandom(), array);
                    Assert.Contains(array.GetRandom(ref random), array);
                    Assert.Contains(list.GetRandom(), list);
                    Assert.Contains(list.GetRandom(ref random), list);
                }
            }
        }

        public class GetRandom_IListWeighted
        {
            private readonly int[] nullArray = null;
            private readonly int[] emptyArray = new int[0];
            private readonly int[] array = {0, 1, 2, 3};
            private readonly List<int> nullList = null;
            private readonly List<int> emptyList = new();
            private readonly List<int> list = new() {0, 1, 2, 3};
            private readonly float[] nullWeights = null;
            private readonly float[] emptyWeights = new float[0];
            private readonly float[] weights1 = {1};
            private readonly float[] weights1111 = {1, 1, 1, 1};
            private readonly float[] negativeWeights = {-100, 1, 1, 1};
            private MRandom random = new(1337);

            [Test]
            public void NullListThrowsException()
            {
                Assert.Catch<ArgumentNullException>(() => nullArray.GetRandom(weights1111));
                Assert.Catch<ArgumentNullException>(() => nullArray.GetRandom(weights1111, ref random));
                Assert.Catch<ArgumentNullException>(() => nullList.GetRandom(weights1111));
                Assert.Catch<ArgumentNullException>(() => nullList.GetRandom(weights1111, ref random));
            }

            [Test]
            public void EmptyListThrowsException()
            {
                Assert.Catch<ArgumentException>(() => emptyArray.GetRandom(weights1111));
                Assert.Catch<ArgumentException>(() => emptyArray.GetRandom(weights1111, ref random));
                Assert.Catch<ArgumentException>(() => emptyList.GetRandom(weights1111));
                Assert.Catch<ArgumentException>(() => emptyList.GetRandom(weights1111, ref random));
            }

            [Test]
            public void NullWeightsThrowsException()
            {
                Assert.Catch<ArgumentNullException>(() => array.GetRandom(nullWeights));
                Assert.Catch<ArgumentNullException>(() => array.GetRandom(nullWeights, ref random));
                Assert.Catch<ArgumentNullException>(() => list.GetRandom(nullWeights));
                Assert.Catch<ArgumentNullException>(() => list.GetRandom(nullWeights, ref random));
            }

            [Test]
            public void EmptyWeightsThrowsException()
            {
                Assert.Catch<ArgumentException>(() => array.GetRandom(emptyWeights));
                Assert.Catch<ArgumentException>(() => array.GetRandom(emptyWeights, ref random));
                Assert.Catch<ArgumentException>(() => list.GetRandom(emptyWeights));
                Assert.Catch<ArgumentException>(() => list.GetRandom(emptyWeights, ref random));
            }

            [Test]
            public void DifferentSizeThrowsException()
            {
                Assert.Catch<ArgumentException>(() => array.GetRandom(weights1));
                Assert.Catch<ArgumentException>(() => array.GetRandom(weights1, ref random));
                Assert.Catch<ArgumentException>(() => list.GetRandom(weights1));
                Assert.Catch<ArgumentException>(() => list.GetRandom(weights1, ref random));
            }

            [Test]
            public void NegativeWeightsThrowsException()
            {
                Assert.Catch<ArgumentException>(() => array.GetRandom(negativeWeights));
                Assert.Catch<ArgumentException>(() => array.GetRandom(negativeWeights, ref random));
                Assert.Catch<ArgumentException>(() => list.GetRandom(negativeWeights));
                Assert.Catch<ArgumentException>(() => list.GetRandom(negativeWeights, ref random));
            }

            [Test]
            public void ListContainsReturnedItem()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.Contains(array.GetRandom(weights1111), array);
                    Assert.Contains(array.GetRandom(weights1111, ref random), array);
                    Assert.Contains(list.GetRandom(weights1111), list);
                    Assert.Contains(list.GetRandom(weights1111, ref random), list);
                }
            }
        }

        public class GetRandom_Params
        {
            private readonly int[] nullArray = null;
            private readonly int[] emptyArray = new int[0];
            private readonly int[] array0123 = {0, 1, 2, 3};
            private readonly int[] array01 = {0, 1};
            private readonly int[] array23 = {2, 3};

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

        public class GetRandom_Dictionary
        {
            private readonly Dictionary<int, int> nullDictionary = null;
            private readonly Dictionary<int, int> emptyDictionary = new();
            private readonly Dictionary<int, int> dictionary = new()
            {
                {1, 10},
                {2, 20},
                {3, 30},
                {4, 40},
            };
            private MRandom random = new(1337);

            [Test]
            public void NullDictionaryThrowsException()
            {
                Assert.Catch<ArgumentNullException>(() => nullDictionary.GetRandom());
                Assert.Catch<ArgumentNullException>(() => nullDictionary.GetRandom(ref random));
            }

            [Test]
            public void EmptyDictionaryThrowsException()
            {
                Assert.Catch<ArgumentException>(() => emptyDictionary.GetRandom());
                Assert.Catch<ArgumentException>(() => emptyDictionary.GetRandom(ref random));
            }

            [Test]
            public void DictionaryContainsReturnedItem()
            {
                for (int i = 0; i < 100; i++)
                {
                    Assert.True(dictionary.ContainsValue(dictionary.GetRandom()));
                    Assert.True(dictionary.ContainsValue(dictionary.GetRandom(ref random)));
                }
            }
        }

        public class PopRandom_IList
        {
            private readonly List<int> nullList = null;
            private readonly List<int> emptyList = new();
            private MRandom random = new(1337);

            [Test]
            public void NullListThrowsException()
            {
                Assert.Catch<ArgumentNullException>(() => nullList.PopRandom());
                Assert.Catch<ArgumentNullException>(() => nullList.PopRandom(ref random));
            }

            [Test]
            public void EmptyListThrowsException()
            {
                Assert.Catch<ArgumentException>(() => emptyList.PopRandom());
                Assert.Catch<ArgumentException>(() => emptyList.PopRandom(ref random));
            }

            [Test]
            public void ListDoesNotContainReturnedItem()
            {
                var list = new List<int> {0, 1, 2, 3};
                for (int i = 0; i < list.Count; i++)
                {
                    Assert.False(list.Contains(list.PopRandom()));
                }
                list = new List<int> {0, 1, 2, 3};
                for (int i = 0; i < list.Count; i++)
                {
                    Assert.False(list.Contains(list.PopRandom(ref random)));
                }
            }
        }
    }
}

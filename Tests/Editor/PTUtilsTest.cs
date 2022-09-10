using System.Collections.Generic;
using NUnit.Framework;

namespace ProceduralToolkit.Tests.PTUtilsTest
{
    public class PTUtilsTest
    {
        public class Knapsack_NoOverload
        {
            private readonly Dictionary<int, float> set = new()
            {
                { 1, 1 },
                { 2, 2 },
                { 4, 4 },
            };
            private readonly Dictionary<int, int> knapsack0 = new()
            {
                { 4, 0 },
                { 2, 0 },
                { 1, 0 },
            };
            private readonly Dictionary<int, int> knapsack1 = new()
            {
                { 4, 0 },
                { 2, 0 },
                { 1, 1 },
            };
            private readonly Dictionary<int, int> knapsack2 = new()
            {
                { 4, 0 },
                { 2, 1 },
                { 1, 0 },
            };
            private readonly Dictionary<int, int> knapsack3 = new()
            {
                { 4, 0 },
                { 2, 1 },
                { 1, 1 },
            };
            private readonly Dictionary<int, int> knapsack10 = new()
            {
                { 4, 2 },
                { 2, 1 },
                { 1, 0 },
            };

            [Test]
            public void MinusOne()
            {
                Assert.AreEqual(knapsack0, PTUtils.Knapsack(set, -1, overloadKnapsack: false));
            }

            [Test]
            public void Zero()
            {
                Assert.AreEqual(knapsack0, PTUtils.Knapsack(set, 0, overloadKnapsack: false));
            }

            [Test]
            public void PointFive()
            {
                Assert.AreEqual(knapsack0, PTUtils.Knapsack(set, 0.5f, overloadKnapsack: false));
            }

            [Test]
            public void One()
            {
                Assert.AreEqual(knapsack1, PTUtils.Knapsack(set, 1, overloadKnapsack: false));
            }

            [Test]
            public void OnePointFive()
            {
                Assert.AreEqual(knapsack1, PTUtils.Knapsack(set, 1.5f, overloadKnapsack: false));
            }

            [Test]
            public void Two()
            {
                Assert.AreEqual(knapsack2, PTUtils.Knapsack(set, 2, overloadKnapsack: false));
            }

            [Test]
            public void TwoPointFive()
            {
                Assert.AreEqual(knapsack2, PTUtils.Knapsack(set, 2.5f, overloadKnapsack: false));
            }

            [Test]
            public void Three()
            {
                Assert.AreEqual(knapsack3, PTUtils.Knapsack(set, 3, overloadKnapsack: false));
            }

            [Test]
            public void ThreePointFive()
            {
                Assert.AreEqual(knapsack3, PTUtils.Knapsack(set, 3.5f, overloadKnapsack: false));
            }

            [Test]
            public void Ten()
            {
                Assert.AreEqual(knapsack10, PTUtils.Knapsack(set, 10, overloadKnapsack: false));
            }

            [Test]
            public void TenPointFive()
            {
                Assert.AreEqual(knapsack10, PTUtils.Knapsack(set, 10.5f, overloadKnapsack: false));
            }
        }

        public class Knapsack_Overload
        {
            private readonly Dictionary<int, float> set = new()
            {
                { 1, 1 },
                { 2, 2 },
                { 4, 4 },
            };
            private readonly Dictionary<int, int> knapsack0 = new()
            {
                { 4, 0 },
                { 2, 0 },
                { 1, 1 },
            };
            private readonly Dictionary<int, int> knapsack1_5 = new()
            {
                { 4, 0 },
                { 2, 0 },
                { 1, 2 },
            };
            private readonly Dictionary<int, int> knapsack2 = new()
            {
                { 4, 0 },
                { 2, 1 },
                { 1, 0 },
            };
            private readonly Dictionary<int, int> knapsack3 = new()
            {
                { 4, 0 },
                { 2, 1 },
                { 1, 1 },
            };
            private readonly Dictionary<int, int> knapsack3_5 = new()
            {
                { 4, 0 },
                { 2, 1 },
                { 1, 2 },
            };
            private readonly Dictionary<int, int> knapsack10 = new()
            {
                { 4, 2 },
                { 2, 1 },
                { 1, 0 },
            };
            private readonly Dictionary<int, int> knapsack10_5 = new()
            {
                { 4, 2 },
                { 2, 1 },
                { 1, 1 },
            };

            [Test]
            public void MinusOne()
            {
                Assert.AreEqual(knapsack0, PTUtils.Knapsack(set, -1, overloadKnapsack: true));
            }

            [Test]
            public void Zero()
            {
                Assert.AreEqual(knapsack0, PTUtils.Knapsack(set, 0, overloadKnapsack: true));
            }

            [Test]
            public void PointFive()
            {
                Assert.AreEqual(knapsack0, PTUtils.Knapsack(set, 0.5f, overloadKnapsack: true));
            }

            [Test]
            public void One()
            {
                Assert.AreEqual(knapsack0, PTUtils.Knapsack(set, 1, overloadKnapsack: true));
            }

            [Test]
            public void OnePointFive()
            {
                Assert.AreEqual(knapsack1_5, PTUtils.Knapsack(set, 1.5f, overloadKnapsack: true));
            }

            [Test]
            public void Two()
            {
                Assert.AreEqual(knapsack2, PTUtils.Knapsack(set, 2, overloadKnapsack: true));
            }

            [Test]
            public void TwoPointFive()
            {
                Assert.AreEqual(knapsack3, PTUtils.Knapsack(set, 2.5f, overloadKnapsack: true));
            }

            [Test]
            public void Three()
            {
                Assert.AreEqual(knapsack3, PTUtils.Knapsack(set, 3, overloadKnapsack: true));
            }

            [Test]
            public void ThreePointFive()
            {
                Assert.AreEqual(knapsack3_5, PTUtils.Knapsack(set, 3.5f, overloadKnapsack: true));
            }

            [Test]
            public void Ten()
            {
                Assert.AreEqual(knapsack10, PTUtils.Knapsack(set, 10, overloadKnapsack: true));
            }

            [Test]
            public void TenPointFive()
            {
                Assert.AreEqual(knapsack10_5, PTUtils.Knapsack(set, 10.5f, overloadKnapsack: true));
            }
        }
    }
}

using System.Collections.Generic;
using NUnit.Framework;
using ProceduralToolkit.Skeleton;
using UnityEngine;

namespace ProceduralToolkit.Tests
{
    public class StraightSkeletonTest : TestBase
    {
        #region Square

        private List<Vector2> square = new List<Vector2>
        {
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(1, -1),
        };
        private List<List<Vector2>> squareSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(-1, 1),
                new Vector2(1, 1),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(0, 0),
            },
        };

        #endregion Square

        [Test]
        public void Square()
        {
            TestEquality(square, squareSkeleton);
        }

        #region Rectangle

        private List<Vector2> rectangle = new List<Vector2>
        {
            new Vector2(-2, -1),
            new Vector2(-2, 1),
            new Vector2(2, 1),
            new Vector2(2, -1),
        };
        private List<List<Vector2>> rectangleSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-2, -1),
                new Vector2(-2, 1),
                new Vector2(-1, 0),
            },
            new List<Vector2>
            {
                new Vector2(-2, 1),
                new Vector2(2, 1),
                new Vector2(1, 0),
                new Vector2(-1, 0),
            },
            new List<Vector2>
            {
                new Vector2(2, 1),
                new Vector2(2, -1),
                new Vector2(1, 0),
            },
            new List<Vector2>
            {
                new Vector2(2, -1),
                new Vector2(-2, -1),
                new Vector2(-1, 0),
                new Vector2(1, 0),
            },
        };

        #endregion Rectangle

        [Test]
        public void Rectangle()
        {
            TestEquality(rectangle, rectangleSkeleton);
        }

        #region ShapeL

        private List<Vector2> shapeL = new List<Vector2>
        {
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(0, 1),
            new Vector2(0, 0.5f),
            new Vector2(1, 0.5f),
            new Vector2(1, -1),
        };
        private List<List<Vector2>> shapeLSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.5f, 0),
                new Vector2(-0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 1),
                new Vector2(0, 1),
                new Vector2(-0.5f, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(0, 0.5f),
                new Vector2(-0.5f, 0),
                new Vector2(-0.5f, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(0, 0.5f),
                new Vector2(1, 0.5f),
                new Vector2(0.25f, -0.25f),
                new Vector2(-0.25f, -0.25f),
                new Vector2(-0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, 0.5f),
                new Vector2(1, -1),
                new Vector2(0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-0.25f, -0.25f),
                new Vector2(0.25f, -0.25f),
            },
        };

        #endregion ShapeL

        [Test]
        public void ShapeL()
        {
            TestEquality(shapeL, shapeLSkeleton);
        }

        #region ShapeP

        private List<Vector2> shapeP = new List<Vector2>
        {
            new Vector2(-1, -1),
            new Vector2(-1, 0.5f),
            new Vector2(0, 0.5f),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, -1),
        };
        private List<List<Vector2>> shapePSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 0.5f),
                new Vector2(-0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 0.5f),
                new Vector2(0, 0.5f),
                new Vector2(0.5f, 0),
                new Vector2(0.25f, -0.25f),
                new Vector2(-0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(0, 0.5f),
                new Vector2(0, 1),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(0.5f, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(0.25f, -0.25f),
                new Vector2(0.5f, 0),
                new Vector2(0.5f, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-0.25f, -0.25f),
                new Vector2(0.25f, -0.25f),
            },
        };

        #endregion ShapeP

        [Test]
        public void ShapeP()
        {
            TestEquality(shapeP, shapePSkeleton);
        }

        #region ShapeBigT

        private List<Vector2> shapeBigT = new List<Vector2>
        {
            new Vector2(-1, -1),
            new Vector2(-1, 0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(-0.5f, 1),
            new Vector2(0.5f, 1),
            new Vector2(0.5f, 0.5f),
            new Vector2(1, 0.5f),
            new Vector2(1, -1),
        };
        private List<List<Vector2>> shapeBigTSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 0.5f),
                new Vector2(-0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 0.5f),
                new Vector2(-0.5f, 0.5f),
                new Vector2(0, 0),
                new Vector2(0, -0.25f),
                new Vector2(-0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.5f, 1),
                new Vector2(0, 0.5f),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 1),
                new Vector2(0.5f, 1),
                new Vector2(0, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 1),
                new Vector2(0.5f, 0.5f),
                new Vector2(0, 0),
                new Vector2(0, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 0.5f),
                new Vector2(1, 0.5f),
                new Vector2(0.25f, -0.25f),
                new Vector2(0, -0.25f),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, 0.5f),
                new Vector2(1, -1),
                new Vector2(0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-0.25f, -0.25f),
                new Vector2(0, -0.25f),
                new Vector2(0.25f, -0.25f),
            },
        };

        #endregion ShapeBigT

        [Test]
        public void ShapeBigT()
        {
            TestEquality(shapeBigT, shapeBigTSkeleton);
        }

        #region ShapeSmallT

        private List<Vector2> shapeSmallT = new List<Vector2>
        {
            new Vector2(-1, -0.5f),
            new Vector2(-1, 0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(-0.5f, 1),
            new Vector2(0.5f, 1),
            new Vector2(0.5f, 0.5f),
            new Vector2(1, 0.5f),
            new Vector2(1, -0.5f),
        };
        private List<List<Vector2>> shapeSmallTSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -0.5f),
                new Vector2(-1, 0.5f),
                new Vector2(-0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(-1, 0.5f),
                new Vector2(-0.5f, 0.5f),
                new Vector2(0, 0),
                new Vector2(-0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.5f, 1),
                new Vector2(0, 0.5f),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 1),
                new Vector2(0.5f, 1),
                new Vector2(0, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 1),
                new Vector2(0.5f, 0.5f),
                new Vector2(0, 0),
                new Vector2(0, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 0.5f),
                new Vector2(1, 0.5f),
                new Vector2(0.5f, 0),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, 0.5f),
                new Vector2(1, -0.5f),
                new Vector2(0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, -0.5f),
                new Vector2(-1, -0.5f),
                new Vector2(-0.5f, 0),
                new Vector2(0, 0),
                new Vector2(0.5f, 0),
            },
        };

        #endregion ShapeSmallT

        [Test]
        public void ShapeSmallT()
        {
            TestEquality(shapeSmallT, shapeSmallTSkeleton);
        }

        #region Cross

        private List<Vector2> cross = new List<Vector2>
        {
            new Vector2(-1, -0.5f),
            new Vector2(-1, 0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(-0.5f, 1),
            new Vector2(0.5f, 1),
            new Vector2(0.5f, 0.5f),
            new Vector2(1, 0.5f),
            new Vector2(1, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(0.5f, -1),
            new Vector2(-0.5f, -1),
            new Vector2(-0.5f, -0.5f),
        };
        private List<List<Vector2>> crossSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -0.5f),
                new Vector2(-1, 0.5f),
                new Vector2(-0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(-1, 0.5f),
                new Vector2(-0.5f, 0.5f),
                new Vector2(0, 0),
                new Vector2(-0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.5f, 1),
                new Vector2(0, 0.5f),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 1),
                new Vector2(0.5f, 1),
                new Vector2(0, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 1),
                new Vector2(0.5f, 0.5f),
                new Vector2(0, 0),
                new Vector2(0, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 0.5f),
                new Vector2(1, 0.5f),
                new Vector2(0.5f, 0),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, 0.5f),
                new Vector2(1, -0.5f),
                new Vector2(0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, -0.5f),
                new Vector2(0.5f, -0.5f),
                new Vector2(0, 0),
                new Vector2(0.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, -0.5f),
                new Vector2(0.5f, -1),
                new Vector2(0, -0.5f),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, -1),
                new Vector2(-0.5f, -1),
                new Vector2(0, -0.5f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, -1),
                new Vector2(-0.5f, -0.5f),
                new Vector2(0, 0),
                new Vector2(0, -0.5f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, -0.5f),
                new Vector2(-1, -0.5f),
                new Vector2(-0.5f, 0),
                new Vector2(0, 0),
            },
        };

        #endregion Cross

        [Test]
        public void Cross()
        {
            TestEquality(cross, crossSkeleton);
        }

        #region ShapeBigH

        private List<Vector2> shapeBigH = new List<Vector2>
        {
            new Vector2(-0.5f, -0.5f),
            new Vector2(-0.5f, -1),
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(-0.5f, 1),
            new Vector2(-0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 1),
            new Vector2(1, 1),
            new Vector2(1, -1),
            new Vector2(0.5f, -1),
            new Vector2(0.5f, -0.5f),
        };
        private List<List<Vector2>> shapeBigHSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-0.5f, -0.5f),
                new Vector2(-0.5f, -1),
                new Vector2(-0.75f, -0.75f),
                new Vector2(-0.75f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, -1),
                new Vector2(-1, -1),
                new Vector2(-0.75f, -0.75f),
            },
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(-0.75f, 0.75f),
                new Vector2(-0.75f, 0.25f),
                new Vector2(-0.5f, 0),
                new Vector2(-0.75f, -0.25f),
                new Vector2(-0.75f, -0.75f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 1),
                new Vector2(-0.5f, 1),
                new Vector2(-0.75f, 0.75f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 1),
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.75f, 0.25f),
                new Vector2(-0.75f, 0.75f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.75f, 0.25f),
                new Vector2(0.5f, 0),
                new Vector2(-0.5f, 0),
                new Vector2(-0.75f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 1),
                new Vector2(0.75f, 0.75f),
                new Vector2(0.75f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 1),
                new Vector2(1, 1),
                new Vector2(0.75f, 0.75f),
            },
            new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(0.75f, -0.75f),
                new Vector2(0.75f, -0.25f),
                new Vector2(0.5f, 0),
                new Vector2(0.75f, 0.25f),
                new Vector2(0.75f, 0.75f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(0.5f, -1),
                new Vector2(0.75f, -0.75f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, -1),
                new Vector2(0.5f, -0.5f),
                new Vector2(0.75f, -0.25f),
                new Vector2(0.75f, -0.75f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, -0.5f),
                new Vector2(-0.5f, -0.5f),
                new Vector2(-0.75f, -0.25f),
                new Vector2(-0.5f, 0),
                new Vector2(0.5f, 0),
                new Vector2(0.75f, -0.25f),
            },
        };

        #endregion ShapeBigH

        [Test]
        public void ShapeBigH()
        {
            TestEquality(shapeBigH, shapeBigHSkeleton);
        }

        #region ShapeSmallH

        private List<Vector2> shapeSmallH = new List<Vector2>
        {
            new Vector2(-0.5f, 0),
            new Vector2(-0.5f, -0.5f),
            new Vector2(-1, -0.5f),
            new Vector2(-1, 1),
            new Vector2(-0.5f, 1),
            new Vector2(-0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 1),
            new Vector2(1, 1),
            new Vector2(1, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(0.5f, 0),
        };
        private List<List<Vector2>> shapeSmallHSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-0.5f, 0),
                new Vector2(-0.5f, -0.5f),
                new Vector2(-0.75f, -0.25f),
                new Vector2(-0.75f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, -0.5f),
                new Vector2(-1, -0.5f),
                new Vector2(-0.75f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1, -0.5f),
                new Vector2(-1, 1),
                new Vector2(-0.75f, 0.75f),
                new Vector2(-0.75f, 0.25f),
                new Vector2(-0.75f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 1),
                new Vector2(-0.5f, 1),
                new Vector2(-0.75f, 0.75f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 1),
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.75f, 0.25f),
                new Vector2(-0.75f, 0.75f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.75f, 0.25f),
                new Vector2(-0.75f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 1),
                new Vector2(0.75f, 0.75f),
                new Vector2(0.75f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 1),
                new Vector2(1, 1),
                new Vector2(0.75f, 0.75f),
            },
            new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -0.5f),
                new Vector2(0.75f, -0.25f),
                new Vector2(0.75f, 0.25f),
                new Vector2(0.75f, 0.75f),
            },
            new List<Vector2>
            {
                new Vector2(1, -0.5f),
                new Vector2(0.5f, -0.5f),
                new Vector2(0.75f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, -0.5f),
                new Vector2(0.5f, 0),
                new Vector2(0.75f, 0.25f),
                new Vector2(0.75f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, 0),
                new Vector2(-0.5f, 0),
                new Vector2(-0.75f, 0.25f),
                new Vector2(0.75f, 0.25f),
            },
        };

        #endregion ShapeSmallH

        [Test]
        public void ShapeSmallH()
        {
            TestEquality(shapeSmallH, shapeSmallHSkeleton);
        }

        #region ShapeSmall8

        private List<Vector2> shapeSmall8 = new List<Vector2>
        {
            new Vector2(-1, -1),
            new Vector2(-1, 0.5f),
            new Vector2(-0.5f, 0.5f),
            new Vector2(-0.5f, 1),
            new Vector2(1, 1),
            new Vector2(1, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(0.5f, -1),
        };
        private List<List<Vector2>> shapeSmall8Skeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 0.5f),
                new Vector2(-0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 0.5f),
                new Vector2(-0.5f, 0.5f),
                new Vector2(0, 0),
                new Vector2(-0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.5f, 1),
                new Vector2(0.25f, 0.25f),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(-0.5f, 1),
                new Vector2(1, 1),
                new Vector2(0.25f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -0.5f),
                new Vector2(0.25f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(1, -0.5f),
                new Vector2(0.5f, -0.5f),
                new Vector2(0, 0),
                new Vector2(0.25f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, -0.5f),
                new Vector2(0.5f, -1),
                new Vector2(-0.25f, -0.25f),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(0.5f, -1),
                new Vector2(-1, -1),
                new Vector2(-0.25f, -0.25f),
            },
        };

        #endregion ShapeSmall8

        [Test]
        public void ShapeSmall8()
        {
            TestEquality(shapeSmall8, shapeSmall8Skeleton);
        }

        #region ShapeBig8

        private List<Vector2> shapeBig8 = new List<Vector2>
        {
            new Vector2(-1.5f, -1),
            new Vector2(-1.5f, 0.5f),
            new Vector2(-1, 0.5f),
            new Vector2(-1, 1),
            new Vector2(1.5f, 1),
            new Vector2(1.5f, -0.5f),
            new Vector2(1, -0.5f),
            new Vector2(1, -1),
        };
        private List<List<Vector2>> shapeBig8Skeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1.5f, -1),
                new Vector2(-1.5f, 0.5f),
                new Vector2(-0.75f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1.5f, 0.5f),
                new Vector2(-1, 0.5f),
                new Vector2(-0.25f, -0.25f),
                new Vector2(-0.75f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 0.5f),
                new Vector2(-1, 1),
                new Vector2(0, 0),
                new Vector2(-0.25f, -0.25f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 1),
                new Vector2(1.5f, 1),
                new Vector2(0.75f, 0.25f),
                new Vector2(0.25f, 0.25f),
                new Vector2(0, 0),
            },
            new List<Vector2>
            {
                new Vector2(1.5f, 1),
                new Vector2(1.5f, -0.5f),
                new Vector2(0.75f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(1.5f, -0.5f),
                new Vector2(1, -0.5f),
                new Vector2(0.25f, 0.25f),
                new Vector2(0.75f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(1, -0.5f),
                new Vector2(1, -1),
                new Vector2(0, 0),
                new Vector2(0.25f, 0.25f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(-1.5f, -1),
                new Vector2(-0.75f, -0.25f),
                new Vector2(-0.25f, -0.25f),
                new Vector2(0, 0),
            },
        };

        #endregion ShapeBig8

        [Test]
        public void ShapeBig8()
        {
            TestEquality(shapeBig8, shapeBig8Skeleton);
        }

        #region Stairs

        private List<Vector2> stairs = new List<Vector2>
        {
            new Vector2(-2, -1.5f),
            new Vector2(-2, 1.5f),
            new Vector2(-1, 1.5f),
            new Vector2(-1, 0.5f),
            new Vector2(1, 0.5f),
            new Vector2(1, -0.5f),
            new Vector2(2, -0.5f),
            new Vector2(2, -1.5f),
        };
        private List<List<Vector2>> stairsSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-2, -1.5f),
                new Vector2(-2, 1.5f),
                new Vector2(-1.5f, 1),
                new Vector2(-1.5f, 0),
                new Vector2(-1, -0.5f),
            },
            new List<Vector2>
            {
                new Vector2(-2, 1.5f),
                new Vector2(-1, 1.5f),
                new Vector2(-1.5f, 1),
            },
            new List<Vector2>
            {
                new Vector2(-1, 1.5f),
                new Vector2(-1, 0.5f),
                new Vector2(-1.5f, 0),
                new Vector2(-1.5f, 1),
            },
            new List<Vector2>
            {
                new Vector2(-1, 0.5f),
                new Vector2(1, 0.5f),
                new Vector2(0, -0.5f),
                new Vector2(-1, -0.5f),
                new Vector2(-1.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(1, 0.5f),
                new Vector2(1, -0.5f),
                new Vector2(0.5f, -1),
                new Vector2(0, -0.5f),
            },
            new List<Vector2>
            {
                new Vector2(1, -0.5f),
                new Vector2(2, -0.5f),
                new Vector2(1.5f, -1),
                new Vector2(0.5f, -1),
            },
            new List<Vector2>
            {
                new Vector2(2, -0.5f),
                new Vector2(2, -1.5f),
                new Vector2(1.5f, -1),
            },
            new List<Vector2>
            {
                new Vector2(2, -1.5f),
                new Vector2(-2, -1.5f),
                new Vector2(-1, -0.5f),
                new Vector2(0, -0.5f),
                new Vector2(0.5f, -1),
                new Vector2(1.5f, -1),
            },
        };

        #endregion Stairs

        [Test]
        public void Stairs()
        {
            TestEquality(stairs, stairsSkeleton);
        }

        #region Bracket

        private List<Vector2> bracket = new List<Vector2>
        {
            new Vector2(-3, -1.5f),
            new Vector2(-3, 1.5f),
            new Vector2(3, 1.5f),
            new Vector2(3, -1.5f),
            new Vector2(0, -1.5f),
            new Vector2(0, -0.5f),
            new Vector2(-2, -0.5f),
            new Vector2(-2, -1.5f),
        };
        private List<List<Vector2>> bracketSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-3, -1.5f),
                new Vector2(-3, 1.5f),
                new Vector2(-2, 0.5f),
                new Vector2(-2.5f, 0),
                new Vector2(-2.5f, -1),
            },
            new List<Vector2>
            {
                new Vector2(-3, 1.5f),
                new Vector2(3, 1.5f),
                new Vector2(1.5f, 0),
                new Vector2(1, 0.5f),
                new Vector2(-2, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(3, 1.5f),
                new Vector2(3, -1.5f),
                new Vector2(1.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(3, -1.5f),
                new Vector2(0, -1.5f),
                new Vector2(1.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(0, -1.5f),
                new Vector2(0, -0.5f),
                new Vector2(1, 0.5f),
                new Vector2(1.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(0, -0.5f),
                new Vector2(-2, -0.5f),
                new Vector2(-2.5f, 0),
                new Vector2(-2, 0.5f),
                new Vector2(1, 0.5f),
            },
            new List<Vector2>
            {
                new Vector2(-2, -0.5f),
                new Vector2(-2, -1.5f),
                new Vector2(-2.5f, -1),
                new Vector2(-2.5f, 0),
            },
            new List<Vector2>
            {
                new Vector2(-2, -1.5f),
                new Vector2(-3, -1.5f),
                new Vector2(-2.5f, -1),
            },
        };

        #endregion Bracket

        [Test]
        public void Bracket()
        {
            TestEquality(bracket, bracketSkeleton);
        }

        #region Bowtie

        private List<Vector2> bowtie = new List<Vector2>
        {
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, -1),
        };
        private List<List<Vector2>> bowtieSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(-0.414213598f, -0.414213598f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 1),
                new Vector2(0, 0),
                new Vector2(0, -0.585786402f),
                new Vector2(-0.414213687f, -0.414213359f),
            },
            new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(0.414213568f, -0.414213449f),
                new Vector2(0, -0.585786402f),
            },
            new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(0.414213568f, -0.414213449f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(-1, -1),
                new Vector2(-0.414213598f, -0.414213598f),
                new Vector2(0, -0.585786402f),
                new Vector2(0.414213568f, -0.414213568f),
            },
        };

        #endregion Bowtie

        [Test]
        public void Bowtie()
        {
            TestEquality(bowtie, bowtieSkeleton);
        }

        #region BowtieCollinear

        private List<Vector2> bowtieCollinear = new List<Vector2>
        {
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, -1),
            new Vector2(0, -1),
        };
        private List<List<Vector2>> bowtieCollinearSkeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1, 1),
                new Vector2(-0.414213598f, -0.414213598f),
            },
            new List<Vector2>
            {
                new Vector2(-1, 1),
                new Vector2(0, 0),
                new Vector2(0, -0.585786283f),
                new Vector2(-0.414213717f, -0.414213359f),
            },
            new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(1, 1),
                new Vector2(0.414213538f, -0.414213419f),
                new Vector2(0, -0.585786283f),
            },
            new List<Vector2>
            {
                new Vector2(1, 1),
                new Vector2(1, -1),
                new Vector2(0.414213538f, -0.414213419f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(0, -0.585786462f),
                new Vector2(0.414213598f, -0.414213598f),
            },
            new List<Vector2>
            {
                new Vector2(0, -1),
                new Vector2(-1, -1),
                new Vector2(-0.414213598f, -0.414213598f),
                new Vector2(0, -0.585786462f),
            },
        };

        #endregion BowtieCollinear

        [Test]
        public void BowtieCollinear()
        {
            TestEquality(bowtieCollinear, bowtieCollinearSkeleton);
        }

        #region ReflexEdgeEvent1

        private List<Vector2> reflexEdgeEvent1 = new List<Vector2>
        {
            new Vector2(-2, -2),
            new Vector2(-2, 2),
            new Vector2(2, 2),
            new Vector2(2, -1),
            new Vector2(1, -1),
        };
        private List<List<Vector2>> reflexEdgeEvent1Skeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-2, -2),
                new Vector2(-2, 2),
                new Vector2(-0.324555397f, 0.324555397f),
            },
            new List<Vector2>
            {
                new Vector2(-2, 2),
                new Vector2(2, 2),
                new Vector2(0.450296491f, 0.450296491f),
                new Vector2(-0.324555397f, 0.324555397f),
            },
            new List<Vector2>
            {
                new Vector2(2, 2),
                new Vector2(2, -1),
                new Vector2(0.80628705f, 0.19371295f),
                new Vector2(0.450296491f, 0.450296491f),
            },
            new List<Vector2>
            {
                new Vector2(2, -1),
                new Vector2(1, -1),
                new Vector2(0.80628705f, 0.19371295f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(-2, -2),
                new Vector2(-0.324555397f, 0.324555397f),
                new Vector2(0.450296521f, 0.450296432f),
                new Vector2(0.80628705f, 0.193712831f),
            },
        };

        #endregion ReflexEdgeEvent1

        [Test]
        public void ReflexEdgeEvent1()
        {
            TestEquality(reflexEdgeEvent1, reflexEdgeEvent1Skeleton);
        }

        #region ReflexEdgeEvent2

        private List<Vector2> reflexEdgeEvent2 = new List<Vector2>
        {
            new Vector2(-2, -2),
            new Vector2(-2, 2),
            new Vector2(2, 2),
            new Vector2(2, -1),
            new Vector2(1, -1),
            new Vector2(-1, -2),
        };
        private List<List<Vector2>> reflexEdgeEvent2Skeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-2, -2),
                new Vector2(-2, 2),
                new Vector2(-0.281152904f, 0.281152904f),
                new Vector2(-1.19098306f, -1.19098306f),
            },
            new List<Vector2>
            {
                new Vector2(-2, 2),
                new Vector2(2, 2),
                new Vector2(0.454915047f, 0.454915047f),
                new Vector2(-0.281152904f, 0.281152904f),
            },
            new List<Vector2>
            {
                new Vector2(2, 2),
                new Vector2(2, -1),
                new Vector2(0.690982878f, 0.309017122f),
                new Vector2(0.454915047f, 0.454915047f),
            },
            new List<Vector2>
            {
                new Vector2(2, -1),
                new Vector2(1, -1),
                new Vector2(0.690982878f, 0.309017122f),
            },
            new List<Vector2>
            {
                new Vector2(1, -1),
                new Vector2(-1, -2),
                new Vector2(-1.19098294f, -1.19098306f),
                new Vector2(-0.281152844f, 0.281152964f),
                new Vector2(0.454915047f, 0.454915047f),
                new Vector2(0.690982878f, 0.309017122f),
            },
            new List<Vector2>
            {
                new Vector2(-1, -2),
                new Vector2(-2, -2),
                new Vector2(-1.19098294f, -1.19098306f),
            },
        };

        #endregion ReflexEdgeEvent2

        [Test]
        public void ReflexEdgeEvent2()
        {
            TestEquality(reflexEdgeEvent2, reflexEdgeEvent2Skeleton);
        }

        #region Degenerate1

        private List<Vector2> degenerate1 = new List<Vector2>
        {
            new Vector2(-2, -2),
            new Vector2(-2, 2),
            new Vector2(2, 2),
            new Vector2(3, -1),
            new Vector2(1, -0.99f),
        };
        private List<List<Vector2>> degenerate1Skeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-2, -2),
                new Vector2(-2, 2),
                new Vector2(-0.32763195f, 0.32763195f),
            },
            new List<Vector2>
            {
                new Vector2(-2, 2),
                new Vector2(2, 2),
                new Vector2(0.92290628f, 0.505612254f),
                new Vector2(0.758832574f, 0.505612254f),
                new Vector2(-0.32763195f, 0.32763195f),
            },
            new List<Vector2>
            {
                new Vector2(2, 2),
                new Vector2(3, -1),
                new Vector2(0.923180342f, 0.504790425f),
                new Vector2(0.92290628f, 0.505612254f),
            },
            new List<Vector2>
            {
                new Vector2(3, -1),
                new Vector2(1, -0.99f),
                new Vector2(0.758832574f, 0.505612254f),
                new Vector2(0.923180342f, 0.504790425f),
            },
            new List<Vector2>
            {
                new Vector2(1, -0.99f),
                new Vector2(-2, -2),
                new Vector2(-0.32763195f, 0.32763195f),
                new Vector2(0.758832574f, 0.505612254f),
            },
        };

        #endregion Degenerate1

        [Test]
        public void Degenerate1()
        {
            TestEquality(degenerate1, degenerate1Skeleton);
        }

        #region Degenerate2

        private List<Vector2> degenerate2 = new List<Vector2>
        {
            new Vector2(-1, -1),
            new Vector2(-1.5f, 1),
            new Vector2(0.4f, 1),
            new Vector2(2, 0.3f),
            new Vector2(1.5f, -1),
            new Vector2(0.3f, -0.4f),
        };
        private List<List<Vector2>> degenerate2Skeleton = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(-1, -1),
                new Vector2(-1.5f, 1),
                new Vector2(-0.438860446f, 0.171487257f),
            },
            new List<Vector2>
            {
                new Vector2(-1.5f, 1),
                new Vector2(0.4f, 1),
                new Vector2(-0.438860536f, 0.171487346f),
            },
            new List<Vector2>
            {
                new Vector2(0.4f, 1),
                new Vector2(2, 0.3f),
                new Vector2(1.13577867f, -0.0609190166f),
            },
            new List<Vector2>
            {
                new Vector2(2, 0.3f),
                new Vector2(1.5f, -1),
                new Vector2(1.13577867f, -0.0609190166f),
            },
            new List<Vector2>
            {
                new Vector2(1.5f, -1),
                new Vector2(0.3f, -0.4f),
                new Vector2(1.13577878f, -0.0609191209f),
            },
            new List<Vector2>
            {
                new Vector2(0.3f, -0.4f),
                new Vector2(-1, -1),
                new Vector2(-0.438860446f, 0.171487257f),
            },
        };

        #endregion Degenerate2

        [Test]
        public void Degenerate2()
        {
            TestEquality(degenerate2, degenerate2Skeleton);
        }

        [Test]
        public void RegularPolygons()
        {
            for (int vertices = 3; vertices < 10; vertices++)
            {
                var polygon = Geometry.Polygon2(vertices, 1);
                var polygonSkeleton = new List<List<Vector2>>();
                for (int i = 0; i < polygon.Count; i++)
                {
                    polygonSkeleton.Add(new List<Vector2>
                    {
                        polygon[i],
                        polygon.GetLooped(i + 1),
                        new Vector2(),
                    });
                }
                TestEquality(polygon, polygonSkeleton);
            }
        }

        [Test]
        public void StarPolygons()
        {
            for (int vertices = 3; vertices < 10; vertices++)
            {
                var polygon = Geometry.StarPolygon2(vertices, 0.5f, 1);
                var polygonSkeleton = new List<List<Vector2>>();
                for (int i = 0; i < polygon.Count; i++)
                {
                    polygonSkeleton.Add(new List<Vector2>
                    {
                        polygon[i],
                        polygon.GetLooped(i + 1),
                        new Vector2(),
                    });
                }
                TestEquality(polygon, polygonSkeleton);
            }
        }

        private static void TestEquality(List<Vector2> vertices, List<List<Vector2>> expectedSkeleton)
        {
            var generator = new StraightSkeletonGenerator();
            var skeleton = generator.Generate(vertices);

            Assert.AreEqual(expectedSkeleton.Count, skeleton.polygons.Count);

            for (int polygonIndex = 0; polygonIndex < expectedSkeleton.Count; polygonIndex++)
            {
                var expectedPolygon = expectedSkeleton[polygonIndex];
                var polygon = skeleton.polygons[polygonIndex];

                Assert.AreEqual(expectedPolygon.Count, polygon.Count);

                for (int vertexIndex = 0; vertexIndex < expectedPolygon.Count; vertexIndex++)
                {
                    Vector2 expectedVertex = expectedPolygon[vertexIndex];
                    Vector2 vertex = polygon[vertexIndex];

                    AreEqual(vertex, expectedVertex);
                }
            }
        }
    }
}

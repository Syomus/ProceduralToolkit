using Unity.Collections;
using Unity.Jobs;

namespace ProceduralToolkit.MarchingSquares
{
    public struct MarchingSquaresJob<T> : IJob
        where T : struct, IMarchingSquaresData
    {
        public Contours contours;

        private T data;

        public MarchingSquaresJob(T data)
        {
            this.data = data;
            contours = new Contours(data.dataLengthX, data.dataLengthY, data.useInterpolation);
        }

        public void Execute()
        {
            var binaryIndex = new NativeArray2D<bool>(data.dataLengthX, data.dataLengthY, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            for (int y = 0; y < data.dataLengthY; y++)
            {
                for (int x = 0; x < data.dataLengthX; x++)
                {
                    binaryIndex[x, y] = data.TestValue(x, y);
                }
            }

            for (int y = 0; y < contours.squaresLengthY; y++)
            {
                for (int x = 0; x < contours.squaresLengthX; x++)
                {
                    byte square = 0;
                    if (binaryIndex[x, y])
                    {
                        square |= 1;
                    }
                    if (binaryIndex[x, y + 1])
                    {
                        square |= 2;
                    }
                    if (binaryIndex[x + 1, y + 1])
                    {
                        square |= 4;
                    }
                    if (binaryIndex[x + 1, y])
                    {
                        square |= 8;
                    }
                    if (square == 5 || square == 10)
                    {
                        if (data.TestAverage(x, y))
                        {
                            square |= 16;
                        }
                    }
                    if (data.useInterpolation)
                    {
                        SetupCorners(x, y, square);
                    }
                    contours.SetSquare(x, y, square);
                }
            }

            binaryIndex.Dispose();
        }

        private void SetupCorners(int x, int y, byte square)
        {
            switch (square)
            {
                case 0:
                    break;

                case 1: // ▖
                    SetupBottomSide(x, y);
                    SetupLeftSide(x, y);
                    break;
                case 2: // ▘
                    SetupLeftSide(x, y);
                    SetupTopSide(x, y);
                    break;
                case 4: // ▝
                    SetupTopSide(x, y);
                    SetupRightSide(x, y);
                    break;
                case 8: // ▗
                    SetupBottomSide(x, y);
                    SetupRightSide(x, y);
                    break;

                case 3: // ▌
                    SetupBottomSide(x, y);
                    SetupTopSide(x, y);
                    break;
                case 6: // ▀
                    SetupLeftSide(x, y);
                    SetupRightSide(x, y);
                    break;
                case 12: // ▐
                    SetupTopSide(x, y);
                    SetupBottomSide(x, y);
                    break;
                case 9: // ▄
                    SetupLeftSide(x, y);
                    SetupRightSide(x, y);
                    break;

                case 5: // ▞
                case 21: // ▞ filled center
                    SetupBottomSide(x, y);
                    SetupLeftSide(x, y);
                    SetupTopSide(x, y);
                    SetupRightSide(x, y);
                    break;
                case 10: // ▚
                case 26: // ▚ filled center
                    SetupTopSide(x, y);
                    SetupBottomSide(x, y);
                    SetupLeftSide(x, y);
                    SetupRightSide(x, y);
                    break;

                case 7: // ▛
                    SetupBottomSide(x, y);
                    SetupRightSide(x, y);
                    break;
                case 14: // ▜
                    SetupBottomSide(x, y);
                    SetupLeftSide(x, y);
                    break;
                case 13: // ▟
                    SetupTopSide(x, y);
                    SetupLeftSide(x, y);
                    break;
                case 11: // ▙
                    SetupTopSide(x, y);
                    SetupRightSide(x, y);
                    break;

                case 15: // ⬛
                    break;
            }
        }

        private void SetupLeftSide(int x, int y)
        {
            if (!contours.HasValueLeftSide(x, y))
            {
                contours.SetLeftSide(x, y, data.InverseLerp(x, y, x, y + 1));
            }
        }

        private void SetupBottomSide(int x, int y)
        {
            if (!contours.HasValueBottomSide(x, y))
            {
                contours.SetBottomSide(x, y, data.InverseLerp(x, y, x + 1, y));
            }
        }

        private void SetupRightSide(int x, int y)
        {
            if (!contours.HasValueRightSide(x, y))
            {
                contours.SetRightSide(x, y, data.InverseLerp(x + 1, y, x + 1, y + 1));
            }
        }

        private void SetupTopSide(int x, int y)
        {
            if (!contours.HasValueTopSide(x, y))
            {
                contours.SetTopSide(x, y, data.InverseLerp(x, y + 1, x + 1, y + 1));
            }
        }
    }
}

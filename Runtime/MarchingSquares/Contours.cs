using System;
using Unity.Collections;

namespace ProceduralToolkit.MarchingSquares
{
    public struct Contours : IDisposable
    {
        /// <summary>
        /// 2━3
        /// ┃ ┃
        /// 1━4
        /// 
        /// ▖ 0001 - 1
        /// ▘ 0010 - 2
        /// ▝ 0100 - 4
        /// ▗ 1000 - 8
        /// 
        /// ▌ 0011 - 3
        /// ▀ 0110 - 6
        /// ▐ 1100 - 12
        /// ▄ 1001 - 9
        /// 
        /// ▞ 0101 - 5  | 10101 - 21 filled center
        /// ▚ 1010 - 10 | 11010 - 26 filled center
        /// 
        /// ▛ 0111 - 7
        /// ▜ 1110 - 14
        /// ▟ 1101 - 13
        /// ▙ 1011 - 11
        /// 
        /// ⬛ 1111 - 15 
        /// </summary>
        private NativeArray2D<byte> squares;
        public int squaresLengthX => squares.LengthX;
        public int squaresLengthY => squares.LengthY;

        public readonly bool useInterpolation;
        private NativeArray2D<float> sides;

        public Contours(int dataLengthX, int dataLengthY, bool useInterpolation)
        {
            squares = new NativeArray2D<byte>(dataLengthX - 1, dataLengthY - 1, Allocator.Persistent);

            this.useInterpolation = useInterpolation;
            if (useInterpolation)
            {
                sides = new NativeArray2D<float>(dataLengthX*2, dataLengthY, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                for (int i = 0; i < sides.Length; i++)
                {
                    sides[i] = float.NaN;
                }
            }
            else
            {
                sides = new NativeArray2D<float>(0, 0, Allocator.Persistent);
            }
        }

        public void Dispose()
        {
            squares.Dispose();
            sides.Dispose();
        }

        public byte GetSquare(int x, int y)
        {
            return squares[x, y];
        }

        public void SetSquare(int x, int y, byte square)
        {
            squares[x, y] = square;
        }

        public float GetLeftSide(int x, int y)
        {
            return sides[2*x, y];
        }

        public float GetBottomSide(int x, int y)
        {
            return sides[2*x + 1, y];
        }

        public float GetRightSide(int x, int y)
        {
            return sides[2*x + 2, y];
        }

        public float GetTopSide(int x, int y)
        {
            return sides[2*x + 1, y + 1];
        }

        public bool HasValueLeftSide(int x, int y)
        {
            return !float.IsNaN(GetLeftSide(x, y));
        }

        public bool HasValueBottomSide(int x, int y)
        {
            return !float.IsNaN(GetBottomSide(x, y));
        }

        public bool HasValueRightSide(int x, int y)
        {
            return !float.IsNaN(GetRightSide(x, y));
        }

        public bool HasValueTopSide(int x, int y)
        {
            return !float.IsNaN(GetTopSide(x, y));
        }

        public void SetLeftSide(int x, int y, float value)
        {
            sides[2*x, y] = value;
        }

        public void SetRightSide(int x, int y, float value)
        {
            sides[2*x + 2, y] = value;
        }

        public void SetBottomSide(int x, int y, float value)
        {
            sides[2*x + 1, y] = value;
        }

        public void SetTopSide(int x, int y, float value)
        {
            sides[2*x + 1, y + 1] = value;
        }
    }
}

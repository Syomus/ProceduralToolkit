using Unity.Collections;

namespace ProceduralToolkit.MarchingSquares
{
    public struct Contours
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
        private NativeArray<byte> squares;
        public readonly int squaresLengthX;
        public readonly int squaresLengthY;

        public readonly bool useInterpolation;
        private NativeArray<float> sides;
        private readonly int sidesLengthX;

        public Contours(int dataLengthX, int dataLengthY, bool useInterpolation)
        {
            squaresLengthX = dataLengthX - 1;
            squaresLengthY = dataLengthY - 1;
            squares = new NativeArray<byte>(squaresLengthX*squaresLengthY, Allocator.Persistent);

            sidesLengthX = dataLengthX*2;
            this.useInterpolation = useInterpolation;
            if (useInterpolation)
            {
                sides = new NativeArray<float>(sidesLengthX*dataLengthY, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                for (int i = 0; i < sides.Length; i++)
                {
                    sides[i] = float.NaN;
                }
            }
            else
            {
                sides = new NativeArray<float>(0, Allocator.Persistent);
            }
        }

        public void Dispose()
        {
            squares.Dispose();
            sides.Dispose();
        }

        public byte GetSquare(int x, int y)
        {
            return squares.GetXY(x, y, squaresLengthX);
        }

        public void SetSquare(int x, int y, byte square)
        {
            squares.SetXY(x, y, squaresLengthX, square);
        }

        public float GetLeftSide(int x, int y)
        {
            return sides.GetXY(2*x, y, sidesLengthX);
        }

        public float GetBottomSide(int x, int y)
        {
            return sides.GetXY(2*x + 1, y, sidesLengthX);
        }

        public float GetRightSide(int x, int y)
        {
            return sides.GetXY(2*x + 2, y, sidesLengthX);
        }

        public float GetTopSide(int x, int y)
        {
            return sides.GetXY(2*x + 1, y + 1, sidesLengthX);
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
            sides.SetXY(2*x, y, sidesLengthX, value);
        }

        public void SetRightSide(int x, int y, float value)
        {
            sides.SetXY(2*x + 2, y, sidesLengthX, value);
        }

        public void SetBottomSide(int x, int y, float value)
        {
            sides.SetXY(2*x + 1, y, sidesLengthX, value);
        }

        public void SetTopSide(int x, int y, float value)
        {
            sides.SetXY(2*x + 1, y + 1, sidesLengthX, value);
        }
    }
}

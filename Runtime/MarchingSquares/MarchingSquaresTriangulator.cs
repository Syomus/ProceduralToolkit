using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.MarchingSquares
{
    public class MarchingSquaresTriangulator
    {
        private const float cornerWeight = 0.5f;

        private readonly float squareSize;
        private readonly Vector3 normal = Vector3.back;

        private Contours contours;
        private MeshDraft draft;

        public MarchingSquaresTriangulator(float squareSize)
        {
            this.squareSize = squareSize;
        }

        public MeshDraft Triangulate(Contours contours)
        {
            draft = new MeshDraft();
            Triangulate(contours, draft);
            return draft;
        }

        public void Triangulate(Contours contours, MeshDraft draft)
        {
            this.contours = contours;

            this.draft = draft;
            draft.Clear();

            for (int y = 0; y < contours.squaresLengthY; y++)
            {
                for (int x = 0; x < contours.squaresLengthX; x++)
                {
                    byte square = contours.GetSquare(x, y);
                    switch (square)
                    {
                        case 0:
                            break;

                        case 1: // ▖
                            AddBottomLeftTriangle(x, y);
                            break;
                        case 2: // ▘
                            AddTopLeftTriangle(x, y);
                            break;
                        case 4: // ▝
                            AddTopRightTriangle(x, y);
                            break;
                        case 8: // ▗
                            AddBottomRightTriangle(x, y);
                            break;

                        case 3: // ▌
                            AddLeftHalf(x, y);
                            break;
                        case 6: // ▀
                            AddTopHalf(x, y);
                            break;
                        case 12: // ▐
                            AddRightHalf(x, y);
                            break;
                        case 9: // ▄
                            AddBottomHalf(x, y);
                            break;

                        case 5: // ▞
                            AddBottomLeftTriangle(x, y);
                            AddTopRightTriangle(x, y);
                            break;
                        case 21: // ▞ filled center
                            AddBottomLeftHexagon(x, y);
                            break;
                        case 10: // ▚
                            AddTopLeftTriangle(x, y);
                            AddBottomRightTriangle(x, y);
                            break;
                        case 26: // ▚ filled center
                            AddTopLeftHexagon(x, y);
                            break;

                        case 7: // ▛
                            AddTopLeftPentagon(x, y);
                            break;
                        case 14: // ▜
                            AddTopRightPentagon(x, y);
                            break;
                        case 13: // ▟
                            AddBottomRightPentagon(x, y);
                            break;
                        case 11: // ▙
                            AddBottomLeftPentagon(x, y);
                            break;

                        case 15: // ⬛
                            int lastQuadIndex = x;
                            for (int i = x + 1; i < contours.squaresLengthX; i++)
                            {
                                if (contours.GetSquare(i, y) == 15)
                                {
                                    lastQuadIndex = i;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            AddQuad(x, lastQuadIndex, y);
                            x = lastQuadIndex;
                            break;
                    }
                }
            }
        }

        #region GetCornerPosition

        private Vector2 GetBottomLeftPosition(int x, int y)
        {
            return new Vector2(x*squareSize, y*squareSize);
        }

        private Vector2 GetTopLeftPosition(int x, int y)
        {
            return new Vector2(x*squareSize, (y + 1)*squareSize);
        }

        private Vector2 GetTopRightPosition(int x, int y)
        {
            return new Vector2((x + 1)*squareSize, (y + 1)*squareSize);
        }

        private Vector2 GetBottomRightPosition(int x, int y)
        {
            return new Vector2((x + 1)*squareSize, y*squareSize);
        }

        #endregion GetCornerPosition

        #region Triangles

        private void AddBottomLeftTriangle(int x, int y)
        {
            Vector2 bottomLeft = GetBottomLeftPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangle(
                    bottomLeft,
                    bottomLeft + Vector2.up*contours.GetLeftSide(x, y)*squareSize,
                    bottomLeft + Vector2.right*contours.GetBottomSide(x, y)*squareSize,
                    normal);
            }
            else
            {
                draft.AddTriangle(
                    bottomLeft,
                    bottomLeft + Vector2.up*cornerWeight*squareSize,
                    bottomLeft + Vector2.right*cornerWeight*squareSize,
                    normal);
            }
        }

        private void AddTopLeftTriangle(int x, int y)
        {
            Vector2 topLeft = GetTopLeftPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangle(
                    topLeft + Vector2.down*(1 - contours.GetLeftSide(x, y))*squareSize,
                    topLeft,
                    topLeft + Vector2.right*contours.GetTopSide(x, y)*squareSize,
                    normal);
            }
            else
            {
                draft.AddTriangle(
                    topLeft + Vector2.down*cornerWeight*squareSize,
                    topLeft,
                    topLeft + Vector2.right*cornerWeight*squareSize,
                    normal);
            }
        }

        private void AddTopRightTriangle(int x, int y)
        {
            Vector2 topRight = GetTopRightPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangle(
                    topRight + Vector2.left*(1 - contours.GetTopSide(x, y))*squareSize,
                    topRight,
                    topRight + Vector2.down*(1 - contours.GetRightSide(x, y))*squareSize,
                    normal);
            }
            else
            {
                draft.AddTriangle(
                    topRight + Vector2.left*cornerWeight*squareSize,
                    topRight,
                    topRight + Vector2.down*cornerWeight*squareSize,
                    normal);
            }
        }

        private void AddBottomRightTriangle(int x, int y)
        {
            Vector2 bottomRight = GetBottomRightPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangle(
                    bottomRight + Vector2.up*contours.GetRightSide(x, y)*squareSize,
                    bottomRight,
                    bottomRight + Vector2.left*(1 - contours.GetBottomSide(x, y))*squareSize,
                    normal);
            }
            else
            {
                draft.AddTriangle(
                    bottomRight + Vector2.up*cornerWeight*squareSize,
                    bottomRight,
                    bottomRight + Vector2.left*cornerWeight*squareSize,
                    normal);
            }
        }

        #endregion Triangles

        #region Halves

        private void AddLeftHalf(int x, int y)
        {
            Vector2 bottomLeft = GetBottomLeftPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddQuad(
                    bottomLeft,
                    bottomLeft + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*contours.GetTopSide(x, y)*squareSize + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*contours.GetBottomSide(x, y)*squareSize,
                    normal);
            }
            else
            {
                draft.AddQuad(
                    bottomLeft,
                    bottomLeft + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*cornerWeight*squareSize + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*cornerWeight*squareSize,
                    normal);
            }
        }

        private void AddTopHalf(int x, int y)
        {
            Vector2 topLeft = GetTopLeftPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddQuad(
                    topLeft + Vector2.down*(1 - contours.GetLeftSide(x, y))*squareSize,
                    topLeft,
                    topLeft + Vector2.right*squareSize,
                    topLeft + Vector2.right*squareSize + Vector2.down*(1 - contours.GetRightSide(x, y))*squareSize,
                    normal);
            }
            else
            {
                draft.AddQuad(
                    topLeft + Vector2.down*cornerWeight*squareSize,
                    topLeft,
                    topLeft + Vector2.right*squareSize,
                    topLeft + Vector2.right*squareSize + Vector2.down*cornerWeight*squareSize,
                    normal);
            }
        }

        private void AddRightHalf(int x, int y)
        {
            Vector2 topRight = GetTopRightPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddQuad(
                    topRight + Vector2.left*(1 - contours.GetTopSide(x, y))*squareSize,
                    topRight,
                    topRight + Vector2.down*squareSize,
                    topRight + Vector2.left*(1 - contours.GetBottomSide(x, y))*squareSize + Vector2.down*squareSize,
                    normal);
            }
            else
            {
                draft.AddQuad(
                    topRight + Vector2.left*cornerWeight*squareSize,
                    topRight,
                    topRight + Vector2.down*squareSize,
                    topRight + Vector2.left*cornerWeight*squareSize + Vector2.down*squareSize,
                    normal);
            }
        }

        private void AddBottomHalf(int x, int y)
        {
            Vector2 bottomRight = GetBottomRightPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddQuad(
                    bottomRight + Vector2.left*squareSize,
                    bottomRight + Vector2.left*squareSize + Vector2.up*contours.GetLeftSide(x, y)*squareSize,
                    bottomRight + Vector2.up*contours.GetRightSide(x, y)*squareSize,
                    bottomRight,
                    normal);
            }
            else
            {
                draft.AddQuad(
                    bottomRight + Vector2.left*squareSize,
                    bottomRight + Vector2.left*squareSize + Vector2.up*cornerWeight*squareSize,
                    bottomRight + Vector2.up*cornerWeight*squareSize,
                    bottomRight,
                    normal);
            }
        }

        #endregion Halves

        #region Pentagons

        private void AddBottomLeftPentagon(int x, int y)
        {
            Vector2 bottomLeft = GetBottomLeftPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    bottomLeft,
                    bottomLeft + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*contours.GetTopSide(x, y)*squareSize + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*squareSize + Vector2.up*contours.GetRightSide(x, y)*squareSize,
                    bottomLeft + Vector2.right*squareSize,
                }, normal);
            }
            else
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    bottomLeft,
                    bottomLeft + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*cornerWeight*squareSize + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*squareSize + Vector2.up*cornerWeight*squareSize,
                    bottomLeft + Vector2.right*squareSize,
                }, normal);
            }
        }

        private void AddTopLeftPentagon(int x, int y)
        {
            Vector2 topLeft = GetTopLeftPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    topLeft,
                    topLeft + Vector2.right*squareSize,
                    topLeft + Vector2.right*squareSize + Vector2.down*(1 - contours.GetRightSide(x, y))*squareSize,
                    topLeft + Vector2.right*contours.GetBottomSide(x, y)*squareSize + Vector2.down*squareSize,
                    topLeft + Vector2.down*squareSize,
                }, normal);
            }
            else
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    topLeft,
                    topLeft + Vector2.right*squareSize,
                    topLeft + Vector2.right*squareSize + Vector2.down*cornerWeight*squareSize,
                    topLeft + Vector2.right*cornerWeight*squareSize + Vector2.down*squareSize,
                    topLeft + Vector2.down*squareSize,
                }, normal);
            }
        }

        private void AddTopRightPentagon(int x, int y)
        {
            Vector2 topRight = GetTopRightPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    topRight,
                    topRight + Vector2.down*squareSize,
                    topRight + Vector2.left*(1 - contours.GetBottomSide(x, y))*squareSize + Vector2.down*squareSize,
                    topRight + Vector2.left*squareSize + Vector2.down*(1 - contours.GetLeftSide(x, y))*squareSize,
                    topRight + Vector2.left*squareSize,
                }, normal);
            }
            else
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    topRight,
                    topRight + Vector2.down*squareSize,
                    topRight + Vector2.left*cornerWeight*squareSize + Vector2.down*squareSize,
                    topRight + Vector2.left*squareSize + Vector2.down*cornerWeight*squareSize,
                    topRight + Vector2.left*squareSize,
                }, normal);
            }
        }

        private void AddBottomRightPentagon(int x, int y)
        {
            Vector2 bottomRight = GetBottomRightPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    bottomRight,
                    bottomRight + Vector2.left*squareSize,
                    bottomRight + Vector2.left*squareSize + Vector2.up*contours.GetLeftSide(x, y)*squareSize,
                    bottomRight + Vector2.left*(1 - contours.GetTopSide(x, y))*squareSize + Vector2.up*squareSize,
                    bottomRight + Vector2.up*squareSize,
                }, normal);
            }
            else
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    bottomRight,
                    bottomRight + Vector2.left*squareSize,
                    bottomRight + Vector2.left*squareSize + Vector2.up*cornerWeight*squareSize,
                    bottomRight + Vector2.left*cornerWeight*squareSize + Vector2.up*squareSize,
                    bottomRight + Vector2.up*squareSize,
                }, normal);
            }
        }

        #endregion Pentagons

        private void AddQuad(int fromX, int toX, int y)
        {
            Vector2 bottomLeft = GetBottomLeftPosition(fromX, y);
            Vector2 bottomRight = GetBottomRightPosition(toX, y);
            draft.AddQuad(
                bottomLeft,
                bottomLeft + Vector2.up*squareSize,
                bottomRight + Vector2.up*squareSize,
                bottomRight,
                normal);
        }

        private void AddBottomLeftHexagon(int x, int y)
        {
            Vector2 bottomLeft = GetBottomLeftPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    bottomLeft,
                    bottomLeft + Vector2.up*contours.GetLeftSide(x, y)*squareSize,
                    bottomLeft + Vector2.right*contours.GetTopSide(x, y)*squareSize + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*squareSize + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*squareSize + Vector2.up*contours.GetRightSide(x, y)*squareSize,
                    bottomLeft + Vector2.right*contours.GetTopSide(x, y)*squareSize
                }, normal);
            }
            else
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    bottomLeft,
                    bottomLeft + Vector2.up*cornerWeight*squareSize,
                    bottomLeft + Vector2.right*(1 - cornerWeight)*squareSize + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*squareSize + Vector2.up*squareSize,
                    bottomLeft + Vector2.right*squareSize + Vector2.up*(1 - cornerWeight)*squareSize,
                    bottomLeft + Vector2.right*cornerWeight*squareSize
                }, normal);
            }
        }

        private void AddTopLeftHexagon(int x, int y)
        {
            Vector2 topLeft = GetTopLeftPosition(x, y);
            if (contours.useInterpolation)
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    topLeft,
                    topLeft + Vector2.right*contours.GetTopSide(x, y)*squareSize,
                    topLeft + Vector2.right*squareSize + Vector2.down*(1 - contours.GetRightSide(x, y))*squareSize,
                    topLeft + Vector2.right*squareSize + Vector2.down*squareSize,
                    topLeft + Vector2.right*contours.GetBottomSide(x, y)*squareSize + Vector2.down*squareSize,
                    topLeft + Vector2.down*(1 - contours.GetLeftSide(x, y))*squareSize,
                }, normal);
            }
            else
            {
                draft.AddTriangleFan(new List<Vector3>
                {
                    topLeft,
                    topLeft + Vector2.right*cornerWeight*squareSize,
                    topLeft + Vector2.right*squareSize + Vector2.down*(1 - cornerWeight)*squareSize,
                    topLeft + Vector2.right*squareSize + Vector2.down*squareSize,
                    topLeft + Vector2.right*(1 - cornerWeight)*squareSize + Vector2.down*squareSize,
                    topLeft + Vector2.down*cornerWeight*squareSize,
                }, normal);
            }
        }
    }
}

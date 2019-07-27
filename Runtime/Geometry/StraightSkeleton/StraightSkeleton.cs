using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Skeleton
{
    /// <summary>
    /// A straight skeleton representation
    /// </summary>
    public class StraightSkeleton
    {
        public List<List<Vector2>> polygons = new List<List<Vector2>>();

        public StraightSkeleton()
        {
        }

        public StraightSkeleton(Plan plan)
        {
            foreach (var currentVertex in plan)
            {
                var nextVertex = currentVertex.next;
                var polygon = new List<Vector2> {currentVertex.position, nextVertex.position};
                currentVertex.nextPolygonIndex = polygons.Count;
                currentVertex.previousPolygonIndex = polygons.Count - 1;
                polygons.Add(polygon);
            }
        }

        public void AddVertex(Plan.Vertex vertex)
        {
            if (vertex.previousPolygonIndex == vertex.nextPolygonIndex)
            {
                AddVertex(vertex.previousPolygonIndex, vertex.position);
            }
            else
            {
                AddVertex(vertex.previousPolygonIndex, vertex.position);
                AddVertex(vertex.nextPolygonIndex, vertex.position);
            }
        }

        public void ValidatePolygons()
        {
            foreach (var polygon in polygons)
            {
                ValidatePolygon(polygon);
            }
        }

        private void AddVertex(int polygonIndex, Vector2 vertex)
        {
            var polygon = polygons.GetLooped(polygonIndex);
            if (polygon.Count > 2)
            {
                for (int i = 2; i < polygon.Count; i++)
                {
                    if (polygon[i] == vertex)
                    {
                        return;
                    }
                }

                polygon.Add(vertex);
            }
            else
            {
                polygon.Add(vertex);
            }
        }

        private void ValidatePolygon(List<Vector2> polygon)
        {
            Vector2 controurDirection = polygon[1] - polygon[0];
            int count = 0;
            bool swapped;
            do
            {
                swapped = false;
                if (count > polygon.Count)
                {
                    Debug.LogError("Too many iterations");
                    break;
                }
                for (int i = 3; i < polygon.Count; i++)
                {
                    Vector2 current = polygon[i];
                    Vector2 previous = polygon[i - 1];
                    Vector2 edgeDirection = current - previous;
                    float dot = Vector2.Dot(controurDirection, edgeDirection);
                    if (dot < -Geometry.Epsilon)
                    {
                        // Contradirected
                    }
                    else if (dot > Geometry.Epsilon)
                    {
                        // Codirected
                        polygon[i] = previous;
                        polygon[i - 1] = current;
                        swapped = true;
                    }
                    else
                    {
                        // Perpendicular
                        Vector2 next = polygon.GetLooped(i + 1);
                        Vector2 previousPrevious = polygon[i - 2];
                        if (Intersect.SegmentSegment(current, next, previous, previousPrevious))
                        {
                            polygon[i] = previous;
                            polygon[i - 1] = current;
                            swapped = true;
                        }
                    }
                }
                count++;
            } while (swapped);
        }
    }
}

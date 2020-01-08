using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Skeleton
{
    /// <summary>
    /// Representation of the active plan during generation process
    /// </summary>
    public class Plan : IEnumerable<Plan.Vertex>
    {
        public int Count => vertices.Count;
        public Vertex First => vertices[0];

        private readonly List<Vertex> vertices = new List<Vertex>();

        private Plan()
        {
        }

        public Plan(IEnumerable<Vector2> polygon)
        {
            foreach (var vertex in polygon)
            {
                vertices.Add(new Vertex(vertex));
            }
            for (int i = 0; i < Count; i++)
            {
                var vertex = vertices[i];
                vertex.previous = vertices.GetLooped(i - 1);
                vertex.next = vertices.GetLooped(i + 1);
            }
        }

        private void Add(Vertex vertex)
        {
            vertices.Add(vertex);
        }

        public void Insert(Vertex vertex, Vertex previous, Vertex next)
        {
            vertices.Add(vertex);
            LinkVertices(previous, vertex, next);
        }

        public bool Remove(Vertex vertex)
        {
            return vertices.Remove(vertex);
        }

        public void Offset(float offset)
        {
            foreach (var vertex in vertices)
            {
                vertex.position -= vertex.bisector*Geometry.GetAngleOffset(offset, vertex.angle);
            }
        }

        public List<Plan> Split()
        {
            var plans = new List<Plan>();
            while (Count > 0)
            {
                int i = 0;
                int max = Count;
                var plan = new Plan();
                var startVertex = First;
                var currentVertex = startVertex;
                do
                {
                    if (i >= max)
                    {
                        Debug.LogError("Invalid connectivity");
                        break;
                    }
                    Remove(currentVertex);
                    plan.Add(currentVertex);
                    currentVertex = currentVertex.next;
                    i++;
                } while (!currentVertex.Equals(startVertex));
                plans.Add(plan);
            }
            return plans;
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            if (Count == 0)
            {
                yield break;
            }
            var startVertex = vertices[0];
            var currentVertex = startVertex;
            int i = 0;
            int max = Count;
            do
            {
                if (i >= max)
                {
                    Debug.LogError("Invalid connectivity");
                    yield break;
                }
                yield return currentVertex;
                currentVertex = currentVertex.next;
                i++;
            } while (!currentVertex.Equals(startVertex));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static void LinkVertices(Vertex a, Vertex b)
        {
            a.next = b;
            b.previous = a;
        }

        private static void LinkVertices(Vertex a, Vertex b, Vertex c)
        {
            LinkVertices(a, b);
            LinkVertices(b, c);
        }

        public class Vertex
        {
            public Vector2 position;
            public float angle;
            public Vector2 bisector;
            public Vertex previous;
            public Vertex next;
            public bool inEvent;
            public int previousPolygonIndex;
            public int nextPolygonIndex;

            public bool reflect => angle >= 180;

            public Vertex(Vector2 position)
            {
                this.position = position;
            }

            public override string ToString()
            {
                return string.Format("{0} inEvent: {1}", position, inEvent);
            }
        }
    }
}

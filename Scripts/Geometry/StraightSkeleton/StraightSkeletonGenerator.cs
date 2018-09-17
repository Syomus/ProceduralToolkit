using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Skeleton
{
    public class StraightSkeletonGenerator
    {
        private readonly List<Plan> activePlans = new List<Plan>();
        private readonly List<Plan> newPlans = new List<Plan>();
        private StraightSkeleton skeleton;

        public StraightSkeleton Generate(IList<Vector2> polygon)
        {
            activePlans.Clear();
            newPlans.Clear();

            var activePlan = new Plan(polygon);
            activePlans.Add(activePlan);

            skeleton = new StraightSkeleton(activePlan);

            foreach (var p in activePlans)
            {
                foreach (var vertex in p)
                {
                    CalculateBisector(vertex);
                }
            }

            int count = 0;
            while (activePlans.Count > 0)
            {
                if (count > polygon.Count)
                {
                    Debug.LogError("Too many iterations");
                    break;
                }
                bool success = OffsetAndDetectEvents();
                if (!success)
                {
                    return skeleton;
                }
                count++;
            }
            return skeleton;
        }

        public List<Plan> GetActivePlans()
        {
            return activePlans;
        }

        public bool OffsetAndDetectEvents()
        {
            ResetInEventFlags();
            foreach (var plan in activePlans)
            {
                float offset = FindSmallestOffset(plan);
                if (float.IsPositiveInfinity(offset) || offset <= 0)
                {
                    Debug.LogError("Unable to find offset");
                    return false;
                }

                plan.Offset(-offset);

                var intersectionEvents = DetectIntersectionEvents(plan);
                foreach (var intersectionEvent in intersectionEvents)
                {
                    ProcessIntersectionEvent(plan, intersectionEvent);
                }
                newPlans.AddRange(plan.Split());
            }

            activePlans.AddRange(newPlans);
            newPlans.Clear();
            CleanupLoops();
            activePlans.RemoveAll(p => p.Count == 0);

            skeleton.ValidatePolygons();
            return true;
        }

        private void ResetInEventFlags()
        {
            foreach (var plan in activePlans)
            {
                foreach (var vertex in plan)
                {
                    vertex.inEvent = false;
                }
            }
        }

        private float FindSmallestOffset(Plan plan)
        {
            float smallestOffset = float.PositiveInfinity;
            foreach (var vertex in plan)
            {
                float offset;
                if (BisectorBisector(vertex, vertex.next, out offset))
                {
                    smallestOffset = Mathf.Min(smallestOffset, offset);
                }

                if (vertex.reflect)
                {
                    foreach (var segmentA in plan)
                    {
                        var segmentB = segmentA.next;
                        if (segmentA == vertex || segmentB == vertex)
                        {
                            continue;
                        }
                        if (BisectorSegment(vertex, segmentA.position, segmentB.position, out offset))
                        {
                            smallestOffset = Mathf.Min(smallestOffset, offset);
                        }
                    }
                }
            }
            return smallestOffset;
        }

        private List<IntersectionEvent> DetectIntersectionEvents(Plan plan)
        {
            var intersectionEvents = new List<IntersectionEvent>();
            var vertex = plan.First;
            do
            {
                if (vertex.inEvent)
                {
                    vertex = vertex.next;
                    continue;
                }
                if (IncidentalVertices(vertex, vertex.next))
                {
                    var intersectionEvent = new IntersectionEvent(vertex.position);
                    intersectionEvents.Add(intersectionEvent);

                    vertex.inEvent = true;
                    vertex.next.inEvent = true;
                    var chain = new List<Plan.Vertex> {vertex, vertex.next};
                    FindCollapsedChain(vertex.next, vertex, ref chain);
                    intersectionEvent.chains.Add(chain);

                    var chainStart = chain[0];
                    var chainEnd = chain[chain.Count - 1];
                    if (chainEnd.next != chainStart)
                    {
                        FindIncidentalVertices(plan, chainEnd.next, chainStart, intersectionEvent);
                    }
                    vertex = chainEnd.next;
                }
                else
                {
                    vertex = vertex.next;
                }
            } while (vertex != plan.First);

            vertex = plan.First;
            do
            {
                if (vertex.inEvent)
                {
                    vertex = vertex.next;
                    continue;
                }
                var intersectionEvent = new IntersectionEvent(vertex.position);
                FindIncidentalVertices(plan, vertex.next, vertex, intersectionEvent);
                if (intersectionEvent.chains.Count > 0)
                {
                    vertex.inEvent = true;
                    intersectionEvent.chains.Add(new List<Plan.Vertex> {vertex});
                    intersectionEvents.Add(intersectionEvent);
                }

                vertex = vertex.next;
            } while (vertex != plan.First);
            return intersectionEvents;
        }

        private void FindCollapsedChain(Plan.Vertex searchStart, Plan.Vertex searchEnd, ref List<Plan.Vertex> chain)
        {
            var vertex = searchStart;
            do
            {
                if (!vertex.next.inEvent && IncidentalVertices(vertex, vertex.next))
                {
                    vertex.next.inEvent = true;
                    chain.Add(vertex.next);
                    vertex = vertex.next;
                }
                else
                {
                    break;
                }
            } while (vertex != searchEnd);
        }

        private void FindIncidentalVertices(Plan plan, Plan.Vertex searchStart, Plan.Vertex searchEnd, IntersectionEvent intersectionEvent)
        {
            var vertex = searchStart;
            while (vertex != searchEnd)
            {
                if (!vertex.inEvent && IncidentalVertices(intersectionEvent.position, vertex.position))
                {
                    vertex.inEvent = true;
                    var chain = new List<Plan.Vertex> {vertex};
                    FindCollapsedChain(vertex, searchEnd, ref chain);
                    intersectionEvent.chains.Add(chain);
                    vertex = chain[chain.Count - 1].next;
                }
                else if (vertex.next != searchEnd &&
                         PointSegment(intersectionEvent.position, vertex.position, vertex.next.position))
                {
                    var newVertex = SplitEdge(plan, intersectionEvent, vertex, vertex.next);
                    vertex = newVertex.next;
                }
                else
                {
                    vertex = vertex.next;
                }
            }
        }

        private Plan.Vertex SplitEdge(Plan plan, IntersectionEvent intersectionEvent, Plan.Vertex segmentA, Plan.Vertex segmentB)
        {
            var vertex = new Plan.Vertex(intersectionEvent.position)
            {
                inEvent = true,
                previousPolygonIndex = segmentA.nextPolygonIndex,
                nextPolygonIndex = segmentB.previousPolygonIndex
            };
            plan.Insert(vertex, segmentA, segmentB);
            intersectionEvent.chains.Add(new List<Plan.Vertex> {vertex});
            CalculateBisector(vertex);
            return vertex;
        }

        private void ProcessIntersectionEvent(Plan plan, IntersectionEvent intersectionEvent)
        {
            foreach (var chain in intersectionEvent.chains)
            {
                SimplifyChain(plan, chain);
            }
            intersectionEvent.chains.RemoveAll(c => c.Count == 0);

            if (intersectionEvent.chains.Count >= 2)
            {
                for (var i = 0; i < intersectionEvent.chains.Count; i++)
                {
                    var chain = intersectionEvent.chains[i];
                    var previousChain = intersectionEvent.chains.GetLooped(i - 1);
                    var vertex = chain[0];
                    plan.Remove(vertex);
                    skeleton.AddVertex(vertex);

                    var newVertex = new Plan.Vertex(intersectionEvent.position)
                    {
                        previousPolygonIndex = vertex.previousPolygonIndex,
                        nextPolygonIndex = previousChain[0].nextPolygonIndex
                    };
                    plan.Insert(newVertex, vertex.previous, previousChain[0].next);
                    CalculateBisector(newVertex);
                }
            }
        }

        private void SimplifyChain(Plan plan, List<Plan.Vertex> chain)
        {
            if (chain.Count == 1) return;

            var first = chain[0];
            var previous = first.previous;
            var last = chain[chain.Count - 1];
            var next = last.next;
            foreach (var vertex in chain)
            {
                plan.Remove(vertex);
                skeleton.AddVertex(vertex);
            }
            chain.Clear();
            if (next != first)
            {
                var newVertex = new Plan.Vertex(first.position)
                {
                    previousPolygonIndex = first.previousPolygonIndex,
                    nextPolygonIndex = last.nextPolygonIndex,
                };
                plan.Insert(newVertex, previous, next);
                chain.Add(newVertex);
                CalculateBisector(newVertex);
            }
        }

        private void CleanupLoops()
        {
            foreach (var plan in activePlans)
            {
                if (plan.Count == 1)
                {
                    Debug.LogError("Invalid plan: " + plan.First);
                }
                else if (plan.Count == 2)
                {
                    var vertex = plan.First;
                    plan.Remove(vertex);
                    plan.Remove(vertex.next);
                }
            }
        }

        private static void CalculateBisector(Plan.Vertex vertex)
        {
            Plan.Vertex previous = vertex.previous;
            Plan.Vertex next = vertex.next;
            float angle;
            Vector2 direction = Geometry.GetAngleBisector(previous.position, vertex.position, next.position, out angle);
            vertex.angle = angle;
            vertex.bisector = direction;
        }

        private static bool PointSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            if (!IncidentalVertices(point, segmentA) &&
                !IncidentalVertices(point, segmentB))
            {
                return Intersect.PointSegment(point, segmentA, segmentB);
            }
            return false;
        }

        private static bool BisectorBisector(Plan.Vertex vertexA, Plan.Vertex vertexB, out float offset)
        {
            IntersectionRayRay2 intersection;
            if (Intersect.RayRay(vertexA.position, vertexA.bisector, vertexB.position, vertexB.bisector, out intersection))
            {
                if (intersection.type == IntersectionType.Point)
                {
                    float offsetA = GetBisectorBisectorOffset(vertexA, intersection.pointA);
                    float offsetB = GetBisectorBisectorOffset(vertexB, intersection.pointA);
                    offset = Mathf.Min(offsetA, offsetB);
                    return true;
                }
                if (intersection.type == IntersectionType.Segment)
                {
                    float toIntersection = Vector2.Distance(vertexA.position, vertexB.position);
                    float offsetA = GetBisectorBisectorOffset(vertexA, toIntersection)/2;
                    float offsetB = GetBisectorBisectorOffset(vertexB, toIntersection)/2;
                    offset = Mathf.Min(offsetA, offsetB);
                    return true;
                }

                Debug.LogErrorFormat("Invalid bisector intersection\ntype: {0}\npointA: {1} pointB: {2}\nbisectorA: {3} bisectorB:{4}",
                    intersection.type, intersection.pointA, intersection.pointB, vertexA.bisector, vertexB.bisector);
                offset = 0;
                return false;
            }
            offset = 0;
            return false;
        }

        private static float GetBisectorBisectorOffset(Plan.Vertex vertex, Vector2 intersection)
        {
            float toIntersection = Vector2.Distance(vertex.position, intersection);
            return GetBisectorBisectorOffset(vertex, toIntersection);
        }

        private static float GetBisectorBisectorOffset(Plan.Vertex vertex, float toIntersection)
        {
            return toIntersection*Geometry.GetAngleBisectorSin(vertex.angle);
        }

        private static bool BisectorSegment(Plan.Vertex vertex, Vector2 segmentA, Vector2 segmentB, out float offset)
        {
            IntersectionRaySegment2 intersection;
            if (Intersect.RaySegment(vertex.position, vertex.bisector, segmentA, segmentB, out intersection))
            {
                if (intersection.type == IntersectionType.Point)
                {
                    Vector2 segmentDirection = (segmentB - segmentA).normalized;
                    float toIntersection = Vector2.Distance(vertex.position, intersection.pointA);
                    float intersectionAngle = Vector2.Angle(vertex.bisector, segmentDirection);
                    float intersectionSin = Mathf.Sin(intersectionAngle*Mathf.Deg2Rad);
                    float bisectorSin = Geometry.GetAngleBisectorSin(vertex.angle);
                    offset = toIntersection/(1/intersectionSin + 1/bisectorSin);
                    return true;
                }

                Debug.LogErrorFormat("Invalid bisector intersection\ntype: {0}\npointA: {1} pointB: {2}\nray: {3} segmentA:{4} segmentB:{5}",
                    intersection.type, intersection.pointA, intersection.pointB, vertex.bisector, segmentA, segmentB);
                offset = 0;
                return false;
            }
            offset = 0;
            return false;
        }

        private static bool IncidentalVertices(Plan.Vertex a, Plan.Vertex b)
        {
            return IncidentalVertices(a.position, b.position);
        }

        private static bool IncidentalVertices(Vector2 a, Vector2 b)
        {
            return (a - b).sqrMagnitude < Geometry.Epsilon;
        }

        private class IntersectionEvent
        {
            public readonly Vector2 position;
            public readonly List<List<Plan.Vertex>> chains = new List<List<Plan.Vertex>>();

            public IntersectionEvent(Vector2 position)
            {
                this.position = position;
            }

            public override string ToString()
            {
                return String.Format("{0} Count: {1}", position, chains.Count);
            }
        }
    }
}

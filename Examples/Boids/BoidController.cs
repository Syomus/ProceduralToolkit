using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace ProceduralToolkit.Examples
{
    public class Boid
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 cohesion;
        public Vector3 separation;
        public Vector3 alignment;
    }

    /// <summary>
    /// A single-mesh particle system with birds-like behaviour 
    /// </summary>
    /// <remarks>
    /// http://en.wikipedia.org/wiki/Boids
    /// </remarks>
    public class BoidController
    {
        public Vector3 anchor = Vector3.zero;
        public float spawnSphere = 10;
        public float worldSphere = 15;

        public int swarmCount = 2000;
        public int maxSpeed = 10;
        public float interactionRadius = 5;
        public float cohesionCoefficient = 1;
        public float separationDistance = 3;
        public float separationCoefficient = 10;
        public float alignmentCoefficient = 5;

        /// <summary>
        /// Number of neighbours participating in calculations
        /// </summary>
        public int maxBoids = 5;
        /// <summary>
        /// Percentage of swarm simulated in each frame
        /// </summary>
        public float simulationPercent = 0.01f;

        private List<Boid> boids = new List<Boid>();
        private MeshDraft template;
        private MeshDraft draft;
        private Mesh mesh;
        private int simulationCount;
        private List<Boid> neighbours = new List<Boid>();
        private int separationCount;
        private Boid other;
        private Vector3 toOther;
        private Vector3 distanceToAnchor;
        private int simulationUpdate;

        public BoidController(MeshFilter meshFilter)
        {
            template = MeshDraft.Tetrahedron(0.3f);

            // Avoid vertex count overflow
            swarmCount = Mathf.Min(65000/template.vertices.Count, swarmCount);
            // Optimization trick: in each frame we simulate only small percent of all boids
            simulationUpdate = Mathf.RoundToInt(swarmCount*simulationPercent);
            int vertexCount = swarmCount*template.vertices.Count;

            draft = new MeshDraft
            {
                name = "Boids",
                vertices = new List<Vector3>(vertexCount),
                triangles = new List<int>(vertexCount),
                normals = new List<Vector3>(vertexCount),
                uv = new List<Vector2>(vertexCount),
                colors = new List<Color>(vertexCount)
            };
            for (var i = 0; i < swarmCount; i++)
            {
                boids.Add(new Boid());
                draft.Add(template);
            }

            mesh = draft.ToMesh();
            mesh.MarkDynamic();
            meshFilter.mesh = mesh;
        }

        /// <summary>
        /// Generate new colors and positions for boids
        /// </summary>
        public void Generate()
        {
            // Paint template in random color
            template.colors.Clear();
            var color = RandomE.colorHSV;
            // Assuming that we are dealing with tetrahedron, first vertex should be boid's "nose"
            template.colors.Add(color.Inverted());
            for (int i = 1; i < template.vertices.Count; i++)
            {
                template.colors.Add(color);
            }

            // Paint draft and mesh
            draft.colors.Clear();
            for (int i = 0; i < swarmCount; i ++)
            {
                draft.colors.AddRange(template.colors);
            }
            mesh.colors = draft.colors.ToArray();

            // Assign random starting values for each boid
            foreach (var boid in boids)
            {
                boid.position = Random.insideUnitSphere*spawnSphere;
                boid.rotation = Random.rotation;
                boid.velocity = Random.onUnitSphere*maxSpeed;
            }
        }

        /// <summary>
        /// Run simulation
        /// </summary>
        public IEnumerator Simulate()
        {
            simulationCount = 0;
            while (true)
            {
                for (int i = 0; i < boids.Count; i++)
                {
                    // Optimization trick: in each frame we simulate only small percent of all boids
                    simulationCount++;
                    if (simulationCount > simulationUpdate)
                    {
                        simulationCount = 0;
                        yield return null;
                    }

                    var boid = boids[i];
                    // Search for nearest neighbours
                    neighbours.Clear();
                    for (int j = 0; j < boids.Count; j++)
                    {
                        var b = boids[j];
                        if ((b.position - boid.position).sqrMagnitude < interactionRadius)
                        {
                            neighbours.Add(b);
                            if (neighbours.Count == maxBoids)
                            {
                                break;
                            }
                        }
                    }

                    if (neighbours.Count < 2) continue;

                    boid.velocity = Vector3.zero;
                    boid.cohesion = Vector3.zero;
                    boid.separation = Vector3.zero;
                    boid.alignment = Vector3.zero;

                    // Calculate boid parameters
                    separationCount = 0;
                    for (var j = 0; j < neighbours.Count && j < maxBoids; j++)
                    {
                        other = neighbours[j];
                        boid.cohesion += other.position;
                        boid.alignment += other.velocity;
                        toOther = other.position - boid.position;
                        if (toOther.sqrMagnitude > 0 && toOther.sqrMagnitude < separationDistance*separationDistance)
                        {
                            boid.separation += toOther/toOther.sqrMagnitude;
                            separationCount++;
                        }
                    }

                    // Clamp all parameters to safe values
                    boid.cohesion /= Mathf.Min(neighbours.Count, maxBoids);
                    boid.cohesion = Vector3.ClampMagnitude(boid.cohesion - boid.position, maxSpeed);
                    boid.cohesion *= cohesionCoefficient;
                    if (separationCount > 0)
                    {
                        boid.separation /= separationCount;
                        boid.separation = Vector3.ClampMagnitude(boid.separation, maxSpeed);
                        boid.separation *= separationCoefficient;
                    }
                    boid.alignment /= Mathf.Min(neighbours.Count, maxBoids);
                    boid.alignment = Vector3.ClampMagnitude(boid.alignment, maxSpeed);
                    boid.alignment *= alignmentCoefficient;

                    // Calculate resulting velocity
                    boid.velocity = Vector3.ClampMagnitude(boid.cohesion + boid.separation + boid.alignment, maxSpeed);
                    if (boid.velocity == Vector3.zero)
                    {
                        // Prevent boids from stopping
                        boid.velocity = Random.onUnitSphere*maxSpeed;
                    }
                }
            }
        }

        private void SetBoidVertices(Boid boid, int index)
        {
            for (int i = 0; i < template.vertices.Count; i++)
            {
                draft.vertices[index*template.vertices.Count + i] = boid.rotation*template.vertices[i] + boid.position;
            }
        }

        /// <summary>
        /// Apply simulation to mesh
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < boids.Count; i++)
            {
                var boid = boids[i];
                boid.rotation = Quaternion.FromToRotation(Vector3.up, boid.velocity);

                // Contain boids in sphere
                distanceToAnchor = anchor - boid.position;
                if (distanceToAnchor.sqrMagnitude > worldSphere*worldSphere)
                {
                    boid.velocity += distanceToAnchor/worldSphere;
                    boid.velocity = Vector3.ClampMagnitude(boid.velocity, maxSpeed);
                }

                boid.position += boid.velocity*Time.deltaTime;
                SetBoidVertices(boid, i);
            }
            mesh.vertices = draft.vertices.ToArray();
            mesh.RecalculateNormals();
        }
    }
}
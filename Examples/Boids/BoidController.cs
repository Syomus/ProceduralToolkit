using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A single-mesh particle system with birds-like behaviour 
    /// </summary>
    /// <remarks>
    /// http://en.wikipedia.org/wiki/Boids
    /// </remarks>
    public class BoidController
    {
        [Serializable]
        public class Config
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

            public MeshDraft template;
        }

        private Config config;
        private List<Boid> boids = new List<Boid>();
        private MeshDraft draft;
        private Mesh mesh;
        private List<Boid> neighbours = new List<Boid>();
        private int maxSimulationSteps;

        /// <summary>
        /// Generate new colors and positions for boids
        /// </summary>
        public Mesh Generate(Config config)
        {
            this.config = config;

            // Avoid vertex count overflow
            config.swarmCount = Mathf.Min(65000/config.template.vertexCount, config.swarmCount);
            // Optimization trick: in each frame we simulate only small percent of all boids
            maxSimulationSteps = Mathf.RoundToInt(config.swarmCount*config.simulationPercent);
            int vertexCount = config.swarmCount*config.template.vertexCount;

            draft = new MeshDraft
            {
                name = "Boids",
                vertices = new List<Vector3>(vertexCount),
                triangles = new List<int>(vertexCount),
                normals = new List<Vector3>(vertexCount),
                uv = new List<Vector2>(vertexCount),
                colors = new List<Color>(vertexCount)
            };

            for (var i = 0; i < config.swarmCount; i++)
            {
                // Assign random starting values for each boid
                var boid = new Boid
                {
                    position = Random.insideUnitSphere*config.spawnSphere,
                    rotation = Random.rotation,
                    velocity = Random.onUnitSphere*config.maxSpeed
                };
                boids.Add(boid);

                draft.Add(config.template);
            }

            mesh = draft.ToMesh();
            mesh.MarkDynamic();
            // Set bounds manually for correct culling
            mesh.bounds = new Bounds(Vector3.zero, Vector3.one*config.worldSphere*2);
            return mesh;
        }

        /// <summary>
        /// Run simulation
        /// </summary>
        public IEnumerator CalculateVelocities()
        {
            int simulationStep = 0;

            for (int currentIndex = 0; currentIndex < boids.Count; currentIndex++)
            {
                // Optimization trick: in each frame we simulate only small percent of all boids
                simulationStep++;
                if (simulationStep > maxSimulationSteps)
                {
                    simulationStep = 0;
                    yield return null;
                }

                var boid = boids[currentIndex];
                // Search for nearest neighbours
                neighbours.Clear();
                for (int i = 0; i < boids.Count; i++)
                {
                    Boid neighbour = boids[i];

                    Vector3 toNeighbour = neighbour.position - boid.position;
                    if (toNeighbour.sqrMagnitude < config.interactionRadius)
                    {
                        neighbours.Add(neighbour);
                        if (neighbours.Count == config.maxBoids)
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
                int separationCount = 0;
                for (int i = 0; i < neighbours.Count && i < config.maxBoids; i++)
                {
                    Boid neighbour = neighbours[i];

                    boid.cohesion += neighbour.position;
                    boid.alignment += neighbour.velocity;

                    Vector3 toNeighbour = neighbour.position - boid.position;
                    if (toNeighbour.sqrMagnitude > 0 &&
                        toNeighbour.sqrMagnitude < config.separationDistance*config.separationDistance)
                    {
                        boid.separation += toNeighbour/toNeighbour.sqrMagnitude;
                        separationCount++;
                    }
                }

                // Clamp all parameters to safe values
                boid.cohesion /= Mathf.Min(neighbours.Count, config.maxBoids);
                boid.cohesion = Vector3.ClampMagnitude(boid.cohesion - boid.position, config.maxSpeed);
                boid.cohesion *= config.cohesionCoefficient;

                if (separationCount > 0)
                {
                    boid.separation /= separationCount;
                    boid.separation = Vector3.ClampMagnitude(boid.separation, config.maxSpeed);
                    boid.separation *= config.separationCoefficient;
                }

                boid.alignment /= Mathf.Min(neighbours.Count, config.maxBoids);
                boid.alignment = Vector3.ClampMagnitude(boid.alignment, config.maxSpeed);
                boid.alignment *= config.alignmentCoefficient;

                // Calculate resulting velocity
                Vector3 velocity = boid.cohesion + boid.separation + boid.alignment;
                boid.velocity = Vector3.ClampMagnitude(velocity, config.maxSpeed);
                if (boid.velocity == Vector3.zero)
                {
                    // Prevent boids from stopping
                    boid.velocity = Random.onUnitSphere*config.maxSpeed;
                }
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
                Vector3 distanceToAnchor = config.anchor - boid.position;
                if (distanceToAnchor.sqrMagnitude > config.worldSphere*config.worldSphere)
                {
                    boid.velocity += distanceToAnchor/config.worldSphere;
                    boid.velocity = Vector3.ClampMagnitude(boid.velocity, config.maxSpeed);
                }

                boid.position += boid.velocity*Time.deltaTime;
                SetBoidVertices(boid, i);
            }
            mesh.SetVertices(draft.vertices);
            mesh.RecalculateNormals();
        }

        private void SetBoidVertices(Boid boid, int boidIndex)
        {
            for (int i = 0; i < config.template.vertexCount; i++)
            {
                int vertexIndex = boidIndex*config.template.vertexCount + i;
                draft.vertices[vertexIndex] = boid.rotation*config.template.vertices[i] + boid.position;
            }
        }

        private class Boid
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 velocity;
            public Vector3 cohesion;
            public Vector3 separation;
            public Vector3 alignment;
        }
    }
}
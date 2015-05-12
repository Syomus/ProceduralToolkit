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
    }

    /// <summary>
    /// A single-mesh particle system with birds-like behaviour
    /// http://en.wikipedia.org/wiki/Boids
    /// </summary>
    [RequireComponent(typeof (MeshRenderer), typeof (MeshFilter))]
    public class Boids : MonoBehaviour
    {
        public Vector3 anchor = Vector3.zero;
        public float spawnSphere = 20;
        public float worldSphere = 25;
        public int maxSpeed = 10;
        public float cohesionRadius = 5;
        public int maxBoids = 5;
        public float separationDistance = 3;
        public float cohesionCoefficient = 1;
        public float alignmentCoefficient = 5;
        public float separationCoefficient = 10;
        public float simulationPercent = 0.01f;
        public int swarmCount = 5000;

        private List<Boid> boids = new List<Boid>();
        private MeshDraft template;
        private MeshDraft draft;
        private Mesh mesh;
        private int simulationCount;
        private List<Boid> neighbours = new List<Boid>();
        private int separationCount;
        private Vector3 alignment;
        private Boid other;
        private Vector3 toOther;
        private Vector3 distanceToAnchor;
        private int simulationUpdate;

        private void Awake()
        {
            template = MeshE.TetrahedronDraft(0.3f);

            swarmCount = Mathf.Min(65000/template.vertices.Count, swarmCount);
            simulationUpdate = Mathf.RoundToInt(swarmCount*simulationPercent);
            var vertexCount = swarmCount*template.vertices.Count;

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
            GetComponent<MeshFilter>().mesh = mesh;

            Generate();

            StartCoroutine(Simulate());
        }

        private void Generate()
        {
            template.colors.Clear();
            var color = RandomE.colorHSV;
            template.colors.Add(color.Inverted());
            for (int i = 1; i < template.vertices.Count; i++)
            {
                template.colors.Add(color);
            }

            draft.colors.Clear();
            for (int i = 0; i < draft.vertices.Count; i += template.colors.Count)
            {
                for (int j = 0; j < template.colors.Count; j++)
                {
                    draft.colors.Add(template.colors[j]);
                }
            }
            mesh.colors = draft.colors.ToArray();

            foreach (var boid in boids)
            {
                boid.position = Random.insideUnitSphere*spawnSphere;
                boid.rotation = Random.rotation;
                boid.velocity = Random.onUnitSphere*maxSpeed;
            }
        }

        private IEnumerator Simulate()
        {
            simulationCount = 0;
            while (true)
            {
                for (int i = 0; i < swarmCount; i++)
                {
                    simulationCount++;
                    if (simulationCount > simulationUpdate)
                    {
                        simulationCount = 0;
                        yield return null;
                    }
                    var boid = boids[i];
                    neighbours.Clear();
                    for (int j = 0; j < swarmCount; j++)
                    {
                        var b = boids[j];
                        if ((b.position - boid.position).sqrMagnitude < cohesionRadius)
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

                    separationCount = 0;
                    alignment = Vector3.zero;

                    for (var j = 0; j < neighbours.Count && j < maxBoids; j++)
                    {
                        other = neighbours[j];
                        boid.cohesion += other.position;
                        alignment += other.velocity;
                        toOther = other.position - boid.position;
                        if (toOther.sqrMagnitude > 0 && toOther.sqrMagnitude < separationDistance*separationDistance)
                        {
                            boid.separation += toOther/toOther.sqrMagnitude;
                            separationCount++;
                        }
                    }

                    boid.cohesion /= Mathf.Min(neighbours.Count, maxBoids);
                    boid.cohesion = Vector3.ClampMagnitude(boid.cohesion - boid.position, maxSpeed);
                    boid.cohesion *= cohesionCoefficient;
                    if (separationCount > 0)
                    {
                        boid.separation /= separationCount;
                        boid.separation = Vector3.ClampMagnitude(boid.separation, maxSpeed);
                        boid.separation *= separationCoefficient;
                    }
                    alignment /= Mathf.Min(neighbours.Count, maxBoids);
                    alignment = Vector3.ClampMagnitude(alignment, maxSpeed);
                    alignment *= alignmentCoefficient;

                    boid.velocity = Vector3.ClampMagnitude(boid.cohesion + boid.separation + alignment, maxSpeed);
                    if (boid.velocity == Vector3.zero)
                    {
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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Generate();
            }
            for (int i = 0; i < swarmCount; i++)
            {
                var boid = boids[i];
                boid.rotation = Quaternion.FromToRotation(Vector3.up, boid.velocity);

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

        private void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(20, 20, Screen.width, Screen.height), "Click to restart simulation");
        }
    }
}
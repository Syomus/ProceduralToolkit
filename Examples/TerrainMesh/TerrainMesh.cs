using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A simple terrain based on Perlin noise and coloured according to height
    /// </summary>
    public static class TerrainMesh
    {
        public static MeshDraft TerrainDraft(int xSize, int zSize, int xSegments, int zSegments, int noiseScale)
        {
            var draft = MeshDraft.Plane(xSize, zSize, xSegments, zSegments);
            draft.Move(Vector3.left*xSize/2 + Vector3.back*zSize/2);

            var gradient = RandomE.gradientHSV;

            var noiseOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
            draft.colors.Clear();
            for (int i = 0; i < draft.vertices.Count; i++)
            {
                // Calculate noise value
                Vector3 vertex = draft.vertices[i];
                float x = noiseScale*vertex.x/xSize + noiseOffset.x;
                float y = noiseScale*vertex.z/zSize + noiseOffset.y;
                float noise = Mathf.PerlinNoise(x, y);
                draft.vertices[i] = new Vector3(vertex.x, noise, vertex.z);

                draft.colors.Add(gradient.Evaluate(noise));
            }
            return draft;
        }
    }
}
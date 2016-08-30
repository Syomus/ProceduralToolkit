using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A simple Perlin noise based low poly terrain generator
    /// </summary>
    public static class LowPolyTerrainGenerator
    {
        public static MeshDraft TerrainDraft(Vector3 terrainSize, float cellSize, float noiseScale, Gradient gradient)
        {
            var noiseOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));

            int xSegments = Mathf.FloorToInt(terrainSize.x/cellSize);
            int zSegments = Mathf.FloorToInt(terrainSize.z/cellSize);

            float xStep = terrainSize.x/xSegments;
            float zStep = terrainSize.z/zSegments;
            int vertexCount = 6*xSegments*zSegments;
            var draft = new MeshDraft
            {
                name = "Terrain",
                vertices = new List<Vector3>(vertexCount),
                triangles = new List<int>(vertexCount),
                normals = new List<Vector3>(vertexCount),
                colors = new List<Color>(vertexCount)
            };

            for (int i = 0; i < vertexCount; i++)
            {
                draft.vertices.Add(Vector3.zero);
                draft.triangles.Add(0);
                draft.normals.Add(Vector3.zero);
                draft.colors.Add(Color.black);
            }

            for (int x = 0; x < xSegments; x++)
            {
                for (int z = 0; z < zSegments; z++)
                {
                    int index0 = 6*(x + z*xSegments);
                    int index1 = index0 + 1;
                    int index2 = index0 + 2;
                    int index3 = index0 + 3;
                    int index4 = index0 + 4;
                    int index5 = index0 + 5;

                    float height00 = GetHeight(x + 0, z + 0, xSegments, zSegments, noiseOffset, noiseScale);
                    float height01 = GetHeight(x + 0, z + 1, xSegments, zSegments, noiseOffset, noiseScale);
                    float height10 = GetHeight(x + 1, z + 0, xSegments, zSegments, noiseOffset, noiseScale);
                    float height11 = GetHeight(x + 1, z + 1, xSegments, zSegments, noiseOffset, noiseScale);

                    var vertex00 = new Vector3((x + 0)*xStep, height00*terrainSize.y, (z + 0)*zStep);
                    var vertex01 = new Vector3((x + 0)*xStep, height01*terrainSize.y, (z + 1)*zStep);
                    var vertex10 = new Vector3((x + 1)*xStep, height10*terrainSize.y, (z + 0)*zStep);
                    var vertex11 = new Vector3((x + 1)*xStep, height11*terrainSize.y, (z + 1)*zStep);

                    draft.vertices[index0] = vertex00;
                    draft.vertices[index1] = vertex01;
                    draft.vertices[index2] = vertex11;
                    draft.vertices[index3] = vertex00;
                    draft.vertices[index4] = vertex11;
                    draft.vertices[index5] = vertex10;

                    draft.colors[index0] = gradient.Evaluate(height00);
                    draft.colors[index1] = gradient.Evaluate(height01);
                    draft.colors[index2] = gradient.Evaluate(height11);
                    draft.colors[index3] = gradient.Evaluate(height00);
                    draft.colors[index4] = gradient.Evaluate(height11);
                    draft.colors[index5] = gradient.Evaluate(height10);

                    Vector3 normal000111 = Vector3.Cross(vertex01 - vertex00, vertex11 - vertex00).normalized;
                    Vector3 normal001011 = Vector3.Cross(vertex11 - vertex00, vertex10 - vertex00).normalized;

                    draft.normals[index0] = normal000111;
                    draft.normals[index1] = normal000111;
                    draft.normals[index2] = normal000111;
                    draft.normals[index3] = normal001011;
                    draft.normals[index4] = normal001011;
                    draft.normals[index5] = normal001011;

                    draft.triangles[index0] = index0;
                    draft.triangles[index1] = index1;
                    draft.triangles[index2] = index2;
                    draft.triangles[index3] = index3;
                    draft.triangles[index4] = index4;
                    draft.triangles[index5] = index5;
                }
            }

            return draft;
        }

        private static float GetHeight(int x, int z, int xSegments, int zSegments, Vector2 noiseOffset, float noiseScale)
        {
            float noiseX = noiseScale*x/xSegments + noiseOffset.x;
            float noiseZ = noiseScale*z/zSegments + noiseOffset.y;
            return Mathf.PerlinNoise(noiseX, noiseZ);
        }
    }
}
using System;
using System.Collections.Generic;
using ProceduralToolkit.FastNoiseLib;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// A simple low poly terrain generator based on fractal noise
    /// </summary>
    public static class LowPolyTerrainGenerator
    {
        [Serializable]
        public class Config
        {
            public Vector3 terrainSize = new Vector3(32, 5, 32);
            public float cellSize = 0.5f;
            public float noiseFrequency = 4;
            public Gradient gradient = ColorE.Gradient(Color.black, Color.white);
        }

        public static MeshDraft TerrainDraft(Config config)
        {
            Assert.IsTrue(config.terrainSize.x > 0);
            Assert.IsTrue(config.terrainSize.z > 0);
            Assert.IsTrue(config.cellSize > 0);

            var noiseOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));

            int xSegments = Mathf.FloorToInt(config.terrainSize.x/config.cellSize);
            int zSegments = Mathf.FloorToInt(config.terrainSize.z/config.cellSize);

            float xStep = config.terrainSize.x/xSegments;
            float zStep = config.terrainSize.z/zSegments;
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

            var noise = new FastNoise();
            noise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
            noise.SetFrequency(config.noiseFrequency);

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

                    float height00 = GetHeight(x + 0, z + 0, xSegments, zSegments, noiseOffset, noise);
                    float height01 = GetHeight(x + 0, z + 1, xSegments, zSegments, noiseOffset, noise);
                    float height10 = GetHeight(x + 1, z + 0, xSegments, zSegments, noiseOffset, noise);
                    float height11 = GetHeight(x + 1, z + 1, xSegments, zSegments, noiseOffset, noise);

                    var vertex00 = new Vector3((x + 0)*xStep, height00*config.terrainSize.y, (z + 0)*zStep);
                    var vertex01 = new Vector3((x + 0)*xStep, height01*config.terrainSize.y, (z + 1)*zStep);
                    var vertex10 = new Vector3((x + 1)*xStep, height10*config.terrainSize.y, (z + 0)*zStep);
                    var vertex11 = new Vector3((x + 1)*xStep, height11*config.terrainSize.y, (z + 1)*zStep);

                    draft.vertices[index0] = vertex00;
                    draft.vertices[index1] = vertex01;
                    draft.vertices[index2] = vertex11;
                    draft.vertices[index3] = vertex00;
                    draft.vertices[index4] = vertex11;
                    draft.vertices[index5] = vertex10;

                    draft.colors[index0] = config.gradient.Evaluate(height00);
                    draft.colors[index1] = config.gradient.Evaluate(height01);
                    draft.colors[index2] = config.gradient.Evaluate(height11);
                    draft.colors[index3] = config.gradient.Evaluate(height00);
                    draft.colors[index4] = config.gradient.Evaluate(height11);
                    draft.colors[index5] = config.gradient.Evaluate(height10);

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

        private static float GetHeight(int x, int z, int xSegments, int zSegments, Vector2 noiseOffset, FastNoise noise)
        {
            float noiseX = x/(float) xSegments + noiseOffset.x;
            float noiseZ = z/(float) zSegments + noiseOffset.y;
            return noise.GetNoise01(noiseX, noiseZ);
        }
    }
}

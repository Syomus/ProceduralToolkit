﻿using UnityEngine;

namespace ProceduralToolkit.Samples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Plane : MonoBehaviour
    {
        public float xSize = 1f;
        public float zSize = 1f;
        public int xSegments = 16;
        public int zSegments = 16;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.Plane(xSize, zSize, xSegments, zSegments).ToMesh();
        }
    }
}

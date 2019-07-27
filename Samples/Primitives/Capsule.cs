using UnityEngine;

namespace ProceduralToolkit.Samples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Capsule : MonoBehaviour
    {
        public float height = 2;
        public float radius = 0.5f;
        public int segments = 32;
        public int rings = 8;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.Capsule(height, radius, segments, rings).ToMesh();
        }
    }
}
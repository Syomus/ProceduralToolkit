using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Pyramid : MonoBehaviour
    {
        public float radius = 1f;
        public int segments = 16;
        public float height = 1f;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.Pyramid(radius, segments, height).ToMesh();
        }
    }
}

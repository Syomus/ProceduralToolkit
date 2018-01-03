using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Teardrop : MonoBehaviour
    {
        public float radius = 1f;
        public float height = 1f;
        public int horizontalSegments = 16;
        public int verticalSegments = 16;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.Teardrop(radius, height, horizontalSegments, verticalSegments).ToMesh();
        }
    }
}

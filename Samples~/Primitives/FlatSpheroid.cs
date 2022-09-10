using UnityEngine;

namespace ProceduralToolkit.Samples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class FlatSpheroid : MonoBehaviour
    {
        public float radius = 0.75f;
        public float height = 1f;
        public int horizontalSegments = 16;
        public int verticalSegments = 16;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.FlatSpheroid(radius, height, horizontalSegments, verticalSegments).ToMesh();
        }
    }
}

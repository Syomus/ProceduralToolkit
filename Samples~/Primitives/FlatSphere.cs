using UnityEngine;

namespace ProceduralToolkit.Samples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class FlatSphere : MonoBehaviour
    {
        public float radius = 1f;
        public int horizontalSegments = 16;
        public int verticalSegments = 16;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.FlatSphere(radius, horizontalSegments, verticalSegments).ToMesh();
        }
    }
}

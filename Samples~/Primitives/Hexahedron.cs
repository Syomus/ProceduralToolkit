using UnityEngine;

namespace ProceduralToolkit.Samples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Hexahedron : MonoBehaviour
    {
        public float width = 1f;
        public float length = 1f;
        public float height = 2f;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.Hexahedron(width, length, height).ToMesh();
        }
    }
}

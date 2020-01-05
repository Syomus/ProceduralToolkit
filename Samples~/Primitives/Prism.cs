using UnityEngine;

namespace ProceduralToolkit.Samples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Prism : MonoBehaviour
    {
        public float radius = 1f;
        public int segments = 16;
        public float height = 2f;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.Prism(radius, segments, height).ToMesh();
        }
    }
}

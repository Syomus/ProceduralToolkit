using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Dodecahedron : MonoBehaviour
    {
        public float radius = 1f;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.Dodecahedron(radius).ToMesh();
        }
    }
}

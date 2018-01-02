using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Icosahedron : MonoBehaviour
    {
        public float radius = 1f;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshDraft.Icosahedron(radius).ToMesh();
        }
    }
}

using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Hexahedron : MonoBehaviour
    {
        public float width = 1f;
        public float length = 1f;
        public float height = 2f;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshE.Hexahedron(width, length, height);
        }
    }
}
using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Octahedron : MonoBehaviour
    {
        public float radius = 1f;

        public void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshE.Octahedron(radius);
        }
    }
}
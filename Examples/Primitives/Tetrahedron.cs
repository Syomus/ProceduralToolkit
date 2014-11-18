using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Tetrahedron : MonoBehaviour
    {
        public float radius = 1f;

        public void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshE.Tetrahedron(radius);
        }
    }
}
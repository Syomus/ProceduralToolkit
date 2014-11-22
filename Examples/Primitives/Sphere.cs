using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class Sphere : MonoBehaviour
    {
        public float radius = 1f;
        public int longitudeSegments = 16;
        public int latitudeSegments = 16;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshE.Sphere(radius, longitudeSegments, latitudeSegments);
        }
    }
}
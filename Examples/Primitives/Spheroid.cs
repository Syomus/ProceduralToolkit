using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof (MeshRenderer), typeof (MeshFilter))]
    public class Spheroid : MonoBehaviour
    {
        public float radius = 0.75f;
        public float height = 1f;
        public int longitudeSegments = 16;
        public int latitudeSegments = 16;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshE.Spheroid(radius, height, longitudeSegments, latitudeSegments);
        }
    }
}
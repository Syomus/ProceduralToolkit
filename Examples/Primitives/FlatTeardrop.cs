using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class FlatTeardrop : MonoBehaviour
    {
        public float radius = 1f;
        public float height = 1f;
        public int longitudeSegments = 16;
        public int latitudeSegments = 16;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = MeshE.FlatTeardrop(radius, longitudeSegments, latitudeSegments, height);
        }
    }
}
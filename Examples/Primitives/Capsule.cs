using UnityEngine;

namespace ProceduralToolkit.Examples.Primitives
{
	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	public class Capsule : MonoBehaviour
	{
		public float height = 1f;
		public float radius = 1f;
		public int segments = 12;
		public int rings = 8;

		private void Start()
		{
			GetComponent<MeshFilter>().mesh = MeshDraft.Capsule(height, radius, segments, rings).ToMesh();
		}
	}
}

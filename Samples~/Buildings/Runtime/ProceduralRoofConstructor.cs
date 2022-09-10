using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Samples.Buildings
{
    [CreateAssetMenu(menuName = "ProceduralToolkit/Buildings/Procedural Roof Constructor", order = 4)]
    public class ProceduralRoofConstructor : RoofConstructor
    {
        [SerializeField]
        private RendererProperties rendererProperties = null;
        [SerializeField]
        private Material roofMaterial = null;

        public override void Construct(IConstructible<MeshDraft> constructible, Transform parentTransform)
        {
            var draft = constructible.Construct(Vector2.zero);

            var meshFilter = parentTransform.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = draft.ToMesh();

            var meshRenderer = parentTransform.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.ApplyProperties(rendererProperties);
            meshRenderer.material = roofMaterial;
        }
    }
}

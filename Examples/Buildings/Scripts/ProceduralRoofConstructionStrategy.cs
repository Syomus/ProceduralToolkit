using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    [CreateAssetMenu(menuName = "ProceduralToolkit/Buildings/Procedural Roof Construction Strategy", order = 4)]
    public class ProceduralRoofConstructionStrategy : RoofConstructionStrategy
    {
        [SerializeField]
        private RendererProperties rendererProperties;
        [SerializeField]
        private Material roofMaterial;

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

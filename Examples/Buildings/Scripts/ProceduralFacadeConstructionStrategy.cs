using System.Collections.Generic;
using ProceduralToolkit.Buildings;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralToolkit.Examples.Buildings
{
    [CreateAssetMenu(menuName = "ProceduralToolkit/Buildings/Procedural Facade Construction Strategy", order = 2)]
    public class ProceduralFacadeConstructionStrategy : FacadeConstructionStrategy
    {
        [SerializeField]
        private LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes;
        [SerializeField]
        private GameObject lightProbeProxyVolumeOverride;
        [SerializeField]
        private ReflectionProbeUsage reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
        [SerializeField]
        private Transform probeAnchor;
        [SerializeField]
        private ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;
        [SerializeField]
        private bool receiveShadows = true;
        [SerializeField]
        private MotionVectorGenerationMode motionVectorGenerationMode = MotionVectorGenerationMode.Object;
        [SerializeField]
        private Material glassMaterial;
        [SerializeField]
        private Material roofMaterial;
        [SerializeField]
        private Material wallMaterial;

        public override void Construct(Transform parentTransform, ILayout layout)
        {
            var compoundDraft = new CompoundMeshDraft();
            ConstructLayout(compoundDraft, Vector2.zero, layout);

            compoundDraft.MergeDraftsWithTheSameName();
            compoundDraft.SortDraftsByName();

            var meshFilter = parentTransform.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = compoundDraft.ToMeshWithSubMeshes();

            var meshRenderer = parentTransform.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.lightProbeUsage = lightProbeUsage;
            meshRenderer.lightProbeProxyVolumeOverride = lightProbeProxyVolumeOverride;
            meshRenderer.reflectionProbeUsage = reflectionProbeUsage;
            meshRenderer.probeAnchor = probeAnchor;
            meshRenderer.shadowCastingMode = shadowCastingMode;
            meshRenderer.receiveShadows = receiveShadows;
            meshRenderer.motionVectorGenerationMode = motionVectorGenerationMode;

            var materials = new List<Material>();
            foreach (var draft in compoundDraft)
            {
                if (draft.name == "Glass")
                {
                    materials.Add(glassMaterial);
                }
                else if (draft.name == "Roof")
                {
                    materials.Add(roofMaterial);
                }
                else if (draft.name == "Wall")
                {
                    materials.Add(wallMaterial);
                }
            }
            meshRenderer.materials = materials.ToArray();
        }

        public static void ConstructLayout(CompoundMeshDraft draft, Vector2 parentLayoutOrigin, ILayout layout)
        {
            foreach (var element in layout.elements)
            {
                ConstructElement(draft, parentLayoutOrigin + layout.origin, element);
            }
        }

        public static void ConstructElement(CompoundMeshDraft draft, Vector2 parentLayoutOrigin, ILayoutElement element)
        {
            var layout = element as ILayout;
            if (layout != null)
            {
                ConstructLayout(draft, parentLayoutOrigin, layout);
                return;
            }
            var constructible = element as IConstructible<CompoundMeshDraft>;
            if (constructible != null)
            {
                draft.Add(constructible.Construct(parentLayoutOrigin));
            }
        }
    }
}

using ProceduralToolkit.Buildings;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralToolkit.Examples.Buildings
{
    [CreateAssetMenu(menuName = "ProceduralToolkit/Buildings/Procedural Roof Construction Strategy", order = 3)]
    public class ProceduralRoofConstructionStrategy : RoofConstructionStrategy
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
        private Material roofMaterial;

        public override void Construct(Transform parentTransform, IConstructible<MeshDraft> constructible)
        {
            var draft = constructible.Construct(Vector2.zero);

            var meshFilter = parentTransform.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = draft.ToMesh();

            var meshRenderer = parentTransform.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.lightProbeUsage = lightProbeUsage;
            meshRenderer.lightProbeProxyVolumeOverride = lightProbeProxyVolumeOverride;
            meshRenderer.reflectionProbeUsage = reflectionProbeUsage;
            meshRenderer.probeAnchor = probeAnchor;
            meshRenderer.shadowCastingMode = shadowCastingMode;
            meshRenderer.receiveShadows = receiveShadows;
            meshRenderer.motionVectorGenerationMode = motionVectorGenerationMode;
            meshRenderer.material = roofMaterial;
        }
    }
}

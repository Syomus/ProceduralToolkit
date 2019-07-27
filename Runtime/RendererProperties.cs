using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralToolkit
{
    /// <summary>
    /// Serializable Renderer properties
    /// </summary>
    [Serializable]
    public class RendererProperties
    {
        public LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes;
        public GameObject lightProbeProxyVolumeOverride = null;
        public ReflectionProbeUsage reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
        public Transform probeAnchor = null;
        public ShadowCastingMode shadowCastingMode = ShadowCastingMode.On;
        public bool receiveShadows = true;
        public MotionVectorGenerationMode motionVectorGenerationMode = MotionVectorGenerationMode.Object;
    }
}

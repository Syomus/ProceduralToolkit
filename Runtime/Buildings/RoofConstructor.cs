using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class RoofConstructor : ScriptableObject, IRoofConstructor
    {
        public abstract void Construct(IConstructible<MeshDraft> constructible, Transform parentTransform);
    }
}

using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public interface IFacadePanel
    {
        Vector2? origin { get; set; }
        float? width { get; set; }
        float? height { get; set; }
        MeshDraft GetMeshDraft();
    }
}
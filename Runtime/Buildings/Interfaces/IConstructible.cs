using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface IConstructible<out T>
    {
        T Construct(Vector2 parentLayoutOrigin);
    }
}

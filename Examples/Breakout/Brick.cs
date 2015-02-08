using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public class Brick : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
    }
}
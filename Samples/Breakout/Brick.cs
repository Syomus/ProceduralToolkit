using System;
using UnityEngine;

namespace ProceduralToolkit.Samples
{
    public class Brick : MonoBehaviour
    {
        public event Action onHit = () => { };

        public SpriteRenderer spriteRenderer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            onHit();
        }
    }
}

using UnityEngine;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// An example showing usage of DebugE, GLE and GizmosE
    /// </summary>
    [ExecuteInEditMode]
    public class Drawer : MonoBehaviour
    {
        private const float radius = 1.5f;
        private const float coneAngle = 30;
        private const float coneLength = 1;

        private void Update()
        {
            Color color = ColorE.aqua;
            Quaternion rotation = transform.rotation*Quaternion.Euler(0, 0, -120);
            Vector3 position = transform.position + rotation*Vector3.up;

            DebugE.DrawWireHemisphere(position, rotation, radius, color);
            DebugE.DrawWireCone(position, rotation, radius, coneAngle, coneLength, color);
        }

        private void OnRenderObject()
        {
            GLE.BeginLines();
            {
                GL.Color(ColorE.fuchsia);
                Quaternion rotation = transform.rotation;
                Vector3 position = transform.position + Vector3.up;

                GLE.DrawWireHemisphere(position, rotation, radius);
                GLE.DrawWireCone(position, rotation, radius, coneAngle, coneLength);
            }
            GL.End();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = ColorE.lime;
            Quaternion rotation = transform.rotation*Quaternion.Euler(0, 0, 120);
            Vector3 position = transform.position + rotation*Vector3.up;

            GizmosE.DrawWireHemisphere(position, rotation, radius);
            GizmosE.DrawWireCone(position, rotation, radius, coneAngle, coneLength);
        }
    }
}

using UnityEngine;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Simple camera controller
    /// </summary>
    public class CameraRotator : MonoBehaviour
    {
        public Transform target;
        public Vector3 distance = new Vector3(0, 1, -10);
        public float turnSpeed = 10;
        public float turnSmoothing = 0.1f;
        public float tiltMax = 85f;
        public float tiltMin = 45f;

        private float lookAngle;
        private float tiltAngle;
        private float smoothX = 0;
        private float smoothY = 0;
        private float smoothXvelocity = 0;
        private float smoothYvelocity = 0;

        private void LateUpdate()
        {
            if (target == null) return;

            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");

            if (turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXvelocity, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYvelocity, turnSmoothing);
            }
            else
            {
                smoothX = x;
                smoothY = y;
            }

            lookAngle += smoothX*turnSpeed;
            tiltAngle -= smoothY*turnSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);

            var rotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
            transform.rotation = rotation;
            transform.position = rotation*distance + target.position;
        }
    }
}
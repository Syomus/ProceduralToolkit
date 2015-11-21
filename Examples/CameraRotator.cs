using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Simple camera controller
    /// </summary>
    [RequireComponent(typeof (Image))]
    public class CameraRotator : UIBehaviour, IDragHandler
    {
        public new Transform camera;
        public Transform target;
        [Header("Position")]
        public float distanceMin = 10;
        public float distanceMax = 30;
        public float yOffset = 0;
        public float scrollSensitivity = 1000;
        public float scrollSmoothing = 10;
        [Header("Rotation")]
        public float tiltMin = -85;
        public float tiltMax = 85;
        public float rotationSensitivity = 0.5f;
        public float rotationSpeed = 20;

        private float distance;
        private float scrollDistance;
        private float velocity;
        private float lookAngle;
        private float tiltAngle;
        private Quaternion rotation;

        protected override void Awake()
        {
            base.Awake();
            tiltAngle = (tiltMin + tiltMax)/2;
            distance = scrollDistance = (distanceMax + distanceMin)/2;

            if (camera == null || target == null) return;

            camera.rotation = rotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
            camera.position = CalculateCameraPosition();
        }

        private void LateUpdate()
        {
            if (camera == null || target == null) return;

            if (camera.rotation != rotation)
            {
                camera.rotation = Quaternion.Lerp(camera.rotation, rotation, Time.deltaTime*rotationSpeed);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                scrollDistance -= scroll*Time.deltaTime*scrollSensitivity;
                scrollDistance = Mathf.Clamp(scrollDistance, distanceMin, distanceMax);
            }

            if (distance != scrollDistance)
            {
                distance = Mathf.SmoothDamp(distance, scrollDistance, ref velocity, Time.deltaTime*scrollSmoothing);
            }

            camera.position = CalculateCameraPosition();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (camera == null || target == null) return;

            lookAngle += eventData.delta.x*rotationSensitivity;
            tiltAngle -= eventData.delta.y*rotationSensitivity;
            tiltAngle = Mathf.Clamp(tiltAngle, tiltMin, tiltMax);
            rotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
        }

        private Vector3 CalculateCameraPosition()
        {
            return target.position + camera.rotation*(Vector3.back*distance) + Vector3.up*yOffset;
        }
    }
}
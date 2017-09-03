using UnityEngine;

namespace LMS {
    public static class CameraExtension {
        /// <summary>
        /// Scale object full camera
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="objScale"></param>
        public static void CameraFillOjbect(this Camera cam, GameObject objScale) {
            //Vector3 camRotation = cam.transform.rotation.eulerAngles;
            cam.transform.rotation = Quaternion.Euler(Vector3.zero);
            float distance = Vector3.Distance(objScale.transform.position, cam.transform.position);
            Vector3 viewBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, distance));
            Vector3 viewTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, distance));
            Vector3 scale = viewTopRight - viewBottomLeft;
            scale.z = objScale.transform.localScale.z;
            objScale.transform.localScale = scale;
        }
        /// <summary>
        /// Lazy function for lazy people to shake camera without any references
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="shakeAmount"></param>
        /// <param name="shakeDuration"></param>
        /// <param name="smooth"></param>
        /// <param name="smoothAmount"></param>
        //public static void Shake(this Camera camera, float shakeAmount, float shakeDuration, bool smooth = false, float smoothAmount = 5f) {
        //    CameraShake cameraShake = camera.GetComponent<CameraShake>();
        //    if (cameraShake != null)
        //        cameraShake.ShakeCamera(shakeAmount, shakeDuration, smooth, smoothAmount);
        //}
        public static void Shake(this Camera camera, CameraShake.Types type, CameraShake.TypesAxis axis = CameraShake.TypesAxis.All) {
            CameraShake cameraShake = camera.GetComponent<CameraShake>();
            if (cameraShake != null) cameraShake.ShakeCamera(type, axis);
        }
    }
}
///Daniel Moore (Firedan1176) - Firedan1176.webs.com/
///26 Dec 2015
///
///Shakes camera parent object

using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace LMS {
    public class CameraShake : MonoBehaviour {
        #region CONSTANT FIELDS
        private const float HARD_AMOUNT = 10f;
        private const float HARD_DURATION = 10f;
        private const float HARD_SMOOTH = 5f;
        private const float MID_AMOUNT = 6f;
        private const float MID_DURATION = 4f;
        private const float MID_SMOOTH = 3f;
        private const float LOW_AMOUNT = 4f;
        private const float LOW_DURATION = 1.5f;
        private const float LOW_SMOOTH = 1f;

        private const string ID_CAM_TWEEN = "tween_cam";
        //private const float HARD_SHAKE_DURATION = 1.5f;
        //private const int HARD_SHAKE_AMOUNT = 15;
        //private Vector3 HARD_SHAKE_FORCE = new Vector3(3.5f, 3.5f, 0);
        private const float HARD_SHAKE_DURATION = 1.2f;
        private const int HARD_SHAKE_AMOUNT = 50;
        private Vector3 HARD_SHAKE_FORCE = new Vector3(/*3f*/2.25f, /*3f*/2.25f, 0);

        //private const float MID_SHAKE_DURATION = 1f;
        //private const int MID_SHAKE_AMOUNT = 10;
        //private Vector3 MID_SHAKE_FORCE = new Vector3(1.5f, 1.5f, 0);
        private const float MID_SHAKE_DURATION = 1f;
        private const int MID_SHAKE_AMOUNT = 40;
        private Vector3 MID_SHAKE_FORCE = new Vector3(/*2f*/1.5f, /*2f*/1.5f, 0);

        //private const float LOW_SHAKE_DURATION = 1f;
        //private const int LOW_SHAKE_AMOUNT = 10;
        //private Vector3 LOW_SHAKE_FORCE = new Vector3(0.3f, 0.3f, 0);
        private const float LOW_SHAKE_DURATION = 0.8f;
        private const int LOW_SHAKE_AMOUNT = 30;
        private Vector3 LOW_SHAKE_FORCE = new Vector3(/*1f*/0.75f, /*1f*/0.75f, 0);

        #endregion
        #region FIELDS
        [Header("Configurations")]
        [SerializeField]
        private bool debugMode = false;//Test-run/Call ShakeCamera() on start
        //[SerializeField]
        private float shakeAmount;//The amount to shake this frame.
        //[SerializeField]
        private float shakeDuration;//The duration this frame.
        //[SerializeField]
        private bool smooth;//Smooth rotation?
        //[SerializeField]
        private float smoothAmount = 5f;//Amount to smooth
        [SerializeField]
        private TypesAxis Axis;
        [SerializeField]
        private float durationShake;
        [SerializeField]
        private int amountShake;
        [SerializeField]
        private Vector3 forceShake;
        //Readonly values...
        float shakePercentage;//A percentage (0-1) representing the amount of shake to be applied when setting rotation.
        float startAmount;//The initial shake amount (to determine percentage), set when ShakeCamera is called.
        float startDuration;//The initial shake duration, set when ShakeCamera is called.

        bool isRunning = false; //Is the coroutine running right now?
        #endregion
        #region ENUM
        public enum Types { None, HARD, MID, LOW }
        public enum TypesAxis { All, AxisX, AxisY }
        #endregion
        #region MONOBEHAVIOUR
        void Start() {
            if (debugMode) ShakeCamera();
        }
        #endregion
        #region METHODS
        void ShakeCamera() {
            startAmount = shakeAmount;//Set default (start) values
            startDuration = shakeDuration;//Set default (start) values
            if (!isRunning) StartCoroutine(Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
        }
        public void ShakeCamera(Types type, TypesAxis _axis = TypesAxis.All) {
            if (!debugMode) {
                switch (type) {
                    //case Types.HARD: ShakeCamera(HARD_AMOUNT, HARD_DURATION, true, HARD_SMOOTH); break;
                    //case Types.MID: ShakeCamera(MID_AMOUNT, MID_DURATION, true, MID_SMOOTH); break;
                    //case Types.LOW: ShakeCamera(LOW_AMOUNT, LOW_DURATION, true, LOW_SMOOTH); break;
                    //case Types.None: ResetCamera(); break;
                    //default: throw new System.NotSupportedException();
                    case Types.HARD:
                        ShakeCamera(HARD_SHAKE_AMOUNT, HARD_SHAKE_DURATION, HARD_SHAKE_FORCE, _axis);
                        break;
                    case Types.MID:
                        ShakeCamera(MID_SHAKE_AMOUNT, MID_SHAKE_DURATION, MID_SHAKE_FORCE, _axis);
                        break;
                    case Types.LOW:
                        ShakeCamera(LOW_SHAKE_AMOUNT, LOW_SHAKE_DURATION, LOW_SHAKE_FORCE, _axis);
                        break;
                    case Types.None:
                        ResetTweening();
                        break;
                }
            } else ShakeCamera(amountShake, durationShake, forceShake);
        }
        public void ShakeCamera(float amount, float duration, TypesAxis axis = TypesAxis.All) {
            shakeAmount += amount;//Add to the current amount.
            startAmount = shakeAmount;//Reset the start amount, to determine percentage.
            shakeDuration += duration;//Add to the current time.
            startDuration = shakeDuration;//Reset the start time.
            if (!isRunning) StartCoroutine(Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
        }
        public void ShakeCamera(float amount, float duration, bool smooth, float smoothAmount, TypesAxis axis = TypesAxis.All) {
            shakeAmount += amount;//Add to the current amount.
            startAmount = shakeAmount;//Reset the start amount, to determine percentage.
            shakeDuration += duration;//Add to the current time.
            startDuration = shakeDuration;//Reset the start time.
            this.smooth = smooth;
            this.smoothAmount = smoothAmount;
            if (!isRunning) StartCoroutine(Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
        }
        public void ResetCamera() {
            shakeAmount = 0f;
            startAmount = 0f;
            shakeDuration = 0f;
            startDuration = 0f;
            StopCoroutine(Shake());
            isRunning = false;
            smooth = false;
            transform.localRotation = Quaternion.identity;
        }
        public void ShakeCamera(int amount, float duration, Vector3 force, TypesAxis _axis = TypesAxis.All) {
            if (DOTween.IsTweening(ID_CAM_TWEEN)) DOTween.Kill(ID_CAM_TWEEN);
            //if (_axis == TypesAxis.AxisX) force = new Vector3(force.x, 0, 0);
            //if (_axis == TypesAxis.AxisY) force = new Vector3(0, force.y, 0);
            if (_axis == TypesAxis.AxisX) force = new Vector3(0, force.x, 0);
            if (_axis == TypesAxis.AxisY) force = new Vector3(force.y, 0, 0);
            transform.DOShakeRotation(duration, force, amount).SetEase(Ease.InOutQuad).SetId(ID_CAM_TWEEN).OnComplete(() => {
                transform.localRotation = Quaternion.identity;
            });

        }
        public void ResetTweening(bool is_complete = false) {
            if (DOTween.IsTweening(ID_CAM_TWEEN)) DOTween.Kill(ID_CAM_TWEEN, is_complete);
            transform.localRotation = Quaternion.identity;
        }

        IEnumerator Shake() {
            isRunning = true;
            while (shakeDuration > 0.01f) {
                Vector3 rotationAmount = Random.insideUnitSphere * shakeAmount;//A Vector3 to add to the Local Rotation                
                switch (Axis) {
                    case TypesAxis.AxisX:
                        //rotationAmount.y = 0;
                        rotationAmount.x = 0;
                        break;
                    case TypesAxis.AxisY:
                        //rotationAmount.x = 0;
                        rotationAmount.y = 0;
                        break;
                }
                rotationAmount.z = 0;//Don't change the Z; it looks funny.
                shakePercentage = shakeDuration / startDuration;//Used to set the amount of shake (% * startAmount).
                shakeAmount = startAmount * shakePercentage;//Set the amount of shake (% * startAmount).
                shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);//Lerp the time, so it is less and tapers off towards the end.
                if (smooth)
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * smoothAmount);
                else
                    transform.localRotation = Quaternion.Euler(rotationAmount);//Set the local rotation the be the rotation amount.
                yield return null;
            }
            //transform.localRotation = Quaternion.identity;//Set the local rotation to 0 when done, just to get rid of any fudging stuff.
            //isRunning = false;
            ResetCamera();
        }
        #endregion
    }
}
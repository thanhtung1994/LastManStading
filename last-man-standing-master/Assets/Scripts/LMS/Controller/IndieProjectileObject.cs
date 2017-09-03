using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Model;

namespace LMS.Battle
{
    public class IndieProjectileObject : MonoBehaviour
    {

        public delegate void CallbackOnHitTarget(IndieProjectileObject projectile);
        public CallbackOnHitTarget OnHitTarget;

        public enum TargetType
        {
            None,
            Unit,
            Position,
        }

        public UnitObject TargetUnit;
        public Vector3 TargetPos;
        [SerializeField]
        private float ProjectileSpeed;
        [SerializeField]
        private bool facingToUnit = true;
        private bool flying = false;
        private TargetType targetType = TargetType.None;

        public void FlyToTarget(UnitObject target, CallbackOnHitTarget onHit, float speed)
        {
            this.TargetUnit = target;
            OnHitTarget = onHit;
            ProjectileSpeed = speed;
            targetType = TargetType.Unit;
            flying = true;
        }

        public void FlyToPosition(Vector2 endPos, CallbackOnHitTarget onHit, float speed)
        {
            this.TargetPos = endPos;
            OnHitTarget = onHit;
            ProjectileSpeed = speed;
            targetType = TargetType.Position;
            flying = true;
        }


        private void DestroyItself()
        {
            SimplePool.Despawn(this.gameObject);
            //Destroy(gameObject);
        }
        // Update is called once per frame
        void Update()
        {
            if (targetType == TargetType.Unit && TargetUnit == null)
            {
                DestroyItself();
                return;
            }
            if (flying)
            {
                float step = ProjectileSpeed * Time.deltaTime;

                Vector3 relativePos;
                float angle;
                switch (targetType)
                {
                    case TargetType.Unit:
                        if (facingToUnit)
                        {
                            relativePos = TargetUnit.HipPosition - transform.position;
                            angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                        }
                        if (step >= Vector2.Distance(transform.position, TargetUnit.HipPosition))
                        {
                            transform.position = TargetUnit.HipPosition;
                            OnHitTarget(this);
                            DestroyItself();
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, TargetUnit.HipPosition, step);
                        }
                        break;
                    case TargetType.Position:
                        if (facingToUnit)
                        {
                            relativePos = TargetPos - transform.position;
                            angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                        }
                        if (step >= Vector2.Distance(transform.position, TargetPos))
                        {
                            transform.position = TargetPos;
                            OnHitTarget(this);
                            DestroyItself();
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, TargetPos, step);
                        }
                        break;
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LMS.Battle
{
    public class ProjectileObject : MonoBehaviour
    {
        public enum ProjectileType
        {
            Single = 1,
            Rocket = 2,
            Meteor = 3,
            SingleSpine = 4,
        }

        private Animator animator
        {
            get { return GetComponent<Animator>(); }
        }

        private SpriteRenderer spriteRenderer
        {
            get { return GetComponent<SpriteRenderer>(); }
        }
        [SerializeField]
        private UnitObject attacker;
        [SerializeField]
        private UnitObject target;
        [SerializeField]
        private ProjectileType Type = ProjectileType.Single;
        [SerializeField]
        private float splashRadius = 0.5f;
        private float ProjectileSpeed;
        private AttackInfo attackInfo;
        [SerializeField]
        private bool flying = false;
        [SerializeField]
        private float LocalScale = 1;
        public void FireOnTarget(UnitObject attacker, UnitObject target, float projectileSpeed, AttackInfo attackInfo)
        {
            transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
            this.attacker = attacker;
            this.target = target;
            ProjectileSpeed = projectileSpeed;
            this.attackInfo = attackInfo;
            switch (Type)
            {
                case ProjectileType.Meteor:
                    //Debug.Log(transform.localPosition);
                    animator.Play("Explose");
                    transform.position = target.transform.position;
                    break;
                default:
                    flying = true;
                    break;
            }
        }

        private void OnHit()
        {
            Collider2D[] hits;
            switch (Type)
            {
                case ProjectileType.SingleSpine:
                    flying = false;
                    target.ReceiveAttackInfo(attackInfo);
                    OnExplose();
                    break;
                case ProjectileType.Single:
                    flying = false;
                    target.ReceiveAttackInfo(attackInfo);
                    animator.SetBool("Hit", true);
                    break;
                case ProjectileType.Rocket:
                    flying = false;
                    animator.SetBool("Hit", true);
                    // Hit target
                    hits = Physics2D.OverlapCircleAll(transform.position.ToVector2(), splashRadius);
                    foreach (Collider2D col in hits)
                    {
                        if (attacker == null) Debug.Log("NULL");
                        else if (BattleManager.ColliderIsCharacter(col, attacker.gameObject))
                        {
                            if (BattleManager.IsEnemyAndAlive(col.gameObject, attacker.TeamIndex))
                            {
                                float _distance = Vector3.Distance(col.transform.position, attacker.transform.position);
                                attackInfo.ReduceSplashByDistance = GameMath.ReduceSplashDamageByDistance(_distance, splashRadius);
                                col.GetComponent<UnitObject>().ReceiveAttackInfo(attackInfo);
                            }
                        }
                    }
                    break;
                case ProjectileType.Meteor:
                    // Hit target
                    hits = Physics2D.OverlapCircleAll(transform.position.ToVector2(), splashRadius);
                    foreach (Collider2D col in hits)
                    {
                        if (BattleManager.ColliderIsCharacter(col, attacker.gameObject))
                        {
                            if (BattleManager.IsEnemyAndAlive(col.gameObject, attacker.TeamIndex))
                            {
                                float _distance = Vector3.Distance(col.transform.position, attacker.transform.position);
                                attackInfo.ReduceSplashByDistance = GameMath.ReduceSplashDamageByDistance(_distance, splashRadius);
                                col.GetComponent<UnitObject>().ReceiveAttackInfo(attackInfo);
                            }
                        }
                    }
                    break;
            }
        }

        public void OnExplose()
        {
            SimplePool.Despawn(this.gameObject);
            //Destroy(gameObject);
        }

        private void Update()
        {
            if (target == null)
            {
                OnExplose();
                return;
            }
            if (flying)
            {
                float step = ProjectileSpeed * Time.deltaTime;

                Vector3 relativePos = target.HipPosition - transform.position;
                float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                if (step >= Vector2.Distance(transform.position.ToVector2(), target.HipPosition))
                {
                    transform.position = target.HipPosition;
                    OnHit();
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.HipPosition, step);
                }
            }
        }

    }
}
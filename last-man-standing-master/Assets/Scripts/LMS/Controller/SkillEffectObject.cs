using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Model;
using Spine.Unity;
using Spine;

namespace LMS.Battle
{
    public class SkillEffectObject : MonoBehaviour
    {
        [SerializeField]
        private bool loop = false;
        [SerializeField]
        private bool AutoDestroyOnEnd = true;
        [SerializeField]
        private SkeletonAnimation skeletonAnimation;
        [SerializeField]
        private string animationName = "animation";

        public SkillScript skillScript;

        private bool isInit = false;
        private float _lifetime = 0;

        private Vector3 targetPos;
        private bool moving = false;
        private float speed = 0;
        public bool isAlive = false;

        private void Start()
        {
            if (!isInit)
            {
                if (skeletonAnimation != null)
                {
                    skeletonAnimation.state.End += OnEndAnimation;
                    skeletonAnimation.state.Event += OnEventAnimation;
                    isInit = true;
                    skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
                }
            }
        }

        private void OnEventAnimation(TrackEntry trackEntry, Spine.Event e)
        {
            OnHitSpell();
        }

        private void OnEndAnimation(TrackEntry trackEntry)
        {
            if (AutoDestroyOnEnd && !loop)
            {
                DestroyItself();
            }
        }

        public void LoadSkill(SkillScript skillScript, bool loop = false, bool flip = false)
        {
            this.skillScript = skillScript;
            this.loop = loop;
            isAlive = true;
            if (skeletonAnimation != null)
                skeletonAnimation.skeleton.flipX = flip;
        }

        public void OnHitSpell()
        {
            skillScript.OnHitSkill(transform.position);
        }

        public void OnFinish()
        {
            skillScript = null;
            BattleManager.DestroyGameObject(gameObject);
        }

        public void AutoDestroyAfter(float time)
        {
            _lifetime = time;
        }

        public void MoveToPosition(Vector2 endPos, float speed)
        {
            targetPos = endPos;
            this.speed = speed;
            moving = true;
        }
        // Update is called once per frame
        void Update()
        {
            if (skillScript != null)
            {
                skillScript.specified.UpdateOnEffectObject(Time.deltaTime);
            }
            if (moving)
            {
                float step = speed * Time.deltaTime;
                //Vector2 relativePos;
                //float angle;
                //relativePos = targetPos - transform.position;
                //angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                if (step >= Vector2.Distance(transform.position, targetPos))
                {
                    transform.position = targetPos;
                    DestroyItself();
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                }
            }
        }
        private void DestroyItself()
        {
            BattleManager.DestroyGameObject(gameObject);
            isAlive = false;
        }
    }
}
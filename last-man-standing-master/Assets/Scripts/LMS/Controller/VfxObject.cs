using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMS.Battle
{
    public class VfxObject : MonoBehaviour
    {
        [SerializeField]
        private bool Loop = false;
        [SerializeField]
        private bool AutoDestroyOnEnd = true;
        [SerializeField]
        private SkeletonAnimation skeletonAnimation;
        [SerializeField]
        private string animationName = "animation";

        private bool isInit = false;
        private float _lifetime = 0;

        private void Start()
        {
            if (!isInit)
            {
                if (skeletonAnimation != null)
                {
                    skeletonAnimation.state.End += OnEndSpineAnimation;
                    isInit = true;
                    skeletonAnimation.AnimationState.SetAnimation(0, animationName, Loop);
                }
            }
        }

        private void Update()
        {
            if (_lifetime > 0)
            {
                _lifetime -= Time.deltaTime;
                if (_lifetime < 0)
                {
                    DestroyItself();
                }
            }
        }

        private void OnEnable()
        {
            if (isInit)
            {
                if (skeletonAnimation != null)
                {
                    skeletonAnimation.skeleton.SetToSetupPose();
                    skeletonAnimation.AnimationState.SetAnimation(0, animationName, Loop);
                }
            }
        }

        private void OnEndSpineAnimation(TrackEntry trackEntry)
        {
            if (AutoDestroyOnEnd && !Loop)
            {
                DestroyItself();
            }
        }
        public void OnEndUnityAnimation()
        {
            if (AutoDestroyOnEnd && !Loop)
            {
                BattleManager.DestroyGameObject(gameObject);
            }
        }

        public void AutoDestroyAfter(float time)
        {
            _lifetime = time;
        }

        private void DestroyItself()
        {
            BattleManager.DestroyGameObject(gameObject);
        }

    }
}
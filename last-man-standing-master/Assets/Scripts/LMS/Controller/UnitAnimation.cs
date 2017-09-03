using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMS.Battle
{
    public class UnitAnimation : MonoBehaviour
    {
        #region Animation Event
        public void OnHitAttack()
        {
            unitObject.OnHitAttack();
        }

        public void OnDead()
        {
            unitObject.OnDead();
        }

        public void OnCastSpellEvent()
        {
            unitObject.OnCastSpellEvent();
        }

        #endregion

        public enum AnimationState
        {
            Idle,
            Move,
            Attack,
            Die,
            Stun,
            Spell,
            KnockUp,
        }

        [SerializeField]
        private AnimationState currentState = AnimationState.Idle;
        private float rdmSpeed = 1;
        private Color32 mainColor = new Color32(255, 255, 255, 255);
        public bool IsFacingLeft = true;

        public UnitObject unitObject
        {
            get { return transform.parent.GetComponent<UnitObject>(); }
        }

        private SpriteRenderer spriteRenderer
        {
            get { return GetComponent<SpriteRenderer>(); }
        }
        private SkeletonAnimator skeletonAnimator
        {
            get { return GetComponent<SkeletonAnimator>(); }
        }

        private Animator animator
        {
            get { return GetComponent<Animator>(); }
        }

        public Bone ShootBone
        {
            get
            {
                if (skeletonAnimator != null)
                {
                    Bone bone = skeletonAnimator.skeleton.FindBone("shoot");
                    if (bone == null)
                        Debug.LogError(string.Format("{0} - Can't find bone: {1}", unitObject.name, "shoot"));
                    return bone;
                }
                return null;
            }
        }

        public Bone HipBone
        {
            get
            {
                if (skeletonAnimator != null)
                {
                    Bone bone = skeletonAnimator.skeleton.FindBone("hip");
                    if (bone == null)
                        Debug.LogError(string.Format("{0} - Can't find bone: {1}", unitObject.name, "hip"));
                    return bone;
                }
                return null;
            }
        }

        public Bone HeadEffectBone
        {
            get
            {
                if (skeletonAnimator != null)
                {
                    Bone bone = skeletonAnimator.skeleton.FindBone("head_effect");
                    if (bone == null)
                        Debug.LogError(string.Format("{0} - Can't find bone: {1}", unitObject.name, "head_effect"));
                    return bone;
                }
                return null;
            }
        }

        public bool IsOnAttack()
        {
            for (int i = 0; i < listAniAttack.Count; i++)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(listAniAttack[i])) return true;
            }
            return false;
        }

        public bool IsOnSpelling()
        {
            for (int i = 0; i < listAniSpell.Count; i++)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(listAniSpell[i])) return true;
            }
            return false;
        }

        public bool IsOnAnimationCantMove
        {
            //get { return  IsOnSpelling(); }
            get { return IsOnAttack() || IsOnSpelling(); }
        }

        public bool HasAnimation(string name)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        #region MonoBehaviour
        //private void Start()
        //{
        //    rdmSpeed = Random.Range(8f, 13f) / 10f;
        //    animator.SetFloat("RdmSpeed", rdmSpeed);
        //}
        //private void Update()
        //{

        //}
        #endregion

        #region Methods
        public void SetSkin(string skin)
        {
            skeletonAnimator.skeleton.SetSkin(skin);
        }

        public void SetMainColor(Color32 color)
        {
            mainColor = color;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
            else if (skeletonAnimator != null)
            {
                skeletonAnimator.skeleton.SetColor(color);
            }
        }
        public void SetColor(Color32 color)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
            else if (skeletonAnimator != null)
            {
                skeletonAnimator.skeleton.SetColor(color);
            }
        }

        public void ReturnToMainColor()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = mainColor;
            }
            else if (skeletonAnimator != null)
            {
                skeletonAnimator.skeleton.SetColor(mainColor);
            }
        }

        public void SetFacingLeft(bool left)
        {
            IsFacingLeft = left;
            if (!unitObject.IsFlipable) return;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = left;
            }
            else if (skeletonAnimator != null)
            {
                skeletonAnimator.skeleton.flipX = left;
            }
        }

        private List<string> listAniAttack = new List<string>();
        private List<string> listAniSpell = new List<string>();
        public void LoadCharacter(UnitObject unitObject)
        {
            listAniAttack = new List<string>();
            listAniSpell = new List<string>();
            if (unitObject.unitScript.unitDefineAnimation != null)
            {
                listAniAttack.Add(unitObject.unitScript.unitDefineAnimation.AniAttack);
                listAniAttack.Add(unitObject.unitScript.unitDefineAnimation.AniAttackCrit);
            }
            else
            {
                listAniAttack.Add(CONSTANT.AniAttack);
            }
            for (int i = 0; i < unitObject.listSkill.Count; i++)
            {
                if (!string.IsNullOrEmpty(unitObject.listSkill[i].AniName))
                {
                    listAniSpell.Add(unitObject.listSkill[i].AniName);
                }
            }
        }

        public void PlayAnimation(AnimationState state)
        {
            switch (state)
            {
                case AnimationState.Attack:
                    animator.Play(CONSTANT.AniAttack);
                    break;
            }
        }

        public void PlayAnimation(string AniName)
        {
            animator.Play(AniName);
        }

        public void PauseAnimation(bool pause)
        {
            animator.speed = pause ? 0 : 1;
        }

        public void StopAction()
        {
            animator.Play(CONSTANT.AniIdle);
            if (unitObject.IsMoveable)
                animator.SetBool(CONSTANT.AniVarMoving, false);
        }

        public void SetAnimation(AnimationState state)
        {
            if (unitObject.DebugUnit) Debug.Log(state);
            currentState = state;
            switch (state)
            {
                case AnimationState.Idle:
                    animator.Play(CONSTANT.AniIdle);
                    if (unitObject.IsMoveable)
                        animator.SetBool(CONSTANT.AniVarMoving, false);
                    break;
                case AnimationState.Move:
                    animator.Play(CONSTANT.AniMove);
                    if (unitObject.IsMoveable)
                        animator.SetBool(CONSTANT.AniVarMoving, true);
                    break;
                case AnimationState.Attack:
                    animator.Play(CONSTANT.AniAttack);
                    break;
                case AnimationState.Die:
                    if (unitObject.IsMoveable)
                        animator.SetBool(CONSTANT.AniVarMoving, false);
                    //animator.SetBool("IsDead", true);
                    //animator.Stop();
                    animator.Play(CONSTANT.AniDead);
                    break;
                case AnimationState.Stun:
                    animator.Play(CONSTANT.AniStun);
                    if (unitObject.IsMoveable)
                        animator.SetBool(CONSTANT.AniVarMoving, false);
                    break;
                case AnimationState.KnockUp:
                    animator.Play(CONSTANT.AniStun);
                    if (unitObject.IsMoveable)
                        animator.SetBool(CONSTANT.AniVarMoving, false);
                    break;
            }
        }

        public void SetAnimationAttackSpeed(float speed)
        {
            animator.SetFloat(CONSTANT.AniVarAttackSpeed, speed);
        }

        public void SetAnimationMovementSpeed(float speed)
        {
            animator.SetFloat(CONSTANT.AniVarMovementSpeed, speed);
        }

        #endregion

    }
}
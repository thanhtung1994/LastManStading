//using Spine.Unity;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CharacterAnimation : MonoBehaviour {

//    public enum AnimationState
//    {
//        Idle,
//        Move,
//        Attack,
//        Die,
//        Stun,
//        Spell,
//    }
//    public UnitObject unitObject
//    {
//        get { return transform.parent.GetComponent<UnitObject>(); }
//    }

//    private Animator animator
//    {
//        get { return GetComponent<Animator>(); }
//    }

//    private SpriteRenderer spriteRenderer
//    {
//        get { return GetComponent<SpriteRenderer>(); }
//    }

//    private SkeletonAnimator skeletonAnimator
//    {
//        get { return GetComponent<SkeletonAnimator>(); }
//    }

//    [SerializeField]
//    private AnimationState currentState = AnimationState.Idle;
//    private Color32 mainColor = new Color32(255, 255, 255, 255);
//    public bool IsFacingLeft = false;



//    #region MonoBehaviour
//    //private void Start()
//    //{

//    //}
//    //private void Update()
//    //{

//    //}
//    #endregion
//    #region Methods
//    public void SetMainColor(Color32 color)
//    {
//        mainColor = color;
//        if (spriteRenderer != null)
//        {
//            spriteRenderer.color = color;
//        }
//        else if (skeletonAnimator != null)
//        {
//            skeletonAnimator.skeleton.SetColor(color);
//        }
//    }
//    public void SetColor(Color32 color)
//    {
//        if (spriteRenderer != null)
//        {
//            spriteRenderer.color = color;
//        }
//        else if (skeletonAnimator != null)
//        {
//            skeletonAnimator.skeleton.SetColor(color);
//        }
//    }

//    public void ReturnToMainColor()
//    {
//        if (spriteRenderer != null)
//        {
//            spriteRenderer.color = mainColor;
//        }
//        else if (skeletonAnimator != null)
//        {
//            skeletonAnimator.skeleton.SetColor(mainColor);
//        }
//    }

//    public void SetFacingLeft(bool left)
//    {
//        IsFacingLeft = left;
//        //if (!unitObject.IsFlipable) return;
//        if (spriteRenderer != null)
//        {
//            spriteRenderer.flipX = left;
//        }
//        else if (skeletonAnimator != null)
//        {
//            skeletonAnimator.skeleton.flipX = !left;
//        }
//    }

//    public void LoadCharacter(CharacterScript charScript)
//    {

//    }

//    public void PlayAnimation(AnimationState state)
//    {
//        switch (state)
//        {
//            case AnimationState.Attack:
//                animator.Play(CONSTANT.AniAttack);
//                break;
//            case AnimationState.Spell:
//                animator.Play(CONSTANT.AniSpell);
//                break;
//        }
//    }

//    public void SetAnimation(AnimationState state)
//    {
//        if (unitObject.DebugUnit) Debug.Log(state);
//        currentState = state;
//        switch (state)
//        {
//            case AnimationState.Idle:
//                if (unitObject.IsMoveable)
//                    animator.SetBool(CONSTANT.AniVarMoving, false);
//                break;
//            case AnimationState.Move:
//                if (unitObject.IsMoveable)
//                    animator.SetBool(CONSTANT.AniVarMoving, true);
//                break;
//            case AnimationState.Attack:
//                animator.Play(CONSTANT.AniAttack);
//                break;
//            case AnimationState.Die:
//                if (unitObject.IsMoveable)
//                    animator.SetBool(CONSTANT.AniVarMoving, false);
//                //animator.SetBool("IsDead", true);
//                //animator.Stop();
//                animator.Play(CONSTANT.AniDead);
//                break;
//            case AnimationState.Stun:
//                animator.Play(CONSTANT.AniIdle);
//                if (unitObject.IsMoveable)
//                    animator.SetBool(CONSTANT.AniVarMoving, false);
//                break;
//        }
//    }

//    public void SetAttackAnimationSpeed(float speed)
//    {
//        animator.SetFloat(CONSTANT.AniVarAttackSpeed, speed);
//    }
//}

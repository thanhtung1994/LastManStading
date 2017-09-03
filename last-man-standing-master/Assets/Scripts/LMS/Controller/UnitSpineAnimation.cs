using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpineAnimation : MonoBehaviour
{

    #region FIELDS
    /// <summary>
    /// Callback when animation fire event
    /// <para>AnimationState state, int trackIndex, Event event, Animations currentAnimation</para>
    /// </summary>
    public Action<Spine.AnimationState, int, Spine.Event, SpineAnimationsEnum> onAnimationEvent;
    /// <summary>
    /// Callback when animation complete (meaning reaching end, might loop)
    /// <para>AnimationState state, int trackIndex, int loopCount, Animations currentAnimation</para>
    /// </summary>
    public Action<Spine.AnimationState, int, int, SpineAnimationsEnum> onAnimationComplete;
    /// <summary>
    /// Callback when overall timescale changed
    /// <para>float currentTimescale, Animations currentAnimation</para>
    /// </summary>
    public Action<float, SpineAnimationsEnum> onOverallTimescaleChanged;
    /// <summary>
    /// Callback when spine flip to other way
    /// <para>bool currentFlip, Animations currentAnimation</para>
    /// </summary>
    public Action<bool, SpineAnimationsEnum> onFlipped;

    private List<Spine.Animation> cachedAnimations = new List<Spine.Animation>();
    [SerializeField]
    private SpineAnimationsEnum currentAnimation;
    [SerializeField]
    private SpineAnimationsEnum lastAnimation;


    [Header("Configurations")]
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Amount of alpha to mix between animations")]
    private float alphaMix = 0.05f;
    [Header("References")]
    [SerializeField]
    private SkeletonAnimation spine;
    [SerializeField]
    private MeshRenderer meshRenderer;
    bool hasEvent;
    #endregion
    #region EVENTS
    /// <summary>
    /// Event fire when overall timescale change
    /// </summary>
    protected virtual void OnOverallTimescaleChanged()
    {
        if (onOverallTimescaleChanged != null)
            onOverallTimescaleChanged.Invoke(spine.timeScale, currentAnimation);
    }
    /// <summary>
    /// Event fire when spine flip to other way
    /// </summary>
    protected virtual void OnFlipped()
    {
        if (onFlipped != null)
            onFlipped.Invoke(FlipX, currentAnimation);
    }
    /// <summary>
    /// Event fire to call when animation reach event
    /// </summary>
    /// <param name="state">current state</param>
    /// <param name="trackIndex">track index animation is stay on</param>
    /// <param name="eventInfo">info of event from spine</param>
    protected virtual void OnAnimationEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        //if (onAnimationEvent != null)
        //    onAnimationEvent.Invoke(state, trackIndex, eventInfo, currentAnimation);
        Debug.Log(trackEntry.TrackIndex + " " + trackEntry.Animation.Name + ": event " + e + ", " + e.Int);
    }
    /// <summary>
    /// Event fire to call when animation reach the end (might loop)
    /// </summary>
    /// <param name="state">current state</param>
    /// <param name="trackIndex">track index animation is stay on</param>
    /// <param name="loopCount">count how many time animation has been looped</param>
    protected virtual void OnAnimationComplete(Spine.TrackEntry trackEntry)
    {
        //if (onAnimationComplete != null)
        //    onAnimationComplete.Invoke(state, trackIndex, loopCount, currentAnimation);
        Debug.Log(trackEntry.TrackIndex + " " + trackEntry.Animation.Name);
    }
    public void RemoveEvent()
    {
        onAnimationComplete = null;
        onAnimationEvent = null;
        //if(spine.da)       
        if (spine.Skeleton.data.events.Count != 0)
            spine.state.Event -= OnAnimationEvent;
        spine.state.Complete -= OnAnimationComplete;

        hasEvent = false;
    }

    public void AddEventSpine()
    {
        if (!hasEvent)
        {
            spine.state.Event += OnAnimationEvent;
            spine.state.Complete += OnAnimationComplete;
            hasEvent = true;
        }
    }
    #endregion
    #region ENUMS
    /// <summary>
    /// Animations character have
    /// </summary>
    public enum SpineAnimationsEnum
    {
        Idle = 1,
        Move = 2,
        Stun = 3,
        Dead = 4,
        Attack_1 = 5,
        Spell_1 = 6,
    }
    #endregion
    #region PROPERTIES
    /// <summary>
    /// Get or set timescale of all animations
    /// <para>The rate at which animations progress over time. 1 means 100%. 0.5 means 50%.</para>
    /// </summary>
    public float OverallTimescale
    {
        get { return spine.timeScale; }
        set
        {
            if (spine.timeScale != value)
            {
                spine.timeScale = value;
                OnOverallTimescaleChanged();
            }
        }
    }
    public SkeletonAnimation Spine { get { return this.spine; } }
    /// <summary>
    /// Get or set which way spine face to
    /// <para>default face to left, which is false</para>
    /// </summary>
    public bool FlipX
    {
        get { return spine.Skeleton.FlipX; }
        set
        {
            if (spine.Skeleton.FlipX != value)
            {
                spine.Skeleton.FlipX = value;
                OnFlipped();
            }
        }
    }
    public Vector3 ProjectileLaunchPosition
    {
        get { return this.spine.Skeleton.FindBone("ProjectileLaunchPos").GetWorldPosition(this.transform); }
    }
    public int Sorting
    {
        get { return this.meshRenderer.sortingOrder; }
        set { this.meshRenderer.sortingOrder = value; }
    }
    public MeshRenderer MeshRenderer { get { return this.meshRenderer; } }
    public SpineAnimationsEnum CurrentAnimations { get { return this.currentAnimation; } }
    public Color Color
    {
        get
        {
            return new Color(this.spine.skeleton.r, this.spine.skeleton.g, this.spine.skeleton.b, this.spine.skeleton.a);
        }
        set
        {
            this.spine.skeleton.r = value.r;
            this.spine.skeleton.g = value.g;
            this.spine.skeleton.b = value.b;
            this.spine.skeleton.a = value.a;
        }
    }
    #endregion
    #region MONOBEHAVIOR
    private void Start()
    {
        if (spine == null)
            spine = GetComponent<SkeletonAnimation>();
        if (this.meshRenderer == null)
            this.meshRenderer = GetComponent<MeshRenderer>();
        spine.state.Event += OnAnimationEvent;
        spine.state.Complete += OnAnimationComplete;
        hasEvent = true;
    }
    private void OnDisable()
    {
        spine.state.Event -= OnAnimationEvent;
        spine.state.Complete -= OnAnimationComplete;
        hasEvent = false;
    }
    #endregion
    #region METHODS
    /// <summary>
    /// Play selected animation
    /// </summary>
    /// <param name="animation">animation name to play</param>
    /// <param name="lastTime">the last time the animation was applied</param>
    /// <param name="time">the time in duration to start animation at</param>
    /// <param name="isLoop">should animation loop</param>
    /// <param name="trackIndex">track to play animation in</param>
    /// <param name="individualTimeScale">time scale of this animation</param>
    public void Play(string animation, float lastTime = 0, float time = 0, bool isLoop = false, int trackIndex = 0, float individualTimeScale = 1f)
    {
        SpineAnimationsEnum anim = (SpineAnimationsEnum)Enum.Parse(typeof(SpineAnimationsEnum), animation);
        this.Play(anim, lastTime, time, isLoop, trackIndex, individualTimeScale);
    }
    /// <summary>
    /// Play selected animation
    /// </summary>
    /// <param name="animation">animation to play</param>
    /// <param name="lastTime">the last time the animation was applied</param>
    /// <param name="time">the time in duration to start animation at</param>
    /// <param name="isLoop">should animation loop</param>
    /// <param name="trackIndex">track to play animation in</param>
    /// <param name="individualTimeScale">time scale of this animation</param>
    public void Play(SpineAnimationsEnum animation, float lastTime = 0, float time = 0, bool isLoop = false, int trackIndex = 0, float individualTimeScale = 1f)
    {
        if (currentAnimation == animation)
            return;
        lastAnimation = currentAnimation;
        currentAnimation = animation;
        //if (animation != Animations.hit)
        //{
        //    if (currentAnimation == animation)
        //        return;
        //    lastAnimation = currentAnimation;
        //    currentAnimation = animation;
        //}
        //else
        //{
        //    if (currentAnimation != Animations.idle || currentAnimation != Animations.move || currentAnimation != Animations.up_move)
        //        return;
        //}
        //Spine.Animation curAnimation = null;
        //if (lastAnimation != currentAnimation) {
        //    if (spine.state.GetCurrent(trackIndex) != null) {
        //        spine.state.GetCurrent(trackIndex).Loop = false;
        //        curAnimation = spine.state.GetCurrent(trackIndex).Animation;
        //    }
        //} else return;
        Spine.Animation loadedAnim = getSpineAnimation(animation);
        //loadedAnim.Name(spine.Skeleton, lastTime, time, isLoop, null, alphaMix);
        if (lastAnimation != currentAnimation)
            //if (lastAnimation != Animations.idle
            //    && lastAnimation != Animations.move
            //    && lastAnimation != Animations.knock_back
            //    && lastAnimation != Animations.knock_down
            //    && lastAnimation != Animations.stand_up)
            spine.Skeleton.SetToSetupPose();
        //if (trackIndex == Constant.DEFAULT_TRACK_INDEX) {
        spine.state.ClearTrack(trackIndex);
        //}
        //if (curAnimation != null) {
        //    if (lastAnimation == Animations.idle
        //        || lastAnimation == Animations.move
        //        /*|| lastAnimation == Animations.knock_down*/
        //        /*|| lastAnimation == Animations.stand_up*/
        //        /*|| lastAnimation == Animations.stunned*/) {
        //        curAnimation.Apply(spine.skeleton, 0f, 0f, false, null);
        //        spine.state.AddAnimation(trackIndex, curAnimation, false, 0f);
        //    }
        //}
        spine.state.AddAnimation(trackIndex, loadedAnim, isLoop, Time.deltaTime);
        spine.state.GetCurrent(trackIndex).TimeScale = individualTimeScale;
    }
    public Spine.Animation getSpineAnimation(SpineAnimationsEnum animation)
    {
        // check in cached
        Spine.Animation load = cachedAnimations.Find(a => a.Name == animation.ToString());
        if (load == null)
        {
            // load from spine
            load = spine.Skeleton.Data.FindAnimation(animation.ToString());
            if (load == null)
                throw new ArgumentNullException("Can not find animation '" + animation.ToString() + "'" + " on " + name);
        }
        return load;
    }
    #endregion
    private void OnGUI()
    {
        for (int i = 1; i < 7; i++)
        {
            if (GUI.Button(new Rect(Screen.width * i * 0.1f, Screen.height * 0.9f, Screen.width * 0.1f, Screen.height * 0.1f), ((SpineAnimationsEnum)i).ToString()))
            {
                Play((SpineAnimationsEnum)i);
            }
        }
    }
}

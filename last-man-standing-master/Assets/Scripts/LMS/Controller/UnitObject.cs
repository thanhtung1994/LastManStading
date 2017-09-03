using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LMS.Model;

namespace LMS.Battle
{
    public class AttackInfo
    {
        public UnitObject attacker;
        public bool IsCrit;
        public bool IsStunHit;
        public float StunDuration;
        public float AttackDamage;
        public float ReduceSplashByDistance;

        public AttackInfo()
        {

        }

        public AttackInfo(AttackInfo atkInfo)
        {
            attacker = atkInfo.attacker;
            IsCrit = atkInfo.IsCrit;
            IsStunHit = atkInfo.IsStunHit;
            StunDuration = atkInfo.StunDuration;
            AttackDamage = atkInfo.AttackDamage;
            ReduceSplashByDistance = atkInfo.ReduceSplashByDistance;
        }

    }
    public class UnitObject : MonoBehaviour
    {
        public bool DebugUnit;
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (unitScript != null)
            {
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, AttackRange);
                UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, CONSTANT.FindTargetRadius);
            }
        }
#endif
        #region Event Animation
        public Vector3 ShootPosition
        {
            get
            {
                if (unitAnimation.ShootBone != null)
                    return unitAnimation.ShootBone.GetWorldPosition(unitAnimation.transform);
                return transform.position;
            }
        }
        public Vector3 HipPosition
        {
            get
            {
                if (unitAnimation.HipBone != null)
                    return unitAnimation.HipBone.GetWorldPosition(unitAnimation.transform);
                return transform.position;
            }
        }
        public Vector3 HeadEffectPosition
        {
            get
            {
                if (unitAnimation.HeadEffectBone != null)
                    return unitAnimation.HeadEffectBone.GetWorldPosition(unitAnimation.transform);
                return transform.position;
            }
        }
        public void OnHitAttack()
        {
            if (target == null) return;
            if (unitScript == null) return;
            if (target != _tmpTarget) return;
            //Debug.Log("On Hit");
            // For Ranger
            if (unitScript.Type == UnitScript.UnitType.Ranger || unitScript.Type == UnitScript.UnitType.Building)
            {
                //GameObject _prefab = Resources.Load(string.Format(CONSTANT.PathFormatProjectilePrefabs, unitScript.ProjectTypeCode)) as GameObject;
                //GameObject obj = SimplePool.Spawn(_prefab, ShootPosition);
                //obj.transform.SetParent(BattleManager.Instance.transform);

                //obj.GetComponent<ProjectileObject>().FireOnTarget(this, target, unitScript.ProjectileSpeed, lastAttackInfo);

                GameObject _prefab = Resources.Load(string.Format(CONSTANT.PathFormatProjectilePrefabs, unitScript.ProjectTypeCode)) as GameObject;
                //Vector2 startPos = new Vector2(target.transform.position.x, target.transform.position.y + 3);
                GameObject obj = SimplePool.Spawn(_prefab, ShootPosition);
                obj.transform.SetParent(BattleManager.Instance.transform);

                obj.GetComponent<IndieProjectileObject>().FlyToTarget(target, ProjectOnHit, unitScript.ProjectileSpeed);
            }
            // For Melee
            else
            {
                target.ReceiveAttackInfo(lastAttackInfo);

                // Load Passive
                LoadPassiveEffect();
                //VfxImpact = BattleManager.CreateVFX(CONSTANT.VfxImpact);
                //VfxImpact.transform.SetParent(target.transform);
                //VfxImpact.transform.localScale = new Vector3(0.5f, 0.5f);
                //VfxImpact.transform.position = target.HipPosition;
            }
        }

        private void LoadPassiveEffect()
        {
            // Load Passive
            if (!listSkill.IsNullOrEmpty())
            {
                for (int i = 0; i < listSkill.Count; i++)
                {
                    if (listSkill[i].Type == SkillScript.SkillType.Passive)
                    {
                        listSkill[i].OnHitNormalAttack(target);
                        listSkill[i].OnHitNormalAttack(target, lastAttackInfo);
                    }
                }
            }
            // Load abilityEffect
            if (!listEffects.IsNullOrEmpty())
            {
                for (int i = 0; i < listEffects.Count; i++)
                {
                    listEffects[i].OnHitNormalAttack(target);
                    listEffects[i].OnHitNormalAttack(target, lastAttackInfo);
                }
            }
        }

        private void ProjectOnHit(IndieProjectileObject obj)
        {
            obj.TargetUnit.ReceiveAttackInfo(lastAttackInfo);
            LoadPassiveEffect();

            VfxImpact = BattleManager.CreateVFX(CONSTANT.VfxImpact);
            VfxImpact.transform.SetParent(obj.TargetUnit.transform);
            VfxImpact.transform.localScale = new Vector3(0.5f, 0.5f);
            VfxImpact.transform.position = obj.TargetUnit.HipPosition;
        }

        public void OnDead()
        {
            if (!listEffects.IsNullOrEmpty())
            {
                for (int i = listEffects.Count - 1; i >= 0; i--)
                {
                    listEffects[i].OnDead();
                    RemoveEffect(listEffects[i]);
                }
            }
            RemoveStun();
            RemoveKnockUp();
            if (CallBackOnDead != null) CallBackOnDead.Invoke();
            if (DestroyOnDead && !IsPlayerUnit)
            {
                SimplePool.Despawn(this.gameObject);
            }
        }

        public void OnCastSpellEvent()
        {
            if (CastSkillCallBack != null)
            {
                CastSkillCallBack();
                CastSkillCallBack = null;
            }
        }
        #endregion

        public UnityEvent CallBackOnDead;
        public enum UnitState
        {
            Idle,
            Move,
            Attack,
            Skill,
            Die,
            Stun,
            KnockUp,
        }

        private UnitState _unitState = UnitState.Idle;
        public UnitState unitState
        {
            get { return _unitState; }
            set
            {
                if (!IsMoveable && _unitState == UnitState.Move)
                    _unitState = UnitState.Idle;
                else
                    _unitState = value;
                switch (_unitState)
                {
                    case UnitState.Idle:
                        unitAnimation.SetAnimation(UnitAnimation.AnimationState.Idle);
                        break;
                    case UnitState.Move:
                        unitAnimation.SetAnimation(UnitAnimation.AnimationState.Move);
                        break;
                    case UnitState.Attack:
                        //characterAnimation.SetAnimation(CharacterAnimation.AnimationState.Attack);
                        break;
                    case UnitState.Die:
                        unitAnimation.SetAnimation(UnitAnimation.AnimationState.Die);
                        break;
                    case UnitState.Stun:
                        unitAnimation.SetAnimation(UnitAnimation.AnimationState.Stun);
                        break;
                    case UnitState.KnockUp:
                        unitAnimation.SetAnimation(UnitAnimation.AnimationState.KnockUp);
                        break;
                }
            }
        }
        public UnitAnimation unitAnimation;
        [SerializeField]
        private GameObject SelectedMark;
        [SerializeField]
        private GameObject UnitShadow;
        [SerializeField]
        private BattleHpBarObject HpBar;
        /// <summary>
        /// Unit is on manual move state or not, skip auto find target's command
        /// </summary>
        [SerializeField]
        private bool isManualMoving = false;
        [SerializeField]
        private bool isManualCasting = false;
        /// <summary>
        /// Unit is auto find closest target or not
        /// </summary>
        private bool isAutoFindTarget = false;
        public bool IsAutoCastSkill = false;
        public bool IsSelected = false;
        /// <summary>
        /// Define unit is boss or not ( will waiting for enemy move to range attack, then change to isAutoFindTarget = true;
        /// </summary>
        private bool isBossEnemy = false;
        private GameObject VfxImpact;
        private GameObject VfxStun;
        private Vector3 manualMovePosition;
        private CallBack CastSkillCallBack;

        // Debug mới để SerializeField 
        /// <summary>
        /// Target tạm thời trong đòn attack
        /// </summary>
        [SerializeField]
        private UnitObject _tmpTarget;
        [SerializeField]
        private UnitObject target;
        public UnitObject CurrentTarget
        {
            get { return target; }
        }

        private float _tempAngle = 0;
        public float TempAngel
        {
            get { return _tempAngle; }
        }

        #region Variables Stats
        private int _teamIndex = 0;
        public int TeamIndex
        {
            get { return _teamIndex; }
        }
        public string Name
        {
            get
            {
                //if (Data == null) return string.Empty;
                return unitScript.DebugName;
            }
        }
        public int MaxHp { get; set; }
        [SerializeField]
        private int _currentHp = 0;
        public int CurrentHp
        {
            get { return _currentHp; }
            set
            {
                _currentHp = Mathf.Clamp(value, 0, MaxHp);
                if (HpBar != null)
                {
                    HpBar.SetValue(CurrentPercentHP);
                }
            }
        }
        public float CurrentPercentHP
        {
            get
            {
                return Mathf.Clamp((float)CurrentHp / (float)MaxHp, 0, 1);
            }
        }
        public int MaxMp { get; set; }
        [SerializeField]
        private int _currentMp = 0;
        public int CurrentMp
        {
            get { return _currentMp; }
            set { _currentMp = Mathf.Clamp(value, 0, MaxMp); }
        }
        public float CurrentPercentMP
        {
            get
            {
                return Mathf.Clamp((float)CurrentMp / (float)MaxMp, 0, 1);
            }
        }

        private float Attack { get; set; }
        public float AttackModified { get; set; }
        public float AttackPercent { get; set; }
        public float TotalAttack
        {
            get
            {
                return (int)((Attack + AttackModified) * (1 + AttackPercent / 100f));
            }
        }
        private float LightArmor { get; set; }
        public float LightArmorModified { get; set; }
        public float TotalLightArmor
        {
            get
            {
                return LightArmor + LightArmorModified;
            }
        }
        private float AttackSpeed { get; set; }
        public float AttackSpeedModified { get; set; }
        public float TotalAttackSpeed
        {
            get
            {
                return AttackSpeed * (1 + AttackSpeedModified / 100);
            }
        }
        public float TimePerAttack
        {
            get { return CONSTANT.BaseAttackSpeed / TotalAttackSpeed; }
        }
        private float MovementSpeed { get; set; }
        public float MovementSpeedPercentModified { get; set; }
        public float TotalMovementSpeed
        {
            get
            {
                return MovementSpeed * (1 + MovementSpeedPercentModified / 100);
            }
        }
        public float AttackRange { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalChanceModified { get; set; }
        public float TotalCriticalChance
        {
            get { return Mathf.Clamp(CriticalChance + CriticalChanceModified, 0, 100); }
        }
        public float CriticalMultipleModified;
        public float TotalCriticalMultiple
        {
            get { return (GameSetting.BaseCriticalMultiple + CriticalChanceModified) / 100; }
        }
        public List<AbilityEffects> listEffects = new List<AbilityEffects>();
        public List<SkillScript> listSkill = new List<SkillScript>();

        private float _attackCooldown = 0;

        #endregion

        #region Define

        public UnitScript unitScript;
        public PlayerStats playerOwned;
        public bool CanTakePowerUp = false;
        public bool IsPlayerUnit = false;
        private bool _isAlive = false;
        public bool IsAlive
        {
            get { return _isAlive; }
        }
        private float _stunDuration;
        public bool OnStun
        {
            get { return _stunDuration > 0; }
        }
        private float _knockUpDuration;
        public bool OnKnockUp
        {
            get { return _knockUpDuration > 0; }
        }
        public bool OnCC
        {
            get
            {
                if (!listEffects.IsNullOrEmpty())
                {
                    for (int i = 0; i < listEffects.Count; i++)
                    {
                        listEffects[i].Update(Time.deltaTime);
                    }
                    for (int i = 0; i < listEffects.Count; i++)
                    {
                        if (listEffects[i].IsCrowdControl && listEffects[i].EffectDuration > 0)
                            return true;
                    }
                }
                return false;
            }
        }
        private bool IsAttackable
        {
            get
            {
                if (unitScript == null) return false;
                return Attack > 0;
            }
        }
        public bool IsMoveable
        {
            get
            {
                if (unitScript == null) return false;
                return unitScript.MovementSpeed > 0;
            }
        }
        public bool IsFlipable
        {
            get
            {
                return unitScript.Type != UnitScript.UnitType.Building;
            }
        }
        public bool DestroyOnDead
        {
            get
            {
                return unitScript.Type == UnitScript.UnitType.Melee
                    || unitScript.Type == UnitScript.UnitType.Ranger;
            }
        }
        public bool CanMove
        {
            get
            {
                return true;
            }
        }
        public bool CanDosomething
        {
            get { return true; }
        }
        #endregion

        #region MonoBehaviour
        void Start()
        {
            if (UnitShadow == null)
            {
                UnitShadow = transform.Find("shadow").gameObject;
                if (UnitShadow == null)
                {
                    // Create shadow
                }
            }
            if (HpBar == null)
            {
                GameObject obj = BattleManager.CreatePrefab(CONSTANT.PathHpBar);
                obj.transform.SetParent(transform);
                obj.transform.position = HeadEffectPosition - new Vector3(0.5f, -0.5f);
                HpBar = obj.GetComponent<BattleHpBarObject>();
                HpBar.ResetScale();
                if (HpBar != null) HpBar.gameObject.SetActive(playerOwned.IsBotPlayer);
            }
        }
        void Update()
        {
            // ================ IsPlaying or Alive ======================
            if (!BattleManager.IsPlaying) return;
            if (BattleManager.IsCinematic) return;
            if (!IsAlive) return;

            // ================ Timing ========================
            _stunDuration -= Time.deltaTime;
            _knockUpDuration -= Time.deltaTime;
            if (_knockUpDuration <= 0 && unitState == UnitState.KnockUp)
            {
                RemoveKnockUp();
                unitState = UnitState.Idle;
            }
            if (_stunDuration <= 0 && unitState == UnitState.Stun)
            {
                RemoveStun();
                unitState = UnitState.Idle;
            }
            _attackCooldown -= Time.deltaTime;
            if (!listEffects.IsNullOrEmpty())
            {
                for (int i = 0; i < listEffects.Count; i++)
                {
                    listEffects[i].Update(Time.deltaTime);
                }
            }
            if (!listSkill.IsNullOrEmpty())
            {
                for (int i = 0; i < listSkill.Count; i++)
                {
                    listSkill[i].Update(Time.deltaTime);
                }
            }
            // ==============================================

            // ================ Attack - Moving ========================
            if (unitScript == null) return;
            if (!IsAttackable) return;
            //if (defaultTarget != null && !defaultTarget.IsAlive) target = null;
            if (target != null && !target.IsAlive) target = null;
            // ================ On CC ========================
            if (OnKnockUp)
            {
                float rad = (CONSTANT.Time_KnockUp - _knockUpDuration) / CONSTANT.Time_KnockUp * Mathf.PI;
                float axisY = Mathf.Sin(rad);
                //Debug.Log("axisY: " + axisY);
                unitAnimation.transform.localPosition = new Vector2(0, axisY);
                UnitShadow.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (2 - axisY) / 2);
                return;
            }
            if (OnStun) return;
            if (OnCC) return;
            // ================  ========================
            if (unitAnimation.IsOnAnimationCantMove && !isManualMoving) return;
            if (isManualCasting)
            {
                isManualCasting = false;
                return;
            }
            // =========================== Skill ===============================
            if (IsAutoCastSkill || !IsSelected)
            {
                for (int i = 0; i < listSkill.Count; i++)
                {
                    if (listSkill[i].Type == SkillScript.SkillType.Active)
                    {
                        if (listSkill[i].CastCondition() && listSkill[i].IsReadyToUse)
                        {
                            CastSkill(listSkill[i]);
                            //Debug.Log("AutoCast: " + listSkill[i].Name + "-" + i + " -cd: " + listSkill[i].CurrentCooldown);
                            return;
                        }
                    }
                }
            }
            // =========================== Moving ===============================
            if (!isAutoFindTarget && isManualMoving)
            {
                unitState = UnitState.Move;
                MoveToPosition(manualMovePosition);
            }
            else
            {
                if (target == null)
                {
                    if (FindClosestTarget())
                    {
                        MoveAndAttackOnTarget();
                    }
                    else
                    {
                        //if (playerOwned.IsBotPlayer)
                        //{
                        //    unitState = UnitState.Move;
                        //    if (unitState == UnitState.Move && !unitAnimation.IsOnAnimationCantMove)
                        //    {
                        //        //UnitObject _enemy = playerOwned.GetNextDefaultTarget();
                        //        //Vector3 _posTarget = new Vector3(_enemy.transform.position.x, transform.position.y, 0);
                        //        //MoveToPosition(_posTarget);
                        //        //if (unitScript.Type != UnitScript.UnitType.Building)
                        //        //    rigid2D.bodyType = RigidbodyType2D.Dynamic;
                        //        //float step = TotalMovementSpeed * Time.deltaTime;
                        //        //unitAnimation.SetFacingLeft(transform.position.x > _enemy.transform.position.x);
                        //        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(_enemy.transform.position.x, transform.position.y, 0), step);
                        //    }
                        //}
                        //else
                        //{
                        unitState = UnitState.Idle;
                        //}
                    }
                }
                else
                {
                    MoveAndAttackOnTarget();
                }
            }
            // ==============================================
            RefreshAxisZ();
        }

        private void MoveAndAttackOnTarget()
        {
            // Check distance between character & target;
            float _distance = Vector3.Distance(transform.position, target.transform.position);
            float _axisY = Mathf.Abs(transform.position.y - target.transform.position.y);
            if (_distance <= AttackRange && (unitScript.Type == UnitScript.UnitType.Melee ? _axisY <= CONSTANT.MinRangeY : true))
            {
                DoAttack();
            }
            else
            {
                unitState = UnitState.Move;
            }
            if (unitState == UnitState.Move && !unitAnimation.IsOnAnimationCantMove)
            {
                //MoveToPosition(target.transform.position);
                float isRight = transform.position.x - target.transform.position.x > 0 ? 1 : -1;
                Vector3 modifiedPos = new Vector3(target.transform.position.x + isRight * (AttackRange / 4 * 3), target.transform.position.y);
                //Debug.Log(target.transform.position + "-" + modifiedPos);
                MoveToPosition(modifiedPos);
            }
        }

        private void MoveToPosition(Vector3 pos)
        {
            float step = TotalMovementSpeed * Time.deltaTime;
            float dis = Vector2.Distance(transform.position, pos);
            FacingPosition(pos);
            if (step >= Vector2.Distance(transform.position, pos))
            {
                transform.position = pos.ToVector2();
                isManualMoving = false;
                unitState = UnitState.Idle;
            }
            else
            {
                if (dis >= CONSTANT.FindTargetRadius)
                {
                    Vector2 _tmpPos = new Vector2(pos.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, _tmpPos, step);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, pos, step);
                }
            }
            SetTempAngle(pos);
        }

        private void SetTempAngle(Vector2 pos)
        {
            Vector2 relativePos = pos - transform.position.ToVector2();
            _tempAngle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        }

        private void FacingPosition(Vector2 pos)
        {
            unitAnimation.SetFacingLeft(transform.position.x > pos.x);
        }

        public void EndGame()
        {
            unitState = UnitState.Idle;
        }
        #endregion

        private void RefreshAxisZ()
        {
            Vector3 _tmp = transform.position;
            _tmp.z = _tmp.y;
            transform.position = _tmp;
        }

        private AttackInfo lastAttackInfo;
        private void DoAttack()
        {
            unitState = UnitState.Attack;
            FacingPosition(target.transform.position);
            //rigid2D.bodyType = RigidbodyType2D.Static;
            if (_attackCooldown <= 0)
            {
                // Attack Info
                lastAttackInfo = new AttackInfo();
                lastAttackInfo.IsCrit = Random.Range(0, 100) < TotalCriticalChance;
                if (lastAttackInfo.IsCrit)
                {
                    lastAttackInfo.AttackDamage = TotalAttack * TotalCriticalMultiple;
                }
                else
                {
                    lastAttackInfo.AttackDamage = TotalAttack;
                }
                lastAttackInfo.attacker = this;

                bool trigger = false;
                for (int i = 0; i < listSkill.Count; i++)
                {
                    if (listSkill[i].ConditionPassiveNormalAttack())
                    {
                        listSkill[i].TriggerPassiveNormalAttack(this, target);
                        trigger = true;
                    }
                }
                if (trigger)
                {

                }
                else
                {
                    // Animation
                    if (unitScript.unitDefineAnimation != null)
                    {
                        unitAnimation.PlayAnimation(lastAttackInfo.IsCrit ? unitScript.unitDefineAnimation.AniAttackCrit : unitScript.unitDefineAnimation.AniAttack);
                    }
                    else
                    {
                        unitAnimation.PlayAnimation(UnitAnimation.AnimationState.Attack);
                    }
                }

                // Set timer
                _attackCooldown = TimePerAttack;
                _tmpTarget = target;

                SetTempAngle(target.transform.position);
            }
        }

        public void CastSkill(SkillScript skill)
        {
            if (!IsAlive) return;
            if (OnCC) return;
            if (skill.CastCondition())
            {
                if (unitAnimation.HasAnimation(skill.AniName))
                {
                    if (target != null)
                        FacingPosition(target.transform.position);
                    unitAnimation.PlayAnimation(skill.AniName);
                    CastSkillCallBack = () =>
                    {
                        skill.OnCast();
                    };
                }
                else
                {
                    skill.OnCast();
                }
                for (int i = 0; i < listSkill.Count; i++)
                {
                    listSkill[i].TriggerPassiveAfterCastSkill(skill);
                }
            }
        }

        public void CastSkillAtPosition(SkillScript skill, Vector2 pos)
        {
            if (!IsAlive) return;
            if (OnCC) return;
            if (skill.CastCondition())
            {
                if (unitAnimation.HasAnimation(skill.AniName))
                {
                    FacingPosition(pos);
                    unitAnimation.PlayAnimation(skill.AniName);
                    CastSkillCallBack = () =>
                    {
                        skill.OnCastDragSkill(pos);
                    };
                }
                else
                {
                    skill.OnCastDragSkill(pos);
                }
            }
        }

        private bool FindClosestTarget()
        {
            bool onlyTarget = false;
            bool findPriority = false;
            // Only target first
            if (unitScript.OnlyTarget != UnitScript.UnitType.None)
            {
                for (int i = 0; i < BattleManager.ListUnitInBattle.Count; i++)
                {
                    if (BattleManager.ListUnitInBattle[i].TeamIndex != TeamIndex &&
                        BattleManager.ListUnitInBattle[i].unitScript.Type == unitScript.OnlyTarget)
                    {
                        onlyTarget = true;
                    }
                }
            }

            // Check Priority target 2nd
            if (unitScript.PriorityTarget != UnitScript.UnitType.None)
            {
                for (int i = 0; i < BattleManager.ListUnitInBattle.Count; i++)
                {
                    if (BattleManager.ListUnitInBattle[i].TeamIndex != TeamIndex &&
                        BattleManager.ListUnitInBattle[i].unitScript.Type == unitScript.PriorityTarget)
                    {
                        findPriority = true;
                    }
                }
            }

            // Find target
            float _disClosest = Mathf.Infinity;
            float maxDistance = isAutoFindTarget ? Mathf.Infinity : CONSTANT.FindTargetRadius;
            UnitObject _unitClosest = null;
            for (int i = 0; i < BattleManager.ListUnitInBattle.Count; i++)
            {
                UnitObject unit = BattleManager.ListUnitInBattle[i];
                float _dis = Vector2.Distance(transform.position, unit.transform.position);
                //Debug.Log(string.Format("Name: {0} - Distance: {1}", col.gameObject.name, _dis));
                if (_dis <= maxDistance && _dis <= _disClosest)
                {
                    if (IsEnemyAndAlive(unit.gameObject))
                    {
                        if (onlyTarget)
                        {
                            if (unit.unitScript.Type == unitScript.OnlyTarget)
                            {
                                _unitClosest = unit;
                                _disClosest = _dis;
                            }
                        }
                        else if (findPriority)
                        {
                            if (unit.unitScript.Type == unitScript.PriorityTarget)
                            {
                                _unitClosest = unit;
                                _disClosest = _dis;
                            }
                        }
                        else
                        {
                            _unitClosest = unit;
                            _disClosest = _dis;
                        }
                    }
                }
            }
            if (_unitClosest != null)
            {
                //Debug.Log(string.Format("Closest = Name: {0} - Distance: {1}", _colClosest.gameObject.name, _disClosest));
                target = _unitClosest;
                if (_disClosest > AttackRange)
                    unitState = UnitState.Move;
            }
            return _unitClosest != null;
        }

        private bool IsEnemyAndAlive(GameObject obj)
        {
            UnitObject charObj = obj.GetComponent<UnitObject>();
            return charObj.TeamIndex != TeamIndex && charObj.TeamIndex != 0 && charObj.IsAlive;
        }

        private void StopAction()
        {
            unitAnimation.StopAction();
        }

        public void GetTauntByUnit(UnitObject unit)
        {
            if (IsAlive)
            {
                StopAction();
                if (unit != null && unit.IsAlive)
                {
                    target = unit;
                    ShowText("Taunt", CONSTANT.ColorBlue);
                }
                //MoveAndAttackOnTarget();
                //Debug.Log(unit.Name);
            }
        }

        #region Method
        public void SetStopAnimationWhenCinematic(bool active)
        {
            unitAnimation.PauseAnimation(active);
        }
        public void LoadCharacter(UnitScript unit, PlayerStats playerOwned, int teamIndex)
        {
            ClearOldCharacter();

            this.unitScript = unit;
            this.playerOwned = playerOwned;
            int levelScale = unit.LevelUpgrade;
            isAutoFindTarget = playerOwned.IsBotPlayer;
            _teamIndex = teamIndex;
            unitState = UnitState.Idle;
            MaxHp = (int)(unit.Hp * (1 + levelScale * CONSTANT.ScalePerLevel));
            MaxMp = unit.Mp;
            CurrentHp = MaxHp;
            CurrentMp = 0;
            Attack = unit.Attack * (1 + levelScale * CONSTANT.ScalePerLevel);
            LightArmor = unit.Defense * (1 + levelScale * CONSTANT.ScalePerLevel);
            CriticalChance = unit.CriticalChance;
            MovementSpeed = unit.MovementSpeed / CONSTANT.MovementSpeedScale;
            AttackRange = unit.AttackRange / CONSTANT.RangeScale;
            AttackSpeed = unit.AttackSpeed / CONSTANT.AttackSpeedScale;
            _isAlive = true;
            //if (!unitScript.listSkill.IsNullOrEmpty())
            //{
            //    for (int i = 0; i < unitScript.listSkill.Count; i++)
            //    {
            //        listSkill.Add(new SkillScript(unitScript.listSkill[i].IndexSkill, this, unitScript.listSkill[i].LevelSkill));
            //    }
            //}
            if (unitScript.CallBackOnLoadUnit != null)
                this.unitScript.CallBackOnLoadUnit(this);
            unitAnimation.LoadCharacter(this);
            RefreshAnimationAttackSpeed();

            if (HpBar != null) HpBar.gameObject.SetActive(playerOwned.IsBotPlayer);

            RefreshAxisZ();
            // Debug
            gameObject.name = Name;
        }

        private void ClearOldCharacter()
        {
            listSkill.Clear();
            CallBackOnDead.RemoveAllListeners();
        }

        public void RefreshAnimationAttackSpeed()
        {
            unitAnimation.SetAnimationAttackSpeed(TotalAttackSpeed * unitScript.AniAttackTime);
            unitAnimation.SetAnimationMovementSpeed((1 + MovementSpeedPercentModified / 100) * GameSetting.ScaleMovementSpeed);
        }

        public void ReceiveAttackInfo(AttackInfo attackInfo)
        {
            // IsDodge
            // Show something
            //return;
            float _total = Mathf.Clamp(attackInfo.AttackDamage - TotalLightArmor, 0, int.MaxValue);
            _total = _total * Mathf.Clamp(1 - attackInfo.ReduceSplashByDistance, 0, 1);
            TakeDamage((int)_total, attackInfo.attacker);
            if (attackInfo.IsStunHit)
            {
                _stunDuration = _stunDuration > attackInfo.StunDuration ? _stunDuration : attackInfo.StunDuration;
            }
        }

        public void TakeStun(float duration)
        {
            _stunDuration = _stunDuration > duration ? _stunDuration : duration;
            unitState = UnitState.Stun;
            if (VfxStun == null)
            {
                VfxStun = BattleManager.CreateVFX(CONSTANT.VfxStun);
                VfxStun.transform.SetParent(transform);
                VfxStun.transform.position = HeadEffectPosition;
            }
        }

        private void RemoveStun()
        {
            _stunDuration = 0;
            if (VfxStun != null)
            {
                BattleManager.DestroyGameObject(VfxStun);
                VfxStun = null;
            }
        }

        public void TakeKnockUpHit()
        {
            _knockUpDuration = CONSTANT.Time_KnockUp;
            unitState = UnitState.KnockUp;
        }

        private void RemoveKnockUp()
        {
            _knockUpDuration = 0;
            unitAnimation.transform.localPosition = Vector2.zero;
            UnitShadow.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        public void TakeDamage(int damage, UnitObject attacker)
        {
            TakeDamage(damage, attacker, Color.red);
        }

        public void TakeDamage(int damage, UnitObject attacker, Color color)
        {
            int _amount = Mathf.Clamp(damage, CONSTANT.MinDamage, int.MaxValue);
            CurrentHp -= _amount;
            if (_amount > 0)
                ShowText((-_amount).ToString(), color);
            if (CurrentHp <= 0)
            {
                Die(attacker != null ? attacker.playerOwned : null);
            }
        }

        public void TakeDamage(int damage, PlayerStats playerAtk)
        {
            TakeDamage(damage, playerAtk, Color.red);
        }

        public void TakeDamage(int damage, PlayerStats playerAtk, Color color)
        {
            int _amount = Mathf.Clamp(damage, CONSTANT.MinDamage, int.MaxValue);
            CurrentHp -= _amount;
            if (_amount > 0)
                ShowText((-_amount).ToString(), color);
            if (CurrentHp <= 0) Die(playerAtk);
        }

        public void Heal(float amount, bool showText = true)
        {
            int _amount = Mathf.Clamp((int)amount, 0, MaxHp - CurrentHp);
            CurrentHp += _amount;
            if (_amount > 0 && showText)
                ShowText("+" + (_amount).ToString(), Color.green);
        }

        public void ShowText(string text, Color32 color)
        {
            GameObject obj = BattleManager.CreatePrefab(CONSTANT.PathTextDamage);
            obj.transform.SetParent(transform);
            obj.transform.position = HeadEffectPosition;
            obj.GetComponent<TextDamageObject>().ShowText(text, color);
        }

        public void Die(PlayerStats playerAtk)
        {
            _isAlive = false;
            unitState = UnitState.Die;
            //if (unitScript.GoldBonus > 0)
            //{
            //    ShowText("+" + (unitScript.GoldBonus).ToString(), Color.blue);
            //    if (playerAtk != null)
            //        playerAtk.AddGold(unitScript.GoldBonus);
            //}
        }

        public void AddEffect(AbilityEffects effect)
        {
            if (effect == null) return;
            int numberOfStack = 0;
            for (int i = 0; i < listEffects.Count; i++)
            {
                if (listEffects[i].Index == effect.Index && listEffects[i].groupEffect == effect.groupEffect)
                {
                    numberOfStack++;
                    if (listEffects[i].StackMode == StackModeEnum.ResetTime)
                        listEffects[i].EffectDuration = effect.EffectDuration;
                }
            }
            if (numberOfStack < effect.MaxOfStacks)
            {
                listEffects.Add(effect);
                effect.targetUnit = this;
                effect.OnAddAbilityEffects();
            }
        }

        public void RemoveEffect(AbilityEffects ailment)
        {
            ailment.OnRemoveAbilityEffects();
            listEffects.Remove(ailment);
        }
        #endregion

        #region PlayerCommand
        public void CommandMoveToPosition(Vector3 pos)
        {
            manualMovePosition = pos;
            isManualMoving = true;
        }

        public void CommandCastSkill(SkillScript skill)
        {
            if (!IsAlive) return;
            if (OnCC) return;
            if (isManualMoving)
            {
                isManualMoving = false;
            }
            isManualCasting = true;
            CastSkill(skill);
        }

        public void CommandCastSkillAtPosition(SkillScript skill, Vector2 pos)
        {
            if (!IsAlive) return;
            if (OnCC) return;
            if (isManualMoving)
            {
                isManualMoving = false;
            }
            isManualCasting = true;
            CastSkillAtPosition(skill, pos);
        }
        #endregion

        public void SetSelected(bool selected)
        {
            if (!IsAlive) return;
            if (OnCC) return;
            IsSelected = selected;
            SelectedMark.SetActive(selected);
        }
    }

}
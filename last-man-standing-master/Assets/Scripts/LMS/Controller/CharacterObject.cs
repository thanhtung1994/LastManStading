//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CharacterObject : MonoBehaviour {

//    #region Event Animation
//    public void OnHitAttack()
//    {
//        if (target == null) return;
//        if (target != _tmpTarget) return;
//        //Debug.Log("On Hit");
//        if (unitScript.Type == UnitScript.UnitType.Ranger || unitScript.Type == UnitScript.UnitType.Building)
//        {
//            Transform shootpoint = null;
//            if (unitScript.Type == UnitScript.UnitType.Building)
//            {
//                //GetComponent<TurretFacingAtTarget>().SetFacingTarGet(target.transform.position);
//                shootpoint = GetComponent<TurretFacingAtTarget>().Source.FindChild("ShootPoint");
//            }
//            GameObject obj = Instantiate(Resources.Load(string.Format("Prefabs/Projectiles/{0}", unitScript.ProjectTypeCode)) as GameObject);
//            obj.transform.SetParent(BattleManager.Instance.transform);
//            obj.transform.position = shootpoint == null ? transform.position : shootpoint.position;

//            obj.GetComponent<ProjectileObject>().FireOnTarget(this, target, TotalAttackSpeed, lastAttackInfo);

//        }
//        else
//        {
//            target.ReceiveAttackInfo(lastAttackInfo);
//        }

//        // Load Passive
//        if (!listSkill.IsNullOrEmpty())
//        {
//            for (int i = 0; i < listSkill.Count; i++)
//            {
//                if (listSkill[i].Type == SkillScript.SkillType.Passive)
//                {
//                    listSkill[i].OnHitAttack(target);
//                }
//            }
//        }
//    }

//    public void OnDead()
//    {
//        if (!listEffects.IsNullOrEmpty())
//        {
//            for (int i = 0; i < listEffects.Count; i++)
//            {
//                listEffects[i].OnDead();
//            }
//        }

//        if (CallBackOnDead != null) CallBackOnDead.Invoke();
//        if (DestroyOnDead)
//            Destroy(gameObject);
//    }

//    public void OnCastSpellEvent()
//    {
//        _skillOnCasting.OnCast();
//        _skillOnCasting = null;
//    }
//    #endregion

//    public UnityEvent CallBackOnDead;
//    public enum UnitState
//    {
//        Idle,
//        Move,
//        Attack,
//        Skill,
//        Die,
//        Stun,
//    }

//    private UnitState _unitState = UnitState.Idle;
//    public UnitState unitState
//    {
//        get { return _unitState; }
//        set
//        {
//            if (!IsMoveable && _unitState == UnitState.Move)
//                _unitState = UnitState.Idle;
//            else
//                _unitState = value;
//            switch (_unitState)
//            {
//                case UnitState.Idle:
//                    unitAnimation.SetAnimation(UnitAnimation.AnimationState.Idle);
//                    break;
//                case UnitState.Move:
//                    unitAnimation.SetAnimation(UnitAnimation.AnimationState.Move);
//                    break;
//                case UnitState.Attack:
//                    //characterAnimation.SetAnimation(CharacterAnimation.AnimationState.Attack);
//                    break;
//                case UnitState.Die:
//                    unitAnimation.SetAnimation(UnitAnimation.AnimationState.Die);
//                    break;
//                case UnitState.Stun:
//                    unitAnimation.SetAnimation(UnitAnimation.AnimationState.Stun);
//                    break;
//            }
//        }
//    }

//    public UnitAnimation unitAnimation
//    {
//        get { return GetComponent<UnitAnimation>(); }
//    }

//    private Rigidbody2D rigid2D
//    {
//        get { return GetComponent<Rigidbody2D>(); }
//    }

//    #region Variables Stats
//    public UnitScript unitScript;
//    public PlayerStats playerOwned;
//    public bool IsAlive = false;
//    private int _teamIndex = 0;
//    public int TeamIndex
//    {
//        get { return _teamIndex; }
//    }
//    public string Name
//    {
//        get
//        {
//            //if (Data == null) return string.Empty;
//            return unitScript.DebugName;
//        }
//    }
//    public int MaxHp;
//    [SerializeField]
//    private int _currentHp = 0;
//    public int CurrentHp
//    {
//        get { return _currentHp; }
//        set { _currentHp = Mathf.Clamp(value, 0, MaxHp); }
//    }
//    public float CurrentPercentHP
//    {
//        get
//        {
//            return Mathf.Clamp((float)CurrentHp / (float)MaxHp, 0, 1);
//        }
//    }

//    private float LightAttack { get; set; }
//    public float LightAttackModified { get; set; }
//    public float LightAttackPercent { get; set; }
//    public float TotalLightAttack
//    {
//        get
//        {
//            return (int)((LightAttack + LightAttackModified) * (1 + LightAttackPercent / 100f));
//        }
//    }

//    private float HeavyAttack { get; set; }
//    public float HeavyAttackModified { get; set; }
//    public float HeavyAttackPercent { get; set; }
//    public float TotalHeavyAttack
//    {
//        get
//        {
//            return (int)((HeavyAttack + HeavyAttackModified) * (1 + HeavyAttackPercent / 100f));
//        }
//    }
//    private float LightArmor { get; set; }
//    public float LightArmorModified { get; set; }
//    public float TotalLightArmor
//    {
//        get
//        {
//            return LightArmor + LightArmorModified;
//        }
//    }
//    private float HeavyArmor { get; set; }
//    public float HeavyArmorModified { get; set; }
//    public float TotalHeavyArmor
//    {
//        get
//        {
//            return HeavyArmor + HeavyArmorModified;
//        }
//    }
//    private float AttackSpeed { get; set; }
//    public float AttackSpeedModified { get; set; }
//    public float TotalAttackSpeed
//    {
//        get
//        {
//            return AttackSpeed * (1 + AttackSpeedModified / 100);
//        }
//    }
//    public float TimePerAttack
//    {
//        get { return CONSTANT.BaseAttackSpeed / TotalAttackSpeed; }
//    }
//    private float MovementSpeed { get; set; }
//    public float MovementSpeedPercentModified { get; set; }
//    public float TotalMovementSpeed
//    {
//        get
//        {
//            return MovementSpeed * (1 + MovementSpeedPercentModified / 100);
//        }
//    }
//    public float AttackRange { get; set; }
//    public float CriticalChance { get; set; }
//    public float CriticalChanceModified { get; set; }
//    public float TotalCriticalChance
//    {
//        get { return Mathf.Clamp(CriticalChance + CriticalChanceModified, 0, 100); }
//    }
//    public float CriticalMultipleModified;
//    public float TotalCriticalMultiple
//    {
//        get { return (GameSetting.BaseCriticalMultiple + CriticalChanceModified) / 100; }
//    }
//    public List<AbilityEffects> listEffects = new List<AbilityEffects>();
//    public List<SkillScript> listSkill = new List<SkillScript>();

//    private float _attackCooldown = 0;
//    // Debug mới để SerializeField 
//    [SerializeField]
//    private UnitObject _tmpTarget;
//    [SerializeField]
//    private UnitObject target;
//    [SerializeField]
//    private UnitObject defaultTarget;

//    private float _stunDuration;
//    private bool onStun
//    {
//        get { return _stunDuration > 0; }
//    }
//    #endregion

//    #region Define
//    private bool IsAttackable
//    {
//        get
//        {
//            if (unitScript == null) return false;
//            return HeavyAttack > 0 || LightAttack > 0;
//        }
//    }
//    public bool IsMoveable
//    {
//        get
//        {
//            if (unitScript == null) return false;
//            return unitScript.MovementSpeed > 0;
//        }
//    }
//    public bool IsFlipable
//    {
//        get
//        {
//            return unitScript.Type != UnitScript.UnitType.Building;
//        }
//    }
//    public bool DestroyOnDead
//    {
//        get
//        {
//            return unitScript.Type == UnitScript.UnitType.Melee
//                || unitScript.Type == UnitScript.UnitType.Ranger;
//        }
//    }
//    public bool CanMove
//    {
//        get
//        {
//            return true;
//        }
//    }
//    public bool CanDosomething
//    {
//        get { return true; }
//    }
//    #endregion

//    #region MonoBehaviour
//    void Start()
//    {

//    }
//    void Update()
//    {
//        // ================ IsPlaying or Alive ======================
//        if (!BattleManager.IsPlaying) return;
//        if (!IsAlive) return;

//        // ================ Timing ========================
//        _stunDuration -= Time.deltaTime;
//        if (_stunDuration <= 0 && unitState == UnitState.Stun)
//        {
//            unitState = UnitState.Idle;
//        }
//        _attackCooldown -= Time.deltaTime;
//        bool onCC = false;
//        if (!listEffects.IsNullOrEmpty())
//        {
//            for (int i = 0; i < listEffects.Count; i++)
//            {
//                listEffects[i].Update(Time.deltaTime);
//            }
//            for (int i = 0; i < listEffects.Count; i++)
//            {
//                if (listEffects[i].IsCrowdControl && listEffects[i].EffectDuration > 0)
//                    onCC = true;
//            }
//        }
//        if (!listSkill.IsNullOrEmpty())
//        {
//            for (int i = 0; i < listSkill.Count; i++)
//            {
//                listSkill[i].Update(Time.deltaTime);
//            }
//        }
//        // ==============================================

//        // ================ Behaviour Attack - Moving ========================
//        if (unitScript == null) return;
//        if (!IsAttackable) return;
//        //if (defaultTarget != null && !defaultTarget.IsAlive) target = null;
//        if (target != null && !target.IsAlive) target = null;
//        if (onStun) return;
//        if (onCC) return;
//        // =========================== Skill ===============================
//        //for (int i = 0; i < listSkill.Count; i++)
//        //{
//        //    if (listSkill[i].Type == SkillScript.SkillType.Active)
//        //    {
//        //        if (listSkill[i].specified.CastCondition(this) && listSkill[i].IsReadyToUse)
//        //        {
//        //            listSkill[i].OnCast();
//        //            return;
//        //        }
//        //    }
//        //}
//        // =========================== Behaviour ===============================
//        if (unitAnimation.IsOnAnimationCantMove) return;
//        for (int i = 0; i < listSkill.Count; i++)
//        {
//            if (listSkill[i].Type == SkillScript.SkillType.Active)
//            {
//                if (listSkill[i].CastCondition() && listSkill[i].IsReadyToUse)
//                {
//                    CastSkill(listSkill[i]);
//                    return;
//                }
//            }
//        }
//        if (target == null)
//        {
//            if (FindClosestTarget())
//            {
//                MoveAndAttackOnTarget();
//            }
//            else
//            {
//                unitState = UnitState.Move;
//                if (unitState == UnitState.Move && !unitAnimation.IsOnAnimationCantMove)
//                {
//                    UnitObject _enemy = playerOwned.GetNextDefaultTarget();
//                    if (unitScript.Type != UnitScript.UnitType.Building)
//                        rigid2D.bodyType = RigidbodyType2D.Dynamic;
//                    float step = TotalMovementSpeed * Time.deltaTime;
//                    unitAnimation.SetFacingLeft(transform.position.x > _enemy.transform.position.x);
//                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(_enemy.transform.position.x, transform.position.y, 0), step);
//                }

//            }
//        }
//        else
//        {
//            MoveAndAttackOnTarget();
//        }
//        // ==============================================
//        RefreshAxisZ();
//    }

//    private void MoveAndAttackOnTarget()
//    {
//        // Check distance between character & target;
//        float _distance = Vector3.Distance(transform.position, target.transform.position);
//        if (_distance <= AttackRange)
//        {
//            DoAttack();
//        }
//        else
//        {
//            unitState = UnitState.Move;
//        }
//        if (unitState == UnitState.Move && !unitAnimation.IsOnAnimationCantMove)
//        {
//            if (unitScript.Type != UnitScript.UnitType.Building)
//                rigid2D.bodyType = RigidbodyType2D.Dynamic;
//            float step = TotalMovementSpeed * Time.deltaTime;
//            unitAnimation.SetFacingLeft(transform.position.x > target.transform.position.x);
//            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

//            FindClosestTarget();
//        }
//    }

//    public void EndGame()
//    {
//        unitState = UnitState.Idle;
//    }
//    #endregion

//    private void RefreshAxisZ()
//    {
//        Vector3 _tmp = transform.position;
//        _tmp.z = _tmp.y;
//        transform.position = _tmp;
//    }

//    private AttackInfo lastAttackInfo;
//    private void DoAttack()
//    {
//        unitState = UnitState.Attack;
//        unitAnimation.SetFacingLeft(transform.position.x > target.transform.position.x);
//        //rigid2D.bodyType = RigidbodyType2D.Static;
//        if (_attackCooldown <= 0)
//        {
//            // Attack Info
//            lastAttackInfo = new AttackInfo();
//            lastAttackInfo.IsCrit = Random.Range(0, 100) < TotalCriticalChance;
//            if (lastAttackInfo.IsCrit)
//            {
//                lastAttackInfo.LightAttack = TotalLightAttack * TotalCriticalMultiple;
//                lastAttackInfo.HeavyAttack = TotalHeavyAttack * TotalCriticalMultiple;
//            }
//            else
//            {
//                lastAttackInfo.LightAttack = TotalLightAttack;
//                lastAttackInfo.HeavyAttack = TotalHeavyAttack;
//            }
//            lastAttackInfo.attacker = this;
//            unitAnimation.PlayAnimation(UnitAnimation.AnimationState.Attack);
//            _attackCooldown = TimePerAttack;
//            _tmpTarget = target;
//        }
//    }

//    private SkillScript _skillOnCasting = null;
//    private void CastSkill(SkillScript skill)
//    {
//        if (unitAnimation.HasAnimation(CONSTANT.AniSpell))
//        {
//            unitAnimation.PlayAnimation(UnitAnimation.AnimationState.Spell);
//            _skillOnCasting = skill;
//        }
//        else
//        {
//            skill.OnCast();
//        }
//    }

//    private bool FindClosestTarget()
//    {
//        bool onlyTarget = false;
//        bool findPriority = false;
//        // Only target first
//        if (unitScript.OnlyTarget != UnitScript.UnitType.None)
//        {
//            for (int i = 0; i < BattleManager.ListUnitInBattle.Count; i++)
//            {
//                if (BattleManager.ListUnitInBattle[i].TeamIndex != TeamIndex &&
//                    BattleManager.ListUnitInBattle[i].unitScript.Type == unitScript.OnlyTarget)
//                {
//                    onlyTarget = true;
//                }
//            }
//        }

//        // Check Priority target 2nd
//        if (unitScript.PriorityTarget != UnitScript.UnitType.None)
//        {
//            for (int i = 0; i < BattleManager.ListUnitInBattle.Count; i++)
//            {
//                if (BattleManager.ListUnitInBattle[i].TeamIndex != TeamIndex &&
//                    BattleManager.ListUnitInBattle[i].unitScript.Type == unitScript.PriorityTarget)
//                {
//                    findPriority = true;
//                }
//            }
//        }

//        // Find target
//        float _disClosest = Mathf.Infinity;
//        Collider2D _colClosest = null;
//        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position.ToVector2(), CONSTANT.FindTargetRadius);
//        foreach (Collider2D col in hits)
//        {
//            if (BattleManager.ColliderIsCharacter(col, this.gameObject))
//            {
//                float _dis = Vector3.Distance(transform.position, col.transform.position);
//                //Debug.Log(string.Format("Name: {0} - Distance: {1}", col.gameObject.name, _dis));
//                if (_dis <= _disClosest)
//                {
//                    if (IsEnemyAndAlive(col.gameObject))
//                    {
//                        if (onlyTarget)
//                        {
//                            if (col.gameObject.GetComponent<UnitObject>().unitScript.Type == unitScript.OnlyTarget)
//                            {
//                                _colClosest = col;
//                                _disClosest = _dis;
//                            }
//                        }
//                        else if (findPriority)
//                        {
//                            if (col.gameObject.GetComponent<UnitObject>().unitScript.Type == unitScript.PriorityTarget)
//                            {
//                                _colClosest = col;
//                                _disClosest = _dis;
//                            }
//                        }
//                        else
//                        {
//                            _colClosest = col;
//                            _disClosest = _dis;
//                        }
//                    }
//                }
//            }
//        }
//        if (_colClosest != null)
//        {
//            //Debug.Log(string.Format("Closest = Name: {0} - Distance: {1}", _colClosest.gameObject.name, _disClosest));
//            target = _colClosest.GetComponent<UnitObject>();
//            if (_disClosest > AttackRange)
//                unitState = UnitState.Move;
//        }
//        return _colClosest != null;
//    }

//    private bool IsEnemyAndAlive(GameObject obj)
//    {
//        UnitObject charObj = obj.GetComponent<UnitObject>();
//        return charObj.TeamIndex != TeamIndex && charObj.TeamIndex != 0 && charObj.IsAlive;
//    }

//    //private void MoveToTarget()
//    //{

//    //}

//    #region Method
//    public void LoadCharacter(UnitScript unit, PlayerStats playerOwned, int teamIndex)
//    {
//        this.unitScript = unit;
//        this.playerOwned = playerOwned;
//        int levelScale = unit.LevelUpgrade;
//        _teamIndex = teamIndex;
//        unitState = UnitState.Idle;
//        MaxHp = (int)(unit.Hp * (1 + levelScale * CONSTANT.ScalePerLevel));
//        CurrentHp = MaxHp;
//        LightAttack = unit.LightAttack * (1 + levelScale * CONSTANT.ScalePerLevel);
//        HeavyAttack = unit.HeavyAttack * (1 + levelScale * CONSTANT.ScalePerLevel);
//        LightArmor = unit.LightArmor * (1 + levelScale * CONSTANT.ScalePerLevel);
//        HeavyArmor = unit.HeavyArmor * (1 + levelScale * CONSTANT.ScalePerLevel);
//        MovementSpeed = unit.MovementSpeed / CONSTANT.MovementSpeedScale;
//        AttackRange = unit.AttackRange / CONSTANT.RangeScale;
//        AttackSpeed = unit.AttackSpeed / CONSTANT.AttackSpeedScale;
//        IsAlive = true;
//        if (unitScript.CallBackOnLoadUnit != null)
//            this.unitScript.CallBackOnLoadUnit(this);
//        unitAnimation.LoadCharacter(unit);
//        RefreshAnimationAttackSpeed();

//        if (unitScript.Type == UnitScript.UnitType.Building)
//            rigid2D.bodyType = RigidbodyType2D.Static;
//        RefreshAxisZ();
//        // Debug
//        gameObject.name = Name;
//    }

//    public void RefreshAnimationAttackSpeed()
//    {
//        unitAnimation.SetAttackAnimationSpeed(TotalAttackSpeed * unitScript.AttackTime);
//    }

//    public void ReceiveAttackInfo(AttackInfo attackInfo)
//    {
//        // IsDodge
//        // Show something
//        //return;
//        float _total = Mathf.Clamp(attackInfo.LightAttack - TotalLightArmor, 0, int.MaxValue) +
//            Mathf.Clamp(attackInfo.HeavyAttack - TotalHeavyArmor, 0, int.MaxValue);
//        _total = _total * Mathf.Clamp(1 - attackInfo.ReduceSplashByDistance, 0, 1);
//        TakeDamage((int)_total, attackInfo.attacker);
//        if (attackInfo.IsStunHit)
//        {
//            _stunDuration = _stunDuration > attackInfo.StunDuration ? _stunDuration : attackInfo.StunDuration;
//        }
//    }

//    public void TakeStun(float duration)
//    {
//        _stunDuration = _stunDuration > duration ? _stunDuration : duration;
//        unitState = UnitState.Stun;
//    }

//    public void TakeDamage(int damage, UnitObject attacker)
//    {
//        TakeDamage(damage, attacker, Color.red);
//    }

//    public void TakeDamage(int damage, UnitObject attacker, Color color)
//    {
//        int _amount = Mathf.Clamp(damage, CONSTANT.MinDamage, int.MaxValue);
//        CurrentHp -= _amount;
//        if (_amount > 0)
//            ShowText((-_amount).ToString(), color);
//        if (CurrentHp <= 0)
//        {
//            Die(attacker != null ? attacker.playerOwned : null);
//        }
//    }

//    public void TakeDamage(int damage, PlayerStats playerAtk)
//    {
//        TakeDamage(damage, playerAtk, Color.red);
//    }

//    public void TakeDamage(int damage, PlayerStats playerAtk, Color color)
//    {
//        int _amount = Mathf.Clamp(damage, CONSTANT.MinDamage, int.MaxValue);
//        CurrentHp -= _amount;
//        if (_amount > 0)
//            ShowText((-_amount).ToString(), color);
//        if (CurrentHp <= 0) Die(playerAtk);
//    }

//    public void Heal(float amount)
//    {
//        int _amount = Mathf.Clamp((int)amount, 0, MaxHp - CurrentHp);
//        CurrentHp += _amount;
//        if (_amount > 0)
//            ShowText("+" + (_amount).ToString(), Color.green);
//    }

//    public void ShowText(string text, Color32 color)
//    {
//        GameObject obj = Instantiate(Resources.Load("Prefabs/TextDamage") as GameObject);
//        obj.transform.SetParent(transform);
//        obj.transform.localPosition = new Vector3(0, 0.5f, 0);

//        obj.GetComponent<TextDamageObject>().ShowText(text, color);
//    }

//    public void Die(PlayerStats playerAtk)
//    {
//        IsAlive = false;
//        unitState = UnitState.Die;
//        if (unitScript.GoldBonus > 0)
//        {
//            ShowText("+" + (unitScript.GoldBonus).ToString(), Color.blue);
//            if (playerAtk != null)
//                playerAtk.AddGold(unitScript.GoldBonus);
//        }
//    }

//    public void MovingToPosition(UnitObject target)
//    {
//        this.target = target;
//        defaultTarget = target;
//        //if()
//        unitState = UnitState.Move;
//    }

//    public void AddEffect(AbilityEffects effect)
//    {
//        if (effect == null) return;
//        int numberOfStack = 0;
//        for (int i = 0; i < listEffects.Count; i++)
//        {
//            if (listEffects[i].Index == effect.Index && listEffects[i].groupEffect == effect.groupEffect)
//            {
//                numberOfStack++;
//                if (listEffects[i].StackMode == StackModeEnum.ResetTime)
//                    listEffects[i].EffectDuration = effect.EffectDuration;
//            }
//        }
//        if (numberOfStack < effect.MaxOfStacks)
//        {
//            listEffects.Add(effect);
//            effect.targetUnit = this;
//            effect.OnAddAbilityEffects();
//        }
//    }

//    public void RemoveEffect(AbilityEffects ailment)
//    {
//        ailment.OnRemoveAbilityEffects();
//        listEffects.Remove(ailment);
//    }
//    #endregion
//#if UNITY_EDITOR
//    void OnDrawGizmosSelected()
//    {
//        if (unitScript != null)
//        {
//            UnityEditor.Handles.color = Color.red;
//            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, AttackRange);
//            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, CONSTANT.FindTargetRadius);
//        }
//    }
//#endif
//}

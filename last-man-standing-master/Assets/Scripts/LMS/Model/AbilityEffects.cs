using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;
namespace LMS.Model
{
    public enum AbilityEffectsGroup
    {
        None = -1,
        Test = 0,
        Burning = 1,
        Poisoning = 2,
        AuraSpeed = 3,
        CurseSlowAttackSpeed = 21,
        CorpseEffect = 22,
        DamageAura = 23,

        // Power up
        PowerUpAttackPercent = 601,
        PowerUpAttackSpeed = 602,
        PowerUpMovementSpeed = 603,
        PowerUpCriticalChance = 604,
        PowerUpSpeed = 605,
        PowerUpHealOverTime = 606,
    }
    public enum StackModeEnum
    {
        ResetTime = 1,
        DontResetTime = 2,
    }

    public class AbilityEffects
    {
        public SpecifiedAbilityEffects specified;
        public SkillScript sourceSkill;
        public UnitObject targetUnit;
        public UnitObject sourceUnit;

        public string Name { get; set; }
        //public string Id;
        public int Index;
        public AbilityEffectsGroup groupEffect;
        public StackModeEnum StackMode = StackModeEnum.ResetTime;
        public bool IsCrowdControl;
        public float PowerParameter { get; set; }
        public string Description { get; set; }


        private float _effectDuration = 1;
        public float EffectDuration
        {
            get { return _effectDuration; }
            set { _effectDuration = value; }
        }

        private float _timePerInvoke = 1;
        public float TimePerInvoke
        {
            get { return _timePerInvoke; }
            set { _timePerInvoke = value; }
        }

        private int _maxOfStacks = 1;
        public int MaxOfStacks
        {
            get { return _maxOfStacks; }
            set { _maxOfStacks = value; }
        }

        private int _level = 1;
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public int LevelIndex
        {
            get { return Mathf.Clamp(Level - 1, 0, MaxLevel - 1); }
        }

        private int _maxLevel = 1;
        public int MaxLevel
        {
            get { return _maxLevel; }
            set { _maxLevel = value; }
        }

        public AbilityEffects(int index)
        {
            this.sourceUnit = null;
            this.Index = index;
            SetAbilityEffects();
        }

        public AbilityEffects(int index, UnitObject source)
        {
            this.sourceUnit = source;
            this.Index = index;
            SetAbilityEffects();
        }

        public AbilityEffects(int index, UnitObject source, SkillScript sourceSkill)
        {
            this.sourceUnit = source;
            this.sourceSkill = sourceSkill;
            this.Index = index;
            SetAbilityEffects();
        }

        public void SetAbilityEffects()
        {
            switch (Index)
            {
                case 1:
                    specified = new FrostArrowEffect();
                    break;
                case 2:
                    specified = new FireEnchantedEffect();
                    break;
                case 3:
                    specified = new ThunderClapEffect();
                    break;
                case 4:
                    specified = new BrutalStrikesEffect();
                    break;
                case 5:
                   // specified = new BardEchoPowerEffect();
                    break;
                case 6:
                    specified = new PoisonDartEffect();
                    break;
                case 606:
                    specified = new PowerUpHealOverTime();
                    break;
                default:
                    specified = new SpecifiedAbilityEffects();
                    break;
            }
            specified.parent = this;
            specified.Init();
            //Id = ""; // id effect of AbilityEffects
        }

        public void OnAddAbilityEffects()
        {
            specified.OnAddAbilityEffects();
        }

        public void WhenTakeDamage(int amount, UnitObject attacker)
        {
            specified.WhenTakeDamage(amount, attacker);
        }

        public void OnRemoveAbilityEffects()
        {
            specified.OnRemoveAbilityEffects();
        }

        public void OnDead()
        {
            specified.OnDead();
        }

        public void Update(float _deltaTime)
        {
            specified.Update(_deltaTime);
            EffectDuration -= _deltaTime;
            if (EffectDuration <= 0 || (sourceSkill != null && sourceSkill.CurrentCooldown > 0))
            {
                targetUnit.RemoveEffect(this);
            }
        }

        public void OnHitNormalAttack(UnitObject target)
        {
            specified.OnHitNormalAttack(target);
        }

        public void OnHitNormalAttack(UnitObject target, AttackInfo atkInfo)
        {
            specified.OnHitNormalAttack(target, atkInfo);
        }
    }

}
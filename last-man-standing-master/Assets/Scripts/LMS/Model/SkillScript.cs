using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public enum SkillEffectCondition
    {
        Enemy,
        Alias,
        Self,
        IsAlive,
        Character,
        Building,
    }
    public class SkillScript
    {
        public enum SkillTargetType
        {
            Enemy,
            Alias,
            Self,
            NoTarget,
        }

        public enum SkillType
        {
            Active,
            Passive,
            Channeling
        }

        public SpecifiedSkill specified;
        /// <summary>
        /// Skill target unit
        /// </summary>
        public UnitObject targetUnit;
        /// <summary>
        /// Unit owned this skill
        /// </summary>
        public UnitObject sourceUnit;
        public SkillType Type;
        public SkillTargetType TargetType;
        public int Index { get; set; }
        public string Name { get; set; }
        public string IconName { get; set; }
        public string Description { get; set; }
        public string AniName { get; set; }
        public float Cooldown { get; set; }
        public bool CanDrag { get; set; }
        public float AreaOfEffect { get; set; }

        private float _currentCooldown = 0;
        public float CurrentCooldown
        {
            get { return _currentCooldown; }
            set
            {
                if (value > 0)
                    _currentCooldown = value;
                else
                    _currentCooldown = 0;
            }
        }
        public bool IsReadyToUse
        {
            get { return CurrentCooldown <= 0; }
        }

        private int _level = 1;
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        /// <summary>
        /// Level index: Mathf.Clamp(Level - 1, 0, MaxLevel - 1)
        /// </summary>
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

        public SkillScript(int index)
        {
            this.Index = index;
            SetSkill();
        }

        public SkillScript(int index, UnitObject source)
        {
            this.Index = index;
            this.sourceUnit = source;
            SetSkill();
        }

        public SkillScript(int index, UnitObject source, int level)
        {
            this.Index = index;
            this.sourceUnit = source;
            this.Level = level;
            SetSkill();
        }

        public void SetSkill()
        {
            switch (Index)
            {
                case 0:
                    specified = new ComingSoonSkill();
                    break;
                // Berserker
                case 1:
                    specified = new BerserkerRage();
                    break;
                case 2:
                    specified = new BerserkerHealing();
                    break;
                case 3:
                    specified = new BerserkerTaunt();
                    break;
                // Frost Ranger
                case 6:
                    specified = new FrostArrow();
                    break;
                case 7:
                    specified = new FrozenShot();
                    break;
                case 8:
                    specified = new RainOfArrow();
                    break;
                // Fallen Angel
                case 11:
                    specified = new SoulSiphon();
                    break;
                case 12:
                    specified = new DarkBinding();
                    break;
                case 13:
                    specified = new TormentedSoil();
                    break;
                // Blade Master
                case 17:
                    specified = new FireEnchanted();
                    break;
                // Light Sorcerer
                case 21:
                    specified = new EnergySurge();
                    break;
                case 22:
                    specified = new Aquabeam();
                    break;
                case 23:
                    specified = new Armageddon();
                    break;
                // Samurai
                case 26:
                    specified = new SpecifiedSkill();
                    break;
                case 27:
                    specified = new WindSlash();
                    break;
                // Priest
                case 31:
                    specified = new HolyAura();
                    break;
                case 32:
                    specified = new Pray();
                    break;
                case 33:
                    specified = new Starfall();
                    break;
                // Golem
                case 36:
                    specified = new StoneSkin();
                    break;
                case 37:
                    specified = new BrutalStrikes();
                    break;
                case 38:
                    specified = new ThunderClap();
                    break;
                // Bard
                case 41:
                   // specified = new BardEcho();
                    break;
                case 42:
                   // specified = new HealingBeat();
                    break;
                case 43:
                  //  specified = new PowerSong();
                    break;
                // Scout
                case 47:
                   // specified = new PoisonDart();
                    break;
                // Gunslinger
                case 51:
                  //  specified = new DoubleShot();
                    break;



                case 901:
                    specified = new Heal();
                    break;
                case 1001:
                    specified = new ElseNova();
                    break;
                case 1002:
                    specified = new ElseJustice();
                    break;
                default:
                    specified = new SpecifiedSkill();
                    break;
            }
            specified.parent = this;
            specified.Init();
        }

        public void ActiveCooldown()
        {
            CurrentCooldown = Cooldown;
        }

        public bool AutoCastCondition()
        {
            return CastCondition() && specified.AutoCastSkillCondition();
        }

        public bool CastCondition()
        {
            return specified.CastSkillCondition();
        }

        public bool DragCastCondition()
        {
            return specified.DragCastSkillCondition();
        }

        public void OnCast()
        {
            if (CastCondition())
            {
                CurrentCooldown = Cooldown;
                specified.OnCast();
            }
        }
        public void OnCastDragSkill(Vector2 pos)
        {
            if (DragCastCondition())
            {
                CurrentCooldown = Cooldown;
                specified.OnCastDragSkill(pos);
            }
        }

        public void OnHitSkill(UnitObject target)
        {
            if (target != null)
                specified.OnHitSkill(target);
        }

        public void OnHitSkill(Vector3 position)
        {
            specified.OnHitSkill(position.ToVector2());
        }

        public void OnHitNormalAttack(UnitObject target)
        {
            specified.OnHitNormalAttack(target);
        }

        public void OnHitNormalAttack(UnitObject target, AttackInfo attackInfo)
        {
            specified.OnHitNormalAttack(target, attackInfo);
        }

        public void Update(float deltaTime)
        {
            specified.Update(deltaTime);
            if (CurrentCooldown > 0)
                CurrentCooldown -= deltaTime;
        }

        public void OnDead(UnitObject unit)
        {
            specified.OnDead(unit);
        }

        public void TriggerPassiveAfterCastSkill(SkillScript skill)
        {
            specified.TriggerPassiveAfterCastSkill(skill);
        }

        public bool ConditionPassiveNormalAttack()
        {
            return specified.ConditonPassiveNormalAttack();
        }

        public void TriggerPassiveNormalAttack(UnitObject owner, UnitObject target)
        {
            specified.TriggerPassiveNormalAttack(owner,target);
        }
    }


    public class SkillInfo
    {
        public int IndexSkill;
        public int LevelSkill;

        public SkillInfo(int index, int level)
        {
            IndexSkill = index;
            LevelSkill = level;
        }
    }
}
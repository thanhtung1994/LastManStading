using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class ComingSoonSkill : SpecifiedSkill
    {
        public override void Init()
        {
            base.Init();
            parent.Name = "Coming Soon";
            parent.IconName = "skill_unknown";
            parent.MaxLevel = 0;
        }

        public override string GetDescriptionByLevel(int level)
        {
            return "Coming Soon";
        }
    }

    /// <summary>
    /// Index = 104
    /// </summary>
    public class SummonZombee : SpecifiedSkill
    {
        public override void Init()
        {
            base.Init();
            parent.Name = "Summon zombee";
            parent.Type = SkillScript.SkillType.Active;
            parent.TargetType = SkillScript.SkillTargetType.NoTarget;
            parent.Cooldown = 5;
        }

        public override void OnCast()
        {
            base.OnCast();
            Vector3 _pos = parent.sourceUnit.transform.position;
            _pos += parent.sourceUnit.unitAnimation.IsFacingLeft ? new Vector3(-0.5f, -0.1f, 0) : new Vector3(0.5f, -0.1f, 0);
            //BattleManager.Instance.SpawnMultiCreepsAtPosition(new UnitInfo(301, 0, 1), parent.sourceUnit.playerOwned.PlayerIndex, _pos);
        }

        public override void OnHitSkill(UnitObject target)
        {

        }

        public override bool CastSkillCondition()
        {
            return true;
        }
    }

    /// <summary>
    /// Index = 105
    /// </summary>
    public class SlowOnHit : SpecifiedSkill
    {

        public override void Init()
        {
            base.Init();
            parent.Type = SkillScript.SkillType.Passive;
        }

        public override void OnHitNormalAttack(UnitObject target)
        {
            base.OnHitNormalAttack(target);
            AbilityEffects effect = new AbilityEffects(24);
            target.GetComponent<UnitObject>().AddEffect(effect);
        }
    }

    public class HypnotizeUnit : SpecifiedSkill
    {
        public override void Init()
        {
            base.Init();
            parent.Type = SkillScript.SkillType.Channeling;
            parent.Cooldown = 5;
        }
    }

    /// <summary>
    /// Index = 106
    /// </summary>
    public class AuraHealing : SpecifiedSkill
    {
        float[] amount = new float[] { 5, 7, 9 };
        float invokeTime = 1f;
        float _timeSinceLastUpdate = 0;
        float areaOfEffect = 3f;
        public override void Init()
        {
            base.Init();
            parent.Type = SkillScript.SkillType.Passive;
            parent.MaxLevel = 3;
        }

        public override void Update(float deltaTime)
        {
            _timeSinceLastUpdate += deltaTime;
            if (_timeSinceLastUpdate >= invokeTime)
            {
                _timeSinceLastUpdate -= invokeTime;

                // Heal alias
                Collider2D[] hits = Physics2D.OverlapCircleAll(parent.sourceUnit.transform.position.ToVector2(), areaOfEffect);
                foreach (Collider2D col in hits)
                {
                    if (BattleManager.ColliderIsCharacter(col, parent.sourceUnit.gameObject))
                    {
                        if (BattleManager.IsAlias(col.gameObject, parent.sourceUnit.TeamIndex)
                            && BattleManager.IsAlive(col.gameObject))
                            col.GetComponent<UnitObject>().Heal(amount[parent.LevelIndex]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Index = 107
    /// </summary>
    public class CorpseAura : SpecifiedSkill
    {
        float invokeTime = 0.1f;
        float _timeSinceLastUpdate = 0;
        float areaOfEffect = 2f;
        public override void Init()
        {
            base.Init();
            parent.Type = SkillScript.SkillType.Passive;
            parent.MaxLevel = 1;
            parent.Cooldown = 10;
        }

        public override void Update(float deltaTime)
        {
            _timeSinceLastUpdate += deltaTime;
            if (parent.IsReadyToUse && _timeSinceLastUpdate >= invokeTime)
            {
                _timeSinceLastUpdate -= invokeTime;

                // Heal alias
                Collider2D[] hits = Physics2D.OverlapCircleAll(parent.sourceUnit.transform.position.ToVector2(), areaOfEffect);
                foreach (Collider2D col in hits)
                {
                    if (BattleManager.ColliderIsCharacter(col, parent.sourceUnit.gameObject))
                    {
                        if (BattleManager.IsEnemy(col.gameObject, parent.sourceUnit.TeamIndex)
                            && BattleManager.IsAlive(col.gameObject) && !BattleManager.IsBuilding(col.gameObject))
                        {
                            AbilityEffects effect = new AbilityEffects(22, parent.sourceUnit, parent);
                            col.gameObject.GetComponent<UnitObject>().AddEffect(effect);
                        }
                    }
                }
            }
        }

        public override void OnDead(UnitObject unit)
        {
            if (parent.sourceUnit == null) return;
            if (parent.IsReadyToUse)
            {
                UnitObject corpse = BattleManager.Instance.SpawnSingleCreepsAtPosition(unit.unitScript.ToUnitInfo(),
                    parent.sourceUnit.playerOwned.PlayerIndex,
                    unit.transform.position);
                corpse.unitAnimation.SetMainColor(new Color32(100, 100, 100, 255));
                corpse.gameObject.name = unit.Name + " - corpse";
                parent.ActiveCooldown();
            }
        }
    }

    /// <summary>
    /// Index = 108
    /// </summary>
    public class DamageAura : SpecifiedSkill
    {
        float invokeTime = 0.25f;
        float _timeSinceLastUpdate = 0;
        float areaOfEffect = 3f;
        public override void Init()
        {
            base.Init();
            parent.Type = SkillScript.SkillType.Passive;
            parent.MaxLevel = 1;
        }

        public override void Update(float deltaTime)
        {
            _timeSinceLastUpdate += deltaTime;
            if (_timeSinceLastUpdate >= invokeTime)
            {
                _timeSinceLastUpdate -= invokeTime;

                // Alias
                Collider2D[] hits = Physics2D.OverlapCircleAll(parent.sourceUnit.transform.position.ToVector2(), areaOfEffect);
                foreach (Collider2D col in hits)
                {
                    if (BattleManager.ColliderIsCharacter(col, parent.sourceUnit.gameObject))
                    {
                        if (BattleManager.IsAlias(col.gameObject, parent.sourceUnit.TeamIndex)
                            && BattleManager.IsAlive(col.gameObject))
                        {
                            AbilityEffects effect = new AbilityEffects(23, parent.sourceUnit, parent);
                            col.gameObject.GetComponent<UnitObject>().AddEffect(effect);
                        }
                    }
                }
                // Self
                AbilityEffects effect2 = new AbilityEffects(23, parent.sourceUnit, parent);
                parent.sourceUnit.AddEffect(effect2);
            }
        }
    }

    /// <summary>
    /// Index = 109
    /// </summary>
    public class PoisonOnHit : SpecifiedSkill
    {

        public override void Init()
        {
            base.Init();
            parent.Type = SkillScript.SkillType.Passive;
        }

        public override void OnHitNormalAttack(UnitObject target)
        {
            base.OnHitNormalAttack(target);
            AbilityEffects effect = new AbilityEffects(2);
            target.GetComponent<UnitObject>().AddEffect(effect);
        }
    }

    /// <summary>
    /// Index = 110
    /// </summary>
    public class AuraSpeed : SpecifiedSkill
    {
        float invokeTime = 0.25f;
        float _timeSinceLastUpdate = 0;
        float areaOfEffect = 3f;
        public override void Init()
        {
            base.Init();
            parent.Type = SkillScript.SkillType.Passive;
            parent.MaxLevel = 1;
            GameObject obj = GameObject.Instantiate(Resources.Load(string.Format(CONSTANT.PathSkillEffect, "AuraSpeed")) as GameObject);
            //obj.SetActive(false);
            obj.transform.SetParent(parent.sourceUnit.transform);
            obj.transform.localPosition = new Vector3(-0.05f, -0.2f, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.SetActive(true);
        }

        public override void Update(float deltaTime)
        {
            _timeSinceLastUpdate += deltaTime;
            if (_timeSinceLastUpdate >= invokeTime)
            {
                _timeSinceLastUpdate -= invokeTime;

                // Alias
                Collider2D[] hits = Physics2D.OverlapCircleAll(parent.sourceUnit.transform.position.ToVector2(), areaOfEffect);
                foreach (Collider2D col in hits)
                {
                    if (BattleManager.ColliderIsCharacter(col, parent.sourceUnit.gameObject))
                    {
                        if (BattleManager.IsAlias(col.gameObject, parent.sourceUnit.TeamIndex)
                            && BattleManager.IsAlive(col.gameObject))
                        {
                            AbilityEffects effect = new AbilityEffects(3, parent.sourceUnit, parent);
                            col.gameObject.GetComponent<UnitObject>().AddEffect(effect);
                        }
                    }
                }
                // Self
                AbilityEffects effect2 = new AbilityEffects(3, parent.sourceUnit, parent);
                parent.sourceUnit.AddEffect(effect2);
            }
        }
    }

    /// <summary>
    /// Index = 201
    /// </summary>
    public class Bloodjust : SpecifiedSkill
    {
        float findingRange = 3f;
        UnitObject _tmpTarget = null;
        public override void Init()
        {
            base.Init();
            parent.Name = "Bloodjust";
            parent.Type = SkillScript.SkillType.Active;
            parent.Cooldown = 7;
        }

        public override bool CastSkillCondition()
        {
            if (!parent.IsReadyToUse) return false;
            Collider2D[] hits = Physics2D.OverlapCircleAll(parent.sourceUnit.transform.position.ToVector2(), findingRange);
            foreach (Collider2D col in hits)
            {
                if (BattleManager.ColliderIsCharacter(col, parent.sourceUnit.gameObject))
                {
                    if (BattleManager.IsAlias(col.gameObject, parent.sourceUnit.TeamIndex)
                        && BattleManager.IsAlive(col.gameObject))
                    {
                        if (col.gameObject.GetComponent<UnitObject>().CurrentPercentHP <= 0.5f)
                        {
                            if (_tmpTarget != null)
                            {
                                if (_tmpTarget.CurrentPercentHP > col.gameObject.GetComponent<UnitObject>().CurrentPercentHP)
                                {
                                    _tmpTarget = col.gameObject.GetComponent<UnitObject>();
                                }
                            }
                            else
                                _tmpTarget = col.gameObject.GetComponent<UnitObject>();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void OnCast()
        {
            base.OnCast();
            AbilityEffects effect = new AbilityEffects(4, parent.sourceUnit, parent);
            _tmpTarget.AddEffect(effect);
        }
    }

    /// <summary>
    /// Index = 202
    /// </summary>
    public class HealingTeammate : SpecifiedSkill
    {
        float[] amount = new float[] { 50, 70, 90 };
        float findingRange = 3f;
        UnitObject _tmpTarget = null;
        public override void Init()
        {
            base.Init();

            parent.Type = SkillScript.SkillType.Active;
            parent.TargetType = SkillScript.SkillTargetType.Alias;
            parent.Cooldown = 1;
        }

        public override void OnCast()
        {
            base.OnCast();
            _tmpTarget.Heal(amount[parent.LevelIndex]);
            _tmpTarget = null;
        }

        public override bool CastSkillCondition()
        {
            if (!parent.IsReadyToUse) return false;
            Collider2D[] hits = Physics2D.OverlapCircleAll(parent.sourceUnit.transform.position.ToVector2(), findingRange);
            foreach (Collider2D col in hits)
            {
                if (BattleManager.ColliderIsCharacter(col, parent.sourceUnit.gameObject))
                {
                    if (BattleManager.IsAlias(col.gameObject, parent.sourceUnit.TeamIndex)
                        && BattleManager.IsAlive(col.gameObject))
                    {
                        if (col.gameObject.GetComponent<UnitObject>().CurrentPercentHP <= 0.5f)
                        {
                            if (_tmpTarget != null)
                            {
                                if (_tmpTarget.CurrentPercentHP > col.gameObject.GetComponent<UnitObject>().CurrentPercentHP)
                                {
                                    _tmpTarget = col.gameObject.GetComponent<UnitObject>();
                                }
                            }
                            else
                                _tmpTarget = col.gameObject.GetComponent<UnitObject>();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
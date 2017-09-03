using System.Collections;
using System.Collections.Generic;
using LMS.Battle;
namespace LMS.Model
{
    public class StoneSkin : SpecifiedSkill
    {
        public override void Init()
        {
            base.Init();
            parent.Name = "Stone Skin";
            parent.IconName = "skill_golem_1";
            parent.Type = SkillScript.SkillType.Passive;
            parent.MaxLevel = 5;
        }
    }

    public class BrutalStrikes : SpecifiedSkill
    {
        float[] duration = new float[] { 6, 7, 8, 9, 10 };
        float[] damagePercent = new float[] { 0.35f, 0.4f, 0.45f, 0.5f, 0.55f };

        public override void Init()
        {
            base.Init();

            parent.Name = "Brutal Strikes";
            parent.IconName = "skill_golem_2";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 15;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }

        public override void OnCast()
        {
            AbilityEffects effect = new AbilityEffects(4, parent.sourceUnit, parent);
            effect.PowerParameter = damagePercent[parent.LevelIndex];
            effect.EffectDuration = duration[parent.LevelIndex];
            parent.sourceUnit.GetComponent<UnitObject>().AddEffect(effect);
        }
    }

    public class BrutalStrikesEffect : SpecifiedAbilityEffects
    {
        float radius = 0.5f;

        public override void Init()
        {
            base.Init();

            parent.Name = "Brutal Strikes Effect";
            parent.StackMode = StackModeEnum.ResetTime;
            parent.Description = "Brutal Strikes Effect";
        }

        public override void OnHitNormalAttack(UnitObject target, AttackInfo atkInfo)
        {
            List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, target.transform.position, radius);
            AttackInfo _info = new AttackInfo()
            {
                attacker = atkInfo.attacker,
                IsCrit = atkInfo.IsCrit,
                AttackDamage = atkInfo.AttackDamage / 2,
            };
            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i] != target)
                {
                    _list[i].ReceiveAttackInfo(_info);
                }
            }
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackPercent += parent.PowerParameter;
        }
        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackPercent -= parent.PowerParameter;
        }
    }

    public class ThunderClap : SpecifiedSkill
    {
        float radius = 2;
        float[] druration = new float[] { 3, 3, 3, 3, 3 };
        float[] damageBonus = new float[] { 25, 50, 75, 100, 125 };
        float damageScale = 0.5f;

        public override void Init()
        {
            base.Init();

            parent.Name = "Thunder Clap";
            parent.IconName = "skill_golem_3";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 10;
            parent.CanDrag = false;
            parent.AniName = "spell_2";
        }

        public override void OnCast()
        {
            List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, parent.sourceUnit.transform.position, radius);
            for (int i = 0; i < _list.Count; i++)
            {
                float _damage = damageBonus[parent.LevelIndex] + parent.sourceUnit.TotalAttack * damageScale;
                _list[i].TakeDamage((int)_damage, parent.sourceUnit);
                AbilityEffects effect = new AbilityEffects(3, parent.sourceUnit, parent);
                effect.Level = parent.Level;
                _list[i].GetComponent<UnitObject>().AddEffect(effect);
            }
        }
    }

    public class ThunderClapEffect : SpecifiedAbilityEffects
    {
        float[] slowAS = new float[] { -10, -15, -20, -25, -30 };
        float[] slowMS = new float[] { -30, -35, -40, -45, -50 };
        public override void Init()
        {
            base.Init();

            parent.Name = "ThunderClapEffect";
            parent.StackMode = StackModeEnum.ResetTime;
            parent.EffectDuration = 3;
            parent.Description = "ThunderClapEffect";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified += slowAS[parent.LevelIndex];
            parent.targetUnit.MovementSpeedPercentModified += slowMS[parent.LevelIndex];
            parent.targetUnit.RefreshAnimationAttackSpeed();
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified -= slowAS[parent.LevelIndex];
            parent.targetUnit.MovementSpeedPercentModified -= slowMS[parent.LevelIndex];
            parent.targetUnit.RefreshAnimationAttackSpeed();
        }
    }

}
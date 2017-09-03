using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class DoubleStrike : SpecifiedSkill
    {

    }

    public class FireEnchanted : SpecifiedSkill
    {
        float[] bonusDamagePercent = new float[] { 20, 25, 30, 35, 40 };
        float[] bonusDuration = new float[] { 6, 7, 8, 9, 10 };
        public override void Init()
        {
            base.Init();

            parent.Name = "Fire Enchanted";
            parent.IconName = "skill_berserker_3";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 11;
            parent.CanDrag = false;
            parent.AniName = "Spell_1";
        }

        public override void OnCast()
        {
            AbilityEffects effect = new AbilityEffects(2);
            effect.Level = parent.Level;
            effect.PowerParameter = bonusDamagePercent[parent.LevelIndex];
            effect.EffectDuration = bonusDuration[parent.LevelIndex];
            parent.sourceUnit.GetComponent<UnitObject>().AddEffect(effect);

            //List<UnitObject> _list = BattleManager.FindUnitInArc(parent.sourceUnit.TeamIndex, parent.sourceUnit.transform.position,
            //    3, 45, parent.sourceUnit.unitAnimation.IsFacingLeft);
            //for (int i = 0; i < _list.Count; i++)
            //{
            //    Debug.Log("Inside: " + _list[i].name);
            //}
            //List<UnitObject> _list = BattleManager.FindUnitObjectInRadius(x => x.IsAlive && x.TeamIndex == parent.sourceUnit.TeamIndex, parent.sourceUnit.transform.position, 10);
            //for (int i = 0; i < _list.Count; i++)
            //{
            //    Debug.Log("Inside: " + _list[i].name);
            //}
        }
    }

    public class FireEnchantedEffect : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "Fire Enchanted Effect";
            parent.StackMode = StackModeEnum.ResetTime;
            parent.Description = "Fire Enchanted Effect";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackPercent += parent.PowerParameter;
            parent.targetUnit.AttackSpeedModified += parent.PowerParameter;
            parent.targetUnit.MovementSpeedPercentModified += parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.unitAnimation.SetSkin("kiem_do");
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackPercent -= parent.PowerParameter;
            parent.targetUnit.AttackSpeedModified -= parent.PowerParameter;
            parent.targetUnit.MovementSpeedPercentModified -= parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.unitAnimation.SetSkin("kiem_trang");
        }
    }
}
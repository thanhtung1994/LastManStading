using System.Text;
using UnityEngine;
using LMS.Battle;
using System.Collections.Generic;

namespace LMS.Model
{
    public class PoisonDart : SpecifiedSkill
    {

        float[] initialHit = new float[] { 25, 50, 75, 100, 125 };
        float damageScale = 0.15f;
        public override void Init()
        {
            base.Init();

            parent.Name = "Poison Dart";
            parent.IconName = "skill_scout_2";
            parent.Type = SkillScript.SkillType.Active;
            parent.AreaOfEffect = 2;
            parent.MaxLevel = 5;
            parent.Cooldown = 9;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }

        public override bool CastSkillCondition()
        {
            return parent.sourceUnit.CurrentTarget != null;
        }

        public override void OnCast()
        {
            GameObject vfx = BattleManager.CreateVFX("vfx_poison_cloud");
            vfx.transform.SetParent(parent.sourceUnit.CurrentTarget.transform);
            vfx.transform.position = parent.sourceUnit.CurrentTarget.HipPosition;
            vfx.GetComponent<VfxObject>().AutoDestroyAfter(2);

            parent.sourceUnit.CurrentTarget.TakeDamage((int)initialHit[parent.LevelIndex], parent.sourceUnit);

            AbilityEffects effect = new AbilityEffects(5, parent.sourceUnit, parent);
            effect.PowerParameter = damageScale * parent.sourceUnit.TotalAttack;
            parent.sourceUnit.CurrentTarget.AddEffect(effect);

        }
    }

    /// <summary>
    /// Index = 1
    /// </summary>
    public class PoisonDartEffect : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "PoisonDartEffect";
            parent.EffectDuration = 3;
            parent.TimePerInvoke = 0.5f;
            parent.MaxLevel = 3;
            parent.StackMode = StackModeEnum.ResetTime;
        }

        float timeSinceLastUpdate;

        public override void Update(float _deltaTime)
        {
            if (timeSinceLastUpdate >= parent.TimePerInvoke)
            {
                timeSinceLastUpdate -= parent.TimePerInvoke;
                // Text
                float _dmg = parent.PowerParameter * parent.TimePerInvoke;
                parent.targetUnit.TakeDamage((int)_dmg, parent.sourceUnit);
            }

            timeSinceLastUpdate += _deltaTime;
        }
    }
}
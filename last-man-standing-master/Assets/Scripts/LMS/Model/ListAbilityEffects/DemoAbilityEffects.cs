using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class PowerUpAttackPercent : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "PowerUpAttack";
            parent.PowerParameter = 100;
            parent.EffectDuration = 30;
            parent.Description = "Increase Attack Damage";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackPercent += parent.PowerParameter;
            parent.targetUnit.ShowText(parent.Description, Color.green);
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackPercent -= parent.PowerParameter;
        }
    }

    public class PowerUpAttackSpeed : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "PowerUpAttackSpeed";
            parent.PowerParameter = 100;
            parent.EffectDuration = 30;
            parent.Description = "Increase Attack Speed";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified += parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.ShowText(parent.Description, Color.green);
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified -= parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
        }
    }

    public class PowerUpMovementSpeed : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "PowerUpMovementSpeed";
            parent.PowerParameter = 100;
            parent.EffectDuration = 30;
            parent.Description = "Increase Movement Speed";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.MovementSpeedPercentModified += parent.PowerParameter;
            parent.targetUnit.ShowText(parent.Description, Color.green);
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.MovementSpeedPercentModified -= parent.PowerParameter;
        }
    }

    public class PowerUpCriticalChance : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "PowerUpCriticalChance";
            parent.PowerParameter = 50;
            parent.EffectDuration = 30;
            parent.Description = "Increase Critical Chance";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.CriticalChanceModified += parent.PowerParameter;
            parent.targetUnit.ShowText(parent.Description, Color.green);
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.CriticalChanceModified -= parent.PowerParameter;
        }
    }

    public class PowerUpSpeed : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "PowerUpSpeed";
            parent.PowerParameter = 50;
            parent.EffectDuration = 30;
            parent.Description = "Speed Up";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified += parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.MovementSpeedPercentModified += parent.PowerParameter;
            parent.targetUnit.ShowText(parent.Description, Color.green);
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified -= parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.MovementSpeedPercentModified -= parent.PowerParameter;
        }
    }

    public class PowerUpHealOverTime : SpecifiedAbilityEffects
    {
        private float firstTime = 0.2f;
        private float overtime = 0.05f;
        private float duration = 3;

        public override void Init()
        {
            base.Init();

            parent.Name = "PowerUpHealOverTime";
            parent.PowerParameter = 15;
            parent.TimePerInvoke = 0.5f;
            parent.EffectDuration = duration;
            parent.Description = "Regeneration";
        }

        float timeSinceLastUpdate;

        public override void Update(float _deltaTime)
        {
            if (timeSinceLastUpdate >= parent.TimePerInvoke)
            {
                timeSinceLastUpdate -= parent.TimePerInvoke;
                if (parent.targetUnit.IsAlive)
                    parent.targetUnit.Heal((int)(overtime * parent.targetUnit.MaxHp));
            }

            timeSinceLastUpdate += _deltaTime;
        }

        public override void OnAddAbilityEffects()
        {
            if (parent.targetUnit.IsAlive)
                parent.targetUnit.Heal((int)(firstTime * parent.targetUnit.MaxHp));
        }

        public override void OnRemoveAbilityEffects()
        {

        }
    }
    public class SlowOnHitEffect : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "SlowOnHitEffect";
            parent.StackMode = StackModeEnum.ResetTime;
            parent.PowerParameter = -30;
            parent.EffectDuration = 3;
            parent.Description = "Slow";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified += parent.PowerParameter;
            parent.targetUnit.MovementSpeedPercentModified += parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.ShowText(parent.Description, new Color32(255, 0, 255, 255));
            parent.targetUnit.unitAnimation.SetColor(new Color32(100, 100, 255, 255));
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified -= parent.PowerParameter;
            parent.targetUnit.MovementSpeedPercentModified -= parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.unitAnimation.ReturnToMainColor();
        }
    }

    public class CurseSlowAttackSpeed : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "CurseSlowAttackSpeed";
            parent.StackMode = StackModeEnum.ResetTime;
            parent.PowerParameter = -30;
            parent.EffectDuration = 3;
            parent.Description = "Slow";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified += parent.PowerParameter;
            parent.targetUnit.MovementSpeedPercentModified += parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.ShowText(parent.Description, new Color32(255, 0, 255, 255));
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified -= parent.PowerParameter;
            parent.targetUnit.MovementSpeedPercentModified -= parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
        }
    }

    public class Hypnosis : SpecifiedAbilityEffects
    {

        public override void Init()
        {
            base.Init();

            parent.Name = "Hypnosis";
            parent.StackMode = StackModeEnum.ResetTime;
            parent.EffectDuration = 3600;
            parent.Description = "Just stay with me :3";
            parent.IsCrowdControl = true;
        }

        public override void OnAddAbilityEffects()
        {

        }

        public override void OnRemoveAbilityEffects()
        {

        }
    }

    public class CorpseAuraEffect : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            parent.Name = "CorpseAuraEffect";
            parent.StackMode = StackModeEnum.ResetTime;
            parent.EffectDuration = 2;
            parent.Description = "My servent never die";

        }

        public override void OnAddAbilityEffects()
        {

        }

        public override void OnRemoveAbilityEffects()
        {

        }

        public override void OnDead()
        {
            parent.sourceSkill.OnDead(parent.targetUnit);
        }
    }

    public class DamageAuraEffect : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();
            //parent.Id = "damageaura";
            parent.Name = "DamageAuraEffect";
            parent.PowerParameter = 25;
            parent.EffectDuration = 1;
            parent.Description = "Increase Attack";
            parent.StackMode = StackModeEnum.ResetTime;
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackPercent += parent.PowerParameter;
            parent.targetUnit.ShowText(parent.Description, Color.green);
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackPercent -= parent.PowerParameter;
        }
    }

    /// <summary>
    /// Index = 1
    /// </summary>
    public class BurningEffect : SpecifiedAbilityEffects
    {
        int[] dps = new int[] { 10, 15, 20 };
        public override void Init()
        {
            base.Init();

            parent.Name = "BurningEffect";
            parent.EffectDuration = 3;
            parent.TimePerInvoke = 0.5f;
            parent.MaxLevel = 3;
        }

        float timeSinceLastUpdate;

        public override void Update(float _deltaTime)
        {
            if (timeSinceLastUpdate >= parent.TimePerInvoke)
            {
                timeSinceLastUpdate -= parent.TimePerInvoke;
                // Text
                float _dmg = dps[parent.LevelIndex] * parent.TimePerInvoke;
                parent.targetUnit.TakeDamage((int)_dmg, parent.sourceUnit);
            }

            timeSinceLastUpdate += _deltaTime;
        }
    }

    /// <summary>
    /// Index = 2
    /// </summary>
    public class PoisoningEffect : SpecifiedAbilityEffects
    {
        int[] dps = new int[] { 10, 15, 20 };
        public override void Init()
        {
            base.Init();

            parent.Name = "PoisonEffect";
            parent.EffectDuration = 3;
            parent.TimePerInvoke = 0.5f;
            parent.MaxLevel = 3;
        }

        float timeSinceLastUpdate;

        public override void Update(float _deltaTime)
        {
            if (timeSinceLastUpdate >= parent.TimePerInvoke)
            {
                timeSinceLastUpdate -= parent.TimePerInvoke;
                // Text
                float _dmg = dps[parent.LevelIndex] * parent.TimePerInvoke;
                parent.targetUnit.TakeDamage((int)_dmg, parent.sourceUnit, Color.green);
            }

            timeSinceLastUpdate += _deltaTime;
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.ShowText(parent.Description, new Color32(0, 255, 0, 255));
            parent.targetUnit.unitAnimation.SetColor(new Color32(0, 255, 0, 255));
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.unitAnimation.ReturnToMainColor();
        }
    }

    /// <summary>
    /// Index = 3
    /// </summary>
    public class AuraSpeedEffect : SpecifiedAbilityEffects
    {
        public override void Init()
        {
            base.Init();

            parent.Name = "AuraSpeedEffect";
            parent.PowerParameter = 25;
            parent.EffectDuration = 2;
            parent.Description = "Speed Up";
            parent.StackMode = StackModeEnum.ResetTime;
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified += parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.ShowText(parent.Description, Color.green);
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified -= parent.PowerParameter;
            parent.targetUnit.RefreshAnimationAttackSpeed();
        }
    }


    /// <summary>
    /// Index = 4
    /// </summary>
    public class BloodjustEffect : SpecifiedAbilityEffects
    {

        private float _atk = 50;
        private float _as = 50;
        private float _ms = 50;
        public override void Init()
        {
            base.Init();

            parent.Name = "BloodjustEffect";
            parent.PowerParameter = 50;
            parent.EffectDuration = 10;
            parent.Description = "Bloodjust";
            parent.StackMode = StackModeEnum.ResetTime;
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackModified += _atk;
            parent.targetUnit.AttackSpeedModified += _as;
            parent.targetUnit.MovementSpeedPercentModified += _ms;
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.ShowText(parent.Description, Color.red);
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackModified -= _atk;
            parent.targetUnit.AttackSpeedModified -= _as;
            parent.targetUnit.MovementSpeedPercentModified -= _ms;
            parent.targetUnit.RefreshAnimationAttackSpeed();
        }
    }

}
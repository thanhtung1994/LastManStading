using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class PowerUpScript
    {
        public enum PowerUpType
        {
            BonusDamage = 1,
            BonusAttackSpeed = 2,
            BonusMovementSpeed = 3,
            BonusCriticalChance = 4,
            BonusSpeed = 5,
            HealOverTime = 6,
        }

        public PowerUpType type;
        public string Code;
        public PowerUpScript(int index)
        {
            type = (PowerUpType)index;
            switch (type)
            {
                case PowerUpType.BonusDamage:
                    Code = "attack";
                    break;
                case PowerUpType.BonusAttackSpeed:
                    Code = "attackspeed";
                    break;
                case PowerUpType.BonusMovementSpeed:
                    Code = "movementspeed";
                    break;
                case PowerUpType.BonusCriticalChance:
                    Code = "Critical";
                    break;
                case PowerUpType.BonusSpeed:
                    Code = "Speed";
                    break;
                case PowerUpType.HealOverTime:
                    Code = "Regen";
                    break;
            }
        }

        public void OnTriggerPowerUp(UnitObject unit)
        {
            AbilityEffectsGroup effectType;
            AbilityEffects effect;
            switch (type)
            {
                case PowerUpType.BonusDamage:
                    effect = new AbilityEffects(601);
                    break;
                case PowerUpType.BonusAttackSpeed:
                    effect = new AbilityEffects(602);
                    break;
                case PowerUpType.BonusMovementSpeed:
                    effect = new AbilityEffects(603);
                    break;
                case PowerUpType.BonusCriticalChance:
                    effect = new AbilityEffects(604);
                    break;
                case PowerUpType.BonusSpeed:
                    effect = new AbilityEffects(605);
                    break;
                case PowerUpType.HealOverTime:
                    effect = new AbilityEffects(606);
                    break;
                default:
                    effect = null;
                    break;
            }
            unit.AddEffect(effect);
        }

    }
}
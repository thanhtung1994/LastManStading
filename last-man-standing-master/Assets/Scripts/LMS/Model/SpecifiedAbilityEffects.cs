using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class SpecifiedAbilityEffects
    {
        public AbilityEffects parent;

        public virtual void Init()
        {

        }

        public virtual void Update(float _deltaTime)
        {

        }

        public virtual void OnAddAbilityEffects()
        {

        }

        public virtual void OnRemoveAbilityEffects()
        {

        }

        public virtual void WhenTakeDamage(int amount, UnitObject attacker)
        {

        }

        public virtual void OnDead()
        {

        }

        public virtual void OnDodge()
        {

        }

        public virtual void OnHitNormalAttack(UnitObject target)
        {

        }

        public virtual void OnHitNormalAttack(UnitObject target, AttackInfo atkInfo)
        {

        }
    }
}
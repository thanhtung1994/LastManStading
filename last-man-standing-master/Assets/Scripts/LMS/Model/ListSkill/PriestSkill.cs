
using UnityEngine;
using LMS.Battle;
using System.Collections.Generic;

namespace LMS.Model
{
    public class HolyAura : SpecifiedSkill
    {
        public override void Init()
        {
            base.Init();
            parent.Name = "Holy Aura";
            parent.IconName = "skill_priest_1";
            parent.Type = SkillScript.SkillType.Passive;
            parent.MaxLevel = 5;
        }
    }

    public class Pray : SpecifiedSkill
    {
        float[] healAmount = new float[] { 100, 150, 200, 250, 300 };
        float healScale = 0.6f;
        public override void Init()
        {
            base.Init();

            parent.Name = "Pray";
            parent.IconName = "skill_priest_2";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 8;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }

        public override void OnCast()
        {

            List<UnitObject> _list = BattleManager.FindUnitObjectInRadius(TargetCondition, parent.sourceUnit.transform.position, int.MaxValue);
            for (int i = 0; i < _list.Count; i++)
            {
                HealTeammate(_list[i]);
            }
        }

        private bool TargetCondition(UnitObject unit)
        {
            return unit.TeamIndex == parent.sourceUnit.TeamIndex;
        }

        private void HealTeammate(UnitObject unit)
        {
            float amount = healAmount[parent.LevelIndex] + parent.sourceUnit.TotalAttack * healScale;
            unit.Heal(amount);

            //GameObject VfxImpact = BattleManager.CreateVFX(CONSTANT.VfxHealingEffect);
            //VfxImpact.transform.SetParent(unit.transform);
            ////VfxImpact.transform.localScale = new Vector3(0.5f, 0.5f);
            //VfxImpact.transform.position = unit.transform.position;


            GameObject obj = BattleManager.CreateVFX("vfx_priest_pray");
            obj.transform.SetParent(unit.transform);
            obj.transform.position = unit.transform.position;
            obj.GetComponent<VfxObject>().AutoDestroyAfter(1.5f);
        }
    }

    public class Starfall : SpecifiedSkill
    {
        private string effect = "starfall";
        float[] damageBonus = new float[] { 25, 50, 75, 100, 125 };
        float dmgScale = 0.75f;
        float radius = 2;

        public override void Init()
        {
            base.Init();
            parent.Name = "Aquabeam";
            parent.IconName = "skill_priest_3";
            parent.Type = SkillScript.SkillType.Active;
            parent.AreaOfEffect = radius;
            parent.MaxLevel = 5;
            parent.Cooldown = 10;
            parent.CanDrag = true;
            parent.AniName = "spell_2";
        }

        public override bool CastSkillCondition()
        {
            return parent.sourceUnit.CurrentTarget != null;
        }

        public override void OnCast()
        {
            CastStarFallAtPosition(parent.sourceUnit.CurrentTarget.transform.position);
        }

        private void CastStarFallAtPosition(Vector2 pos)
        {
            GameObject starfall = BattleManager.CreatePrefab(string.Format(CONSTANT.PathSkillEffect, effect));
            starfall.transform.parent = BattleManager.Instance.transform;
            starfall.transform.position = pos;

            starfall.GetComponent<SkillEffectObject>().LoadSkill(parent, flip: parent.sourceUnit.unitAnimation.IsFacingLeft);
        }

        public override void OnHitSkill(Vector2 pos)
        {
            List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, parent.sourceUnit.transform.position, radius);
            for (int i = 0; i < _list.Count; i++)
            {
                float _damage = damageBonus[parent.LevelIndex] + parent.sourceUnit.TotalAttack * dmgScale;
                _list[i].TakeDamage((int)_damage, parent.sourceUnit);
            }
        }
    }

}
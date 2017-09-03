using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class BerserkerRage : SpecifiedSkill
    {
        int rageNormal = 5;
        int rageCrit = 10;
        float[] critChancePerRage = new float[] { 0.15f, 0.2f, 0.25f, 0.3f, 0.35f };
        float tmpCritBobnus = 0;
        public override void Init()
        {
            base.Init();

            parent.Name = "Berserker Rage";
            parent.IconName = "skill_berserker_1";
            parent.Type = SkillScript.SkillType.Passive;
            parent.Description = string.Format("Gain {0} rage per hit attack. Double when critical hit", rageNormal);
            parent.MaxLevel = 5;

        }

        public override string GetDescriptionByLevel(int level)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Berserker gains {0} Fury each time he lands a basic attack, increased to {1} Fury on critical strikes, and {1} Fury each time he kills an enemy.\n", rageNormal, rageNormal * 2);
            sb.AppendFormat("Berserker also gains {0}% critical strike chance per point of Fury he currently has, up to a maximum of {1}% critical strike chance.", critChancePerRage[parent.LevelIndex], critChancePerRage[parent.LevelIndex] * 100);
            return sb.ToString();
        }

        public override void OnHitNormalAttack(UnitObject target, AttackInfo attackInfo)
        {
            parent.sourceUnit.CurrentMp += attackInfo.IsCrit ? rageCrit : rageNormal;
            parent.sourceUnit.CriticalChanceModified -= tmpCritBobnus;
            tmpCritBobnus = parent.sourceUnit.CurrentMp * critChancePerRage[parent.LevelIndex];
            parent.sourceUnit.CriticalChanceModified += tmpCritBobnus;
        }


    }

    public class BerserkerHealing : SpecifiedSkill
    {
        float[] minHp = new float[] { 20, 40, 60, 80, 100 };
        float[] hpPerRage = new float[] { 1f, 1.5f, 2f, 2.5f, 3f };
        public override void Init()
        {
            base.Init();

            parent.Name = "Berserker Healing";
            parent.IconName = "skill_berserker_2";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 10;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }

        public override void OnCast()
        {
            parent.sourceUnit.Heal(minHp[parent.LevelIndex] + parent.sourceUnit.CurrentMp * hpPerRage[parent.LevelIndex]);
            parent.sourceUnit.CurrentMp = 0;
        }
    }

    public class BerserkerTaunt : SpecifiedSkill
    {
        float[] radius = new float[] { 3, 3, 3, 3, 3 };
        float[] druration = new float[] { 3, 3, 3, 3, 3 };

        public override void Init()
        {
            base.Init();

            parent.Name = "Berserker Taunt";
            parent.IconName = "skill_berserker_3";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 15;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }

        public override void OnCast()
        {
            for (int i = 0; i < BattleManager.ListUnitInBattle.Count; i++)
            {
                UnitObject unit = BattleManager.ListUnitInBattle[i];
                if (unit.TeamIndex != parent.sourceUnit.TeamIndex && unit.IsAlive)
                {
                    float _dis = Vector2.Distance(parent.sourceUnit.transform.position, unit.transform.position);
                    if (_dis <= radius[parent.LevelIndex])
                    {
                        unit.GetTauntByUnit(parent.sourceUnit);
                        //Debug.Log(unit.Name);
                    }
                }
            }
        }
    }
}
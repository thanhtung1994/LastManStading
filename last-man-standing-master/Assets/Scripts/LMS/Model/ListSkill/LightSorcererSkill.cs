using LMS.Battle;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LMS.Model
{
    public class EnergySurge : SpecifiedSkill
    {
        int[] manaPerHit = new int[] { 2, 3, 4, 5, 6 };
        //float[] critChancePerRage = new float[] { 0.15f, 0.2f, 0.25f, 0.3f, 0.35f };
        //float tmpCritBobnus = 0;
        public override void Init()
        {
            base.Init();

            parent.Name = "Energy Surge";
            parent.IconName = "skill_lightsorcerer_1";
            parent.Type = SkillScript.SkillType.Passive;
            parent.Description = string.Format("Gain mana per hit attack. Double when critical hit");
            parent.MaxLevel = 5;

        }

        public override string GetDescriptionByLevel(int level)
        {
            StringBuilder sb = new StringBuilder();
            int mana = manaPerHit[parent.LevelIndex];
            sb.AppendFormat("Light Sorcerer gains {0} mana each time he lands a basic attack, increased to {1} mana on critical strikes, and {1} mana each time he kills an enemy.\n", mana, mana * 2);
            sb.AppendFormat("Everytime he uses his ability, he will spend all his mana to powerup his skill.");
            return sb.ToString();
        }

        public override void OnHitNormalAttack(UnitObject target, AttackInfo attackInfo)
        {
            int mana = manaPerHit[parent.LevelIndex];
            parent.sourceUnit.CurrentMp += attackInfo.IsCrit ? mana * 2 : mana;
        }


    }

    public class Aquabeam : SpecifiedSkill
    {
        float[] damageBonus = new float[] { 25, 50, 75, 100, 125 };
        float[] damageScale = new float[] { 1, 1, 1, 1, 1 };
        float height = 1;
        float width = 4;
        float offset = 0;
        string effect = "aquabeam";
        public override void Init()
        {
            base.Init();

            parent.Name = "Aquabeam";
            parent.IconName = "skill_lightsorcerer_2";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 10;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }
        public override bool CastSkillCondition()
        {
            return parent.sourceUnit.CurrentTarget != null;
        }

        private bool UnitObjectCondition(UnitObject unit) {
            return unit.TeamIndex == parent.sourceUnit.TeamIndex;
        }

        public override void OnCast()
        {
            GameObject beam = BattleManager.CreatePrefab(string.Format(CONSTANT.PathSkillEffect, effect));
            beam.transform.parent = BattleManager.Instance.transform;
            beam.transform.position = parent.sourceUnit.ShootPosition;
            //beam.transform.localScale = new Vector2(1, 1) * parent.AreaOfEffect * 2f;

            Vector2 relativePos = parent.sourceUnit.CurrentTarget.HipPosition - parent.sourceUnit.HipPosition;
            float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            beam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            beam.GetComponent<SkillEffectObject>().LoadSkill(parent);

            Rect _rect = new Rect(width / 2 + offset, height / 2, width, height);
            List <UnitObject> _list = BattleManager.FindUnitObjectInRect(UnitObjectCondition, parent.sourceUnit.transform.position, _rect);
            for (int i = 0; i < _list.Count; i++)
            {
                float _damage = damageBonus[parent.LevelIndex] + parent.sourceUnit.TotalAttack * damageScale[parent.LevelIndex];
                _damage = _damage * (1 + parent.sourceUnit.CurrentPercentMP);
                _list[i].TakeDamage((int)_damage, parent.sourceUnit);
            }
            parent.sourceUnit.CurrentMp = 0;
        }
    }

    public class Armageddon : SpecifiedSkill
    {
        float[] radius = new float[] { 3, 3, 3, 3, 3 };
        float[] damageBonus = new float[] { 25, 50, 75, 100, 125 };
        float[] damageScale = new float[] { 1, 1, 1, 1, 1 };

        public override void Init()
        {
            base.Init();

            parent.Name = "Armageddon";
            parent.IconName = "skill_lightsorcerer_3";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 15;
            parent.CanDrag = false;
            parent.AniName = "spell_2";
        }

        public override void OnCast()
        {
            List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, parent.sourceUnit.transform.position, radius[parent.LevelIndex]);
            for (int i = 0; i < _list.Count; i++)
            {
                float _damage = damageBonus[parent.LevelIndex] + parent.sourceUnit.TotalAttack * damageScale[parent.LevelIndex];
                _damage = _damage * (1 + parent.sourceUnit.CurrentPercentMP);
                _list[i].TakeDamage((int)_damage, parent.sourceUnit);
            }
            parent.sourceUnit.CurrentMp = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class FrostArrow : SpecifiedSkill
    {
        public override void Init()
        {
            base.Init();
            parent.Name = "Frost Arrow";
            parent.IconName = "skill_frostranger_1";
            parent.Type = SkillScript.SkillType.Passive;
            parent.MaxLevel = 5;
        }

        public override void OnHitNormalAttack(UnitObject target)
        {
            if (target == null) return;
            base.OnHitNormalAttack(target);
            AbilityEffects effect = new AbilityEffects(1);
            effect.Level = parent.Level;
            target.GetComponent<UnitObject>().AddEffect(effect);
        }
    }

    public class FrostArrowEffect : SpecifiedAbilityEffects
    {
        float[] slowAS = new float[] { -10, -15, -20, -25, -30 };
        float[] slowMS = new float[] { -30, -35, -40, -45, -50 };
        public override void Init()
        {
            base.Init();

            parent.Name = "Frost Arrow Slow Effect";
            parent.StackMode = StackModeEnum.ResetTime;
            parent.PowerParameter = -30;
            parent.EffectDuration = 3;
            parent.Description = "Frost Arrow Slow Effect";
        }

        public override void OnAddAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified += slowAS[parent.LevelIndex];
            parent.targetUnit.MovementSpeedPercentModified += slowMS[parent.LevelIndex];
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.unitAnimation.SetColor(new Color32(100, 100, 255, 255));
        }

        public override void OnRemoveAbilityEffects()
        {
            parent.targetUnit.AttackSpeedModified -= slowAS[parent.LevelIndex];
            parent.targetUnit.MovementSpeedPercentModified -= slowMS[parent.LevelIndex];
            parent.targetUnit.RefreshAnimationAttackSpeed();
            parent.targetUnit.unitAnimation.ReturnToMainColor();
        }
    }

    public class FrozenShot : SpecifiedSkill
    {
        float[] radius = new float[] { 1, 1, 1, 1, 1 };
        float[] stunDuration = new float[] { 1f, 1f, 1f, 1f, 1f };
        float[] dmgBonus = new float[] { 3f, 3f, 3f, 3f, 3f };
        string projectile = "frozenshot";
        float speed = 5;
        public override void Init()
        {
            base.Init();

            parent.Name = "Frozen Shot";
            parent.IconName = "skill_frostranger_6";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 10;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }

        public override bool CastSkillCondition()
        {
            if (parent.sourceUnit.CurrentTarget == null) return false;
            if (!parent.IsReadyToUse) return false;
            return true;
        }

        public override void OnCast()
        {
            GameObject _prefab = Resources.Load(string.Format(CONSTANT.PathFormatProjectilePrefabs, projectile)) as GameObject;
            GameObject obj = SimplePool.Spawn(_prefab, parent.sourceUnit.ShootPosition);
            obj.transform.SetParent(BattleManager.Instance.transform);
            //obj.transform.position = shootpoint;

            obj.GetComponent<IndieProjectileObject>().FlyToTarget(parent.sourceUnit.CurrentTarget, ProjectOnHit, speed);
        }

        private void ProjectOnHit(IndieProjectileObject obj)
        {
            List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, obj.transform.position, radius[parent.LevelIndex]);

            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].TakeDamage((int)(dmgBonus[parent.LevelIndex] + parent.sourceUnit.TotalAttack), parent.sourceUnit.playerOwned);
                _list[i].TakeStun(stunDuration[parent.LevelIndex]);
            }
        }
    }

    public class RainOfArrow : SpecifiedSkill
    {
        float[] radius = new float[] { 2, 2, 2, 2, 2 };
        float[] stunDuration = new float[] { 1f, 1f, 1f, 1f, 1f };
        float[] dmgBonus = new float[] { 3f, 3f, 3f, 3f, 3f };
        string projectile = "frozenshot";
        float speed = 15;
        public override void Init()
        {
            base.Init();

            parent.Name = "Rain Of Arrow";
            parent.IconName = "skill_frostranger_2";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 14;
            parent.CanDrag = true;
            parent.AreaOfEffect = radius[parent.LevelIndex];
            parent.AniName = "spell_1";
        }
        public override bool CastSkillCondition()
        {
            return parent.sourceUnit.CurrentTarget != null;
        }

        public override void OnCast()
        {
            List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, parent.sourceUnit.CurrentTarget.transform.position, radius[parent.LevelIndex]);
            for (int i = 0; i < _list.Count; i++)
            {
                FireArrowOnTarget(_list[i]);
            }
        }

        public override void OnCastDragSkill(Vector2 pos)
        {
            List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, pos, radius[parent.LevelIndex]);
            for (int i = 0; i < _list.Count; i++)
            {
                FireArrowOnTarget(_list[i]);
            }
        }

        private void FireArrowOnTarget(UnitObject target)
        {
            GameObject _prefab = Resources.Load(string.Format(CONSTANT.PathFormatProjectilePrefabs, projectile)) as GameObject;
            Vector2 startPos = new Vector2(target.transform.position.x, target.transform.position.y + 3);
            GameObject obj = SimplePool.Spawn(_prefab, startPos);
            obj.transform.SetParent(BattleManager.Instance.transform);

            obj.GetComponent<IndieProjectileObject>().FlyToTarget(target, ProjectOnHit, speed);
        }

        private void ProjectOnHit(IndieProjectileObject obj)
        {
            obj.TargetUnit.TakeDamage((int)(dmgBonus[parent.LevelIndex] + parent.sourceUnit.TotalAttack), parent.sourceUnit.playerOwned);
        }

    }
}
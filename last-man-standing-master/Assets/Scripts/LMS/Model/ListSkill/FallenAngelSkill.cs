using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class SoulSiphon : SpecifiedSkill
    {
        public override void Init()
        {
            base.Init();
            parent.Name = "Soul Siphon";
            parent.IconName = "skill_fallenangel_1";
            parent.Type = SkillScript.SkillType.Passive;
            parent.MaxLevel = 5;
        }
    }



    public class DarkBinding : SpecifiedSkill
    {
        float[] radius = new float[] { 1, 1, 1, 1, 1 };
        float[] stunDuration = new float[] { 1f, 1f, 1f, 1f, 1f };
        float[] dmgBonus = new float[] { 3f, 3f, 3f, 3f, 3f };
        string projectile = "frozenshot";
        float speed = 5;
        public override void Init()
        {
            base.Init();

            parent.Name = "Dark Binding";
            parent.IconName = "skill_fallenangel_2";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 5;
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


    public class TormentedSoil : SpecifiedSkill
    {
        float[] radius = new float[] { 2, 2, 2, 2, 2 };
        float[] dmgBonus = new float[] { 20, 40, 60, 80, 100 };
        float timePerInvoke = 0.5f;
        float duration = 5;
        string skillEffect = "tormentedsoil";
        GameObject effectObject;

        float _timeSinceLastUpdate = 0;
        float _duration = 0;
        bool isOnActive = false;

        public override void Init()
        {
            base.Init();

            parent.Name = "Tormented Soil";
            parent.IconName = "skill_fallenangel_3";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 5;
            parent.Cooldown = 5;
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
            _duration = 0;
            _timeSinceLastUpdate = 0;
            isOnActive = true;

            UnitObject target = parent.sourceUnit.CurrentTarget;
            if (target != null)
            {
                SpawnEffect(target.transform.position);
            }
        }

        public override void OnCastDragSkill(Vector2 pos)
        {
            _duration = 0;
            _timeSinceLastUpdate = 0;
            isOnActive = true;
            SpawnEffect(pos);
        }

        private void SpawnEffect(Vector2 pos)
        {

            GameObject _prefab = Resources.Load(string.Format(CONSTANT.PathSkillEffect, skillEffect)) as GameObject;
            effectObject = SimplePool.Spawn(_prefab, pos);
            effectObject.transform.SetParent(BattleManager.Instance.transform);
            effectObject.transform.localScale = new Vector3(parent.AreaOfEffect * 2, parent.AreaOfEffect * 2);
        }

        public override void Update(float deltaTime)
        {
            if (!isOnActive) return;

            _duration += deltaTime;
            _timeSinceLastUpdate += deltaTime;
            if (_timeSinceLastUpdate >= timePerInvoke)
            {
                _timeSinceLastUpdate -= timePerInvoke;


                List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, effectObject.transform.position, radius[parent.LevelIndex]);
                for (int i = 0; i < _list.Count; i++)
                {
                    _list[i].TakeDamage((int)((dmgBonus[parent.LevelIndex] + parent.sourceUnit.TotalAttack) * timePerInvoke), parent.sourceUnit.playerOwned);
                }
            }
            if (_duration >= duration)
            {
                BattleManager.DestroyGameObject(effectObject);
                isOnActive = false;
                return;
            }
        }

    }
}
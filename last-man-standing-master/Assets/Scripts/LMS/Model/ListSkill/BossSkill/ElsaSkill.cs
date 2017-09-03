using System.Collections;
using System.Text;
using UnityEngine;
using LMS.Battle;
using System.Collections.Generic;

namespace LMS.Model
{
    public class ElseNova : SpecifiedSkill
    {
        private int numberNova = 5;
        private float delayPerNoVa = 0.4f;
        private float delayImpact = 2;
        private string effect = "nova";

        float[] radius = new float[] { 2, 2, 2, 2, 2 };
        float[] stunDuration = new float[] { 1f, 1f, 1f, 1f, 1f };
        float[] dmgBonus = new float[] { 3f, 3f, 3f, 3f, 3f };

        public override void Init()
        {
            base.Init();

            parent.Name = "Elsa Nova";
            parent.IconName = "";
            parent.Type = SkillScript.SkillType.Active;
            parent.AreaOfEffect = 2;
            parent.MaxLevel = 5;
            parent.Cooldown = 10;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }

        public override void OnCast()
        {
            parent.sourceUnit.StartCoroutine(SkillScenario());
        }

        private Vector3 RandomPostion()
        {
            float x = 3;
            float y = 3;
            return new Vector3(Random.Range(-x, x), Random.Range(-y, y));
        }

        IEnumerator SkillScenario()
        {
            for (int i = 0; i < numberNova; i++)
            {
                parent.sourceUnit.StartCoroutine(CastNovaOnPosition(parent.sourceUnit.transform.position + RandomPostion()));
                yield return new WaitForSeconds(delayPerNoVa);
            }
        }

        IEnumerator CastNovaOnPosition(Vector2 pos)
        {
            GameObject obj = BattleManager.CreateVFX(CONSTANT.VfxSkillBossAoe);
            obj.transform.position = pos;
            obj.transform.localScale = new Vector2(parent.AreaOfEffect, parent.AreaOfEffect);
            obj.GetComponent<VfxObject>().AutoDestroyAfter(delayImpact);
            yield return new WaitForSeconds(delayImpact);

            GameObject nova = BattleManager.CreatePrefab(string.Format("Prefabs/SkillEffects/{0}", effect));
            nova.transform.parent = BattleManager.Instance.transform;
            nova.transform.position = pos;
            nova.transform.localScale = new Vector2(1, 1) * parent.AreaOfEffect * 2f;
            nova.GetComponent<SkillEffectObject>().LoadSkill(parent);
        }

        public override void OnHitSkill(Vector2 pos)
        {
            List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, pos, radius[parent.LevelIndex]);

            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].TakeDamage((int)(dmgBonus[parent.LevelIndex] + parent.sourceUnit.TotalAttack), parent.sourceUnit.playerOwned);
                _list[i].TakeStun(stunDuration[parent.LevelIndex]);
            }
        }
    }
    public class ElseJustice : SpecifiedSkill
    {
        int number = 5;
        float rangePerHit = 1f;
        private float delayPerNoVa = 0.4f;
        private float delayImpact = 1.5f;

        float[] radius = new float[] { 1, 1, 1, 1, 1 };
        float[] stunDuration = new float[] { 1f, 1f, 1f, 1f, 1f };
        float[] dmgBonus = new float[] { 3f, 3f, 3f, 3f, 3f };
        string projectile = "frozenshot";
        float speed = 15;

        public override void Init()
        {
            base.Init();

            parent.Name = "Elsa Justice";
            parent.IconName = "";
            parent.Type = SkillScript.SkillType.Active;
            parent.AreaOfEffect = 1;
            parent.MaxLevel = 5;
            parent.Cooldown = 15;
            parent.CanDrag = false;
            parent.AniName = "spell_1";
        }

        public override bool CastSkillCondition()
        {
            return parent.sourceUnit.CurrentTarget != null;
        }

        public override void OnCast()
        {
            parent.sourceUnit.StartCoroutine(SkillScenario());
        }

        IEnumerator SkillScenario()
        {
            Vector2 direction = parent.sourceUnit.CurrentTarget.transform.position - parent.sourceUnit.transform.position;
            for (int i = 0; i < number; i++)
            {
                //direction = direction.normalized;
                parent.sourceUnit.StartCoroutine(CastArrowAtPosistion(parent.sourceUnit.transform.position.ToVector2() + direction.normalized * rangePerHit * (i + 1)));
                yield return new WaitForSeconds(delayPerNoVa);
            }
        }


        IEnumerator CastArrowAtPosistion(Vector2 pos)
        {
            GameObject obj = BattleManager.CreateVFX(CONSTANT.VfxSkillBossAoe);
            obj.transform.position = pos;
            obj.transform.localScale = new Vector2(parent.AreaOfEffect, parent.AreaOfEffect);
            obj.GetComponent<VfxObject>().AutoDestroyAfter(delayImpact);
            yield return new WaitForSeconds(delayImpact);
            
            GameObject _prefab = Resources.Load(string.Format(CONSTANT.PathFormatProjectilePrefabs, projectile)) as GameObject;
            Vector2 startPos = new Vector2(pos.x, pos.y + 3);
            GameObject arrow = SimplePool.Spawn(_prefab, startPos);
            arrow.transform.SetParent(BattleManager.Instance.transform);

            arrow.GetComponent<IndieProjectileObject>().FlyToPosition(pos, ProjectOnHit, speed);
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
}
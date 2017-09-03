using System.Text;
using UnityEngine;
using LMS.Battle;
using System.Collections.Generic;

namespace LMS.Model
{
    public class WindSlash : SpecifiedSkill
    {
        float duration = 1;
        float range = 5;
        string skillEffect = "tornado";
        GameObject effectObject;

        float timePerInvoke = 0.05f;
        float _timeSinceLastUpdate = 0;
        float _duration = 0;
        float radius = 1;
        private List<UnitObject> _listAlreadyTaken = new List<UnitObject>();

        public override void Init()
        {
            base.Init();

            parent.Name = "Wind Slash";
            parent.IconName = "skill_samurai_2";
            parent.Type = SkillScript.SkillType.Active;
            parent.AreaOfEffect = 2;
            parent.MaxLevel = 5;
            parent.Cooldown = 12;
            parent.CanDrag = false;
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

            GameObject _prefab = Resources.Load(string.Format(CONSTANT.PathSkillEffect, skillEffect)) as GameObject;
            effectObject = SimplePool.Spawn(_prefab, parent.sourceUnit.transform.position);
            effectObject.transform.SetParent(BattleManager.Instance.transform);
            //effectObject.transform.localScale = new Vector3(parent.AreaOfEffect * 2, parent.AreaOfEffect * 2);
            effectObject.transform.localScale = new Vector3(1, 1);
            effectObject.GetComponent<SkillEffectObject>().LoadSkill(parent, loop:true, flip: parent.sourceUnit.unitAnimation.IsFacingLeft);

            Vector2 relativePos = parent.sourceUnit.CurrentTarget.transform.position - parent.sourceUnit.transform.position;
            Vector2 endPos = parent.sourceUnit.transform.position.ToVector2() + relativePos.normalized * range;
            float speed = range / duration;
            effectObject.GetComponent<SkillEffectObject>().MoveToPosition(endPos, speed);

        }

        public override void Update(float deltaTime)
        {
            if (effectObject == null) return;
            if (effectObject.GetComponent<SkillEffectObject>() == null) return;
            if (!effectObject.GetComponent<SkillEffectObject>().isAlive) return;
            _duration += deltaTime;
            _timeSinceLastUpdate += deltaTime;
            if (_timeSinceLastUpdate >= timePerInvoke)
            {
                _timeSinceLastUpdate -= timePerInvoke;


                List<UnitObject> _list = BattleManager.FindEnemyInRadius(parent.sourceUnit.TeamIndex, effectObject.transform.position, radius);
                for (int i = 0; i < _list.Count; i++)
                {
                    if (!_listAlreadyTaken.Contains(_list[i]))
                    {
                        _list[i].TakeKnockUpHit();
                        _listAlreadyTaken.Add(_list[i]);
                        Debug.Log("Knock up: " + _list[i].name);
                    }
                }

            }
            if (_duration >= duration)
            {
                _listAlreadyTaken.Clear();
                return;
            }
        }
    }
}
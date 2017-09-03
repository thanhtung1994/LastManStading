using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class SpecifiedSkill
    {

        public SkillScript parent;

        public virtual void Init()
        {

        }

        public virtual string GetDescriptionByLevel(int level)
        {
            return "Missing description";
        }

        public virtual void Update(float deltaTime)
        {

        }

        public virtual void UpdateOnEffectObject(float deltaTime)
        {

        }

        public virtual void OnHitNormalAttack(UnitObject target)
        {
            parent.targetUnit = target;
        }

        public virtual void OnHitNormalAttack(UnitObject target, AttackInfo attackInfo)
        {
            parent.targetUnit = target;
        }

        /// <summary>
        /// Dành cho những skill target vào unit
        /// </summary>
        /// <param name="target"></param>
        public virtual void OnHitSkill(UnitObject target)
        {
            parent.targetUnit = target;
        }

        /// <summary>
        /// Dành cho những skill aoe
        /// </summary>
        /// <param name="pos"></param>
        public virtual void OnHitSkill(Vector2 pos)
        {

        }

        public virtual void OnCast()
        {

        }

        public virtual void OnCastDragSkill(Vector2 pos)
        {

        }

        public virtual bool ValidateTarget(UnitObject target)
        {
            return false;
        }

        /// <summary>
        /// Điều kiện để có thể sử dụng skill( ví dụ: skill yêu cầu target, yêu cầu unit phải ít hơn 50% máu ...)
        /// </summary>
        /// <returns></returns>
        public virtual bool CastSkillCondition()
        {
            return true;
        }

        /// <summary>
        /// Điều kiện để có thể sử dụng drag skill
        /// </summary>
        /// <returns></returns>
        public virtual bool DragCastSkillCondition()
        {
            return true;
        }

        /// <summary>
        /// Điều kiện để auto sử dụng skill, tất nhiên vẫn phải kiểm tra CastSkillCondition()
        /// </summary>
        /// <returns></returns>
        public virtual bool AutoCastSkillCondition()
        {
            return true;
        }

        public virtual bool ActivePassiveCondition()
        {
            return false;
        }

        public virtual void OnChanneling()
        {

        }

        public virtual void OnStopChanneling()
        {

        }

        public virtual void OnDead(UnitObject unit)
        {

        }

        public virtual void OnHpChanged()
        {

        }

        public virtual void OnMpChanged()
        {

        }

        public virtual void OnStun()
        {

        }

        public virtual void TriggerPassiveAfterCastSkill(SkillScript skill)
        {

        }

        public virtual bool ConditonPassiveNormalAttack()
        {
            return false;
        }

        public virtual void TriggerPassiveNormalAttack(UnitObject owner, UnitObject target)
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Model;

namespace LMS.Battle
{
    public class SkillObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject SpriteImage;
        public SkillScript skillScript;
        public void LoadSkill(SkillScript skillScript)
        {
            this.skillScript = skillScript;
        }
        public void Cast()
        {
            gameObject.SetActive(true);
            //skillScript.OnCast();
        }

        public void OnHitSkill()
        {

        }

        public void OnFinish()
        {
            BattleManager.DestroyGameObject(this.gameObject);
        }

        private void Update()
        {
            //skillScript.Update(Time.deltaTime);
        }
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (skillScript != null)
            {
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, skillScript.AreaOfEffect);
            }
        }
#endif
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LMS.Model;

namespace LMS.UI
{
    public class SingleSkillInfoObject : MonoBehaviour
    {

        [SerializeField]
        private Image Icon;
        [SerializeField]
        private Image Border;
        [SerializeField]
        private List<Image> ListLevel;
        public Toggle Toggle;

        private SkillScript _skillScript;
        public SkillScript Skill
        {
            get { return _skillScript; }
        }

        public void LoadSkill(SkillInfo info)
        {
            _skillScript = new SkillScript(info.IndexSkill);
            _skillScript.Level = info.LevelSkill;
            Icon.sprite = Resources.Load<Sprite>(string.Format(CONSTANT.PathUISkillIcon, _skillScript.IconName));
            for (int i = 0; i < ListLevel.Count; i++)
            {
                ListLevel[i].gameObject.SetActive(i < _skillScript.MaxLevel);
                ListLevel[i].color = i < _skillScript.Level ? Color.red : Color.black;
            }
        }
    }
}
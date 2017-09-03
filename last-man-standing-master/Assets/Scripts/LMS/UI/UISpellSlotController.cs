using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LMS.Model;
using LMS.Battle;

namespace LMS.UI
{

    public class UISpellSlotController : MonoBehaviour
    {
        [SerializeField]
        private Image SpellImage;
        [SerializeField]
        private Image CooldownOverlay;
        [SerializeField]
        private SpellDragObject SpellDrag;
        [SerializeField]
        private Material GrayScaleMaterial;
        [SerializeField]
        private Button BtnCast;

        private SkillScript skill;

        public void LoadSkill(SkillScript skill, SpellDragObject.OnFinishDrag onFinish)
        {
            this.skill = skill;
            SpellDrag.enabled = skill.CanDrag;
            SpellDrag.CallBackOnFinishDrag = CastSkillAtPosition;
            SpellDrag.ScaleAOE = skill.AreaOfEffect * 2;
            SpellImage.sprite = Resources.Load<Sprite>(string.Format(CONSTANT.PathUISkillIcon, skill.IconName));

            BtnCast.enabled = skill.Type == SkillScript.SkillType.Active;
            BtnCast.onClick.RemoveAllListeners();
            BtnCast.onClick.AddListener(CastSkill);
        }

        private void Update()
        {
            if (skill != null && skill.Type == SkillScript.SkillType.Active)
            {
                //spell.Update(Time.deltaTime);
                float _amount = skill.CurrentCooldown / skill.Cooldown;
                CooldownOverlay.fillAmount = _amount;
                SpellDrag.SetEnableDragable(skill.IsReadyToUse);
                if (skill.IsReadyToUse)
                    SpellImage.material = null;
                else
                    SpellImage.material = GrayScaleMaterial;
            }
        }

        private void CastSkill()
        {
            Debug.Log(skill.Name);
            if (skill.IsReadyToUse)
                BattleManager.Instance.playerSelectedUnit.CommandCastSkill(skill);
        }

        private void CastSkillAtPosition(Vector2 pos)
        {
            Debug.Log(skill.Name);
            if (skill.IsReadyToUse)
                BattleManager.Instance.playerSelectedUnit.CommandCastSkillAtPosition(skill, pos);
        }
    }
}
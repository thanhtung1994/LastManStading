using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using LMS.Model;
using LMS.Battle;

namespace LMS.UI
{
    public class UIToggleUnitController : MonoBehaviour
    {
        [SerializeField]
        private Image UnitImage;
        [SerializeField]
        private Image CooldownOverlay;
        [SerializeField]
        private Material GrayScaleMaterial;
        [SerializeField]
        private Slider HpBar;
        [SerializeField]
        private Slider MpBar;
        public Toggle Toggle;
        public UnitObject unitObject;

        public bool ToggleIsOn
        {
            get { return Toggle.isOn; }
        }
        public void LoadUnit(UnitObject unit)
        {
            unitObject = unit;
            if (unit != null)
            {
                Toggle.gameObject.name = unit.unitScript.Code;
                //UnitImage.sprite = Resources.Load<Sprite>(string.Format(CONSTANT.PathUIUnitIcon, unit.Index));
                UnitImage.sprite = Resources.Load<Sprite>(string.Format(CONSTANT.PathUIUnitIconByCode, unit.unitScript.Code));
                HpBar.value = unitObject.CurrentPercentHP;
                MpBar.value = unitObject.CurrentPercentMP;
            }
            else
            {
                // Clear
            }
        }

        private void Update()
        {
            if (unitObject != null)
            {
                HpBar.value = unitObject.CurrentPercentHP;
                MpBar.value = unitObject.CurrentPercentMP;
            }
        }
    }
}
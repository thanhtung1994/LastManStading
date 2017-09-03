using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LMS.Model;

namespace LMS.UI
{
    public class HeroAvatarInfoObject : MonoBehaviour
    {
        [SerializeField]
        private Image UnitImage;
        [SerializeField]
        private Image Border;
        [SerializeField]
        private GameObject MarkEquipped;

        public Toggle Toggle;

        private UnitScript _unitScript;
        public UnitScript UnitScript
        {

            get { return _unitScript; }
        }

        public void LoadUnitInfo(UnitScript unitScript)
        {
            _unitScript = unitScript;
            UnitImage.sprite = Resources.Load<Sprite>(string.Format(CONSTANT.PathUIUnitIconByCode, unitScript.Code));
            //UnitImage.material = _unitScript.unlocked ? null : Resources.Load(CONSTANT.MatSpriteGray) as Material;
            MarkEquipped.SetActive(unitScript.IsEquipped);
        }
    }
}

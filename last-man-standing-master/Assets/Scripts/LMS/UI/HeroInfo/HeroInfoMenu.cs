using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LMS.Model;
using UnityEngine.SceneManagement;

namespace LMS.UI
{
    public class HeroInfoMenu : MonoBehaviour
    {
        [SerializeField]
        private Image HeroThumb;

        // ============================ Hero ====================================
        [SerializeField]
        private List<HeroAvatarInfoObject> listHero;
        [SerializeField]
        private Transform GridHeroIcon;
        [SerializeField]
        private TextMeshProUGUI TxtUnitStats;

        // ============================ Skill ====================================
        [SerializeField]
        private List<SingleSkillInfoObject> listSkill;
        [SerializeField]
        private Transform GridSkillInfo;
        [SerializeField]
        private TextMeshProUGUI TxtSkillDes;
        // ============================ Function =====================================
        [SerializeField]
        private Button BtnEquipUnit;
        [SerializeField]
        private Button BtnStartGame;
        // ============================  =====================================

        private bool isInit = false;
        private HeroAvatarInfoObject selectSlot;
        private UnitScript selectedUnit;

        // Use this for initialization
        void Start()
        {
            InitMenu();
            LoadPlayerInfo();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LoadPlayerInfo()
        {
            for (int i = 0; i < listHero.Count; i++)
            {
                if (i < GameManager.playerInfo.ListUnitUnlocked.Count)
                {
                    listHero[i].LoadUnitInfo(GameManager.playerInfo.ListUnitUnlocked[i]);
                }
            }
            listHero[0].Toggle.isOn = true;
        }

        private void InitMenu()
        {
            if (isInit) return;
            // ============ Unit =======================
            listHero[0].Toggle.onValueChanged.AddListener((x) =>
            {
                if (x) OnChangeValueHeroAvatar(listHero[0]);
            });
            for (int i = 1; i < GameSetting.ListHeroIndex.Count; i++)
            {
                GameObject obj = GameObject.Instantiate(listHero[0].gameObject);
                obj.transform.SetParent(GridHeroIcon);
                obj.transform.localScale = new Vector3(1, 1, 1);

                listHero.Add(obj.GetComponent<HeroAvatarInfoObject>());
                obj.GetComponent<HeroAvatarInfoObject>().Toggle.onValueChanged.AddListener((x) =>
                {
                    if (x) OnChangeValueHeroAvatar(obj.GetComponent<HeroAvatarInfoObject>());
                });
            }

            // ============ Skill =======================
            listSkill[0].Toggle.onValueChanged.AddListener((x) =>
            {
                if (x) OnClickSkillInfo(listSkill[0]);
            });
            for (int i = 0; i < 4; i++)
            {
                GameObject obj = GameObject.Instantiate(listSkill[0].gameObject);
                obj.transform.SetParent(GridSkillInfo);
                obj.transform.localScale = new Vector3(1, 1, 1);

                listSkill.Add(obj.GetComponent<SingleSkillInfoObject>());
                obj.GetComponent<SingleSkillInfoObject>().Toggle.onValueChanged.AddListener((x) =>
                {
                    if (x) OnClickSkillInfo(obj.GetComponent<SingleSkillInfoObject>());
                });
            }

            BtnEquipUnit.onClick.AddListener(OnClickBtnEquipUnit);
            BtnStartGame.onClick.AddListener(OnClickBtnStart);

            isInit = true;
        }

        private void LoadHeroInfo(UnitScript unitScript)
        {
            selectedUnit = unitScript;
            HeroThumb.sprite = Resources.Load<Sprite>(string.Format(CONSTANT.PathUIUnitThumbByCode, unitScript.Code));
            for (int i = 0; i < listSkill.Count; i++)
            {
                if (i < unitScript.listSkill.Count)
                {
                    listSkill[i].LoadSkill(unitScript.listSkill[i]);
                }
                else
                {
                    listSkill[i].LoadSkill(new SkillInfo(0, 0));
                }
            }
            listSkill[0].Toggle.isOn = true;
            TxtUnitStats.text = unitScript.UnitStatsDescription();

            BtnEquipUnit.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = unitScript.IsEquipped ? "Unequip" : "Equip";
        }

        private void EquipUnit()
        {
            GameManager.playerInfo.EquipUnit(selectedUnit);
        }

        private void UnequipUnit()
        {
            GameManager.playerInfo.UnequipUnit(selectedUnit);
        }

        private void OnClickSkillInfo(SingleSkillInfoObject obj)
        {
            TxtSkillDes.text = obj.Skill.specified.GetDescriptionByLevel(obj.Skill.Level);
        }

        private void OnChangeValueHeroAvatar(HeroAvatarInfoObject obj)
        {
            selectSlot = obj;
            LoadHeroInfo(obj.UnitScript);
        }

        private void OnClickBtnEquipUnit()
        {
            if (GameManager.playerInfo.IsEquipUnit(selectedUnit))
            {
                UnequipUnit();
                //selectedUnit.IsEquipped = false;
            }
            else
            {
                EquipUnit();
                //selectedUnit.IsEquipped = true;
            }
            LoadHeroInfo(selectedUnit);
            selectSlot.LoadUnitInfo(selectedUnit);
        }

        private void OnClickBtnStart()
        {
            SceneManager.LoadScene(1);
        }
    }
}
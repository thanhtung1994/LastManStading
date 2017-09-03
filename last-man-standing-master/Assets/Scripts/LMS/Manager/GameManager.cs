using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LMS.Model;

namespace LMS
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;
        public static PlayerInfo playerInfo;
        public static UnitDataInfo unitDataInfo;
        public int SelectedGroupMapIndex = 0;
        public int SelectedMapIndex = 0;
        public bool CheatModeEnabled = false;
        private bool IsSelectLevel = false;

        private void Awake()
        {
            if (Instance == null)
            {
                //if (unitDataInfo == null)
                //    unitDataInfo = new UnitDataInfo(DemoUnitDataCsv.text);
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                DestroyObject(this.gameObject);
            }
            if (playerInfo == null)
            {
                playerInfo = new PlayerInfo();
                for (int i = 0; i < GameSetting.ListHeroIndex.Count; i++)
                {
                    UnitInfo unit = new UnitInfo(GameSetting.ListHeroIndex[i], 0, 1);
                    if (GameSetting.ListHeroIndex[i] == 1 || GameSetting.ListHeroIndex[i] == 2)
                    {
                        unit.unlocked = true;
                    }
                    playerInfo.ListUnitUnlocked.Add(new UnitScript(unit));

                }
            }
        }

        private void SelectLevel(int groupMapIndex, int mapIndex)
        {
            SelectedGroupMapIndex = groupMapIndex;
            SelectedMapIndex = mapIndex;
            SceneManager.LoadScene(1);
        }

        public void DestroyManager()
        {
            DestroyObject(this.gameObject);
            Instance = null;
        }
    }


    public class PlayerInfo
    {
        public string PlayerId;
        public List<UnitScript> ListUnitUnlocked;
        public List<UnitScript> ListUnitEquipped;

        public PlayerInfo()
        {
            ListUnitUnlocked = new List<UnitScript>();
            ListUnitEquipped = new List<UnitScript>();
        }

        public bool IsEquipUnit(UnitScript unitScript)
        {
            UnitScript _unit = ListUnitEquipped.Find(x => x.Code.Equals(unitScript.Code));
            return _unit != null;
        }

        public void EquipUnit(UnitScript unitScript)
        {
            unitScript.IsEquipped = true;
            ListUnitEquipped.Add(unitScript);
        }

        public void UnequipUnit(UnitScript unitScript)
        {
            unitScript.IsEquipped = false;
            ListUnitEquipped.Remove(unitScript);
        }

    }
}
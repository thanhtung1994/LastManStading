using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LMS.Battle
{
    public class BattleBossStage : MonoBehaviour
    {

        [SerializeField]
        private List<Transform> listSpawnPointHeroes;
        [SerializeField]
        private List<Transform> listSpawnPointEnemy;
        [SerializeField]
        private Transform spawnPointBoss;

        private UnitObject BossUnit;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private bool WinCondition()
        {
            if (BossUnit == null) return true;
            if (!BossUnit.IsAlive) return true;
            return false;
        }


    }
}
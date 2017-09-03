using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMS.Battle
{
    public class BattleSurvivalStage : MonoBehaviour
    {
        #region Reference
        [SerializeField]
        private List<Transform> listSpawnPointHeroes;
        [SerializeField]
        private List<Transform> listSpawnPointEnemy;
        [SerializeField]
        private Transform bossSpawnLocation;
        #endregion
        
        private BattleManager battleManager;
        private int _enemyCount = 0;
        private int _enemyDead = 0;
        private int _killedBoss = 0;
        private int _numberPlayerHeroAlive = 0;

        // Stage config
        private int bossSpawn = 10;

        private bool WinCondition()
        {
            return false;
        }
        #region Method
        public Vector2 RandomEnemySpawnPosition()
        {
            return listSpawnPointEnemy.GetRandomValue().position.ToVector2();
        }
        public void LoadBattle(BattleManager battleManager)
        {
            this.battleManager = battleManager;
            _enemyCount = 0;
            _enemyDead = 0;
        }

        public void SetupPlayerTeamPosition(List<UnitObject> listUnit)
        {
            _numberPlayerHeroAlive = listUnit.Count;
            for (int i = 0; i < listUnit.Count; i++)
            {
                if (i < listSpawnPointHeroes.Count)
                    listUnit[i].transform.position = listSpawnPointHeroes[i].position;
                listUnit[i].CallBackOnDead.AddListener(() =>
                {
                    _numberPlayerHeroAlive -= 1;
                    if (_numberPlayerHeroAlive <= 0)
                    {
                        battleManager.ShowEndGame(false);
                    }
                });
                }
        }

        public void CountingEnemySpawn(int number)
        {
            _enemyCount += number;
        }

        public void CountingEnemyDie(int number)
        {
            _enemyCount -= number;
            _enemyDead += number;
            if (_enemyDead == bossSpawn)
            StartCoroutine(SpawnBoss());
        }

        private IEnumerator SpawnBoss()
        {
            Vector3 _tmp = battleManager.MainCamera.transform.position;
            battleManager.PauseGameForCinematic();
            Vector3 pos = new Vector3(bossSpawnLocation.position.x, bossSpawnLocation.position.y, _tmp.z);
            battleManager.MainCamera.transform.DOMove(pos, 0.5f);
            battleManager.MainCamera.Shake(CameraShake.Types.MID);
            yield return new WaitForSeconds(0.5f);
            GameObject obj = BattleManager.CreateVFX(CONSTANT.VfxSpawn);
            obj.transform.position = bossSpawnLocation.position;
            yield return new WaitForSeconds(0.5f);
            UnitObject unit = battleManager.SpawnSingleCreepsAtPosition(new UnitInfo(401, 0, 1), battleManager.enemyPlayer.PlayerIndex, bossSpawnLocation.position);
            unit.IsAutoCastSkill = true;
            unit.CallBackOnDead.AddListener(() =>
            {
                battleManager.ShowEndGame(true);
            });
            battleManager.ResumeGameAfterCinematic();

        }

        #endregion
    }
}
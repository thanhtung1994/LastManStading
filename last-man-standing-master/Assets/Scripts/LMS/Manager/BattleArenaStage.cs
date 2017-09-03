using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LMS.Battle
{
    public enum GameMode
    {
        Arena,
        BossFight,
        HeroDefense,
        AttackOnTower,
    }

    public class BattleArenaStage
    {

        private BattleManager manager;

        public void LoadBattleManager(BattleManager manager)
        {
            this.manager = manager;
        }

        public void Update(float deltaTime)
        {

        }
    }

    public class GameStageInfo
    {
    }

    public class GameStageArena : GameStageInfo
    {
        public float TimeStage;
        public List<NewWaveInfo> listWaveInfo;

        public GameStageArena()
        {
            TimeStage = 0;
            listWaveInfo = new List<NewWaveInfo>();
        }
    }

    public class NewWaveInfo
    {
        public float DelayTime;
        public RangeValue TimePerSpawn;
        public RangeValue NumberUnitPerSpawn;
        public UnitInfo Unit;

        public float RandomTimePerSpawn()
        {
            return Random.Range(TimePerSpawn.min, TimePerSpawn.max);
        }
        public int RandomNumberUnitPerSpawn()
        {
            return (int)Random.Range(NumberUnitPerSpawn.min, NumberUnitPerSpawn.max);
        }
    }

    public class RangeValue
    {
        public float min;
        public float max;

        public RangeValue(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
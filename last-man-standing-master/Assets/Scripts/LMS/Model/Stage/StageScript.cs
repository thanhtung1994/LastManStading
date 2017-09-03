using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript
{

    public SpecifiedStage specified;

    public int GroupMapIndex { get; set; }
    public int MapIndex { get; set; }
    public int StartingGold { get; set; }
    public UnitInfo CastleUnit { get; set; }
    public List<StageGold> ListGoldIncome { get; set; }
    public List<UnitInfo> ListUnitEquiped { get; set; }
    public float CastleDistance { get; set; }
    public float TowerDistance { get; set; }

    public StageScript(int groupMapIndex, int mapIndex)
    {
        GroupMapIndex = groupMapIndex;
        MapIndex = mapIndex;
        int index = groupMapIndex * 100 + mapIndex;
        switch (index)
        {
            case 101:
                specified = new Stage101();
                break;
            default:
                specified = new SpecifiedStage();
                break;
        }
        specified.parent = this;

        // Default value
        CastleDistance = 14;
        TowerDistance = 10;
        specified.Init();
    }

    public StageGold GetIncomeByTime(float timestamp)
    {
        for (int i = 0; i < ListGoldIncome.Count; i++)
        {
            if (timestamp > ListGoldIncome[i].Timestamp && !ListGoldIncome[i].Passed)
            {
                //_listWave[i].Spawned = true;
                return ListGoldIncome[i];
            }

        }
        return null;
    }
}

public class UnitInfo
{
    public int index;
    public int level;
    public int star;
    public bool unlocked;

    public UnitInfo()
    {

    }

    public UnitInfo(int index, int level, int star)
    {
        this.index = index;
        this.level = level;
        this.star = star;
    }
}

public class StageGold
{
    public int Timestamp;
    public int GoldIncome;
    public bool Passed;
    public StageGold()
    {

    }

    public StageGold(int time, int gold)
    {
        this.Timestamp = time;
        this.GoldIncome = gold;
    }
}
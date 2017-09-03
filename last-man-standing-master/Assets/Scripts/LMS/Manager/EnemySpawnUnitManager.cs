using GAMO.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnUnitManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset TextFile;
    // Use this for initialization
    void Start()
    {
        StageInfo stage = new StageInfo(TextFile.text);
    }



    // Update is called once per frame
    void Update()
    {

    }
}

public class UnitDataInfo
{
    private List<UnitDataCSV> _listUnit;
    public UnitDataInfo(string lines)
    {
        _listUnit = new List<UnitDataCSV>();

        string[] linesInFile = lines.Split('\n');

        for (int i = 1; i < linesInFile.Length; i++)
        {
            //Debug.Log(line);
            UnitDataCSV unit = ReadUnitData(linesInFile[i]);
            if (unit != null)
                _listUnit.Add(unit);
        }
        _listUnit.Sort((x1, x2) => x1.ID.CompareTo(x2.ID));
    }
    private UnitDataCSV ReadUnitData(string line)
    {
        if (string.IsNullOrEmpty(line)) return null;
        string[] _arr = line.Split(',');
        if (_arr.Length < 14) return null;
        UnitDataCSV unit = new UnitDataCSV();
        try
        {
            unit.ID = int.Parse(_arr[0]);
            unit.Name = _arr[1];
            unit.Code = _arr[2];
            unit.LightAttack = float.Parse(_arr[3]);
            unit.HeavyAttack = float.Parse(_arr[4]);
            unit.AttackRange = float.Parse(_arr[5]);
            unit.AttackSpeed = float.Parse(_arr[6]);
            unit.MovementSpeed = float.Parse(_arr[7]);
            unit.HP = int.Parse(_arr[8]);
            unit.LightArmor = float.Parse(_arr[9]);
            unit.HeavyArmor = float.Parse(_arr[10]);
            unit.GoldCost = int.Parse(_arr[11]);
            unit.Scale = float.Parse(_arr[12]);
            unit.NumberUnit = int.Parse(_arr[13]);
            unit.GoldBonus = int.Parse(_arr[14]);
            unit.Cooldown = float.Parse(_arr[15]);
        }
        catch (Exception ex)
        {
            Debug.Log("Line not valid - " + ex.Message);
            return null;
        }
        return unit;
    }
    public UnitDataCSV GetUnitDataInfo(int index)
    {
        for (int i = 0; i < _listUnit.Count; i++)
        {
            if (_listUnit[i].ID == index)
            {
                return _listUnit[i];
            }

        }
        return null;
    }
}

public class StageInfo
{
    private List<WaveInfo> _listWave;
    //public List<WaveInfo> listWave
    //{
    //    get
    //    {
    //        return _listWave;
    //    }
    //}

    public StageInfo(string lines)
    {
        _listWave = new List<WaveInfo>();

        string[] linesInFile = lines.Split('\n');

        foreach (string line in linesInFile)
        {
            //Debug.Log(line);
            WaveInfo wave = ReadWaveData(line);
            if (wave != null)
                _listWave.Add(wave);
        }
        _listWave.Sort((x1, x2) => x1.Time.CompareTo(x2.Time));
        //Debug.Log(Ulti.ToJson(_listWave));
    }

    private WaveInfo ReadWaveData(string line)
    {
        if (string.IsNullOrEmpty(line)) return null;
        string[] _arr = line.Split(',');
        if (_arr.Length < 4) return null;
        WaveInfo wave = new WaveInfo();
        try
        {
            wave.Time = int.Parse(_arr[0]);
            wave.ListUnitIndex.Add(int.Parse(_arr[1]));
            wave.ListLevel.Add(int.Parse(_arr[2]));
            wave.ListNumber.Add(int.Parse(_arr[3]));
        }
        catch
        {
            Debug.Log("Line not valid");
            return null;
        }
        return wave;
    }
    public WaveInfo GetWaveInfo(float timestamp)
    {
        for (int i = 0; i < _listWave.Count; i++)
        {
            if (timestamp > _listWave[i].Time && !_listWave[i].Spawned)
            {
                //_listWave[i].Spawned = true;
                return _listWave[i];
            }

        }
        return null;
    }
}

public class WaveInfo
{
    public int Time { get; set; }
    public List<int> ListUnitIndex { get; set; }
    public List<int> ListLevel { get; set; }
    public List<int> ListNumber { get; set; }
    public bool Spawned { get; set; }

    public WaveInfo()
    {
        Time = 0;
        ListUnitIndex = new List<int>();
        ListLevel = new List<int>();
        ListNumber = new List<int>();
        Spawned = false;
    }
}

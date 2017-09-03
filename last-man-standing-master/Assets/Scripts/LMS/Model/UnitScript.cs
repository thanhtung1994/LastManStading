using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LMS.Battle;
namespace LMS.Model
{
    public class UnitDefineAnimation
    {
        public string AniAttack { get; set; }
        public string AniAttackCrit { get; set; }
        public string AniPassiveAttack { get; set; }
    }

    public class UnitScript
    {
        public delegate void OnLoadUnitObject(UnitObject unitObject);
        public enum UnitType
        {
            None,
            Building,
            Melee,
            Ranger,
        }
        public SpecifiedUnit specified;
        public PlayerStats PlayerOwner;
        public int Index { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string DebugName { get; set; }
        public string Code { get; set; }
        public int Level { get; set; }
        public int LevelUpgrade
        {
            get { return Mathf.Clamp(Level, 0, CONSTANT.MaxLevelUpgrade); }
        }
        public int Star { get; set; }
        public bool IsEquipped { get; set; }
        #region Stats
        public int Hp { get; set; }
        public int Mp { get; set; }
        public float Attack { get; set; }
        public float Defense { get; set; }
        public float AttackSpeed { get; set; }
        public float MovementSpeed { get; set; }
        public float AttackRange { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalMultiple { get; set; }

        public int HpScaleLevel
        {
            get { return (int)(Hp * Mathf.Pow(GameSetting.ScaleStatLevel, LevelUpgrade)); }
        }
        public float AttackScaleLevel
        {
            get { return Attack * Mathf.Pow(GameSetting.ScaleStatLevel, LevelUpgrade); }
        }
        public float DefenseScaleLevel
        {
            get { return Defense * Mathf.Pow(GameSetting.ScaleStatLevel, LevelUpgrade); }
        }
        public float AttackSpeedScaleLevel
        {
            get { return AttackSpeed; }
        }
        public float MovementSpeedScaleLevel
        {
            get { return MovementSpeed; }
        }
        public float CriticalChanceScaleLevel
        {
            get { return CriticalChance; }
        }
        public float CriticalMultipleScaleLevel
        {
            get { return CriticalMultiple; }
        }
        #endregion
        public float AniAttackTime { get; set; }
        public string ProjectTypeCode { get; set; }
        private float _projectileSpeed = 5;
        public float ProjectileSpeed
        {
            get
            {
                return _projectileSpeed;
            }
            set
            {
                _projectileSpeed = value;
            }
        }
        public UnitType PriorityTarget = UnitType.None;
        public UnitType OnlyTarget = UnitType.None;

        public List<SkillInfo> listSkill { get; set; }
        public UnitDefineAnimation unitDefineAnimation { get; set; }

        public int GoldBonus { get; set; }
        public UnitType Type { get; set; }
        public bool IsUpgradeable { get; set; }
        public float Scale { get; set; }
        public OnLoadUnitObject CallBackOnLoadUnit;

        public string UnitStatsDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Code: {0}\n", Code);
            sb.AppendFormat("Name: {0}\n", Name);
            sb.AppendFormat("Level: {0}\n", Level);
            sb.AppendFormat("Hp: {0}\n", HpScaleLevel);
            sb.AppendFormat("Attack:{0}\n", AttackScaleLevel);
            sb.AppendFormat("Defense: {0}\n", DefenseScaleLevel);
            sb.AppendFormat("Range: {0}\n", AttackRange);
            sb.AppendFormat("AS: {0}\n", AttackSpeed);
            sb.AppendFormat("MS: {0}\n", MovementSpeed);
            return sb.ToString();
        }

        public UnitInfo ToUnitInfo()
        {
            return new UnitInfo(Index, Level, Star);
        }

        public UnitScript(UnitInfo unitInfo) : this(unitInfo.index, unitInfo.level, unitInfo.star)
        {

        }

        public UnitScript(int index, int level = 0, int star = 1)
        {
            Index = index;
            Level = level;
            Star = star;
            listSkill = new List<SkillInfo>();

            switch (index)
            {
                case 1:
                    specified = new HeroBerserker();
                    break;
                case 2:
                    specified = new HeroFrostRanger();
                    break;
                case 3:
                    specified = new HeroFallenAngel();
                    break;
                case 4:
                    specified = new HeroBladeMaster();
                    break;
                case 5:
                    specified = new HeroSamurai();
                    break;
                case 6:
                    specified = new HeroLightSorcerer();
                    break;
                case 7:
                    specified = new HeroGolem();
                    break;
                case 8:
                    specified = new HeroPriest();
                    break;
                case 9:
                    specified = new HeroScout();
                    break;
                case 10:
                    //specified = new HeroBard();
                    break;
                case 201:
                    specified = new Unit201();
                    break;
                case 202:
                    specified = new Unit202();
                    break;
                case 401:
                    specified = new Unit401();
                    break;
                default:
                    specified = new SpecifiedUnit();
                    break;
            }
            specified.parent = this;
            specified.parent.PriorityTarget = UnitType.None;
            specified.Init();
        }
    }
}
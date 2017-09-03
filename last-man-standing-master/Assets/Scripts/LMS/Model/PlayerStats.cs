using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMS.Model;

namespace LMS.Battle
{
    public class PlayerStats
    {
        #region Var
        public int PlayerIndex { get; set; }
        public int Id { get; set; }
        public int TeamIndex { get; set; }

        public List<PlayerStats> ListEnemyPlayer { get; set; }
        public List<SpellScript> ListEquippedSpell { get; set; }
        public List<UnitScript> ListEquippedUnit { get; set; }
        public List<UnitObject> ListPlayUnit { get; set; }
        public bool IsBotPlayer { get; set; }
        #endregion

        #region Constructor
        public PlayerStats()
        {

        }

        public PlayerStats(int playerIndex, int teamIndex, bool isBotPlayer)
        {
            PlayerIndex = playerIndex;
            Id = playerIndex;
            TeamIndex = teamIndex;
            ListEnemyPlayer = new List<PlayerStats>();
            ListEquippedUnit = new List<UnitScript>();
            ListEquippedSpell = new List<SpellScript>();
            ListPlayUnit = new List<UnitObject>();
            IsBotPlayer = isBotPlayer;
        }
        #endregion

        public void EquipUnit(int index, int level, int star)
        {
            UnitScript _unit = new UnitScript(index, level, star);
            _unit.PlayerOwner = this;
            if (!ListEquippedUnit.Exists(x => x == _unit))
            {
                ListEquippedUnit.Add(_unit);
            }
        }

        public void EquipSpell(SpellScript spell)
        {
            if (!ListEquippedSpell.Exists(x => x.Code.Equals(spell.Code)))
            {
                ListEquippedSpell.Add(spell);
            }
        }

        /// <summary>
        /// Cơ chế tìm trụ hoặc nhà chính tiếp theo - Chưa hoàn thiện
        /// </summary>
        /// <returns></returns>
        public UnitObject GetNextDefaultTarget()
        {

            return null;
        }

        public Vector3 GetPositionAutoSpawn()
        {
            return Vector3.zero;
        }

        public void Update(float deltaTime)
        {
            if (!ListEquippedSpell.IsNullOrEmpty())
            {
                for (int i = 0; i < ListEquippedSpell.Count; i++)
                {
                    if (ListEquippedSpell[i].CurrentCooldown > 0)
                        ListEquippedSpell[i].CurrentCooldown -= deltaTime;
                }
            }
        }

    }
}
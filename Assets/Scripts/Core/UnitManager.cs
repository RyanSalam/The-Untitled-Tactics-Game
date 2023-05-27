using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tactics.UnitSystem;
using System;

namespace Tactics
{
    public class UnitManager : MonoBehaviour
    {
        private List<UnitObject> allUnits = new List<UnitObject>();
        private List<UnitObject> playerUnits = new List<UnitObject>();
        private List<UnitObject> enemyUnits = new List<UnitObject>();

        public static UnitManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (Instance != this)
                Destroy(this.gameObject);
        }

        private void Start()
        {
            UnitObject.OnAnyUnitSpawned += AnyUnitSpawned_Callback;
            UnitObject.OnAnyUnitDied += AnyUnitDied_Callback;
        }

        private void AnyUnitSpawned_Callback(UnitObject unit)
        {
            allUnits.Add(unit);
            if (unit.GetUnitFaction() == UnitFaction.PLAYER)
                playerUnits.Add(unit);
            else
                enemyUnits.Add(unit);
        }

        private void AnyUnitDied_Callback(UnitObject unit)
        {
            allUnits.Remove(unit);
            if (unit.GetUnitFaction() == UnitFaction.PLAYER)
                playerUnits.Remove(unit);
            else
                enemyUnits.Remove(unit);
        }

        public IEnumerable<UnitObject> GetPlayerUnits()
        {
            return playerUnits;
        }
        public IEnumerable<UnitObject> GetEnemyUnits()
        {
            return enemyUnits;
        }
    }
}



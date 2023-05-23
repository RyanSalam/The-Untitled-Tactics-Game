using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tactics.GridSystem;

namespace Tactics.UnitSystem
{
    public class UnitObject : MonoBehaviour
    {
        public static event Action<UnitObject> OnAnyUnitDied; 

        [Header("Components")]
        [SerializeField] private HealthComponent health;

        [Header("Attributes")]
        [SerializeField] private UnitFaction unitFaction;


        private GridPosition position;

        private void Awake()
        {
            if (health == null)
                health = GetComponent<HealthComponent>();
        }

        public GridPosition GetUnitGridPosition()
        {
            return position;
        }

        public UnitFaction GetUnitFaction()
        {
            return unitFaction;
        }
    }

    public class HealthComponent : MonoBehaviour
    {

    }

    public enum UnitFaction { PLAYER, ENEMY, NEUTRAL}
}

namespace Tactics.ActionSystem
{
}


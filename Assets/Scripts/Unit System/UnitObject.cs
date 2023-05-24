using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tactics.GridSystem;

namespace Tactics.UnitSystem
{
    [RequireComponent(typeof(HealthComponent))]
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

        private void Start()
        {
            position = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.SetUnitAtGridPosition(this, position);
            transform.position = LevelGrid.Instance.GetWorldPosition(position);
        }

        public GridPosition GetUnitGridPosition()
        {
            return position;
        }

        public UnitFaction GetUnitFaction()
        {
            return unitFaction;
        }

        public void SetUnitPosition(GridPosition position)
        {
            this.position = position;
        }
    }

    public enum UnitFaction { PLAYER, ENEMY, NEUTRAL}
}

namespace Tactics.ActionSystem
{
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tactics.GridSystem;
using Tactics.ActionSystem;

namespace Tactics.UnitSystem
{
    [RequireComponent(typeof(HealthComponent))]
    public class UnitObject : MonoBehaviour
    {
        public static event Action<UnitObject> OnAnyUnitDied; 

        [Header("Components")]
        [SerializeField] private HealthComponent health;
        [SerializeField] private MoveAction moveAction;

        [Header("Attributes")]
        [SerializeField] private UnitFaction unitFaction;


        private GridPosition position;

        private void Awake()
        {
            if (health == null)
                health = GetComponent<HealthComponent>();

            health.OnDeath += Health_OnDeath_Callback;
        }

        private void Start()
        {
            position = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.SetUnitAtGridPosition(this, position);
            transform.position = LevelGrid.Instance.GetWorldPosition(position);
        }

        public void SetUnitPosition(GridPosition position)
        {
            this.position = position;
        }

        public GridPosition GetUnitGridPosition()
        {
            return position;
        }

        // Not using spriteRenderer because there are multiple sprites on the player
        public void TryFlipUnit(GridPosition direction)
        {
            if (direction.x <= -1 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            else if (direction.x >= 1 && transform.localScale.x < 0)
            {
                transform.localScale = Vector3.one;
            }
        }

        #region FactionSystem

        public bool CheckIfEnemy(UnitObject unit)
        {
            switch (unitFaction)
            {
                case UnitFaction.PLAYER:
                    return unit.GetUnitFaction() == UnitFaction.ENEMY || unit.GetUnitFaction() == UnitFaction.NEUTRAL;
                case UnitFaction.NEUTRAL:
                    return unit.GetUnitFaction() == UnitFaction.ENEMY || unit.GetUnitFaction() == UnitFaction.PLAYER;
                case UnitFaction.ENEMY:
                    return unit.GetUnitFaction() == UnitFaction.PLAYER || unit.GetUnitFaction() == UnitFaction.NEUTRAL;
                default:
                    return false;
            }
        }

        public bool CheckIfFriendly(UnitObject unit)
        {
            switch (unitFaction)
            {
                case UnitFaction.PLAYER:
                    return unit.GetUnitFaction() == UnitFaction.PLAYER;
                case UnitFaction.NEUTRAL:
                    return unit.GetUnitFaction() == UnitFaction.NEUTRAL;
                case UnitFaction.ENEMY:
                    return unit.GetUnitFaction() == UnitFaction.ENEMY;
                default:
                    return false;
            }
        }

        public UnitFaction GetUnitFaction()
        {
            return unitFaction;
        }

        #endregion

        public HealthComponent GetHealthComponent()
        {
            return health;
        }
        private void Health_OnDeath_Callback()
        {
            GetComponent<Animator>().SetTrigger("Death");
        }
    }

    public enum UnitFaction { PLAYER, ENEMY, NEUTRAL}
}

namespace Tactics.ActionSystem
{
}


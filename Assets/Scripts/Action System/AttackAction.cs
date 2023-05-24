using System;
using System.Collections;
using System.Collections.Generic;
using Tactics.GridSystem;
using Tactics.UnitSystem;
using UnityEngine;

namespace Tactics.ActionSystem
{
    public class AttackAction : ActionBase
    {
        [Header("Attack Properties")]
        [SerializeField] private int attackRangeX = 1;
        [SerializeField] private int attackRangeY = 1;
        [SerializeField] private int damageAmount = 50;

        private UnitObject target;

        public override List<GridPosition> GetValidActionGridPositions()
        {
            List<GridPosition> validPositions = new List<GridPosition>();

            for (int x = -attackRangeX; x <= attackRangeX; x++)
                for (int y = -attackRangeY; y <= attackRangeY; y++)
                {
                    GridPosition offset = new GridPosition(x, y);
                    GridPosition final = owningUnit.GetUnitGridPosition() + offset;

                    if (!LevelGrid.Instance.IsValidGridPosition(final)) continue;

                    UnitObject unit = LevelGrid.Instance.GetUnitAtPosition(final);
                    if (unit == null) continue;

                    if (!owningUnit.CheckIfEnemy(unit)) continue;

                    validPositions.Add(final);
                }

            return validPositions;
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            GridPosition unitPos = owningUnit.GetUnitGridPosition();
            GridPosition dir = gridPosition - unitPos;
            owningUnit.TryFlipUnit(dir);

            target = LevelGrid.Instance.GetUnitAtPosition(gridPosition);
            GetComponent<Animator>().SetTrigger("Attack");

            ActionStart(OnActionComplete);
        }

        public void AttackActiveAnimCallback()
        {
            if (target == null) return;

            target.GetHealthComponent().TakeDamage(damageAmount);
        }

        public void AttackCompleteAnimCallback()
        {
            ActionComplete();
        }
    }
}



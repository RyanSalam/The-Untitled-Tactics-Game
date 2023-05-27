using UnityEngine;

using Tactics.GridSystem;
using Tactics.UnitSystem;

namespace Tactics.AI
{
    [CreateAssetMenu(fileName = "New Move For Primary Action", menuName = "Tactics/AI Filters/MoveFilter")]
    public class MoveForPrimaryAction : AIFilterBase
    {
        public override EnemyAction GetEnemyAction(UnitObject unit, GridPosition position, EnemyActionQuery query)
        {
            int validPositionCounts = query.secondaryAction.GetValidActionGridPositions(position).Count;
            GridPosition dir = unit.GetUnitGridPosition() - position;
            int distanceNeeded = Mathf.Abs(dir.x) + Mathf.Abs(dir.y);

            return new EnemyAction
            {
                action = query.primaryAction,
                actionValue = validPositionCounts * 100,
                position = position
            };
        }
    }
}


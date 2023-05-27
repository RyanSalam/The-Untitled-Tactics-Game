using UnityEngine;

using Tactics.GridSystem;
using Tactics.UnitSystem;

namespace Tactics.AI
{
    [CreateAssetMenu(fileName = "New Attack Filter", menuName = "Tactics/AI Filters/Attack Filter")]
    public class AttackCheckFilter : AIFilterBase
    {
        public override EnemyAction GetEnemyAction(UnitObject unit, GridPosition position, EnemyActionQuery query)
        {
            int value = 0;

            // There may be need to be a check if this position is being called from other actions.

            UnitObject target = LevelGrid.Instance.GetUnitAtPosition(position);
            if (target == null) return null;

            if (unit.CheckIfEnemy(target))
            {
                float healthScore = target.GetHealthComponent().GetHealthNormalized();
                // additional scoring here afterwards;
                value = 200 + Mathf.RoundToInt(1 - healthScore) * 100;
                return new EnemyAction()
                {
                    actionValue = value,
                    position = position,
                    action = query.primaryAction
                };

                Debug.Log($"Enemy Found, the target was scored: {value}");
            }

            else
            {
                // unit is friendly however, leaving code block here in case for charmed units.
            }

            return null;
        }
    }
}


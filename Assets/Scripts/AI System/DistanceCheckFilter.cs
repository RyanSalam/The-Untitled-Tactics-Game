using UnityEngine;

using System.Collections.Generic;

using Tactics.GridSystem;
using Tactics.UnitSystem;

namespace Tactics.AI
{
    [CreateAssetMenu(fileName = "New Distance Check", menuName = "Tactics/AI Filters/Distance Check Filter")]
    public class DistanceCheckFilter : AIFilterBase
    {

        [SerializeField] private int distanceThreshold = 3;
        [SerializeField] private AnimationCurve distanceCurve;

        public override EnemyAction GetEnemyAction(UnitObject unit, GridPosition position, EnemyActionQuery query)
        {
            List<UnitObject> potentialUnits = new List<UnitObject>();

            int closestDistance = 10000;

            foreach(UnitObject target in UnitManager.Instance.GetPlayerUnits())
            {
                GridPosition direction = target.GetUnitGridPosition() - position;
                int distance = Mathf.Abs(direction.x) + Mathf.Abs(direction.y);

                if (distance <= distanceThreshold)
                {
                    potentialUnits.Add(target);
                }

                if (distance < closestDistance)
                    closestDistance = distance;
            }

            if (potentialUnits.Count > 0)
            {
                return new EnemyAction()
                {
                    actionValue = potentialUnits.Count * 10 + closestDistance * 5,
                    position = position,
                    action = query.primaryAction
                };
            }

            return null;
        }
    }
}


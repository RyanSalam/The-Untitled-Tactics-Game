using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tactics.GridSystem;
using Tactics.UnitSystem;

namespace Tactics.AI
{
    public abstract class AIFilterBase : ScriptableObject
    {
        public virtual EnemyAction GetBestEnemyAction(UnitObject unit, EnemyActionQuery query)
        {
            List<EnemyAction> enemyActionList = new List<EnemyAction>();
            List<GridPosition> validPositions = query.primaryAction.GetValidActionGridPositions();

            foreach(GridPosition position in validPositions)
            {
                EnemyAction action = GetEnemyAction(unit, position, query);

                if (action == null) continue;

                enemyActionList.Add(action);
                
            }

            if (enemyActionList.Count <= 0)
            {
                Debug.Log("Enemy action list count was considered to be 0");
                return null;
            }
            enemyActionList.Sort((EnemyAction a, EnemyAction b) => b.actionValue - a.actionValue);
            Debug.Log($"The best action found was scored: {enemyActionList[0].actionValue}");
            return enemyActionList[0];
        }

        public abstract EnemyAction GetEnemyAction(UnitObject unit, GridPosition position, EnemyActionQuery query);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tactics.GridSystem;
using Tactics.ActionSystem;
using Tactics.UnitSystem;

namespace Tactics.AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private EnemyActionQuery[] queries;
        [SerializeField] private int scoreThreshold = 100;

        private UnitObject unit;

        private void Awake()
        {
            unit = GetComponent<UnitObject>();
        }

        public EnemyAction GetBestEnemyAction()
        {
            EnemyAction bestEnemyAction = null;

            if (queries.Length <= 0) return null;

            foreach (EnemyActionQuery query in queries)
            {
                ActionBase mainAction = query.primaryAction;
                if (mainAction == null) continue;

                EnemyAction temp = query.AIFilter.GetBestEnemyAction(unit, query);
                if (temp == null)
                {
                    Debug.Log("query filter action was found to be null");
                    continue;
                }

                if (bestEnemyAction == null)
                {
                    Debug.Log("Best enemy action was changed");
                    bestEnemyAction = temp;
                    continue;
                }

                else if (temp.actionValue > bestEnemyAction.actionValue)
                {
                    bestEnemyAction = temp;
                }

                if (bestEnemyAction.actionValue >= scoreThreshold)
                {
                    break;
                }
            }

            Debug.Log($"The action: {bestEnemyAction.action.GetType()} was chosen with the score: {bestEnemyAction.actionValue}");
            return bestEnemyAction;
        } 
    }

    public class EnemyAction
    {
        public ActionBase action;
        public GridPosition position;
        public int actionValue;
    }

    [System.Serializable]
    public class EnemyActionQuery
    {
        public AIFilterBase AIFilter;
        public ActionBase primaryAction;
        public ActionBase secondaryAction;
    }
}


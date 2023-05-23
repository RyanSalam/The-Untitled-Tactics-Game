using System;
using System.Collections.Generic;
using UnityEngine;

using Tactics.GridSystem;

namespace Tactics.ActionSystem
{
    public class MoveAction : ActionBase
    {
        [Header("Movement Properties")]
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float maxMoveDistance = 3;

        private GridPosition start;
        private GridPosition target;
        private Vector3 targetWorld;

        private bool isMoving = false;

        private void Update()
        {
            if (isMoving)
                HandleMovement();
        }

        private void HandleMovement()
        {
            if (Vector3.Distance(targetWorld, transform.position) > 0.1f)
            {
                Vector3 moveDir = (targetWorld - transform.position).normalized;
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = LevelGrid.Instance.GetWorldPosition(target);
                ActionComplete();
            }
        }

        public override List<GridPosition> GetValidActionGridPositions()
        {
            List<GridPosition> validPositions = new List<GridPosition>();

            GridPosition unitPosition = owningUnit.GetUnitGridPosition();

            for (int x = 0; x < maxMoveDistance; x++)
                for (int y = 0; y < maxMoveDistance; y++)
                {
                    GridPosition offset = new GridPosition(x, y);
                    GridPosition finalPosition = unitPosition + offset;

                    if (!LevelGrid.Instance.IsValidGridPosition(finalPosition)) continue;
                    if (!LevelGrid.Instance.IsGridPositionEmpty(finalPosition)) continue;
                    if (unitPosition == finalPosition) continue;

                    validPositions.Add(finalPosition);
                }

            return validPositions;
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            start = owningUnit.GetUnitGridPosition();
            target = gridPosition;
            targetWorld = LevelGrid.Instance.GetWorldPosition(target);

            ActionStart(OnActionComplete);
        }
    }
}


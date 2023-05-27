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
        [SerializeField] private int maxMoveDistance = 3;
        [SerializeField] private bool handleDiagonals = false;

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
                isMoving = false;
                transform.position = LevelGrid.Instance.GetWorldPosition(target);
                LevelGrid.Instance.UnitMovedGridPosition(owningUnit, start, target);
                GetComponent<Animator>().SetBool("Moving", false);
                ActionComplete();
            }
        }

        public override List<GridPosition> GetValidActionGridPositions(GridPosition position)
        {
            List<GridPosition> validPositions = new List<GridPosition>();

            for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
                for (int y = -maxMoveDistance; y <= maxMoveDistance; y++)
                {
                    GridPosition offset = new GridPosition(x, y);
                    GridPosition finalPosition = position + offset;

                    if (!handleDiagonals)
                    {
                        if (offset.x == maxMoveDistance && offset.y == maxMoveDistance) continue;
                        if (offset.x == -maxMoveDistance && offset.y == -maxMoveDistance) continue;
                        if (offset.x == maxMoveDistance && offset.y == -maxMoveDistance) continue;
                        if (offset.x == -maxMoveDistance && offset.y == maxMoveDistance) continue;
                    }

                    if (!LevelGrid.Instance.IsValidGridPosition(finalPosition)) continue;
                    if (!LevelGrid.Instance.IsGridPositionEmpty(finalPosition)) continue;
                    if (!LevelGrid.Instance.IsGridPositionWalkable(finalPosition)) continue;
                    if (position == finalPosition) continue;



                    validPositions.Add(finalPosition);
                }

            return validPositions;
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {           
            start = owningUnit.GetUnitGridPosition();
            target = gridPosition;
            targetWorld = LevelGrid.Instance.GetWorldPosition(target);

            owningUnit.TryFlipUnit(target - start);

            isMoving = true;
            GetComponent<Animator>().SetBool("Moving", true);

            ActionStart(OnActionComplete);
        }
    }
}


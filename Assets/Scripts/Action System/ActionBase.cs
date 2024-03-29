﻿using System;
using System.Collections.Generic;
using UnityEngine;

using Tactics.GridSystem;
using Tactics.UnitSystem;

using Tactics.AI;

namespace Tactics.ActionSystem
{
    public abstract class ActionBase : MonoBehaviour
    {
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;

        [Header("Action Details")]
        [SerializeField] private string actionName;
        [SerializeField] private string actionDesc;
        [SerializeField] private Sprite actionIcon;

        protected UnitObject owningUnit;
        protected Action OnActionComplete;

        protected virtual void Awake()
        {
            owningUnit = GetComponent<UnitObject>();
        }

        public virtual void ActionStart(Action OnActionComplete)
        {
            this.OnActionComplete = OnActionComplete;
            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        }

        public virtual void ActionComplete()
        {
            if (OnActionComplete != null)
                OnActionComplete();

            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public abstract void TakeAction(GridPosition gridPosition, Action OnActionComplete);
        public abstract List<GridPosition> GetValidActionGridPositions(GridPosition position);
        public virtual List<GridPosition> GetValidActionGridPositions() => GetValidActionGridPositions(owningUnit.GetUnitGridPosition());
        public virtual bool IsValidActionGridPosition(GridPosition position)
        {
            return GetValidActionGridPositions().Contains(position);
        }

        public EnemyAction CalculateAIAction(GridPosition position)
        {
            int targetCount = owningUnit.GetPrimaryAction().GetValidActionGridPositions().Count;

            return new EnemyAction
            {
                position = position,
                actionValue = targetCount * 10
            };
        }

        public string GetActionName()
        {
            return actionName;
        }

    }
}


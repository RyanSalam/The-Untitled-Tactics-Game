using System;
using UnityEngine;

using Tactics.GridSystem;
using Tactics.UnitSystem;

namespace Tactics
{
    public class LevelGrid : MonoBehaviour
    {
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        [SerializeField] private float cellSize = 1;

        private Grid<GridObject> levelGrid;

        public event EventHandler<OnAnyUnitMovedEventArgs> OnAnyUnitMoved;
        public class OnAnyUnitMovedEventArgs : EventArgs
        {
            public UnitObject unit;
            public GridPosition from;
            public GridPosition to;
        }

        public static LevelGrid Instance { get; private set; }

        public void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (Instance != this)
                Destroy(this.gameObject);

            levelGrid = new Grid<GridObject>(width, height, cellSize, CreateGridObject);
        }

        private GridObject CreateGridObject(GridPosition position)
        {
            return new GridObject(position);
        }

        #region UnitMovement
        public void SetUnitAtGridPosition (UnitObject unit, GridPosition position)
        {
            GridObject gridObject = levelGrid.GetGridItem(position);
            gridObject.SetUnit(unit);
        }
        public UnitObject GetUnitAtPosition(GridPosition position)
        {
            return levelGrid.GetGridItem(position).GetUnit();
        }
        public void RemoveUnitAtPosition(GridPosition position)
        {
            levelGrid.GetGridItem(position).RemoveUnit();
        }
        public void UnitMovedGridPosition(UnitObject unit, GridPosition from, GridPosition to)
        {
            RemoveUnitAtPosition(from);
            SetUnitAtGridPosition(unit, to);

            OnAnyUnitMovedEventArgs movedEventArgs = new OnAnyUnitMovedEventArgs
            {
                unit = unit,
                from = from,
                to = to
            };

            OnAnyUnitMoved?.Invoke(this, movedEventArgs);
        }
        #endregion

        #region Wrapper Functions
        public GridPosition GetGridPosition(Vector3 worldPosition) => levelGrid.GetWorldToGrid(worldPosition);
        public Vector3 GetWorldPosition(GridPosition gridPosition) => levelGrid.GetGridToWorld(gridPosition);
        public GridObject GetGridObjectAtPosition(GridPosition position) => levelGrid.GetGridItem(position);
        public GridObject GetGridObjectAtPosition(int x, int y) => levelGrid.GetGridItem(x, y);
        public bool IsValidGridPosition(GridPosition position) => levelGrid.IsValidGridPosition(position);
        public bool IsValidGridPosition(int x, int y) => levelGrid.IsValidGridPosition(x, y);
        public bool IsGridPositionEmpty(GridPosition position) => !levelGrid.GetGridItem(position).HasUnit();
        #endregion
        public (float, float) GetGridWidthAndHeight()
        {
            return (width, height);
        }
    }
}

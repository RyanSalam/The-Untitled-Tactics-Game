using System;
using UnityEngine;
using UnityEngine.Tilemaps;

using Tactics.GridSystem;
using Tactics.UnitSystem;

namespace Tactics
{
    public class LevelGrid : MonoBehaviour
    {
        [Header("Grid Properties")]
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        [SerializeField] private float cellSize = 1;

        [Header("Tile Map Components")]
        [SerializeField] private Tilemap unwalkableMask;
        [SerializeField] private bool drawGizmos = false;


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

            levelGrid = new Grid<GridObject>(width, height, cellSize, transform.position, CreateGridObject);
        }

        private GridObject CreateGridObject(GridPosition position)
        {
            GridObject gridObject = new GridObject(position);

            // Have to calculate world pos without helper function as grid is still not initialized
            Vector3 worldPos = new Vector3(position.x, position.y) * cellSize + transform.position;

            int x = Mathf.RoundToInt(worldPos.x);
            int y = Mathf.RoundToInt(worldPos.y);

            Vector3Int cellPos = unwalkableMask.WorldToCell(worldPos);

            if (unwalkableMask.GetTile(cellPos) != null)
            {
                gridObject.SetIsWalkable(false);
            }

            return gridObject;
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
            unit.SetUnitPosition(to);

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
        public bool IsGridPositionWalkable(GridPosition position) => GetGridObjectAtPosition(position).GetIsWalkable();
        #endregion
        public (float, float) GetGridWidthAndHeight()
        {
            return (width, height);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (drawGizmos == false) return;

            for (int x = 0; x < width; x++)
                for(int y = 0; y < height; y++)
                {
                    Vector3 pos = transform.position;
                    Vector3 cubePos = new Vector3(x + pos.x, y + pos.y) * cellSize;
                    Gizmos.DrawWireCube(cubePos, Vector3.one * cellSize);
                }
        }
#endif

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}

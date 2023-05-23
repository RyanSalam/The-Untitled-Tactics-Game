using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tactics.GridSystem
{
    public class Grid<TGridItem>
    {
        private int width;
        private int height;

        private float cellSize;

        private TGridItem[,] gridItemArray;
        private List<TGridItem> gridItemList;

        public Grid(int width, int height, float cellSize, Func<GridPosition, TGridItem> CreateGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            gridItemArray = new TGridItem[width, height];
            gridItemList = new List<TGridItem>();

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    GridPosition position = new GridPosition(x, y);
                    TGridItem gridItem = CreateGridObject(position);
                    gridItemArray[x, y] = gridItem;
                    gridItemList.Add(gridItem);
                }
        }

        public Vector3 GetGridToWorld(GridPosition position)
        {
            return new Vector3(position.x, position.y) * cellSize;
        }

        public GridPosition GetWorldToGrid(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / cellSize);
            int y = Mathf.RoundToInt(worldPosition.y / cellSize);
            return new GridPosition(x, y);
        }

        public TGridItem GetGridItem(GridPosition position)
        {
            return gridItemArray[position.x, position.y];
        }

        public TGridItem GetGridItem(int x, int y)
        {
            return gridItemArray[x, y];
        }

        public bool IsValidGridPosition(GridPosition position)
        {
            return position.x >= 0 &&
                position.y >= 0 &&
                position.x < width &&
                position.y < height;
        }

        public bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && y >= 0 &&
                x < width && y < height;
        }

        public IEnumerable<TGridItem> GetGridItems()
        {
            return gridItemList;
        }
    }
}



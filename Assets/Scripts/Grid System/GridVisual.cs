using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

using Tactics;
using Tactics.ActionSystem;
using System;

namespace Tactics.GridSystem
{
    public class GridVisual : MonoBehaviour
    {
        [Header("Tile maps")]
        [SerializeField] private Tilemap visualTileMap;
        [SerializeField] private TileBase moveTile;

        public static GridVisual Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (Instance != this)
                Destroy(this.gameObject);
        }

        private void Start()
        {
            UnitActionSystem.Instance.OnActionChanged += UAS_ActionChangedCallback;
            UnitActionSystem.Instance.OnActionStarted += UAS_ActionStartedCallback;
        }

        private void UAS_ActionChangedCallback()
        {
            ActionBase action = UnitActionSystem.Instance.GetSelectedAction();
            foreach (GridPosition position in action.GetValidActionGridPositions())
            {
                Vector3 worldPos = LevelGrid.Instance.GetWorldPosition(position);

                int x = Mathf.RoundToInt(worldPos.x);
                int y = Mathf.RoundToInt(worldPos.y);

                visualTileMap.SetTile(new Vector3Int(x, y), moveTile);
            }
        }

        private void UAS_ActionStartedCallback()
        {
            visualTileMap.ClearAllTiles();
        }
    }
}


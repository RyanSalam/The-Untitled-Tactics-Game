using System;
using System.Collections.Generic;
using UnityEngine;

using Tactics.GridSystem;

namespace Tactics
{
    public class PlayerMouse : MonoBehaviour
    {
        public static PlayerMouse Instance { get; private set; }

        [SerializeField] private Transform cellVisual;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (Instance != this)
                Destroy(this.gameObject);
        }

        private void Update()
        {
            cellVisual.position = LevelGrid.Instance.GetWorldPosition(GetMouseGridPosition());
        }

        public static Vector3 GetPosition()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);
            return hit.point;
        }
        public static GridPosition GetMouseGridPosition()
        {
            GridPosition position = LevelGrid.Instance.GetGridPosition(PlayerMouse.GetPosition());
            return position;
        }
    }
}

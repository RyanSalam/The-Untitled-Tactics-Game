using System;
using System.Linq;
using System.Collections;
using UnityEngine;

using UnityEngine.EventSystems;

using Tactics.ActionSystem;
using Tactics.GridSystem;
using Tactics.UnitSystem;
using Tactics.AI;

namespace Tactics
{
    public class UnitActionSystem : MonoBehaviour
    {
        public event Action OnUnitSelected;
        public event Action OnActionChanged;
        public event Action OnActionStarted;

        private UnitObject selectedUnit;
        private ActionBase selectedAction;

        private bool isActionOccuring = false;
        private event Action<bool> OnStateChanged;

        public static UnitActionSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            if (Instance != this)
                Destroy(this.gameObject);
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (isActionOccuring) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (!TryHandleUnitSelection())
                HandleSelectedAction();

            if (Input.GetButtonDown("Jump"))
                StartCoroutine(HandleEnemyActions());
        }

        private bool TryHandleUnitSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GridPosition mouseGridPosition = PlayerMouse.GetMouseGridPosition();

                if (!LevelGrid.Instance.IsValidGridPosition(mouseGridPosition))
                {
                    Debug.Log($"Grid position was not valid: {mouseGridPosition}");
                    return false;
                }

                UnitObject unit = LevelGrid.Instance.GetUnitAtPosition(mouseGridPosition);

                if (unit == null) return false;
                if (unit == selectedUnit) return false;
                if (unit.GetUnitFaction() != UnitFaction.PLAYER) return false;

                SetSelectedUnit(unit);
                SetSelectedAction(unit.GetComponent<MoveAction>());
                return true;
            }

            return false;
        }

        private void HandleSelectedAction()
        {
            if (selectedAction == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                GridPosition mouseGridPosition = PlayerMouse.GetMouseGridPosition();

                if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;

                StartAction();
                selectedAction.TakeAction(mouseGridPosition, ClearAction);

                OnActionStarted?.Invoke();
            }
        }

        private IEnumerator HandleEnemyActions()
        {
            bool waitingForAction = false;
            yield return new WaitForSeconds(2);

            foreach (UnitObject unit in UnitManager.Instance.GetEnemyUnits())
            {
                AIController controller = unit.GetComponent<AIController>();

                if (controller == null)
                {
                    Debug.Log("AI Controller was found to be null");
                }

                EnemyAction action = unit.GetComponent<AIController>().GetBestEnemyAction();

                if (action == null)
                {
                    Debug.Log("Action has found to be null, please report");
                    continue;
                }

                ActionBase actionBase = action.action;
                if (actionBase == null) continue;

                actionBase.TakeAction(action.position, ClearWait);
                yield return new WaitUntil(() => waitingForAction);
            }

            void ClearWait()
            {
                waitingForAction = true;
            }
        }

        public void StartAction()
        {
            isActionOccuring = true;
            OnStateChanged?.Invoke(true);
        }

        public void ClearAction()
        {
            Debug.Log("Action has finished");
            isActionOccuring = false;
            OnStateChanged?.Invoke(false);
        }

        public void SetSelectedUnit(UnitObject newUnit)
        {
            selectedUnit = newUnit;
            OnUnitSelected?.Invoke();
        }

        public void SetSelectedAction(ActionBase action)
        {
            selectedAction = action;
            OnActionChanged?.Invoke();
        }

        public UnitObject GetSelectedUnit()
        {
            return selectedUnit;
        }

        public ActionBase GetSelectedAction()
        {
            return selectedAction;
        }
    }
}

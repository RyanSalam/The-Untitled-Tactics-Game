using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tactics;
using Tactics.ActionSystem;
using Tactics.UnitSystem;
using System;

using UnityEngine.UI;
using TMPro;

namespace Tactics.UI
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform abilityHolder;
        [SerializeField] private Button abilityButtonPrefab;

        private void Start()
        {
            UnitActionSystem.Instance.OnUnitSelected += UAS_UnitSelectedCallback;
        }

        private void UAS_UnitSelectedCallback()
        {
            foreach (Transform child in abilityHolder)
            {
                Destroy(child.gameObject);
            }

            UnitObject unit = UnitActionSystem.Instance.GetSelectedUnit();
            if (unit == null) return;


            ActionBase[] actions = unit.GetComponents<ActionBase>();
            foreach (ActionBase action in actions)
            {
                Button temp = Instantiate(abilityButtonPrefab, abilityHolder);
                temp.onClick.AddListener(() => UnitActionSystem.Instance.SetSelectedAction(action));
                temp.GetComponentInChildren<TextMeshProUGUI>().text = action.GetActionName();
            }
        }
    }
}



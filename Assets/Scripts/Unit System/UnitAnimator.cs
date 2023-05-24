using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tactics.ActionSystem;

namespace Tactics.UnitSystem
{
    public class UnitAnimator : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Animator anim;
        [SerializeField] private MoveAction moveAction;
        
    }
}


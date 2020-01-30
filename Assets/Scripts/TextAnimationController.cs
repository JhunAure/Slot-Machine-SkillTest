using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimationController : MonoBehaviour
{
    [SerializeField] Animator winAmountTextAnimator;

    private void OnEnable()
    {
        CustomEvents.GainWinnings += TriggerPreviousWin;
    }

    private void OnDisable()
    {
        CustomEvents.GainWinnings -= TriggerPreviousWin;
    }

    private void TriggerPreviousWin(float amount)
    {
        if (amount <= 0) return;
        winAmountTextAnimator.SetTrigger("fade"); 
    }
}

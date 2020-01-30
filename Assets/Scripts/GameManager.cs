using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float startingCoins = 100f;
    [SerializeField] float startingBet = 1.00f;

    [HideInInspector] public float currentCoins;
    [HideInInspector] public float currentBet;
    [HideInInspector] public float currentTotalBet;

    float defaultBetMultiplier = 5f;

    private void Start()
    {
        currentBet = startingBet;
        currentCoins = startingCoins;
        CustomEvents.UpdateCoins?.Invoke(currentCoins);
        CustomEvents.UpdateBets?.Invoke(currentBet, defaultBetMultiplier);
    }

    private void OnEnable()
    {
        CustomEvents.StartSpin += CalculateSpinCost;
    }

    private void OnDisable()
    {
        CustomEvents.StartSpin -= CalculateSpinCost;
    }

    private void CalculateSpinCost()
    {
        float result = Calculator.GetSpinCost(defaultBetMultiplier, currentBet);
        currentCoins -= result;
        CustomEvents.UpdateCoins?.Invoke(currentCoins);
    }

    public void UpdateCurrentCoins(float amount)
    {
        currentCoins += amount;
        CustomEvents.UpdateCoins?.Invoke(currentCoins);
    }

    //On Button Clicked
    public void SpinReels()
    {
        if (currentBet <= 0 || currentCoins < currentTotalBet) return;
        CustomEvents.StartSpin?.Invoke();
    }

    public void IncreaseBet()
    {
        if (currentTotalBet < currentCoins)
        {
            currentBet++;
        }
        CustomEvents.UpdateBets?.Invoke(currentBet, defaultBetMultiplier);

    }
    public void DecreaseBet()
    {
        if(currentBet > 0)
        {
            currentBet--;
        }
        CustomEvents.UpdateBets?.Invoke(currentBet, defaultBetMultiplier);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlotMachine : MonoBehaviour
{
    public static int stoppedReelCount = 0;

    [SerializeField] Reel[] reels;

    GameManager gameManager;

    public List<SymbolController> symbols;
    public float reelSpinSpeed = 5f;
    public float reelSlowTimeDelay = 3f;

    private void Awake()
    {
        reels = GetComponentsInChildren<Reel>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        CustomEvents.StopReelSpin += IncreaseStoppedReelCount;
        CustomEvents.StartSpin += ResetReelCount;
        CustomEvents.SpinFullStop += DetermineAllCombinations;
    }

    private void OnDisable()
    {
        CustomEvents.StopReelSpin -= IncreaseStoppedReelCount;
        CustomEvents.StartSpin -= ResetReelCount;
        CustomEvents.SpinFullStop -= DetermineAllCombinations;
    }

    private void IncreaseStoppedReelCount(int amount)
    {
        stoppedReelCount = amount;

        if (stoppedReelCount == 3)
        {
            CustomEvents.SpinFullStop?.Invoke();
        }
    }

    private void ResetReelCount()
    {
        stoppedReelCount = 0;
    }

    private void DetermineAllCombinations()
    {
        float allCombinationsAmount = VerifyDiagonal() + VerifyHorizontal();
        CustomEvents.GainWinnings?.Invoke(allCombinationsAmount);
    }

    private float VerifyDiagonal()
    {
        float totalDiagonalAmount = 0;

        if (SymbolSlot(0, 2) == SymbolSlot(1, 1) && SymbolSlot(1, 1) == SymbolSlot(2, 0))
        {
            totalDiagonalAmount += CalculateCombination(0, 2, Directions.DiagDown);
        }

        if (SymbolSlot(0, 0) == SymbolSlot(1, 1) && SymbolSlot(1, 1) == SymbolSlot(2, 2))
        {
            totalDiagonalAmount += CalculateCombination(0, 0, Directions.DiagUp);
        }
        return totalDiagonalAmount;
    }

    private float VerifyHorizontal()
    {
        float totalHorizontalAmount = 0;

        if (SymbolSlot(0, 0) == SymbolSlot(1, 0) && SymbolSlot(1, 0) == SymbolSlot(2, 0))
        {
            totalHorizontalAmount += CalculateCombination(0, 0, Directions.HorBottom);
        }

        if (SymbolSlot(0, 1) == SymbolSlot(1, 1) && SymbolSlot(1, 1) == SymbolSlot(2, 1))
        {
            totalHorizontalAmount += CalculateCombination(0, 1, Directions.HorMid);
        }

        if (SymbolSlot(0, 2) == SymbolSlot(1, 2) && SymbolSlot(1, 2) == SymbolSlot(2, 2))
        {
            totalHorizontalAmount += CalculateCombination(0, 2, Directions.HorTop);
        }
        return totalHorizontalAmount;
    }

    private SymbolName SymbolSlot(int reelIndex, int symbolPosY)
    {
        return reels[reelIndex].GetCombinations()[symbolPosY];
    }

    private float CalculateCombination(int reelIndex, int combinationIndex, Directions direction)
    {
        int symbolValue = (int)SymbolSlot(reelIndex, combinationIndex);
        float wonCombinationAmount = Calculator.GetCombinationValue(symbolValue, gameManager.currentBet);
        gameManager.UpdateCurrentCoins(wonCombinationAmount);
        CustomEvents.Winner?.Invoke((int)direction);
        return wonCombinationAmount;
    }
}

public enum Directions
{
    DiagUp,
    DiagDown,
    HorTop,
    HorMid,
    HorBottom
}

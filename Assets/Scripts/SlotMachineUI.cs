using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotMachineUI : MonoBehaviour
{
    [SerializeField] Button spinButton = null;
    [SerializeField] Button increaseButton = null;
    [SerializeField] Button decreaseButton = null;
    [SerializeField] TextMeshProUGUI currentCoins;
    [SerializeField] TextMeshProUGUI currentBet;
    [SerializeField] TextMeshProUGUI totalBets;
    [SerializeField] TextMeshProUGUI winAmount;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        CustomEvents.UpdateCoins += UpdateCoinsUI;
        CustomEvents.UpdateBets += UpdateBetsUI;
        CustomEvents.GainWinnings += UpdateEarnedWinnings;
        CustomEvents.StartSpin += DisableButton;
        CustomEvents.SpinFullStop += EnableButton;
    }

    private void OnDisable()
    {
        CustomEvents.UpdateCoins -= UpdateCoinsUI;
        CustomEvents.UpdateBets -= UpdateBetsUI;
        CustomEvents.GainWinnings -= UpdateEarnedWinnings;
        CustomEvents.StartSpin -= DisableButton;
        CustomEvents.SpinFullStop -= EnableButton;
    }

    private void DisableButton()
    {
        increaseButton.interactable = false;
        decreaseButton.interactable = false;
        spinButton.interactable = false;
    }

    private void EnableButton()
    {
        increaseButton.interactable = true;
        decreaseButton.interactable = true;
        spinButton.interactable = true;
    }

    public void UpdateCoinsUI(float amount)
    {
        currentCoins.SetText(amount.ToString());
    }

    public void UpdateEarnedWinnings(float amount)
    {
        if (amount <= 0) return;
        winAmount.SetText($"+{amount.ToString()}");
    }

    public void UpdateBetsUI(float bet, float totalBet)
    {
        float totalBetResult = Calculator.GetSpinCost(bet, totalBet);
        gameManager.currentTotalBet = totalBetResult;
        currentBet.SetText(bet.ToString());
        totalBets.SetText(totalBetResult.ToString());
    }
}

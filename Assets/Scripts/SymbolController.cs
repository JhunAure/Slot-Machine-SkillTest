using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolController : MonoBehaviour
{
    public SymbolName symbolName;
    public Sprite symbolSprite;
    public int valueMultiplier;

    SlotMachine slotMachine;
    Reel parentReel;

    float stopSpinCounter;
    float currentStopSpinCount;
    float currentSpeed;

    bool isSpinning = false;

    private void Awake()
    {
        slotMachine = FindObjectOfType<SlotMachine>();
        parentReel = GetComponentInParent<Reel>();
    }

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = symbolSprite;
        currentSpeed = slotMachine.reelSpinSpeed;
        currentStopSpinCount = stopSpinCounter;
    }

    private void OnEnable()
    {
        CustomEvents.StartSpin += StartSpinning;
    }

    private void OnDisable()
    {
        CustomEvents.StartSpin -= StartSpinning;
    }

    private void FixedUpdate()
    {
        if(isSpinning)
        {
            if (transform.position.y <= 0)
            {
                ResetPosition();
            }
            Spin();
            StopToPosition();
        }
    }

    private void StopToPosition()
    {
        if (currentStopSpinCount > 0)
        {
            currentStopSpinCount -= Time.deltaTime;
        }
        else
        {
            if (symbolName == parentReel.GetCombinations()[0])
            {
                if(transform.position.y == 2)
                {
                    parentReel.StopReel();
                }
            }
        }
    }

    private void ResetPosition()
    {
        transform.position = new Vector2(transform.position.x, slotMachine.symbols.Count);
    }

    private void Spin()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - currentSpeed);
    }

    private void StartSpinning()
    {
        isSpinning = true;
    }

    public void SetSlowTimeMultiplier(float amount)
    {
        stopSpinCounter = slotMachine.reelSlowTimeDelay * amount;
    }

    public void StopSpinning()
    {
        isSpinning = false;

        if (SlotMachine.stoppedReelCount == 3) return;
        CustomEvents.StopReelSpin?.Invoke(parentReel.reelNumber);
    }

    public void ResetStopSpinCounter()
    {
        currentStopSpinCount = stopSpinCounter;
    }
}

public enum SymbolName
{
    A = 1, B, C, D, E, F, G, H
}
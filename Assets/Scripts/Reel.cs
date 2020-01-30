using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    [SerializeField] float reelSlowTimeMultiplier;

    public int reelNumber;

    SlotMachine slotMachine;
    List<GameObject> symbols;
    List<SymbolName> randomSymbolCombinations;

    private void Awake()
    {
        slotMachine = FindObjectOfType<SlotMachine>();
    }

    private void Start()
    {
        GenerateSymbols();
    }

    private void OnEnable()
    {
        CustomEvents.StartSpin += GenerateRandomCombination;
    }

    private void OnDisable()
    {
        CustomEvents.StartSpin -= GenerateRandomCombination;
    }

    private void GenerateSymbols()
    {
        symbols = new List<GameObject>();
        int symbolCount = slotMachine.symbols.Count;

        for (int i = 0; i < symbolCount; i++)
        {
            Vector2 tempLoc = new Vector2(transform.position.x, i);
            GameObject newSymbol = Instantiate(slotMachine.symbols[i].gameObject, tempLoc, Quaternion.identity, transform);
            newSymbol.GetComponent<SymbolController>().SetSlowTimeMultiplier(reelSlowTimeMultiplier);
            symbols.Add(newSymbol);
        }
    }

    private void GenerateRandomCombination()
    {
        randomSymbolCombinations = new List<SymbolName>();
        int startingSymbolIndex = (int)GetRandomSymbolName();

        for (int i = 0; i < 3; i++)
        {
            randomSymbolCombinations.Add((SymbolName)startingSymbolIndex);
            startingSymbolIndex = (startingSymbolIndex >= symbols.Count) ? 1 : ++startingSymbolIndex;
        }
        DisplayRandomCombination();
    }

    private SymbolName GetRandomSymbolName()
    {
        return (SymbolName)Random.Range(1, symbols.Count + 1);
    }

    private bool IsInteger(float value)
    {
        return Mathf.Approximately(value, Mathf.RoundToInt(value));
    }

    private void CorrectingPosition(GameObject s)
    {
        if (!IsInteger(s.transform.position.y))
        {
            s.transform.position = new Vector2(s.transform.position.x, Mathf.RoundToInt(s.transform.position.y));
        }
    }

    public List<SymbolName> GetCombinations()
    {
        return randomSymbolCombinations;
    }

    public void StopReel()
    {
        foreach (GameObject s in symbols)
        {
            var symbols = s.GetComponent<SymbolController>();
            symbols.ResetStopSpinCounter();
            symbols.StopSpinning();
            CorrectingPosition(s);
        }
    }
    private void DisplayRandomCombination()
    {
        for (int i = 0; i < randomSymbolCombinations.Count; i++)
        {
            Debug.Log($"{gameObject.name} {i}: {randomSymbolCombinations[i].ToString()}");
        }
    }
}


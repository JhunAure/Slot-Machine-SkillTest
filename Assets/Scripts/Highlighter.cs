using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public List<GameObject> highlights;

    private void OnEnable()
    {
        CustomEvents.StartSpin += ResetHighlights;
        CustomEvents.Winner += SetHighlight;
    }

    private void OnDisable()
    {
        CustomEvents.StartSpin -= ResetHighlights;
        CustomEvents.Winner -= SetHighlight;
    }

    private void Start()
    {
        ResetHighlights();
    }

    public void ResetHighlights()
    {
        foreach(var h in highlights)
        {
            h.SetActive(false);
        }
    }

    public void SetHighlight(int index)
    {
        highlights[index].SetActive(true);
    }
}

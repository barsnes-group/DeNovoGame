using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Score : MonoBehaviour
{

    public float currentScore;

    private void SetText(string text)
    {
        GetComponent<TextMeshPro>().text = text.ToString();
    }

    private void UpdateScore()
    {
        SetText("Score: " + (int)Math.Round(currentScore, 0));
    }

    internal float GetScore()
    {
        return currentScore;
    }

    internal void CalculateScore(DraggableBox[] draggableBoxes)
    {
        currentScore=0;
        for (int i = 0; i < draggableBoxes.Length; i++)
        {
            DraggableBox box = draggableBoxes[i];
            if (box.GetIsPlaced())
            {
                float avgInt = ((box.GetStartPeak().intensity + box.GetEndPeak().intensity)/2);
                currentScore += avgInt;
            }
        }
        UpdateScore();
    }
}

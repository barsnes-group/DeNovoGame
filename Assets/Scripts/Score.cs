using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{

    private int currentScore;

    private void SetText(string text)
    {
        GetComponent<TextMeshPro>().text = text.ToString();
    }

    public int AddScore(int score)
    {
        currentScore += score;
        UpdateScore();
        return currentScore;
    }

    private void UpdateScore()
    {
        SetText("Score: " + currentScore);
    }

}

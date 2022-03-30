using System;
using UnityEngine;


public class MenuGameController : MonoBehaviour
{
    public GameObject scoreObject;

    void Start()
    {
        if (scoreObject == null)
        {
            throw new Exception("score obj. null");
        }
    }

    private void Update()
    {
        Score scoreComponent = scoreObject.GetComponentInChildren<Score>();
        scoreComponent.UpdateHighScore();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BoxContainer : MonoBehaviour
{

    public string numberOfSlots;
    [SerializeField] private GameObject textObject;

    public void SetScale(float scaleX, float scaleY)
    {
        if (scaleX == 0 || scaleY == 0)
        {
            throw new ArgumentException();
        }
        transform.localScale = new Vector3(scaleX, scaleY, 0);
    }

    public void SetPos(float posX, float posY)
    {
        Vector3 parentTransform = transform.parent.position;
        transform.localPosition = new Vector3((parentTransform.x + posX), (parentTransform.y + posY), 0);
    }

    private void Update()
    {
        SetText(numberOfSlots);
    }

    internal void SetText(string text)
    {
        textObject.GetComponent<TextMeshProUGUI>().text = text.ToString();
    }

}

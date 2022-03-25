using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;
//https://gist.github.com/Matthew-J-Spencer/65aea3d55f1e2c6ccb2c3586bccbdbdb 

public class DraggableBox : MonoBehaviour
{
    public List<int> startPeakNumbers;
    public List<int> endPeakNumbers;
    public List<float> startCoord;
    public List<float> endCoord;


    public string width;
    public float boxToSlotTheshold = 2;

    private GameObject scoreObject;
    private Vector3 _dragOffset;
    private Camera _cam;
    public Vector3 startPos;
    [SerializeField] private float _speed = 10;
    [SerializeField] private GameObject textObject;
    internal JSONReader.AminoAcid aminoAcidChar;
    private bool isPlaced = false;
    public Peak placedStartPeak;
    public Peak placedEndPeak;
    internal float posX;
    private Color currentColor;
    private float defaultXScale = 1.5f;
    private float defaultYScale = 1.2f;

/*     private void Update()
    {
        SetText(getGameController().validSlots.ToString() + " / " + aminoAcidChar.slots.Length.ToString());
    } */

    public bool GetIsPlaced()
    {
        return isPlaced;
    }

    public Peak GetStartPeak()
    {
        if (!isPlaced)
        {
            throw new Exception("no start peak");
        }
        if (placedStartPeak == null)
        {
            throw new NullReferenceException("start peak is not set");
        }
        return placedStartPeak;
    }

    public Peak GetEndPeak()
    {
        if (!isPlaced)
        {
            throw new Exception("this box does is not placed at a peak");
        }
        if (placedEndPeak == null)
        {
            throw new NullReferenceException("endpeak is not set");
        }
        return placedEndPeak;
    }
    void Awake()
    {
        _cam = Camera.main;
    }

    private String IndexesToString()
    {
        String str = "";
        foreach (var s in startPeakNumbers)
        {
            str += s + ", ";
        }
        str += "\n endpoints:";
        foreach (var s in endPeakNumbers)
        {
            str += s + ", ";
        }
        return str;
    }

    void OnMouseDown()
    {
        print("START: Box was Clicked");
        print("box can be placed in " + IndexesToString());
        _dragOffset = transform.position - GetMousePos();
        getGameController().HighlightValidSlots(startPeakNumbers, endPeakNumbers);
        //getGameController().HighlightUnvalidSlots(startPeakNumbers, endPeakNumbers);
        SetText("");
    }

    private void OnMouseUp()
    {
        Peak closestPeak = CheckClosestPeak();
        if (closestPeak != null)
        {
            //placed on a peak, may be occupied
            PlaceBox(closestPeak);
        }
        else
        {
            //no peak close enough
            ReturnToStartPos();
        }
        getGameController().ClearSlots();
    }

    /*
        check if this box is close to any peak
        return Peak, or null if not found
    */
    private Peak CheckClosestPeak()
    {
        foreach (int startPeakIndex in startPeakNumbers)
        {
            Peak startPeak = getGameController().GetPeak(startPeakIndex);
            if (startPeak == null)
            {
                throw new NullReferenceException("startpeak, on mouse up");
            }
            Vector2 peakStartPos = startPeak.transform.position;
            if (Vector2.Distance(peakStartPos, transform.position) < boxToSlotTheshold)
            {
                return startPeak;
            }
        }
        return null;
    }
    /*
    Box was placed on peak startPeak
    box placement may be occupied
    */
    private void PlaceBox(Peak startPeak)
    {
        Vector2 peakStartPos = startPeak.transform.position;
        SnapPosition(peakStartPos);
        Slot slot = getGameController().BoxPlaced(1, this, startPeak);
        if (!getGameController().lastBoxPlacedOccupied)
        {
            isPlaced = true;
            Vector2 peakEndPos = slot.endpeak.transform.position;
            int occupiedSlots = getGameController().occupiedSlotsCount;
        }
    }

    private GameController getGameController()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (gameController == null)
        {
            throw new NullReferenceException();
        }
        return gameController;
    }

    public void SetScoreObject(GameObject scoreObjectSetter)
    {
        if (scoreObjectSetter == null)
        {
            throw new Exception("score object null");
        }
        scoreObject = scoreObjectSetter;
    }

    public void ReturnToStartPos()
    {
        transform.position = startPos;
        SetScale(defaultXScale, defaultYScale);
        isPlaced = false;
        SetText(getGameController().validSlotsCount.ToString() + " / " + aminoAcidChar.slots.Length.ToString());
    }

    private void SnapPosition(Vector2 peakPos)
    {
        transform.position = peakPos;
    }

    void OnMouseDrag()
    {
        transform.position = Vector3.MoveTowards(transform.position, GetMousePos() + _dragOffset, _speed * Time.deltaTime);
    }

    Vector3 GetMousePos()
    {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 1;
        return mousePos;
    }

    public void SetScale(float scaleX, float scaleY)
    {
        if (scaleX == 0 || scaleY == 0)
        {
            throw new ArgumentException();
        }
        //transform.localScale = new Vector3(scaleX, scaleY, 0);
        GetComponentInChildren<Transform>().localScale = new Vector3(scaleX, scaleY, 0);
    }

    public void SetPos(float posX, float posY)
    {
        Vector3 parentTransform = transform.parent.position;
        transform.localPosition = new Vector3((parentTransform.x + posX), (parentTransform.y + posY), 0);
        startPos = transform.position;
    }

    public void SetColor(Color color)
    {
        currentColor = color;
        GetComponentInChildren<SpriteRenderer>().color = color;
    }

    public Color GetColor()
    {
        return currentColor;
    }

    public void SetText(string text)
    {
        TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = text.ToString();
    }

    /*
    switch the start and end index if the start index is bigger than the end index
    */
    internal void SwitchStartAndEndIndexes()
    {
        for (int i = 0; i < startPeakNumbers.Count(); i++)
        {
            if (startPeakNumbers[i] > endPeakNumbers[i])
            {
                int temp = endPeakNumbers[i];
                endPeakNumbers[i] = startPeakNumbers[i];
                startPeakNumbers[i] = temp;
            }
        }
    }

}
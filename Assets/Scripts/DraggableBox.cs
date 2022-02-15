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

    public bool getIsPlaced()
    {
        return isPlaced;
    }
    public Peak getStartPeak()
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

    public Peak getEndPeak()
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

    private String indexesToString()
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
        print("box can be placed in " + indexesToString());
        _dragOffset = transform.position - GetMousePos();
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.HighlightValidSlots(startPeakNumbers, endPeakNumbers, true);
    }

    private void OnMouseUp()
    {

        //check if close enough to start index, snap into slot
        foreach (int startPeakIndex in startPeakNumbers)
        {
            Peak startPeak = getGameController().GetPeak(startPeakIndex);
            if (startPeak == null) {
                throw new NullReferenceException("startpeak, on mouse up");
            }
            Vector2 peakStartPos = startPeak.transform.position;
            print("peakStartPos: " + peakStartPos);
            if (Vector2.Distance(peakStartPos, transform.position) < boxToSlotTheshold)
            {
                placeBox(startPeak);
                return;
            }
        }
        getGameController().HighlightValidSlots(startPeakNumbers, endPeakNumbers, false);
        ReturnToStartPos();
    }

    private void placeBox(Peak startPeak)
    {
        Vector2 peakStartPos = startPeak.transform.position;
        SnapPosition(peakStartPos);
        isPlaced = true;
        Slot slot = getGameController().BoxPlaced(1, true, this, startPeak);
        Vector2 peakEndPos = slot.endpeak.transform.position;
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

    private void ReturnToStartPos()
    {
        transform.position = startPos;
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
        transform.localScale = new Vector3(scaleX, scaleY, 0);
    }

    public void SetPos(float posX, float posY)
    {
        Vector3 parentTransform = transform.parent.position;
        transform.localPosition = new Vector3((parentTransform.x + posX), (parentTransform.y + posY), 0);
        startPos = transform.position;
    }

    public void SetColor()
    {
        Color color = new(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255), 255);
        GetComponentInChildren<SpriteRenderer>().color = color;
    }

    private void Update()
    {
        SetText(width);
    }

    internal void SetText(string text)
    {
        textObject.GetComponent<TextMeshProUGUI>().text = text.ToString();
    }

    internal void SwitchStartAndEndIndexes()
    {
        //switch the start and end index if the start index is bigger than the end index
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
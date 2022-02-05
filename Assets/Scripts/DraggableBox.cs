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
    public List<int> startIndexes;
    public List<int> endIndexes;
    public List<float> startCoord;
    public List<float> endCoord;

    public string width;
    public float boxToSlotTheshold = 5;

    private GameObject scoreObject;
    private Vector3 _dragOffset;
    private Camera _cam;
    public Vector3 startPos;
    [SerializeField] private float _speed = 10;
    [SerializeField] private GameObject textObject;
    internal JSONReader.AminoAcid aminoAcidChar;
    private bool isPlaced = false;
    private int placedStartPeak;
    private int placedEndPeak;
    internal float posX;

    public bool getIsPlaced()
    {
        return isPlaced;
    }
    public int getStartPeak()
    {
        if (!isPlaced)
        {
            throw new Exception("no start peak");
        }
        return placedStartPeak;
    }

    public int getEndPeak()
    {
        if (!isPlaced)
        {
            throw new Exception("no end peak");
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
        foreach (var s in startIndexes)
        {
            str += s + ", ";
        }
        str += "\n endpoints:";
        foreach (var s in endIndexes)
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
        gameController.HighlightValidSlots(startIndexes, endIndexes, true);
        gameController.SetHighlight(startIndexes, endIndexes, true);
    }

    private void OnMouseUp()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.HighlightValidSlots(startIndexes, endIndexes, false);
        gameController.SetHighlight(startIndexes, endIndexes, false);

        //check if close enough to start index, snap into slot
        foreach (int startpeak in startIndexes)
        {
            Vector2 peakPos = gameController.GetPeak(startpeak).transform.position;
            if (Vector2.Distance(peakPos, transform.position) < boxToSlotTheshold)
            {
                SnapPosition(peakPos);
                isPlaced = true;
                placedStartPeak = startpeak;
                //TODO:
                //placedEndPeak = startpeak;
                gameController.BoxPlaced(1, true, this);
                return;
            }
        }
        ReturnToStartPos();
        Score scoreComponent = scoreObject.GetComponent<Score>();
        if (scoreComponent.currentScore > 0)
        {
            print("score: " + gameController.score);
            gameController.BoxPlaced(-1, false, this);

        }  
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
        for (int i = 0; i < startIndexes.Count(); i++)
        {
            if (startIndexes[i] > endIndexes[i])
            {
                int temp = endIndexes[i];
                endIndexes[i] = startIndexes[i];
                startIndexes[i] = temp;
            }
        }
    }

}
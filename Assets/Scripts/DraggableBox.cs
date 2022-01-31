using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using System.Linq;
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
    

    private Vector3 _dragOffset;
    private Camera _cam;
    private Vector3 startPos;
    [SerializeField] private float _speed = 10;
    [SerializeField] private GameObject textObject;

    void Awake()
    {
        _cam = Camera.main;
    }

    private String indexesToString() {
        String str = "";
        foreach (var s in startIndexes) {
            str += s+", ";
        }
        str += "\n endpoints:";
        foreach (var s in endIndexes)
        {
            str += s+", ";
        }
        return str;
    }



    void OnMouseDown()
    {
        print("box can be placed in " + indexesToString());
        _dragOffset = transform.position - GetMousePos();
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.HighlightValidSlot(startIndexes, endIndexes, true);
        gameController.SetHighlight(startIndexes, endIndexes, true);
    }

    private void OnMouseUp()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.HighlightValidSlot(startIndexes, endIndexes, false);
        gameController.SetHighlight(startIndexes, endIndexes, false);

        //check if close enough to start index, snap into position
        foreach (int peak in startIndexes)
        {
            Vector2 peakPos = gameController.GetPeak(peak).transform.position;
            if (Vector2.Distance(peakPos, transform.position) < boxToSlotTheshold)
            {
                SnapPosition(peakPos);
                gameController.UpdateScore(1);
                return;
            }
        }
        ReturnToStartPos();
        gameController.UpdateScore(-1);
    }

    private void ReturnToStartPos()
    {
        transform.position = startPos;
    }

    private float CalculateCenterOfSlot()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        float center = 0;
        foreach (var startAndEndIndexes in startIndexes.Zip(endIndexes, Tuple.Create))
        {
            float startPeakPos = gameController.GetPeak(startAndEndIndexes.Item1).transform.position.x;
            float endPeakPos = gameController.GetPeak(startAndEndIndexes.Item2).transform.position.x;
            float absPeakPos = Mathf.Abs((startPeakPos - endPeakPos) / 2);
            if (startPeakPos > endPeakPos)
            {
                center = endPeakPos + absPeakPos;
                print("center = endPeakPos + absPeakPos " + endPeakPos + " + " + absPeakPos + " = " + center);
            }
            else
            {
                center = startPeakPos + absPeakPos;
                print("center = startPeakPos + absPeakPos " + startPeakPos + " + " + absPeakPos + " = " + center);
            }
        }
        return center;
    }

    private void SnapInCenter(float center)
    {
        Vector3 temp = new Vector3(center, 0, 0);
        transform.position += temp;
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
    private void Update()
    {
        SetText(width);
    }
    internal void SetText(string text)
    {
        textObject.GetComponent<TextMeshProUGUI>().text = text.ToString();
    }

    internal void sortIndexes()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        foreach (var startAndEndIndexes in startIndexes.Zip(endIndexes, Tuple.Create))
        {
            int startPeakIndex = startAndEndIndexes.Item1;
            int endPeakIndex = startAndEndIndexes.Item2;

            if (startPeakIndex > endPeakIndex)
            {
                int temp = startPeakIndex;
                endPeakIndex = startPeakIndex;
                startPeakIndex = temp;
            }

        }
    }
}